using Application.Store.UserSection.CommentService.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.UserSection.Comments.Queries
{
    internal class GetCountCommentForAProductByUserIdQueryHandler : IRequestHandler<GetCountCommentForAProductByUserIdQuery, long>
    {
        private readonly DataBaseContext _context;
        public GetCountCommentForAProductByUserIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<long> Handle(GetCountCommentForAProductByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Comments.CountAsync(p => p.UserId.Equals(request.UserId) && p.ProductId.Equals(request.ProductId));
        }
    }
}
