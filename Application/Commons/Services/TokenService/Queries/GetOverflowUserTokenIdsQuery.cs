using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons.Services.TokenService.Queries
{
    public class GetOverflowUserTokenIdsQuery : IRequest<List<long>>//Token Ids
    {
        public string UserId { get; set; }
        public int CountOverflow { get; set; }
        public GetOverflowUserTokenIdsQuery(string UserId, int CountOverflow)
        {
            this.UserId = UserId;
            this.CountOverflow = CountOverflow;
        }
    }
}
