using Domain.Users;
using MediatR;

namespace Application.TokenService.Queries
{
    public class GetTokenByHashRefreshTokenQuery : IRequest<Token>
    {
        public string HashRefreshToken { get; set; }
        public GetTokenByHashRefreshTokenQuery(string HashRefreshToken)
        {
            this.HashRefreshToken = HashRefreshToken;
        }
    }
}
