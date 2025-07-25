using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.Dtoes;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.AdminSection.ProductService.Queries;
using Application.Store.UserSection.CommentService.Queries;
using Domain.Users;
using MediatR;

namespace Application.Store.UserSection.CommentService.Commands
{
    public interface ICommentCommandsService
    {
        Task<ResultDto<long>> CreateCommentAsync(CreateCommentDto dto, string UserId);
        Task<ResultDto> VoteOnCommentAsync(VoteOnCommentDto dto, string UserId);
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

        public async Task<ResultDto<long>> CreateCommentAsync(CreateCommentDto dto, string UserId)
        {
            //check exist
            var product = await _mediator.Send(new GetProductByIdQuery(dto.ProductId));
            if (product is null)
            {
                return new ResultDto<long>
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            var commentId = await _mediator.Send(new CreateCommentCommand
            (
                dto.Text,
                dto.Star,
                dto.ProductId,
                UserId
            ));

            return new ResultDto<long>
            {
                IsSuccess = true,
                Data = commentId
            };
        }

        public async Task<ResultDto> VoteOnCommentAsync(VoteOnCommentDto dto, string UserId)
        {
            //check exist
            var comment = await _mediator.Send(new GetCommentByIdQuery(dto.CommentId));
            if (comment is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            await _mediator.Send(new VoteOnCommentCommand
            (
                dto.CommentId,
                dto.WasHelpful,
                UserId
            ));

            return new ResultDto
            {
                IsSuccess = true,
                MessageEventType = MessageEventType.Created
            };
        }
    }


    public class CreateCommentDto
    {
        public string Text { get; set; }
        public byte Star { get; set; }
        public int ProductId { get; set; }
    }
    public class VoteOnCommentDto
    {
        public long CommentId { get; set; }
        public bool WasHelpful { get; set; }
    }
}
