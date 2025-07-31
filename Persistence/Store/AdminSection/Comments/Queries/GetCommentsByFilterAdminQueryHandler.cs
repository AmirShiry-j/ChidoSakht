using Application.Commons.Objects.Dtoes;
using Application.Store.AdminSection.CommentService.Queries;
using Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.AdminSection.Comments.Queries
{
    public class GetCommentsByFilterAdminQueryHandler : IRequestHandler<GetCommentsByFilterAdminQuery, ResultFilterDto>
    {
        private readonly DataBaseContext _context;
        public GetCommentsByFilterAdminQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<ResultFilterDto> Handle(GetCommentsByFilterAdminQuery request, CancellationToken cancellationToken)
        {
            var prComment = PredicateBuilder.True<Comment>();
            var FilterDto = request.Filter;

            //Filter Confirmed
            if (FilterDto.Confirmation is not null)
                prComment = prComment.And(x => x.Confirmation == FilterDto.Confirmation);

            //Filter CommentId
            if (FilterDto.ProductId is not null)
                prComment = prComment.And(x => x.ProductId.Equals(FilterDto.ProductId));

            //Filter UserId
            if (FilterDto.UserId is not null)
                prComment = prComment.And(x => x.UserId.Equals(FilterDto.UserId));

            //Comment by FilterFor
            var comments = await _context.Comments.IgnoreQueryFilters()
                .Where(prComment)
                .Include(p => p.Helpfuls)
                .Include(p => p.User)
                .Include(p => p.Product)
                .OrderByDescending(p => p.CreateTime)
                .Skip((FilterDto.Page.Value - 1) * FilterDto.CountInPage.Value)
                .Take(FilterDto.CountInPage.Value)
                .Select(p => new CommentDto
                {
                    Id = p.Id,
                    Text = p.Text,
                    UserId = p.UserId,
                    FullNameUser = p.User.FullName,
                    Star = p.Star,
                    CreateTime = p.CreateTime,
                    Confirmation = p.Confirmation,
                    ProductId = p.ProductId,
                    ProductName = p.Product.Name
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
