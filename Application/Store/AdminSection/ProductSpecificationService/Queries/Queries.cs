using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.ProductSpecificationService.Queries
{
    public record GetAllSpecGroupsWithSpecsByProductIdQuery(int ProductId) : IRequest<List<SpecGroupDto>>;
    public record GetSpecGroupByIdQuery(long Id) : IRequest<ProductSpecificationGroup>;
    public record GetSpecByIdQuery(long Id) : IRequest<ProductSpecification>;


}
