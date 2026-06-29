namespace DTOs
{
    public record OrderDTO(
        int OrderId,
        DateOnly OrderDate,
        ICollection<OrderItemDTO> OrderItems,
        int userId,
        string Status = "באריזה",
        double OrderSum = 0);
}
