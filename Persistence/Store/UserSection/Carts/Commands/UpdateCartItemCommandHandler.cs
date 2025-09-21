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
    public class UpdateCartItemCommandHandler : IRequestHandler<UpdateCartItemCommand>
    {
        private readonly DataBaseContext _context;
        public UpdateCartItemCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
        {
            _context.Update(request.CartItem);
            await _context.SaveChangesAsync();

            await Task.CompletedTask;
        }
    }
}
