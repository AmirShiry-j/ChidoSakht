using Application.Store.AdminSection.ProductService.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.ProductService.Queries
{
    public class GetProductsByFilterQuery : IRequest<ResultFilterDto>
    {
        public ProductFilterForUserSectionDto Filter { get; set; }
        public GetProductsByFilterQuery(ProductFilterForUserSectionDto filter)
        {
            Filter = filter;
        }
    }

    public class GetOneProductWithDetailsByIdQuery : IRequest<ProductWithDetailsDto>
    {
        public int Id { get; set; }

        public GetOneProductWithDetailsByIdQuery(int id)
        {
            Id = id;
        }
    }
}
