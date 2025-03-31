using Application.PermissionService.Queries;
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
    public class GetPermissionsQueryHandler : IRequestHandler<GetPermissionsQuery, List<PermissionDto>>
    {
        private readonly DataBaseContext _context;

        public GetPermissionsQueryHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<List<PermissionDto>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Permissions
                .Select(p => new PermissionDto
                {
                    Id = p.Id,
                    Area = p.Area,
                    Controller = p.Controller,
                    Action = p.Action,
                    Description = p.Description
                })
                .ToListAsync();
        }
    }
}
