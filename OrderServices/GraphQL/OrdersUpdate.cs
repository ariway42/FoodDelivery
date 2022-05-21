using OrderServices.Models;

namespace OrderServices.GraphQL
{
    public record OrdersUpdate
    (
         int Id,
         
        int Qty,
       
        string Location,
        string Tracker

    );
}
