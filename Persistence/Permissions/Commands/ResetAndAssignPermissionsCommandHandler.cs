using Application.Store.AdminSection.PermissionService.Commands;
using Domain.Users;
using MediatR;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Permissions.Commands
{
    public class ResetAndAssignPermissionsCommandHandler : IRequestHandler<ResetAndAssignPermissionsCommand>
    {
        private readonly DataBaseContext _context;
        public ResetAndAssignPermissionsCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task Handle(ResetAndAssignPermissionsCommand request, CancellationToken cancellationToken)
        {
            //delete old Records
            _context.RolePermissions.RemoveRange(request.BeforeRolePermissions);

            //Map and insert
            var rolePermissions = request.PermissionIds.Select(p => new
            RolePermission
            {
                RoleId = request.RoleId,
                PermissionId = p
            })
                .ToList();

            await _context.RolePermissions.AddRangeAsync(rolePermissions);
            await _context.SaveChangesAsync();
        }
    }
}
