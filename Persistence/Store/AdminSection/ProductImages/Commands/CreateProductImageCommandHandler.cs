using Application.Store.AdminSection.ProductImageService.Commands;
using Domain.Products;
using MediatR;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.AdminSection.ProductImages.Commands
{
    public class CreateProductImageCommandHandler : IRequestHandler<CreateProductImageCommand, int>
    {
        private readonly DataBaseContext _context;

        public CreateProductImageCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(CreateProductImageCommand request, CancellationToken cancellationToken)
        {
            //Define
            var productImage = new ProductImage()
            {
                Name = request.Name,
                ProductId = request.ProductId,
            };

            //Add and save in DB
            await _context.ProductImages.AddAsync(productImage);
            await _context.SaveChangesAsync(cancellationToken);

            //Retrun Id
            return productImage.Id;
        }
    }
}
