using Application.Store.UserSection.AddressService.Commands;
using MediatR;
using Persistence.Contexts;

namespace Persistence.Store.UserSection.Addresses.Commands
{
    public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand>
    {
        private readonly DataBaseContext _context;
        public DeleteAddressCommandHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            _context.Addresses.Remove(request.Address);
            _context.SaveChanges();

            await Task.CompletedTask;
        }
    }
}
