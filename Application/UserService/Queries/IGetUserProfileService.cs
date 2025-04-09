using Application.CategoryService.Queries;
using Application.Common.Dtoes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserService.Queries
{
    public interface IGetUserProfileService
    {
        Task<ResultDto<ProfileDto>> Execute(string UserId);
    }
    public class GetUserProfileService : IGetUserProfileService
    {
        private readonly IMediator _mediator;
        public GetUserProfileService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<ResultDto<ProfileDto>> Execute(string UserId)
        {
            //Get Profile from db
            var profileDto = await _mediator.Send(new GetUserProfileQuery(UserId));

            //Check exist and return
            if (profileDto is not null)
            {
                return new ResultDto<ProfileDto>
                {
                    IsSuccess = true,
                    Data = profileDto,
                };
            }
            else
            {
                return new ResultDto<ProfileDto>
                {
                    IsSuccess = false,
                    Data = null,
                };
            }
        }
    }

    public class ProfileDto
    {
        public string Id { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string? Email { get; set; }
    }
}
