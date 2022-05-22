

using OrderServices.Models;
using System.Collections.Generic;

namespace OrderServices.GraphQL
{
    public record OrdersInput
    (
        int? Id,
        string Code,
         int IdUser,
         bool? Status,
         List<OrderDetailsData> OrdersDetailsData
      
    ); 
    
}
