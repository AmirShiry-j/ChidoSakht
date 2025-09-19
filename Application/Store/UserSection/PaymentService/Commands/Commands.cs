using Domain.Orders;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.PaymentService.Commands
{
    public class CreatePaymentCommand : IRequest<long>
    {
        public Payment Payment { get; set; }
        public CreatePaymentCommand(Payment Payment)
        {
            this.Payment = Payment;
        }
    }
}
