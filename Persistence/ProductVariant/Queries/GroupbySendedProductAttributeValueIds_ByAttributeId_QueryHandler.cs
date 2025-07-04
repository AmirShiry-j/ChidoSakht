using Application.Store.AdminSection.ProductVariant.Commands;
using Application.Store.AdminSection.ProductVariant.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ProductVariant.Queries
{
    public class GroupbySendedProductAttributeValueIds_ByAttributeId_QueryHandler : IRequestHandler<GroupbySendedProductAttributeValueIds_ByAttributeId_Query, List<GroupBy_Values_By_AttributeId_Dto>>
    {
        private readonly DataBaseContext _context;
        public GroupbySendedProductAttributeValueIds_ByAttributeId_QueryHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<List<GroupBy_Values_By_AttributeId_Dto>> Handle(GroupbySendedProductAttributeValueIds_ByAttributeId_Query request, CancellationToken cancellationToken)
        {
            var values = await _context.ProductAttributeValues.Where(p => request.ProductAttributeValueIds.Contains(p.Id)).ToListAsync();

            var results = values.GroupBy(
    p => p.ProductAttributeId,
    (key, g) => new GroupBy_Values_By_AttributeId_Dto { ProductAttributeId = key, ValueIds = g.Select(p => p.Id).ToList() })
                .ToList();

            return results;
        }
    }

}
