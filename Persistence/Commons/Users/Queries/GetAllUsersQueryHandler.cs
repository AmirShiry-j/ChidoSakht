using Application.Commons.Services.UserService.Queries;
using MediatR;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Commons.Users.Queries
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
    {
        private readonly DataBaseContext _context;
        public GetAllUsersQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            //get from db
            var users = _context.Users.Select(p => new UserDto
            {
                Id = p.Id,
                PhoneNumber = p.PhoneNumber,
                FullName = p.FullName,
                Email = p.Email,
            }).ToList();

            return users;
        }
    }
}
