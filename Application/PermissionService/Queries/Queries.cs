using Domain.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PermissionService.Queries
{
    public class GetPermissionsQuery : IRequest<List<PermissionDto>>
    {
        public PermissionFilterDto FilterDto { get; set; }
        public GetPermissionsQuery(PermissionFilterDto filterDto)
        {
            FilterDto = filterDto;
        }
    }
    public class GetRolePermissionQuery : IRequest<RolePermission>
    {
        public string RoleId { get; set; }
        public int PermissionId { get; set; }

        public GetRolePermissionQuery(string roleId, int permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }
    }

    public class GetAssignedPermissionIdsQuery : IRequest<List<RolePermission>>
    {
        public string RoleId { get; set; }
        public int[] PermissionsIds { get; set; }

        public GetAssignedPermissionIdsQuery(string roleId, int[] permissionsIds)
        {
            RoleId = roleId;
            PermissionsIds = permissionsIds;
        }
    }
    public class GetExitingPermissionsByIdsQuery : IRequest<List<Permission>>
    {
        public int[] PermissionsIds { get; set; }

        public GetExitingPermissionsByIdsQuery(int[] permissionsIds)
        {
            PermissionsIds = permissionsIds;
        }
    }
}
