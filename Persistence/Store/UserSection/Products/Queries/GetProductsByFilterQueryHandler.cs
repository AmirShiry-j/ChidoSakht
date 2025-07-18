using Application.Commons.Objects.Dtoes;
using Application.Store.UserSection.ProductService.Queries;
using Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.UserSection.Products.Queries
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string propertyName, bool ascending)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return source;

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            string methodName = ascending ? "OrderBy" : "OrderByDescending";

            var result = typeof(Queryable).GetMethods()
                .First(method => method.Name == methodName
                                 && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.Type)
                .Invoke(null, new object[] { source, lambda });

            return (IQueryable<T>)result!;
        }
    }

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

            //Order by
            string SortBy = "";
            bool Ascending = false;
            switch (request.Filter.TypeOrderByForProduct)
            {
                case TypeOrderByForProduct.Bazdid:
                    SortBy = "CountView";
                    Ascending = false;
                    break;
                case TypeOrderByForProduct.Jadid:
                    SortBy = nameof(Product.CreateTime);
                    Ascending = false;
                    break;
                case TypeOrderByForProduct.Forush:
                    //SortBy = "felan";
                    SortBy = nameof(Product.CreateTime);
                    break;
                case TypeOrderByForProduct.Arzan:
                    //SortBy = "felan";
                    SortBy = nameof(Product.CreateTime);
                    break;
                default:
                    SortBy = nameof(Product.CreateTime);
                    Ascending = false;
                    break;
            }

            //Order by FilterFor
            var products = await _context.Products
                .Where(prProduct)
                .Include(p => p.ProductVariants)
                .OrderByDynamic(SortBy, Ascending)
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
