using Application.PermissionService.Queries;
using Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Permissions.Commands
{
    public class GetRolePermissionQueryHandler : IRequestHandler<GetRolePermissionQuery, RolePermission>
    {
        private readonly DataBaseContext _context;
        public GetRolePermissionQueryHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<RolePermission> Handle(GetRolePermissionQuery request, CancellationToken cancellationToken)
        {
            //Get record from db
            return await _context.RolePermissions
                .FirstOrDefaultAsync(p => p.PermissionId.Equals(request.PermissionId) && p.RoleId.Equals(request.RoleId), cancellationToken);
        }
    }
}
