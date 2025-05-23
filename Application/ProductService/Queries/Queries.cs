using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProductService.Queries
{
    public class GetProductByIdQuery : IRequest<Product>
    {
        public int Id { get; set; }

        public GetProductByIdQuery(int id)
        {
            Id = id;
        }
    }
    public class GetProductsByFilterQuery : IRequest<ResultSearchDto>
    {
        public ProductFilterDto Filter { get; set; }
        public GetProductsByFilterQuery(ProductFilterDto filter)
        {
            Filter = filter;
        }
    }
    public class ValidateUniqeLinkQuery : IRequest<bool>
    {
        public string UniqeLink { get; set; }

        public ValidateUniqeLinkQuery(string uniqeLink)
        {
            UniqeLink = uniqeLink;
        }
    }
}
