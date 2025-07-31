using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.Dtoes;
using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.CommentService.Queries
{
    public interface ICommentQueriesService
    {
        Task<ResultDto<ResultFilterDto>> GetComments(CommentFilterForUserSectionDto filterDto, string? UserId);
    }
    public class CommentQueriesService : ICommentQueriesService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public CommentQueriesService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }

        public async Task<ResultDto<ResultFilterDto>> GetComments(CommentFilterForUserSectionDto filterDto, string? UserId)
        {
            //get from db
            var products = await _mediator.Send(new GetCommentsByFilterQuery(filterDto, UserId));

            //return
            return new ResultDto<ResultFilterDto>
            {
                IsSuccess = true,
                Data = products
            };
        }
    }
    public class CommentFilterForUserSectionDto
    {
        public int ProductId { get; set; }
        public int? Page { get; set; } = 1;
        public int? CountInPage { get; set; } = 10;
    }

    public class CommentDto
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public byte Star { get; set; }
        public string FullNameUser { get; set; }
        public int CountVoteHelpfuls { get; set; }
        public int CountVoteUnHelpfuls { get; set; }
        public bool? UserVoteHelpful { get; set; }
        public DateTime CreateTime { get; set; }
    }
    public class ResultFilterDto
    {
        public int Page { get; set; }
        public int CountInPage { get; set; }
        public int CountAllPages { get; set; }
        public int CountAllItems { get; set; }
        public List<CommentDto> Comments { get; set; }
    }
}
