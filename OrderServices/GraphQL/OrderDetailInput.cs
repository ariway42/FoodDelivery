namespace OrderServices.GraphQL
{
    public record OrderDetailInput
    (
        int Id,
        int Qty,
        string Location,
        string Tracker


    );
}
