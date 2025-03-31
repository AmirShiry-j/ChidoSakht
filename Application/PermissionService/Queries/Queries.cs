using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PermissionService.Queries
{
    public class GetPermissionsQuery : IRequest<List<PermissionDto>> { }

    public class CheckRoleHasPermissionQuery : IRequest<bool>
    {
        public string RoleId { get; set; }
        public int PermissionId { get; set; }

        public CheckRoleHasPermissionQuery(string roleId, int permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }
    }
}
