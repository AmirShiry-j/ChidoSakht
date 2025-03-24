using Application.CategoryService.Commands;
using Domain.Categories;
using MediatR;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Categories.Commands
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
    {
        private readonly DataBaseContext _context;

        public CreateCategoryCommandHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            //Define
            var category = new Category(request.Name, request.ParentCategoryId);

            //Add and save in DB
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync(cancellationToken);

            //Retrun Id
            return category.Id;
        }
    }
}