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
    public class GetExitingPermissionsByIdsQueryHandler : IRequestHandler<GetExitingPermissionsByIdsQuery, List<Permission>>
    {
        private readonly DataBaseContext _context;
        public GetExitingPermissionsByIdsQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<List<Permission>> Handle(GetExitingPermissionsByIdsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Permissions
                .Where(p => request.PermissionsIds.Contains(p.Id))
                .ToListAsync(cancellationToken);
        }
    }
}
