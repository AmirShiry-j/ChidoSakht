using Application.Commons.Services.TokenService.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Commons.Tokens.Queries
{
    public class GetCountUserTokensQueryHandler : IRequestHandler<GetCountUserTokensQuery, int>
    {
        private readonly DataBaseContext _context;
        public GetCountUserTokensQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(GetCountUserTokensQuery request, CancellationToken cancellationToken)
        {
            //get count
            var count = await _context.Tokens.Where(p => p.UserId.Equals(request.UserId)).CountAsync();

            return count;
        }
    }
}
