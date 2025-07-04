using MediatR;

namespace Application.Commons.Services.TokenService.Commands
{
    public class DeleteTokenByIdCommand : IRequest
    {
        public long Id { get; set; }
        public DeleteTokenByIdCommand(long Id)
        {
            this.Id = Id;
        }
    }
}
