using Application.Store.AdminSection.PermissionService.Queries;
using Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.AdminSection.Permissions.Queries
{
    public class GetAssignedPermissionIdsQueryHandler : IRequestHandler<GetAssignedPermissionIdsQuery, List<RolePermission>>
    {
        private readonly DataBaseContext _context;
        public GetAssignedPermissionIdsQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<List<RolePermission>> Handle(GetAssignedPermissionIdsQuery request, CancellationToken cancellationToken)
        {
            //check has any record
            return await _context.RolePermissions.IgnoreQueryFilters()
                .Where(p => p.RoleId.Equals(request.RoleId))
                .ToListAsync(cancellationToken);
        }
    }
}
