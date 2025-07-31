using Application.Commons.Objects.MessageEventTypes;
using Application.Store.AdminSection.CommentService;
using Application.Store.AdminSection.CommentService.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Areas.Admin.ModelsAndDtoes.Comments;

namespace WebApi.Areas.Admin.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[Area]/[controller]/")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly IFacadeAdminCommentService _facadeAdminCommentService;
        public CommentController(IFacadeAdminCommentService facadeAdminCommentService)
        {
            _facadeAdminCommentService = facadeAdminCommentService;
        }

        /// <summary>
        /// نمایش کامنت ها (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] CommentFilterForAdminSectionApiDto dto)
        {
            //map
            var inputService = new CommentFilterForAdminSectionDto
            {
                ProductId = dto.ProductId,
                Confirmation = dto.Confirmation,
                UserId = dto.UserId,

                CountInPage = dto.CountInPage,
                Page = dto.Page,
            };

            //Get data from service
            var resultService = await _facadeAdminCommentService.CommentQueriesService.GetComments(inputService);

            return Ok(resultService.Data);
        }

        /// <summary>
        /// تایید یک کامنت (Auth)
        /// </summary>
        /// <param name="CommentId"></param>
        /// <returns></returns>
        [HttpPut("{CommentId}")]
        public async Task<IActionResult> Put(long CommentId)
        {
            var resultService = await _facadeAdminCommentService.CommentCommandsService.Confirm(CommentId);
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
        /// حذف یک کامنت (Auth)
        /// </summary>
        /// <param name="CommentId"></param>
        /// <returns></returns>
        [HttpDelete("{CommentId}")]
        public async Task<IActionResult> Delete(long CommentId)
        {
            var resultService = await _facadeAdminCommentService.CommentCommandsService.Delete(CommentId);
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
