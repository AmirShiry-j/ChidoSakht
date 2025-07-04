using Domain.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.RoleService.Queries
{
    public class GetRoleByIdQuery : IRequest<Role>
    {
        public string RoleId { get; set; }

        public GetRoleByIdQuery(string roleId)
        {
            RoleId = roleId;
        }
    }
}
