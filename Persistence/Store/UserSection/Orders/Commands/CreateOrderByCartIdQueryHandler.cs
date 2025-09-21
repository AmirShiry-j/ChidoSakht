using Application.Store.UserSection.OrderService.Commands;
using MediatR;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.UserSection.Orders.Commands
{
    public class CreateOrderByCartIdCommandHandler : IRequestHandler<CreateOrderByCartIdCommand, long>
    {
        private readonly DataBaseContext _context;
        public CreateOrderByCartIdCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<long> Handle(CreateOrderByCartIdCommand request, CancellationToken cancellationToken)
        {
            await _context.Orders.AddAsync(request.Order);
            await _context.SaveChangesAsync(cancellationToken);

            return request.Order.Id;
        }
    }
}
