using Application.ProductImageService.Queries;
using Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.ProductImages.Queries
{
    internal class GetAProductImageByNameQueriesHandler : IRequestHandler<GetAProductImageByNameQuery, ProductImage>
    {
        private readonly DataBaseContext _context;
        public GetAProductImageByNameQueriesHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<ProductImage> Handle(GetAProductImageByNameQuery request, CancellationToken cancellationToken)
        {
            var image = await _context.ProductImages.FirstOrDefaultAsync(p => p.Name.Equals(request.Name));

            return image;
        }
    }
}
