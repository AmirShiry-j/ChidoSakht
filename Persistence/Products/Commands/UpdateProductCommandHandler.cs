using Application.ProductService.Commands;
using Application.ProductService.Queries;
using Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Products.Commands
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly DataBaseContext _context;
        public UpdateProductCommandHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            request.Product.LastUpdateTime = DateTime.Now;
            _context.Update(request.Product);
            _context.SaveChanges();
        }
    }
}