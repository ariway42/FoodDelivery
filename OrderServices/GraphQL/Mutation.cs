
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
            [Service] FoodDeliveryContext context)
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


                    for (int i = 0; i < input.OrderDetailDatas.Count; i++)
                    {
                        var orderdetails = new OrderDetail
                        {
                            Location = input.OrderDetailDatas[i].Location,
                            Tracker = input.OrderDetailDatas[i].Tracker,
                            OrderId = order.Id,
                            FoodId = input.OrderDetailDatas[i].FoodId,
                            Qty = input.OrderDetailDatas[i].Qty
                        };
                        order.OrderDetails.Add(orderdetails);
                    }

                    var ret = context.Orders.Add(order);
                    await context.SaveChangesAsync();

                    return ret.Entity;
                }
            
           
        
        }
    }
