using Application.Store.UserSection.CartService.Commands;
using MediatR;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.UserSection.Carts.Commands
{
    public class DeleteCartItemCommandHandler : IRequestHandler<DeleteCartItemCommand>
    {
        private readonly DataBaseContext _context;
        public DeleteCartItemCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task Handle(DeleteCartItemCommand request, CancellationToken cancellationToken)
        {
            //Delete and save in DB
            _context.CartItems.Remove(request.CartItem);
            await _context.SaveChangesAsync(cancellationToken);

            //finish
            await Task.CompletedTask;
        }
    }
}
