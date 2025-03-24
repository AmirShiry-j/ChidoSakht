using MediatR;

namespace Application.TokenService.Commands
{
    public class DeleteOverflowUserTokensCommand : IRequest
    {
        public List<long> Ids { get; set; }
        public DeleteOverflowUserTokensCommand(List<long> Ids)
        {
            this.Ids = Ids;
        }
    }
}
