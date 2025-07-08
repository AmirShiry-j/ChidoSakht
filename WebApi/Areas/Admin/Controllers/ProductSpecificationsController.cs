using Application.Store.AdminSection.ProductSpecification.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Areas.Admin.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductSpecificationsController : ControllerBase
    {
        private readonly IProductSpecificationService _specService;

        public ProductSpecificationsController(IProductSpecificationService specService) => _specService = specService;

        [HttpPost("{productId}/specification-groups")]
        public async Task<IActionResult> CreateGroup(int productId, [FromBody] string title)
        {
            var id = await _specService.CreateSpecGroupAsync(productId, title);
            return CreatedAtAction(nameof(GetGroups), new { productId }, id);
        }

        [HttpPut("specification-groups/{groupId}")]
        public async Task<IActionResult> UpdateGroup(long groupId, [FromBody] string title)
        {
            var result = await _specService.UpdateSpecGroupAsync(groupId, title);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("specification-groups/{groupId}")]
        public async Task<IActionResult> DeleteGroup(long groupId)
        {
            var result = await _specService.DeleteSpecGroupAsync(groupId);
            return result ? NoContent() : NotFound();
        }

        [HttpGet("{productId}/specification-groups")]
        public async Task<IActionResult> GetGroups(int productId)
        {
            var groups = await _specService.GetSpecGroupsAsync(productId);
            return Ok(groups);
        }

        [HttpPost("specifications")]
        public async Task<IActionResult> CreateSpec([FromBody] CreateSpecCommand command)
        {
            var id = await _specService.CreateSpecAsync(command.GroupId, command.Key, command.Value);
            return CreatedAtAction(nameof(GetGroups), new { productId = 0 }, id);
        }

        [HttpPut("specifications/{specId}")]
        public async Task<IActionResult> UpdateSpec(long specId, [FromBody] UpdateSpecCommand command)
        {
            var result = await _specService.UpdateSpecAsync(specId, command.Key, command.Value);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("specifications/{specId}")]
        public async Task<IActionResult> DeleteSpec(long specId)
        {
            var result = await _specService.DeleteSpecAsync(specId);
            return result ? NoContent() : NotFound();
        }
    }
}
