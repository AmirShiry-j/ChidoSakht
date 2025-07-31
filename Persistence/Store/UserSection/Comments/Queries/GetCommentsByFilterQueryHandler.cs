using Application.Commons.Objects.Dtoes;
using Application.Store.UserSection.CommentService.Queries;
using Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.UserSection.Comments.Queries
{
    public class GetCommentsByFilterQueryHandler : IRequestHandler<GetCommentsByFilterQuery, ResultFilterDto>
    {
        private readonly DataBaseContext _context;
        public GetCommentsByFilterQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<ResultFilterDto> Handle(GetCommentsByFilterQuery request, CancellationToken cancellationToken)
        {
            var prComment = PredicateBuilder.True<Comment>();
            var FilterDto = request.Filter;

            //Filter Confirmed
            prComment = prComment.And(x => x.Confirmation);

            //Filter CommentId
            prComment = prComment.And(x => x.Id.Equals(FilterDto.CommentId));

            //Comment by FilterFor
            var comments = await _context.Comments
                .Where(prComment)
                .Include(p => p.Helpfuls)
                .Include(p => p.UserId)
                .OrderByDescending(p => p.CreateTime)
                .Skip((FilterDto.Page.Value - 1) * FilterDto.CountInPage.Value)
                .Take(FilterDto.CountInPage.Value)
                .Select(p => new CommentDto
                {
                    Id = p.Id,
                    Text = p.Text,
                    CountVoteHelpfuls = p.Helpfuls.Count(p => p.WasHelpful),
                    CountVoteUnHelpfuls = p.Helpfuls.Count(p => !p.WasHelpful),
                    FullNameUser = p.User.FullName,
                    Star = p.Star,
                    UserVoteHelpful = (string.IsNullOrEmpty(request.UserId)) && p.Helpfuls.Any(p => p.UserId.Equals(request.UserId)) 
                                        ? null : p.Helpfuls.FirstOrDefault(p => p.UserId.Equals(request.UserId)).WasHelpful
                })
                .ToListAsync();


            //For Pagination
            int CountAllItems = _context.Comments.Where(prComment).Count();
            int CountAllPages = CountAllItems / FilterDto.CountInPage.Value + (CountAllItems % FilterDto.CountInPage.Value > 0 ? 1 : 0);

            return new ResultFilterDto
            {
                Page = FilterDto.Page.Value,
                CountInPage = FilterDto.CountInPage.Value,
                CountAllItems = CountAllItems,
                CountAllPages = CountAllPages,
                Comments = comments
            };
        }
    }
}
