using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PermissionService.Commands
{
    public class AssignPermissionCommand : IRequest
    {
        public string RoleId { get; set; }
        public int PermissionId { get; set; }

        public AssignPermissionCommand(string roleId, int permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }
    }
}
