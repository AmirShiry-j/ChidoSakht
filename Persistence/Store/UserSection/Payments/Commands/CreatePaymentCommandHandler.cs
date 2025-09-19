using Application.Store.UserSection.PaymentService.Commands;
using MediatR;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.UserSection.Payments.Commands
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, long>
    {
        private readonly DataBaseContext _context;
        public CreatePaymentCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<long> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            await _context.Payments.AddAsync(request.Payment);
            await _context.SaveChangesAsync(cancellationToken);

            return request.Payment.Id;
        }
    }
}
