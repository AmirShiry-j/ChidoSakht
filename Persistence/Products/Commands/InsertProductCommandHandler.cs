using Application.CategoryService.Commands;
using Application.ProductService.Commands;
using Domain.Categories;
using Domain.Products;
using MediatR;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Products.Commands
{
    public class InsertProductCommandHandler : IRequestHandler<InsertProductCommand, int?>
    {
        private readonly DataBaseContext _context;

        public InsertProductCommandHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<int?> Handle(InsertProductCommand request, CancellationToken cancellationToken)
        {
            //Define
            var product = new Product(request.Name);

            //Add and save in DB
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync(cancellationToken);

            //Retrun Id
            return product.Id;
        }
    }
}
