using OrderServices.Models;

namespace OrderServices.GraphQL
{
    public record OrdersUpdate
    (
         int? Id,
         int OrderId,
        int Qty,
        int FoodId,
        string? Location,
        string? Tracker

    );
}
