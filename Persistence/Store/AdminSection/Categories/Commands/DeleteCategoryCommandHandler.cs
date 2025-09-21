using Application.Store.AdminSection.CategoryService.Commands;
using Domain.Categories;
using MediatR;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.AdminSection.Categories.Commands
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {

        private readonly DataBaseContext _context;
        public DeleteCategoryCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            //Delete and save in DB
            _context.Categories.Remove(request.Category);
            await _context.SaveChangesAsync(cancellationToken);

            //finish
            await Task.CompletedTask;
        }
    }
}
