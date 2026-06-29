namespace DTOs
{
    public record SemanticSearchRequest(string Query, int TopK = 5);
    public record SemanticSearchResult(string Id, string Name, double Price, double Score);
    public record SemanticSearchResponse(List<SemanticSearchResult> Results);

    // Python AI service payload contracts
    public record ProductPayload(string Id, string Name, string Description, double Price);
    public record SeedPayload(List<ProductPayload> Products);
    public record SearchPayload(string Query, List<ProductPayload> Products, int TopK);
}
