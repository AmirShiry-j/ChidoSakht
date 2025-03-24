using Domain.Users;
using MediatR;

namespace Application.TokenService.Queries
{
    public class GetTokenByHashTokenQuery : IRequest<Token>
    {
        public string HashToken { get; set; }
        public GetTokenByHashTokenQuery(string HashToken)
        {
            this.HashToken = HashToken;
        }
    }
}
