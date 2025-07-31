using Application.Commons.Objects.AppKeyNames;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.UserSection.CommentService;
using Application.Store.UserSection.CommentService.Commands;
using Application.Store.UserSection.CommentService.Queries;
using Application.Store.UserSection.ProductService.Queries;
using Domain.Products;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Filters.Permissions;
using WebApi.ModelsAndDtoes.Product;

namespace WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    public class CommentController : ControllerBase
    {
        private readonly IFacadeCommentService _facadeCommentService;

        public CommentController(IFacadeCommentService facadeCommentService)
        {
            _facadeCommentService = facadeCommentService;
        }

        /// <summary>
        /// نمایش کامنت های یک محصول 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] CommentFilterForUserSectionApiDto dto)
        {
            //map
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;
            var inputService = new CommentFilterForUserSectionDto
            {
                ProductId = dto.ProductId,

                CountInPage = dto.CountInPage,
                Page = dto.Page,
            };

            //Get data from service
            var resultService = await _facadeCommentService.CommentQueriesService.GetComments(inputService, userId);

            return Ok(resultService.Data);
        }

        /// <summary>
        /// گذاشتن یک کامنت (کاربری) (Auth)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(CreateCommentApiDto dto)
        {
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            var inputModel = new CreateCommentDto
            {
                ProductId = dto.ProductId,
                Star = dto.Star,
                Text = dto.Text,
            };
            var resultService = await _facadeCommentService.CommentCommandsService.CreateCommentAsync(inputModel, userId);
            if (resultService.IsSuccess)
            {
                return Created();
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
        /// حذف یک کامنت (کاربری) (Auth)
        /// </summary>
        /// <param name="CommentId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{CommentId}")]
        public async Task<IActionResult> Delete(long CommentId)
        {
            var userId = User.Claims?.FirstOrDefault(p => p.Type == "UserId")?.Value;

            var resultService = await _facadeCommentService.CommentCommandsService.DeleteCommentAsync(CommentId, userId);
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
