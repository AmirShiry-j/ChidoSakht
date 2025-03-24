using Application.CategoryService.Commands;
using Application.Interfaces.ConfigService;
using Application.Interfaces.Contexts;
using Application.Interfaces.Localization;
using Application.TokenService.Commands;
using Application.TokenService.Queries;
using Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TokenService
{
    public interface IUserTokenService
    {
        Task<Token> GetToken(string TokenHash);
        Task DeleteToken(Token token);
        Task DeleteToken(string HashToken);
        Task SaveToken(CreateUserTokenDto userToken);
        Task<Token> FindTokenByRefreshToken(string HashRefreshToken);
    }

    public class UserTokenService : IUserTokenService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        private readonly IConfigService _configService;
        public UserTokenService(IMediator mediator, ILocalizationService localizationService, IConfigService configService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
            _configService = configService;
        }

        public async Task DeleteToken(Token token)
        {
            //Delete it
            var deleteTokenByIdCommand = new
                DeleteTokenByIdCommand(token.Id);
            await _mediator.Send(deleteTokenByIdCommand);
        }

        public async Task<Token> FindTokenByRefreshToken(string HashRefreshToken)
        {
            //Find it
            var getTokenByHashRefreshTokenQuery = new
                GetTokenByHashRefreshTokenQuery(HashRefreshToken);
            var token = await _mediator.Send(getTokenByHashRefreshTokenQuery);

            return token;
        }
        public async Task DeleteToken(string HashToken)
        {
            //Find it
            var getTokenByHashTokenQuery = new
                GetTokenByHashTokenQuery(HashToken);
            var token = await _mediator.Send(getTokenByHashTokenQuery);

            //Delete it and save
            if (token != null)
            {
                //Delete it
                var deleteTokenByIdCommand = new
                    DeleteTokenByIdCommand(token.Id);
                await _mediator.Send(deleteTokenByIdCommand);
            }
        }

        public async Task<Token> GetToken(string TokenHash)
        {
            //Find it
            var getTokenByHashTokenQuery = new
                GetTokenByHashTokenQuery(TokenHash);
            var token = await _mediator.Send(getTokenByHashTokenQuery);
            return token;
        }

        public async Task SaveToken(CreateUserTokenDto userToken)
        {
            //Check count tokens of user
            int maxCountToken = _configService.Config.MainJwtAuthenticationSetting.MaxCountUserTokensAtMoment;

            //Get count Tokens of user in db
            var queryGetCountTokensOfUser = new
                GetCountUserTokensQuery(userToken.UserId);
            var countTokensUser = await _mediator.Send(queryGetCountTokensOfUser);

            if (countTokensUser >= maxCountToken)
            {
                //Get count overflow
                int countOverflow = (countTokensUser - maxCountToken) + 1;//+ 1 for space of new token

                //Get Ids of overflow UserTokens
                var queryGetOverflowUserTokenIds = new
                    GetOverflowUserTokenIdsQuery(userToken.UserId, countOverflow);
                var Ids = await _mediator.Send(queryGetOverflowUserTokenIds);

                //Delete them
                var commandDeleteOverflowUserTokens = new
                    DeleteOverflowUserTokensCommand(Ids);
                await _mediator.Send(commandDeleteOverflowUserTokens);
            }


            //Create Token
            var commandCreate = new
                CreateTokenCommand(userToken.UserId, userToken.ExpireTime, userToken.TokenHash, userToken.RefreshExpireTime, userToken.RefreshTokenHash, DateTime.Now);
            await _mediator.Send(commandCreate);


            await Task.CompletedTask;
        }
    }
    public class CreateUserTokenDto
    {
        public string UserId { get; set; }
        public DateTime ExpireTime { get; set; }
        public string TokenHash { get; set; }
        public DateTime RefreshExpireTime { get; set; }
        public string RefreshTokenHash { get; set; }
    }
}
