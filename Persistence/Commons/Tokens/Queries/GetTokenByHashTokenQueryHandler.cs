using Application.Commons.Services.TokenService.Queries;
using Domain.Users;
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
    public class GetTokenByHashTokenQueryHandler : IRequestHandler<GetTokenByHashTokenQuery, Token>
    {
        private readonly DataBaseContext _context;
        public GetTokenByHashTokenQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<Token> Handle(GetTokenByHashTokenQuery request, CancellationToken cancellationToken)
        {
            //get token
            var token = await _context.Tokens.Where(p => p.TokenHash.Equals(request.HashToken)).FirstOrDefaultAsync();

            return token;
        }
    }
}
