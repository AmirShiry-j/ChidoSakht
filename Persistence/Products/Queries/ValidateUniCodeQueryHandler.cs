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
    public class ValidateUniCodeQueryHandler : IRequestHandler<ValidateUniCodeQuery, bool>
    {
        private readonly DataBaseContext _context;
        public ValidateUniCodeQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(ValidateUniCodeQuery request, CancellationToken cancellationToken)
        {
            return !await _context.Products
                .AnyAsync(p => !p.Id.Equals(request.ProductId) && p.UniCode != null && p.UniCode.Equals(request.UniCode));
        }
    }
}
