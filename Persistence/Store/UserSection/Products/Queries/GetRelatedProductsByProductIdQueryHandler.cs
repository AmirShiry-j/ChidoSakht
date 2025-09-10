using Application.Store.UserSection.ProductService.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.UserSection.Products.Queries
{
    public class GetRelatedProductsByProductIdQueryHandler : IRequestHandler<GetRelatedProductsByProductIdQuery, List<ProductDto>>
    {
        private readonly DataBaseContext _context;
        public GetRelatedProductsByProductIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<List<ProductDto>> Handle(GetRelatedProductsByProductIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Set<Domain.Products.RelatedProduct>()
    .Where(r => r.ProductId == request.ProductId)
    .Include(r => r.RelatedTo)
    .ThenInclude(r => r.ProductVariants)
    .Select(r => new ProductDto
    {
        Id = r.RelatedTo.Id,
        Name = r.RelatedTo.Name,
        ProductType = r.RelatedTo.ProductType,
        Price = r.RelatedTo.ProductVariants.First().Price,
        SpecialPrice = r.RelatedTo.ProductVariants.First().SpecialPrice,
        ImageAltText = r.RelatedTo.ImageAltText,
        NameIndexImage = r.RelatedTo.NameIndexImage,
        UniqeLink = r.RelatedTo.UniqeLink,
    })
    .ToListAsync(cancellationToken);
        }
    }
}
