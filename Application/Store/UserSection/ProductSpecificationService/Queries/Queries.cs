using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.ProductSpecificationService.Queries
{
    public record GetAllSpecGroupsWithSpecsByProductIdQuery(int ProductId) : IRequest<List<SpecGroupWithSpecsDto>>;
}
