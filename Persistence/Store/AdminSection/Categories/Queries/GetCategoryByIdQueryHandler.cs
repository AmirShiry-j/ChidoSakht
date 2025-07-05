using Application.Store.AdminSection.CategoryService.Queries;
using Domain.Categories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Store.AdminSection.Categories.Queries
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Category>
    {
        private readonly DataBaseContext _context;

        public GetCategoryByIdQueryHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<Category> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            //Find and return
            return await _context.Categories
                .Where(c => c.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
