using Domain.Categories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons.Services.UserService.Queries
{
    public class GetAllUsersQuery : IRequest<List<UserDto>>
    {

        public GetAllUsersQuery()
        {
        }
    }
    public class GetUserProfileQuery : IRequest<ProfileDto>
    {
        public string UserId { get; set; }
        public GetUserProfileQuery(string userId)
        {
            UserId = userId;
        }
    }
}
