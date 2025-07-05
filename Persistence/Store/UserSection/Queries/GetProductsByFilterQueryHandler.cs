using Application.Commons.Objects.Dtoes;
using Application.Store.UserSection.ProductService.Queries;
using Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.UserSection.Queries
{
    public class GetProductsByFilterQueryHandler : IRequestHandler<GetProductsByFilterQuery, ResultFilterDto>
    {
        private readonly DataBaseContext _context;
        public GetProductsByFilterQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<ResultFilterDto> Handle(GetProductsByFilterQuery request, CancellationToken cancellationToken)
        {
            var prProduct = PredicateBuilder.True<Product>();
            var FilterDto = request.Filter;

            //Filter Name
            if (string.IsNullOrWhiteSpace(FilterDto.ProductName) == false)
            {
                prProduct = prProduct.And(x => x.Name.Contains(FilterDto.ProductName));
            }

            //Filter CategoryId
            if (FilterDto.CategoryId is not null)
            {
                prProduct = prProduct.And(x => x.CategoryId.Equals(FilterDto.CategoryId));
            }

            //Filter OnlyAvailableGoods
            if (FilterDto.OnlyAvailableGoods is not null && FilterDto.OnlyAvailableGoods == true)
            {
                prProduct = prProduct.And(x => x.ProductVariants.Any(p => p.Stock > 0));
            }

            //Filter FromPrice
            if (FilterDto.FromPrice is not null)
            {
                prProduct = prProduct.And(x => x.ProductVariants.Any(p => p.Price > FilterDto.FromPrice));
            }

            //Filter FromPrice
            if (FilterDto.ToPrice is not null)
            {
                prProduct = prProduct.And(x => x.ProductVariants.Any(p => p.Price > FilterDto.ToPrice));
            }

            //Order by FilterFor

            var products = await _context.Products
                .Where(prProduct)
                .Include(p => p.ProductVariants)
                .OrderBy(p => p.Id)
                .Skip((FilterDto.Page.Value - 1) * FilterDto.CountInPage.Value)
                .Take(FilterDto.CountInPage.Value)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    NameIndexImage = p.NameIndexImage,
                    ImageAltText = p.ImageAltText,
                    Price = p.ProductVariants.FirstOrDefault().Price,
                    SpecialPrice = p.ProductVariants.FirstOrDefault().SpecialPrice,
                    UniqeLink = p.UniqeLink,
                })
                .ToListAsync();

            //For Pagination
            int CountAllItems = _context.Products.Where(prProduct).Count();
            int CountAllPages = CountAllItems / FilterDto.CountInPage.Value + (CountAllItems % FilterDto.CountInPage.Value > 0 ? 1 : 0);

            return new ResultFilterDto
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
