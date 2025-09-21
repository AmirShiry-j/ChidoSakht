using Application.Store.AdminSection.CommentService.Commands;
using MediatR;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.AdminSection.Comments.Commands
{
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand>
    {
        private readonly DataBaseContext _context;
        public UpdateCommentCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            _context.Update(request.Comment);
            await _context.SaveChangesAsync();

            await Task.CompletedTask;
        }
    }
}
