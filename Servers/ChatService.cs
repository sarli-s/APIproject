namespace Servers;

using DTOs;
using Repositories;
using System.Net.Http.Json;

public class ChatService : IChatService
{
    private readonly HttpClient _http;
    private readonly IChatRepository _chatRepository;
    private readonly IProductRepository _productRepository;

    public ChatService(IHttpClientFactory factory, IChatRepository chatRepository, IProductRepository productRepository)
    {
        _http = factory.CreateClient("PythonAI");
        _chatRepository = chatRepository;
        _productRepository = productRepository;
    }

    public async Task<ChatResponse> SendMessageAsync(ChatRequest req)
    {
        var (products, _) = await _productRepository.GetProducts(null, null, null, null, 50, null, null);

        var productList = products.Select(p => new
        {
            Name = p.ProductName,
            p.Price,
            p.Description,
            p.CategoryId
        }).ToList<object>();

        var payload = new
        {
            message  = req.Message,
            history  = req.History,
            products = productList
        };

        var res = await _http.PostAsJsonAsync("http://127.0.0.1:8001/chat", payload);

        if (!res.IsSuccessStatusCode)
            throw new Exception("AI service unavailable");

        return await res.Content.ReadFromJsonAsync<ChatResponse>();
    }
}
