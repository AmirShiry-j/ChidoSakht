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
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly DataBaseContext _context;
        public UpdateOrderCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            _context.Update(request.Order);
            await _context.SaveChangesAsync();

            await Task.CompletedTask;
        }
    }
}
