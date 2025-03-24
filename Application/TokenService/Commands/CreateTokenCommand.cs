using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TokenService.Commands
{
    public class CreateTokenCommand : IRequest<long>
    {
        public string UserId { get; set; }
        public DateTime ExpireTime { get; set; }
        public string TokenHash { get; set; }
        public DateTime RefreshExpireTime { get; set; }
        public string RefreshTokenHash { get; set; }
        public DateTime CreateTime { get; set; }
        public CreateTokenCommand(string UserId, DateTime ExpireTime, string TokenHash, DateTime RefreshExpireTime, string RefreshTokenHash, DateTime CreateTime)
        {
            this.UserId = UserId;
            this.ExpireTime = ExpireTime;
            this.TokenHash = TokenHash;
            this.RefreshExpireTime = RefreshExpireTime;
            this.RefreshTokenHash = RefreshTokenHash;
            this.CreateTime = CreateTime;
        }
    }
}
