using Application.Store.AdminSection.CategoryService.Commands;
using Domain.Categories;
using MediatR;
using Persistence.Contexts;

namespace Persistence.Categories.Commands
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {

        private readonly DataBaseContext _context;
        public UpdateCategoryCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            //Define update model
            var category = request.Category;
            category.LastUpdateTime = DateTime.Now;

            //Update and save in DB
            _context.Categories.Update(category);
            await _context.SaveChangesAsync(cancellationToken);

            //finish
            await Task.CompletedTask;
        }
    }
}
