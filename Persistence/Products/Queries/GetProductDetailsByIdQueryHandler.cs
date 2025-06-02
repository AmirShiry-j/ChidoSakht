using Application.ProductService.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Products.Queries
{
    public class GetProductDetailsByIdQueryHandler : IRequestHandler<GetProductDetailsByIdQuery, ProductDetailsDto>
    {
        private readonly DataBaseContext _context;
        public GetProductDetailsByIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<ProductDetailsDto> Handle(GetProductDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            //get from db
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id.Equals(request.Id));

            if (product == null)
                return null;

            var model = new ProductDetailsDto
            {
                Id = product.Id,
                Name = product.Name,
                CreateTime = product.CreateTime,
                Description = product.Description,
                LastUpdateTime = product.LastUpdateTime,
                UniqeLink = product.UniqeLink,
                ImageAltText = product.ImageAltText,
                NameIndexImage = product.NameIndexImage,
                UniCode = product.UniCode,
                CategoryId = product?.CategoryId,
                CategoryName = product.Category?.Name
            };

            //Retrun It
            return model;
        }
    }
}