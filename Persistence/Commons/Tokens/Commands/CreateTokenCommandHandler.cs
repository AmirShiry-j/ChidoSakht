using Application.Commons.Services.TokenService.Commands;
using Domain.Categories;
using Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Commons.Tokens.Commands
{
    public class CreateTokenCommandHandler : IRequestHandler<CreateTokenCommand, long>
    {

        private readonly DataBaseContext _context;

        public CreateTokenCommandHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<long> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
        {
            //Define
            var newToken = new Token()
            {
                UserId = request.UserId,
                TokenHash = request.TokenHash,
                TokenExpireTime = request.ExpireTime,
                RefreshTokenHash = request.RefreshTokenHash,
                RefreshTokenExpireTime = request.RefreshExpireTime,
                CreateTime = request.CreateTime
            };

            //Add and save in DB
            await _context.Tokens.AddAsync(newToken);
            await _context.SaveChangesAsync(cancellationToken);

            //Retrun Id
            return newToken.Id;
        }
    }
}
