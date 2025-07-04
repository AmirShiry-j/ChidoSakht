using Application.Store.AdminSection.ProductAttribute.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ProductAttribute.Queries
{
    public class GetProductAttributesByProductIdQueryHandler : IRequestHandler<GetProductAttributesByProductIdQuery, List<ProductAttributeDto>>
    {
        private readonly DataBaseContext _context;
        public GetProductAttributesByProductIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<List<ProductAttributeDto>> Handle(GetProductAttributesByProductIdQuery request, CancellationToken cancellationToken)
        {
            //get from db
            var productAttributes = await _context.ProductAttributes.Where(p => p.ProductId.Equals(request.ProductId))
                .Select(p => new ProductAttributeDto
                {
                    AttributeType = p.AttributeType,
                    Name = p.Name,
                    ProductAttributeId = p.Id,
                    UseForVariant=p.UseForVariant
                }).ToListAsync();

            //Retrun It
            return productAttributes;
        }
    }
}
