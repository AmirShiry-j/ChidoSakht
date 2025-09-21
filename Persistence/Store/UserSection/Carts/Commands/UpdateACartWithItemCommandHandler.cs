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
    public class UpdateACartWithItemCommandHandler : IRequestHandler<UpdateACartWithItemCommand>
    {
        private readonly DataBaseContext _context;
        public UpdateACartWithItemCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task Handle(UpdateACartWithItemCommand request, CancellationToken cancellationToken)
        {
            _context.Update(request.Cart);
            await _context.SaveChangesAsync();

            await Task.CompletedTask;
        }
    }
}
