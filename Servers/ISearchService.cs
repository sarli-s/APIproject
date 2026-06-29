using DTOs;

namespace Servers
{
    public interface ISearchService
    {
        Task SeedAsync();
        Task<SemanticSearchResponse> SearchAsync(SemanticSearchRequest req);
    }
}
