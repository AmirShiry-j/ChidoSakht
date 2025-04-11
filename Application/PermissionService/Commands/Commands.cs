using Domain.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PermissionService.Commands
{
    public class AssignPermissionsCommand : IRequest
    {
        public string RoleId { get; set; }
        public int[] PermissionIds { get; set; }

        public AssignPermissionsCommand(string roleId, int[] permissionIds)
        {
            RoleId = roleId;
            PermissionIds = permissionIds;
        }
    }
    public class UnAssignPermissionsCommand : IRequest
    {
        public List<RolePermission> RolePermissions { get; set; }

        public UnAssignPermissionsCommand(List<RolePermission> rolePermissions)
        {
            RolePermissions = rolePermissions;
        }
    }
}
