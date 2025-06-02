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

    public class GetProductDetailsByIdQuery : IRequest<ProductDetailsDto>
    {
        public int Id { get; set; }

        public GetProductDetailsByIdQuery(int id)
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
        public int ProductId { get; set; }
        public string UniqeLink { get; set; }

        public ValidateUniqeLinkQuery(int productId, string uniqeLink)
        {
            UniqeLink = uniqeLink;
            ProductId = productId;
        }
    }
    public class ValidateUniCodeQuery : IRequest<bool>
    {
        public int ProductId { get; set; }
        public string UniCode { get; set; }

        public ValidateUniCodeQuery(int productId, string uniCode)
        {
            UniCode = uniCode;
            ProductId = productId;
        }
    }
}
