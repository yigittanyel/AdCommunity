using AdCommunity.Application.DTOs.Community;
using AdCommunity.Application.Services.ElasticSearch;
using AdCommunity.Domain.Entities.Aggregates.Community;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdCommunity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElasticSearchController : ControllerBase
    {
        private readonly IElasticSearchService<AdCommunity.Domain.Entities.Aggregates.Community.Community> _elasticSearchService;

        public ElasticSearchController(IElasticSearchService<Community> elasticSearchService)
        {
            _elasticSearchService = elasticSearchService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDocumentAsync([FromBody] CommunityCreateDto request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request body");
            }

            var community = new Community(request.Name, request.Description, request.Tags, request.Location, request.Website, request.Facebook, request.Twitter, request.Instagram, request.Github, request.Medium);

            var response = await _elasticSearchService.CreateDocumentAsync(community);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocumentAsync(int id)
        {
            var response = await _elasticSearchService.GetDocumentAsync(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetDocumentsAsync()
        {
            var response = await _elasticSearchService.GetDocumentsAsync();
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDocumentAsync(CommunityUpdateDto request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request body");
            }

            var community = new Community(request.Name, request.Description, request.Tags, request.Location, request.Website, request.Facebook, request.Twitter, request.Instagram, request.Github, request.Medium);

            var response = await _elasticSearchService.UpdateDocumentAsync(community);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDocumentAsync(int id)
        {
            var response = await _elasticSearchService.DeleteDocumentAsync(id);
            return Ok(response);
        }
    }
}
