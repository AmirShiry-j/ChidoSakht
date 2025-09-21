using Application.Store.AdminSection.ProductService.Queries;
using Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.AdminSection.Products.Queries
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        private readonly DataBaseContext _context;
        public GetProductByIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            //get from db
            var product = await _context.Products.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id.Equals(request.Id));

            //Retrun It
            return product;
        }
    }
}