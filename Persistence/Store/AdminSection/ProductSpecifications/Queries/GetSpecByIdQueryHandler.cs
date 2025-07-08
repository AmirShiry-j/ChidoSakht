using Application.Store.AdminSection.ProductSpecificationService.Queries;
using Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.AdminSection.ProductSpecifications.Queries
{
    public class GetSpecByIdQueryHandler : IRequestHandler<GetSpecByIdQuery, ProductSpecification>
    {
        private readonly DataBaseContext _context;
        public GetSpecByIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<ProductSpecification> Handle(GetSpecByIdQuery request, CancellationToken cancellationToken)
        {
            //get from db
            var productSpecification = await _context.ProductSpecifications.FirstOrDefaultAsync(p => p.Id.Equals(request.Id));

            //Retrun It
            return productSpecification;
        }
    }
}
