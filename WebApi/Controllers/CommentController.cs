using Application.Store.UserSection.CommentService;
using Application.Store.UserSection.CommentService.Commands;
using Domain.Users;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.ModelsAndDtoes.Product;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly IFacadeCommentService _facadeCommentService;

        public CommentController(IFacadeCommentService facadeCommentService)
        {
            _facadeCommentService = facadeCommentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateCommentApiDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var model = new CreateCommentDto
            {
                ProductId = dto.ProductId,
                Star = dto.Star,
                Text = dto.Text,
            };
            var id = await _facadeCommentService.CommentCommandsService.CreateCommentAsync(model, userId);
            return Ok(new { CommentId = id });
        }

        [HttpPost("vote")]
        public async Task<IActionResult> Vote(VoteOnCommentApiDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var model = new CreateVoteOnCommentDto
            {
                CommentId = dto.CommentId,
                WasHelpful = dto.WasHelpful
            };
            await _facadeCommentService.CommentCommandsService.CreateVoteOnCommentAsync(model, userId);
            return Ok();
        }
    }

}
