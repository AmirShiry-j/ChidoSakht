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
    public class UpdateVoteOnCommentCommandHandler : IRequestHandler<UpdateVoteOnCommentCommand>
    {
        private readonly DataBaseContext _context;
        public UpdateVoteOnCommentCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task Handle(UpdateVoteOnCommentCommand request, CancellationToken cancellationToken)
        {
            _context.Update(request.Helpful);
            _context.SaveChanges();

            await Task.CompletedTask;
        }
    }
}
