using Application.Store.UserSection.CommentService.Queries;
using Domain.Products;
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
    public class GetVoteAUserOnCommentQueryHandler : IRequestHandler<GetVoteAUserOnCommentQuery, Helpful>
    {
        private readonly DataBaseContext _context;
        public GetVoteAUserOnCommentQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<Helpful> Handle(GetVoteAUserOnCommentQuery request, CancellationToken cancellationToken)
        {
            var vote = await _context.Helpfuls.FirstOrDefaultAsync(p => p.UserId.Equals(request.UserId) && request.CommentId.Equals(request.CommentId));
            return vote;
        }
    }
}
