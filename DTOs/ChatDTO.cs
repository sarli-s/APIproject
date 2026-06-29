namespace DTOs
{
    public record ChatRequest(string Message, List<HistoryItem> History, List<object> Products);
    public record HistoryItem(string Role, string Content);
    public record ChatResponse(string Reply);
}
