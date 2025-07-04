using Application.Store.AdminSection.ProductService.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Products.Queries
{
    public class ValidateUniqeLinkQueryHandler : IRequestHandler<ValidateUniqeLinkQuery, bool>
    {
        private readonly DataBaseContext _context;
        public ValidateUniqeLinkQueryHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(ValidateUniqeLinkQuery request, CancellationToken cancellationToken)
        {
            return !await _context.Products
                .AnyAsync(p => !p.Id.Equals(request.ProductId) && p.UniqeLink != null && p.UniqeLink.Equals(request.UniqeLink));
        }
    }
}
