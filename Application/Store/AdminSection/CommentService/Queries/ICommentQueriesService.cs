using Application.Commons.Interfaces.Localization;
using Application.Commons.Objects.Dtoes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.CommentService.Queries
{
    public interface ICommentQueriesService
    {
        Task<ResultDto<ResultFilterDto>> GetComments(CommentFilterForAdminSectionDto filterDto);
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

        public async Task<ResultDto<ResultFilterDto>> GetComments(CommentFilterForAdminSectionDto filterDto)
        {
            //get from db
            var comments = await _mediator.Send(new GetCommentsByFilterAdminQuery(filterDto));

            //return
            return new ResultDto<ResultFilterDto>
            {
                IsSuccess = true,
                Data = comments
            };
        }
    }

    public class CommentFilterForAdminSectionDto
    {
        public int? ProductId { get; set; }
        public bool? Confirmation { get; set; }
        public string? UserId { get; set; }
        public int? Page { get; set; } = 1;
        public int? CountInPage { get; set; } = 10;
    }
    public class CommentDto
    {
        public long Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Text { get; set; }
        public byte Star { get; set; }
        public string FullNameUser { get; set; }
        public string UserId { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Confirmation { get; set; }
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
