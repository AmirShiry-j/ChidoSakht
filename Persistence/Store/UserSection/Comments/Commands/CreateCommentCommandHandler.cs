using Application.Store.UserSection.CommentService.Commands;
using Domain.Products;
using MediatR;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.UserSection.Comments.Commands
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, long>
    {
        private readonly DataBaseContext _context;
        public CreateCommentCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<long> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            //Define
            var comment = new Comment { Text = request.Text, ProductId = request.ProductId, Star = request.Star, UserId = request.UserId };

            //Add and save in DB
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync(cancellationToken);

            //Retrun Id
            return comment.Id;
        }
    }
}
