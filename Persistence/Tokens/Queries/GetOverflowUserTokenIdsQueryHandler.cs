using Application.TokenService.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Tokens.Queries
{
    public class GetOverflowUserTokenIdsQueryHandler : IRequestHandler<GetOverflowUserTokenIdsQuery, List<long>>
    {
        private readonly DataBaseContext _context;
        public GetOverflowUserTokenIdsQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<List<long>> Handle(GetOverflowUserTokenIdsQuery request, CancellationToken cancellationToken)
        {
            //Get token overflow
            var Ids = await _context.Tokens.Where(p => p.UserId == request.UserId)
                                          .OrderBy(p => p.CreateTime)
                                          .Take(request.CountOverflow)
                                          .Select(p => p.Id)
                                          .ToListAsync();

            return Ids;
        }
    }
}
