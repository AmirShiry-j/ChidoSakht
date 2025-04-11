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

namespace Persistence.Permissions.Queries
{
    public class GetAssignedPermissionsInARoleQueryHandler : IRequestHandler<GetAssignedPermissionsInARoleQuery, List<PermissionDto>>
    {
        private readonly DataBaseContext _context;
        public GetAssignedPermissionsInARoleQueryHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<List<PermissionDto>> Handle(GetAssignedPermissionsInARoleQuery request, CancellationToken cancellationToken)
        {
            return await _context.RolePermissions.Where(p => p.RoleId.Equals(request.RoleId))
                .Include(p => p.Permission)
                .Select(p => new PermissionDto
                {
                    Id = p.PermissionId,
                    Area = p.Permission.Area,
                    Controller = p.Permission.Controller,
                    Action = p.Permission.Action,
                    Description = p.Permission.Description
                }).ToListAsync();
        }
    }
}
