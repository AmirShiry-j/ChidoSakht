using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.UserSection.CommentService;
using Application.Store.UserSection.CommentService.Commands;
using Application.Store.UserSection.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.ModelsAndDtoes.Product;

namespace WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [Authorize]
    public class VoteOnCommentController : ControllerBase
    {
        private readonly IFacadeCommentService _facadeCommentService;
        public VoteOnCommentController(IFacadeCommentService facadeCommentService)
        {
            _facadeCommentService = facadeCommentService;
        }

        /// <summary>
        /// ثبت یا ویرایش یک رای برای کامنت (کاربری) (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateOrUpdateVoteOnCommentApiDto dto)
        {
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            var inputModel = new CreateOrUpdateVoteOnCommentDto
            {
                CommentId = dto.CommentId,
                WasHelpful = dto.WasHelpful
            };
            var resultService = await _facadeCommentService.CommentCommandsService.CreateVoteOnCommentAsync(inputModel, userId);
            if (resultService.IsSuccess)
            {
                return NoContent();
            }
            else
            {
                if (resultService.MessageEventType == MessageEventType.NotFound)
                    return NotFound();
                else //bad request
                    return BadRequest(resultService.Message);
            }
        }
        /// <summary>
        /// حذف یک رای (کاربری) (Auth)
        /// </summary>
        /// <param name="CommentId"></param>
        /// <returns></returns>
        [HttpDelete("{CommentId}")]
        public async Task<IActionResult> Delete(long CommentId)
        {
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            var resultService = await _facadeCommentService.CommentCommandsService.DeleteVoteOnCommentAsync(CommentId, userId);
            if (resultService.IsSuccess)
            {
                return NoContent();
            }
            else
            {
                if (resultService.MessageEventType == MessageEventType.NotFound)
                    return NotFound();
                else //bad request
                    return BadRequest(resultService.Message);
            }
        }
    }
}
