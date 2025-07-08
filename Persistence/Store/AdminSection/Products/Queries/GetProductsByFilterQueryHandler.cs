using Application.Commons.Objects.Dtoes;
using Application.Store.AdminSection.ProductService.Queries;
using Domain.Products;
using Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.AdminSection.Products.Queries
{
    public class GetProductsByFilterQueryHandler : IRequestHandler<GetProductsByFilterQuery, ResultSearchDto>
    {
        private readonly DataBaseContext _context;
        public GetProductsByFilterQueryHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultSearchDto> Handle(GetProductsByFilterQuery request, CancellationToken cancellationToken)
        {
            var prProduct = PredicateBuilder.True<Product>();
            var FilterDto = request.Filter;

            //Filter Name
            if (string.IsNullOrWhiteSpace(FilterDto.Name) == false)
            {
                prProduct = prProduct.And(x => x.Name.Contains(FilterDto.Name));
            }

            //Filter CategoryId
            if (FilterDto.CategoryId is not null)
            {
                prProduct = prProduct.And(x => x.CategoryId.Equals(FilterDto.CategoryId));
            }

            var products = await _context.Products
                .Where(prProduct)
                .OrderBy(p => p.Name)
                .Skip((FilterDto.Page.Value - 1) * FilterDto.CountInPage.Value)
                .Take(FilterDto.CountInPage.Value)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    ProductType = p.ProductType,
                    NameIndexImage = p.NameIndexImage
                })
                .ToListAsync();

            //For Pagination
            int CountAllItems = _context.Products.Where(prProduct).Count();
            int CountAllPages = CountAllItems / FilterDto.CountInPage.Value + (CountAllItems % FilterDto.CountInPage.Value > 0 ? 1 : 0);

            return new ResultSearchDto
            {
                Page = FilterDto.Page.Value,
                CountInPage = FilterDto.CountInPage.Value,
                CountAllItems = CountAllItems,
                CountAllPages = CountAllPages,
                Products = products
            };
        }
    }
}