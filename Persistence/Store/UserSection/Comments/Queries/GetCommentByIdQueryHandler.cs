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
    public class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, Comment>
    {
        private readonly DataBaseContext _context;
        public GetCommentByIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<Comment> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
            //get from db
            var comment = await _context.Comments.FirstOrDefaultAsync(p => p.Id.Equals(request.Id));

            //Retrun It
            return comment;
        }
    }
}
