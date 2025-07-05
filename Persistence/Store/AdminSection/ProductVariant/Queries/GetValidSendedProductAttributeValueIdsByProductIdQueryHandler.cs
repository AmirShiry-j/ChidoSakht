using Application.Store.AdminSection.ProductVariant.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.AdminSection.ProductVariant.Queries
{
    public class GetValidSendedProductAttributeValueIdsByProductIdQueryHandler : IRequestHandler<GetValidSendedProductAttributeValueIdsByProductIdQuery, List<int>>
    {
        private readonly DataBaseContext _context;
        public GetValidSendedProductAttributeValueIdsByProductIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<List<int>> Handle(GetValidSendedProductAttributeValueIdsByProductIdQuery request, CancellationToken cancellationToken)
        {
            var ids = await _context.ProductAttributeValues.Include(p => p.ProductAttribute)
                            .Where(p => request.ProductAttributeValueIds.Contains(p.Id) &&
                            p.ProductAttribute.ProductId.Equals(request.ProductId) &&
                            p.ProductAttribute.UseForVariant
                            ).Select(p => p.Id)
                            .ToListAsync();

            return ids;
        }
    }
}
