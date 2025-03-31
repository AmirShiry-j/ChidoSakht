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
    public class GetPermissionByIdQueryHandler : IRequestHandler<GetPermissionByIdQuery, Permission>
    {
        private readonly DataBaseContext _context;
        public GetPermissionByIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<Permission> Handle(GetPermissionByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Permissions.FirstOrDefaultAsync(p => p.Id.Equals(request.PermissionId));
        }
    }
}
