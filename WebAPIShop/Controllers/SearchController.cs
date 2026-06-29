using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servers;

namespace WebAPIShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<SemanticSearchResponse>> Search([FromQuery] string query, [FromQuery] int topK = 5)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("query is required");

            var result = await _searchService.SearchAsync(new SemanticSearchRequest(query, topK));
            return Ok(result);
        }

        [AuthorizeRoles("Admin")]
        [HttpPost("seed")]
        public async Task<IActionResult> Seed()
        {
            await _searchService.SeedAsync();
            return Ok();
        }
    }
}
