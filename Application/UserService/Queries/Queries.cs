using Domain.Categories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserService.Queries
{
    public class GetAllUsersQuery : IRequest<List<UserDto>>
    {

        public GetAllUsersQuery()
        {
        }
    }
}
