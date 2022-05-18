
using System.Linq;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.AspNetCore.Authorization;

using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using OrderServices.Models;

namespace OrderServices.GraphQL
{
    public class Query
    {

        [Authorize]
        public IQueryable<Order> GetOrders([Service] FoodDeliveryContext context, ClaimsPrincipal claimsPrincipal)
        {
            var userName = claimsPrincipal.Identity.Name;

            // check manager role ?
            var managerRole = claimsPrincipal.Claims.Where(o => o.Type == ClaimTypes.Role && o.Value == "BUYER").FirstOrDefault();
            var user = context.Users.Where(o => o.Username == userName).FirstOrDefault();
            if (user != null)
            {
                if (managerRole != null)
                    return context.Orders.Include(o => o.OrderDetails);

                var orders = context.Orders.Where(o => o.IdUser == user.Id);
                return orders.AsQueryable();
            }


            return new List<Order>().AsQueryable();
        }

    }
}
