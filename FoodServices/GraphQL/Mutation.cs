
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using HotChocolate.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using FoodServices.Models;

namespace FoodServices.GraphQL
{
    public class Mutation
    {
        [Authorize(Roles = new[] { "MANAGER" })]
        public async Task<Food> AddFoodsAsync(
            FoodInput input,
            [Service] FoodDeliveryContext context)
        {
            var food = new Food
            {
                Name = input.Name,
                
                Price = input.Price
              
            };

            var ret = context.Foods.Add(food);
            await context.SaveChangesAsync();

            return ret.Entity;

           
        }
        [Authorize(Roles = new[] { "MANAGER" })]
        public async Task<Food> UpdateFoodAsync(
          FoodInput input,
          [Service] FoodDeliveryContext context)
        {
            var food = context.Foods.Where(o => o.Id == input.Id).FirstOrDefault();
            if (food != null)
            {
                food.Name = input.Name;
                food.Price = input.Price;

                context.Foods.Update(food);
                await context.SaveChangesAsync();
            }


            return await Task.FromResult(food);
        }

        [Authorize(Roles = new[] { "MANAGER" })]
        public async Task<Food> DeleteFoodByIdAsync(
                   int id,
                   [Service] FoodDeliveryContext context)
        {
            var food = context.Foods.Where(o => o.Id == id).FirstOrDefault();
            if (food != null)
            {
                context.Foods.Remove(food);
                await context.SaveChangesAsync();
            }


            return await Task.FromResult(food);
        }
    }
}
