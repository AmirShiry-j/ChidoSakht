using Application.TokenService.Commands;
using Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Tokens.Commands
{
    public class DeleteOverflowUserTokensCommandHandler : IRequestHandler<DeleteOverflowUserTokensCommand>
    {
        private readonly DataBaseContext _context;
        public DeleteOverflowUserTokensCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task Handle(DeleteOverflowUserTokensCommand request, CancellationToken cancellationToken)
        {
            //Delete and save in DB
            _context.Tokens.RemoveRange(request.Ids.Select(p => new Token() { Id = p }).ToList());
            await _context.SaveChangesAsync(cancellationToken);

            //finish
            await Task.CompletedTask;
        }
    }
}
