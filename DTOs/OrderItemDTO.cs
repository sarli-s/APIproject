namespace DTOs
{
    public record OrderItemDTO(
        int OrderItemId,
        int ProductId,
        string ProductName,
        int Quantity,
        string Popularcolore,
        string Customtext,
        double Price);
}
