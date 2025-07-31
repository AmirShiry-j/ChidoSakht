using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.Dtoes;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.UserSection.CommentService.Commands;
using Application.Store.UserSection.CommentService.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.CommentService.Commands
{
    public interface ICommentCommandsService
    {
        Task<ResultDto> Confirm(long CommentId);
        Task<ResultDto> Delete(long CommentId);
    }
    public class CommentCommandsService : ICommentCommandsService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public CommentCommandsService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }

        public async Task<ResultDto> Confirm(long CommentId)
        {
            //check exist
            var comment = await _mediator.Send(new GetCommentByIdQuery(CommentId));
            if (comment is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //change
            comment.Confirmation = true;
            await _mediator.Send(new UpdateCommentCommand(comment));

            return new ResultDto
            {
                IsSuccess = true,
            };
        }

        public async Task<ResultDto> Delete(long CommentId)
        {
            //check exist
            var comment = await _mediator.Send(new GetCommentByIdQuery(CommentId));
            if (comment is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //delete
            await _mediator.Send(new DeleteCommentCommand(comment));
            return new ResultDto
            {
                IsSuccess = true,
                MessageEventType = MessageEventType.Ok
            };
        }
    }
}
