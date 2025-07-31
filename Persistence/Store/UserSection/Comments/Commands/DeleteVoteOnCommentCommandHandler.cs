using Application.Store.UserSection.CommentService.Commands;
using MediatR;
using Persistence.Contexts;

namespace Persistence.Store.UserSection.Comments.Commands
{
    public class DeleteVoteOnCommentCommandHandler : IRequestHandler<DeleteVoteOnCommentCommand>
    {
        private readonly DataBaseContext _context;
        public DeleteVoteOnCommentCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task Handle(DeleteVoteOnCommentCommand request, CancellationToken cancellationToken)
        {
            _context.Helpfuls.Remove(request.Helpful);
            _context.SaveChanges();

            await Task.CompletedTask;
        }
    }
}
