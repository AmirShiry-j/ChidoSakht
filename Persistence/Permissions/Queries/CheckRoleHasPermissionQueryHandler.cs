using Application.PermissionService.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Permissions.Queries
{
    public class CheckRoleHasPermissionQueryHandler : IRequestHandler<CheckRoleHasPermissionQuery, bool>
    {
        private readonly DataBaseContext _context;
        public CheckRoleHasPermissionQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(CheckRoleHasPermissionQuery request, CancellationToken cancellationToken)
        {
            //check has any record
            return await _context.RolePermissions
                .AnyAsync(p => p.PermissionId.Equals(request.PermissionId) && p.RoleId.Equals(request.RoleId));
        }
    }
}
