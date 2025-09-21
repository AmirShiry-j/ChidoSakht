using MediatR;

namespace Application.Commons.Services.TokenService.Commands
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
