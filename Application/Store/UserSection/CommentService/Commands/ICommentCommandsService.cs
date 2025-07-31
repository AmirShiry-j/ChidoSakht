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
        Task<ResultDto> DeleteCommentAsync(long CommentId, string UserId);
        Task<ResultDto> CreateVoteOnCommentAsync(CreateVoteOnCommentDto dto, string UserId);
        Task<ResultDto> DeleteVoteOnCommentAsync(UpdateVoteOnCommentDto dto, string UserId);
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

        public async Task<ResultDto> CreateVoteOnCommentAsync(CreateVoteOnCommentDto dto, string UserId)
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

            //check exist
            var vote = await _mediator.Send(new GetVoteAUserOnCommentQuery(dto.CommentId, UserId));
            if (vote is null)
            {
                await _mediator.Send(new CreateVoteOnCommentCommand
                            (
                                dto.CommentId,
                                dto.WasHelpful,
                                UserId
                            ));

                return new ResultDto
                {
                    IsSuccess = true,
                    MessageEventType = MessageEventType.Ok
                };
            }
            else
            {
                if (vote.WasHelpful == dto.WasHelpful)
                {
                    return new ResultDto
                    {
                        IsSuccess = true,
                        MessageEventType = MessageEventType.Ok
                    };
                }
                else
                {
                    vote.WasHelpful = dto.WasHelpful;
                    await _mediator.Send(new UpdateVoteOnCommentCommand(vote));
                    return new ResultDto
                    {
                        IsSuccess = true,
                        MessageEventType = MessageEventType.Ok
                    };
                }
            }
        }

        public async Task<ResultDto> DeleteCommentAsync(long CommentId, string UserId)
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

            //check comment was for user
            if (comment.UserId.Equals(UserId))
            {
                return new ResultDto
                {
                    Message = "Temp-Mes کامنت متعلق به این یوزر نیست",
                    MessageEventType = MessageEventType.BadRequest
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

        public async Task<ResultDto> DeleteVoteOnCommentAsync(UpdateVoteOnCommentDto dto, string UserId)
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

            //get from db
            var vote = await _mediator.Send(new GetVoteAUserOnCommentQuery(dto.CommentId, UserId));
            if (vote is null)
            {
                return new ResultDto
                {
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //delete from db
            await _mediator.Send(new DeleteVoteOnCommentCommand(vote));

            return new ResultDto
            {
                IsSuccess = true,
                MessageEventType = MessageEventType.Ok
            };
        }
    }


    public class CreateCommentDto
    {
        public string Text { get; set; }
        public byte Star { get; set; }
        public int ProductId { get; set; }
    }
    public class CreateVoteOnCommentDto
    {
        public long CommentId { get; set; }
        public bool WasHelpful { get; set; }
    }
    public class UpdateVoteOnCommentDto
    {
        public long CommentId { get; set; }
    }
}
