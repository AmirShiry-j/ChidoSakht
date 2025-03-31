using Application.RoleService.Queries;
using Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Roles.Queries
{
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, Role>
    {
        private readonly DataBaseContext _context;
        public GetRoleByIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<Role> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Roles.FirstOrDefaultAsync(p => p.Id.Equals(request.RoleId));
        }
    }
}
