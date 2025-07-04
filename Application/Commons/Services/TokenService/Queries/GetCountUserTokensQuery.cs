using MediatR;

namespace Application.Commons.Services.TokenService.Queries
{
    public class GetCountUserTokensQuery : IRequest<int>
    {
        public string UserId { get; set; }
        public GetCountUserTokensQuery(string UserId)
        {
            this.UserId = UserId;
        }
    }
}
