using UserServices.Models;
using System.Linq;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.AspNetCore.Authorization;

using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace UserServices.GraphQL
{
    public class Query
    {
        [Authorize] // dapat diakses kalau sudah login
        public IQueryable<User> GetUsers([Service] FoodDeliveriesContext context, ClaimsPrincipal claimsPrincipal)
        {
            var userName = claimsPrincipal.Identity.Name;

            // check admin role ?
            var adminRole = claimsPrincipal.Claims.Where(o => o.Type == ClaimTypes.Role && o.Value == "ADMIN").FirstOrDefault();
            var user = context.Users.Where(o => o.Username == userName).FirstOrDefault();
            if (user != null)
            {
                if (adminRole != null)
                {
                    return context.Users;
                }
                var users = context.Users.Where(o => o.Id == user.Id);
                return users.AsQueryable();
            }


            return new List<User>().AsQueryable();
        }

        [Authorize] // dapat diakses kalau sudah login
        public IQueryable<User> GetCourier([Service] FoodDeliveriesContext context, ClaimsPrincipal claimsPrincipal)
        {
            var userName = claimsPrincipal.Identity.Name;

            // check admin role ?
            var adminRole = claimsPrincipal.Claims.Where(o => o.Type == ClaimTypes.Role && o.Value == "MANAGER").FirstOrDefault();
            var user = context.Users.Where(o => o.Username == userName).FirstOrDefault();
            var role = context.UserRoles.Where(o => o.RoleId == 4).FirstOrDefault();
            if (role != null)
            {
                if (adminRole != null)
                {
                    var orders = context.Users.Where(o => o.Id == role.RoleId);
                    return orders.AsQueryable();
                }
   
            }


            return new List<User>().AsQueryable();
        }

        [Authorize] 
        public IQueryable<Profile> GetProfiles([Service] FoodDeliveriesContext context, ClaimsPrincipal claimsPrincipal)
        {
            var userName = claimsPrincipal.Identity.Name;

            // check admin role ?
            var adminRole = claimsPrincipal.Claims.Where(o => o.Type == ClaimTypes.Role && o.Value == "ADMIN").FirstOrDefault();
            var user = context.Users.Where(o => o.Username == userName).FirstOrDefault();
            if (user != null)
            {
                if (adminRole!=null)                    
                {                    
                    return context.Profiles;
                }
                var profiles = context.Profiles.Where(o => o.UserId == user.Id);                
                return profiles.AsQueryable();
            }


            return new List<Profile>().AsQueryable();
        }
       

    }
}
