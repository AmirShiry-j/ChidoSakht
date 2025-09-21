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
    public class CreateACartWithItemCommandHandler : IRequestHandler<CreateACartWithItemCommand, long>
    {
        private readonly DataBaseContext _context;
        public CreateACartWithItemCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<long> Handle(CreateACartWithItemCommand request, CancellationToken cancellationToken)
        {
            _context.Carts.Add(request.Cart);
            await _context.SaveChangesAsync();

            return request.Cart.Id;
        }
    }
}
