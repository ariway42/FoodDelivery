
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using HotChocolate.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using UserServices.Models;

namespace UserServices.GraphQL
{
    public class Mutation
    {
        public async Task<UserData> RegisterUserAsync(
            RegisterUser input,
            [Service] FoodDeliveriesContext context)
        {
            var user = context.Users.Where(o => o.Username == input.UserName).FirstOrDefault();
            if (user != null)
            {
                return await Task.FromResult(new UserData());
            }
            var newUser = new User
            {
                Fullname = input.FullName,
                Email = input.Email,
                Username = input.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(input.Password) // encrypt password
            };

            // EF
            var ret = context.Users.Add(newUser);
            await context.SaveChangesAsync();

            return await Task.FromResult(new UserData
            {
                Id = newUser.Id,
                Username = newUser.Username,
                Email = newUser.Email,
                FullName = newUser.Fullname
            });
        }
        public async Task<UserToken> LoginAsync(
            LoginUser input,
            [Service] IOptions<TokenSettings> tokenSettings, // setting token
            [Service] FoodDeliveriesContext context) // EF
        {
            var user = context.Users.Where(o => o.Username == input.Username).FirstOrDefault();
            if (user == null)
            {
                return await Task.FromResult(new UserToken(null, null, "Username or password was invalid"));
            }
            bool valid = BCrypt.Net.BCrypt.Verify(input.Password, user.Password);
            if (valid)
            {
                // generate jwt token
                var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Value.Key));
                var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

                // jwt payload
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, user.Username));

                var userRoles = context.UserRoles.Where(o => o.Id == user.Id).ToList();
                foreach (var userRole in userRoles)
                {
                    var role = context.Roles.Where(o => o.Id == userRole.RoleId).FirstOrDefault();
                    if (role != null)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.Name));
                    }
                }

                var expired = DateTime.Now.AddHours(3);
                var jwtToken = new JwtSecurityToken(
                    issuer: tokenSettings.Value.Issuer,
                    audience: tokenSettings.Value.Audience,
                    expires: expired,
                    claims: claims, // jwt payload
                    signingCredentials: credentials // signature
                );

                return await Task.FromResult(
                    new UserToken(new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    expired.ToString(), null));
                //return new JwtSecurityTokenHandler().WriteToken(jwtToken);
            }

            return await Task.FromResult(new UserToken(null, null, Message: "Username or password was invalid"));
        }
        [Authorize(Roles = new[] { "ADMIN" })]
        public async Task<User> UpdateUserAsync(
           UserInput input,
           [Service] FoodDeliveriesContext context)
        {
            var user = context.Users.Where(o => o.Id == input.Id).FirstOrDefault();
            if (user != null)
            {
                user.Fullname = input.FullName;
                user.Email = input.Email;

                context.Users.Update(user);
                await context.SaveChangesAsync();
            }


            return await Task.FromResult(user);
        }

        [Authorize(Roles = new[] { "ADMIN" })]
        public async Task<User> DeleteUserByIdAsync(
          int id,
          [Service] FoodDeliveriesContext context)
        {
            var product = context.Users.Where(o => o.Id == id).FirstOrDefault();
            if (product != null)
            {
                context.Users.Remove(product);
                await context.SaveChangesAsync();
            }


            return await Task.FromResult(product);
        }

        [Authorize(Roles = new[] { "ADMIN" })]
        public async Task<User> AddUserAsync(
           UserCreate input,
           [Service] FoodDeliveriesContext context)
        {

            // EF
            var user = new User
            {
                Email = input.Email,
                Fullname = input.FullName,
                Username = input.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(input.Password) // encrypt password
            };

            var ret = context.Users.Add(user);
            await context.SaveChangesAsync();

            return ret.Entity;
        }


        [Authorize]
        public async Task<Profile> AddProfileAsync(
            ProfileInput input,
            ClaimsPrincipal claimsPrincipal,
            [Service] FoodDeliveriesContext context)
        {
           
            var userName = claimsPrincipal.Identity.Name;

                var user = context.Users.Where(o => o.Username == userName).FirstOrDefault();
                if (user != null)
                {
                    // EF
                    var profile = new Profile
                    {
                       Name = input.Name,
                       Address = input.Address,
                        Phone = input.Phone,
                         City = input.City,
                       UserId = user.Id
                    };

                var ret = context.Profiles.Add(profile);
                await context.SaveChangesAsync();

                return ret.Entity;

            }
                else
                    throw new Exception("user was not found");
            }

        [Authorize]
        public async Task<User> UpdatePassAsync(
          PassInput input, ClaimsPrincipal claimsPrincipal,
          [Service] FoodDeliveriesContext context)
        {
            var userName = claimsPrincipal.Identity.Name;

            var user = context.Users.Where(o => o.Username == userName).FirstOrDefault();
            if (user != null)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(input.Password);

                context.Users.Update(user);
                await context.SaveChangesAsync();
            }


            return await Task.FromResult(user);
        }


        [Authorize(Roles = new[] { "MANAGER" })]
        public async Task<User> AddCourierAsync(
        UserCreate input,
        [Service] FoodDeliveriesContext context)
        {

            // EF
            var user = new User
            {
                Email = input.Email,
                Fullname = input.FullName,
                Username = input.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(input.Password) // encrypt password
            };

            var ret = context.Users.Add(user);
            await context.SaveChangesAsync();

            return ret.Entity;
        }

        [Authorize(Roles = new[] { "MANAGER" })]
        public async Task<User> UpdateCourierAsync(
         UserInput input,
         [Service] FoodDeliveriesContext context)
        {
            var user = context.Users.Where(o => o.Id == input.Id).FirstOrDefault();
            if (user != null)
            {
                user.Fullname = input.FullName;
                user.Email = input.Email;

                context.Users.Update(user);
                await context.SaveChangesAsync();
            }


            return await Task.FromResult(user);
        }

        [Authorize(Roles = new[] { "MANAGER" })]
        public async Task<User> DeleteCourierByIdAsync(
          int id,
          [Service] FoodDeliveriesContext context)
        {
            var product = context.Users.Where(o => o.Id == id).FirstOrDefault();
            if (product != null)
            {
                context.Users.Remove(product);
                await context.SaveChangesAsync();
            }


            return await Task.FromResult(product);
        }


    }
}
