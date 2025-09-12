using Application.Store.UserSection.AddressService.Commands;
using MediatR;
using Persistence.Contexts;

namespace Persistence.Store.UserSection.Addresses.Commands
{
    public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand>
    {
        private readonly DataBaseContext _context;
        public UpdateAddressCommandHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            _context.Update(request.Address);
            _context.SaveChanges();

            await Task.CompletedTask;
        }
    }
}
