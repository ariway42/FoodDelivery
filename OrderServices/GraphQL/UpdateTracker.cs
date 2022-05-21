using OrderServices.Models;

namespace OrderServices.GraphQL
{
    public record UpdateTracker
    (
         int? Id,
       bool Status,
        string? Tracker

    );
}
