using Application.CategoryService.Commands;
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
            var category = new Category
            {
                Id = request.Id,
                Name = request.Name,
                ParentCategoryId = request.ParentCategoryId,
                LastUpdateTime = DateTime.Now
            };

            //Update and save in DB
            _context.Categories.Update(category);
            await _context.SaveChangesAsync(cancellationToken);

            //finish
            await Task.CompletedTask;
        }
    }
}
