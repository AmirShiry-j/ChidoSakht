using Application.Store.UserSection.ProductService.Queries;
using Domain.Products;
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
    public class GetProductByIdInUserSectionQueryHandler : IRequestHandler<GetProductByIdInUserSectionQuery, Product>
    {
        private readonly DataBaseContext _context;
        public GetProductByIdInUserSectionQueryHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<Product> Handle(GetProductByIdInUserSectionQuery request, CancellationToken cancellationToken)
        {
            //get from db
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id.Equals(request.Id));

            //Retrun It
            return product;
        }
    }
}
