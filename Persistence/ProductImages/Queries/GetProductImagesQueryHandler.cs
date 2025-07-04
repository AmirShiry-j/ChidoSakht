using Application.Store.AdminSection.ProductImageService.Queries;
using MediatR;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ProductImages.Queries
{
    public class GetProductImagesQueriesHandler : IRequestHandler<GetProductImagesQueries, List<ProductImageDto>>
    {
        private readonly DataBaseContext _context;
        public GetProductImagesQueriesHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<List<ProductImageDto>> Handle(GetProductImagesQueries request, CancellationToken cancellationToken)
        {
            var images = _context.ProductImages.Where(p => p.ProductId.Equals(request.ProductId)).Select(p =>
                new ProductImageDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    IsIndex = p.IsIndex
                }).ToList();

            return images;
        }
    }
}
