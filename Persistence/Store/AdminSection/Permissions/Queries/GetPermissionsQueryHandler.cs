using Application.Commons.Objects.Dtoes;
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
    public class GetPermissionsQueryHandler : IRequestHandler<GetPermissionsQuery, List<PermissionDto>>
    {
        private readonly DataBaseContext _context;

        public GetPermissionsQueryHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<List<PermissionDto>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
        {
            var prPermi = PredicateBuilder.True<Permission>();
            var FilterDto = request.FilterDto;

            //Build Predicate
            //Filter Area
            if (string.IsNullOrWhiteSpace(FilterDto.Area) == false)
            {
                prPermi = prPermi.And(x => x.Area.Equals(FilterDto.Area));
            }

            //Filter Controller
            if (string.IsNullOrWhiteSpace(FilterDto.Controller) == false)
            {
                prPermi = prPermi.And(x => x.Controller.Equals(FilterDto.Controller));
            }


            //Filter Action
            if (string.IsNullOrWhiteSpace(FilterDto.Action) == false)
            {
                prPermi = prPermi.And(x => x.Action.Equals(FilterDto.Action));
            }

            return await _context.Permissions.IgnoreQueryFilters()
                .Where(prPermi)
                .OrderBy(p => p.Id)
                //.OrderBy(p => p.Area)
                //.ThenBy(p => p.Controller)
                //.ThenBy(p => p.Action)
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
