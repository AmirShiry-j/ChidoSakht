using Application.Store.UserSection.CommentService.Commands;
using MediatR;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.UserSection.Comments.Commands
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand>
    {
        private readonly DataBaseContext _context;
        public DeleteCommentCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            _context.Comments.Remove(request.Comment);
            _context.SaveChanges();

            await Task.CompletedTask;
        }
    }
}
