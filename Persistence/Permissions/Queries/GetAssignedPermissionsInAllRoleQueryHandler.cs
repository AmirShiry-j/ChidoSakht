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

namespace Persistence.Permissions.Queries
{
    public class GetAssignedPermissionsInAllRoleQueryHandler : IRequestHandler<GetAssignedPermissionsInAllRoleQuery, List<RoleDto>>
    {
        private readonly DataBaseContext _context;
        public GetAssignedPermissionsInAllRoleQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<List<RoleDto>> Handle(GetAssignedPermissionsInAllRoleQuery request, CancellationToken cancellationToken)
        {
            //get roles and their permissions
            var roles = await _context.Roles
                .Select(x => new RoleDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToListAsync(cancellationToken);

            //get rolePermissions  many to many table
            var rolePermissions = await _context.RolePermissions.ToListAsync(cancellationToken);

            //get permissions
            var permissions = await _context.Permissions.Where(p => rolePermissions.Select(s => s.PermissionId).Contains(p.Id))
                .Select(p => new PermissionDto
                {
                    Id = p.Id,
                    Action = p.Action,
                    Area = p.Area,
                    Controller = p.Controller,
                    Description = p.Description
                }).ToListAsync(cancellationToken);

            //assign permissions to roles
            foreach (var role in roles)
            {
                var permissionsIds = rolePermissions.Where(p => p.RoleId.Equals(role.Id)).Select(p => p.PermissionId).ToList();
                var permissionsDto = permissions.Where(p => permissionsIds.Contains(p.Id)).Select(p => new PermissionDto
                {
                    Id = p.Id,
                    Action = p.Action,
                    Area = p.Area,
                    Controller = p.Controller,
                    Description = p.Description
                }).ToList();

                role.Permissions = permissionsDto;
            }

            //return
            return roles;
        }
    }
}
