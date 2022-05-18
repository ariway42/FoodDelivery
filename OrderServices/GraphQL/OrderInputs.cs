

using OrderServices.Models;
using System.Collections.Generic;

namespace OrderServices.GraphQL
{
    public record OrderInputs
    (
        int? Id,
        string Code,
         int IdUser,
          int OrderId,
        int FoodId,
        int Qty,
        decimal? Location,
        decimal? Tracker
      
    ); 
    
}
