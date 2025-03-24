using Application.TokenService.Commands;
using Domain.Categories;
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
    public class DeleteTokenByIdCommandHandler : IRequestHandler<DeleteTokenByIdCommand>
    {
        private readonly DataBaseContext _context;
        public DeleteTokenByIdCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task Handle(DeleteTokenByIdCommand request, CancellationToken cancellationToken)
        {
            //Get from DB
            var token = await _context.Tokens.FindAsync(request.Id);

            //Delete in db
            _context.Tokens.Remove(token);
            await _context.SaveChangesAsync(cancellationToken);

            //finish
            await Task.CompletedTask;
        }
    }
}
