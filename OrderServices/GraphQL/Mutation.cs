
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using HotChocolate.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using OrderServices.Models;

namespace OrderServices.GraphQL
{
    public class Mutation
    {
        [Authorize(Roles = new[] { "BUYER" })]
        public async Task<Order> AddOrderAsync(
            OrdersInput input, ClaimsPrincipal claimsPrincipal,
            [Service] FoodDeliveriesContext context)
        {
            var userName = claimsPrincipal.Identity.Name;
            //var buyerRole = claimsPrincipal.Claims.Where(o => o.Type == ClaimTypes.Role && o.Value == "BUYER").FirstOrDefault();

            var user = context.Users.Where(o => o.Username == userName).FirstOrDefault();

            if (user == null) return new Order();

            var order = new Order
            {
                IdUser = user.Id,
                Code = input.Code

            };


            for (int i = 0; i < input.OrdersDetailsData.Count; i++)
            {
                var orderdetails = new OrderDetail
                {
                    Location = input.OrdersDetailsData[i].Location,
                    Tracker = input.OrdersDetailsData[i].Tracker,
                    OrderId = order.Id,
                    FoodId = input.OrdersDetailsData[i].FoodId,
                    Qty = input.OrdersDetailsData[i].Qty
                };
                order.OrderDetails.Add(orderdetails);
            }

            var ret = context.Orders.Add(order);
            await context.SaveChangesAsync();

            return ret.Entity;
        }

        [Authorize(Roles = new[] { "MANAGER" })]
        public async Task<OrderDetail> UpdateOrderAsync(
        OrderDetailInput input,
        [Service] FoodDeliveriesContext context)
        {
            
            var orderfoo = context.OrderDetails.Where(o => o.Id == input.Id).FirstOrDefault();
                if (orderfoo != null)
                {
       
        
                orderfoo.Qty = input.Qty;
                orderfoo.Location = input.Location;
                orderfoo.Tracker = input.Tracker;
              
                context.OrderDetails.Update(orderfoo);
                await context.SaveChangesAsync();
             
            }


            return await Task.FromResult(orderfoo);
        }

        [Authorize(Roles = new[] { "MANAGER" })]
        public async Task<Order> DeleteOrderByIdAsync(
        int id,
        [Service] FoodDeliveriesContext context)
        {
                var order = context.Orders.Where(o => o.Id == id).FirstOrDefault();
            //var OrderDetail = context.OrderDetails.Where(x => x.OrderId == Order.)
            if (order != null)
            {
                context.OrderDetails.RemoveRange(context.OrderDetails.Where(x => x.OrderId == id));
                context.Orders.RemoveRange(context.Orders.Where(x => x.Id == id));

                await context.SaveChangesAsync();
            }
         


            return await Task.FromResult(order);
        }
    }

    }

    
