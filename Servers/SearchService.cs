namespace Servers;

using DTOs;
using Repositories;
using System.Net.Http.Json;
using System.Text.Json;

public class SearchService : ISearchService
{
    private readonly HttpClient _http;
    private readonly IProductRepository _productRepository;
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public SearchService(IHttpClientFactory factory, IProductRepository productRepository)
    {
        _http = factory.CreateClient("PythonAI");
        _productRepository = productRepository;
    }

    public async Task SeedAsync()
    {
        var productList = await BuildProductListAsync();
        var res = await _http.PostAsJsonAsync("http://127.0.0.1:8001/seed", new SeedPayload(productList), _jsonOptions);
        if (!res.IsSuccessStatusCode)
        {
            var body = await res.Content.ReadAsStringAsync();
            throw new Exception($"AI service seed failed: {body}");
        }
    }

    public async Task<SemanticSearchResponse> SearchAsync(SemanticSearchRequest req)
    {
        var productList = await BuildProductListAsync();
        var payload = new SearchPayload(req.Query, productList, req.TopK);
        var res = await _http.PostAsJsonAsync("http://127.0.0.1:8001/search", payload, _jsonOptions);
        var body = await res.Content.ReadAsStringAsync();
        if (!res.IsSuccessStatusCode)
            throw new Exception($"AI service unavailable ({res.StatusCode}): {body}");
        if (!body.TrimStart().StartsWith('{') && !body.TrimStart().StartsWith('['))
            throw new Exception($"AI service returned non-JSON response ({res.StatusCode}): {body}");
        return JsonSerializer.Deserialize<SemanticSearchResponse>(body, _jsonOptions)
            ?? new SemanticSearchResponse([]);
    }

    private async Task<List<ProductPayload>> BuildProductListAsync()
    {
        var (products, _) = await _productRepository.GetProducts(null, null, null, null, 200, null, null);
        return products.Select(p => new ProductPayload(
            p.ProductId.ToString(),
            p.ProductName,
            p.Description ?? p.ProductName,
            p.Price
        )).ToList();
    }
}
