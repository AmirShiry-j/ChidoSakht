using Application.TokenService.Queries;
using Domain.Users;
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
    public class GetTokenByHashRefreshTokenQueryHandler : IRequestHandler<GetTokenByHashRefreshTokenQuery, Token>
    {
        private readonly DataBaseContext _context;
        public GetTokenByHashRefreshTokenQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<Token> Handle(GetTokenByHashRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            //get token
            var token = await _context.Tokens.Where(p => p.RefreshTokenHash.Equals(request.HashRefreshToken))
                .Include(p => p.User)
                .FirstOrDefaultAsync();

            return token;
        }
    }
}
