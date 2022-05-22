
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using HotChocolate.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using OrderServices.Models;
using Microsoft.EntityFrameworkCore;

namespace OrderServices.GraphQL
{
    public class Mutation
    {
        [Authorize(Roles = new[] { "BUYER" })]
        public async Task<OrderData> AddOrderAsync(
            OrdersInput input, ClaimsPrincipal claimsPrincipal,
            [Service] FoodDeliveriesContext context)
        {

            var userName = claimsPrincipal.Identity.Name;

            var user = context.Users.Where(o => o.Username == userName).FirstOrDefault();

            using var transaction = context.Database.BeginTransaction();

            var max = context.Orders.Max(p => p.Id);
            var sttc = context.OrderDetails.Where(o => o.OrderId == max).FirstOrDefault();

            // var mx = context.OrderDetails.OrderByDescending(p => p.ID).FirstOrDefault().ID;


            foreach (var item in input.OrdersDetailsData)
            {

                // search for existing entity.
                var enti = context.OrderDetails.Where(x => x.CourierId == item.CourierId).FirstOrDefault();



                if (enti == null)
                {



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
                            Qty = input.OrdersDetailsData[i].Qty,
                            CourierId = input.OrdersDetailsData[i].CourierId
                        };
                        order.OrderDetails.Add(orderdetails);
                    }



                    context.Orders.Add(order);
                    context.SaveChanges();
                    await transaction.CommitAsync();

                 
                }
                else if (enti != null && sttc.Status == true)


                {


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
                            Qty = input.OrdersDetailsData[i].Qty,
                            CourierId = input.OrdersDetailsData[i].CourierId
                        };
                        order.OrderDetails.Add(orderdetails);
                    }



                    context.Orders.Add(order);
                    context.SaveChanges();
                    await transaction.CommitAsync();




                }
                 else if (enti != null && sttc.Status == false)
                {
                    return new OrderData();
                }



            }
            return new OrderData();
        }

            [Authorize(Roles = new[] { "MANAGER" })]
            public async Task<OrderDetail> UpdateOrderAsync(
                        OrdersUpdate input,
                        [Service] FoodDeliveriesContext context)
            {
                var orderDetail = context.OrderDetails.Where(o => o.Id == input.Id).FirstOrDefault();
                if (orderDetail != null)
                {


                    orderDetail.Qty = input.Qty;

                    orderDetail.Location = input.Location;
                    orderDetail.Tracker = input.Tracker;

                    context.OrderDetails.Update(orderDetail);
                    await context.SaveChangesAsync();
                }

                return await Task.FromResult(orderDetail);

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

        [Authorize(Roles = new[] { "COURIER" })]
        public async Task<OrderDetail> UpdateTrackerAsync(
        UpdateTracker input,
        [Service] FoodDeliveriesContext context)
        {
            var orderDetail = context.OrderDetails.Where(o => o.Id == input.Id).FirstOrDefault();
            if (orderDetail != null)
            {

           
                orderDetail.Tracker = input.Tracker;
              orderDetail.Status = input.Status;
                context.OrderDetails.Update(orderDetail);
                await context.SaveChangesAsync();
            }

            return await Task.FromResult(orderDetail);

        }


    }

}

    
