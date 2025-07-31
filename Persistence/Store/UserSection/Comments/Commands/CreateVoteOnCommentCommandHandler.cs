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
    public class CreateVoteOnCommentCommandHandler : IRequestHandler<CreateVoteOnCommentCommand>
    {
        private readonly DataBaseContext _context;
        public CreateVoteOnCommentCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task Handle(CreateVoteOnCommentCommand request, CancellationToken cancellationToken)
        {
            var helpful = new Helpful
            {
                CommentId = request.CommentId,
                UserId = request.UserId,
                WasHelpful = request.WasHelpful
            };

            _context.Helpfuls.Add(helpful);
            await _context.SaveChangesAsync(cancellationToken);

            await Task.CompletedTask;
        }
    }
}
