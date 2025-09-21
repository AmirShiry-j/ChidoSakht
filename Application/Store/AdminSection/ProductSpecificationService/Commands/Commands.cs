using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.ProductSpecificationService.Commands
{
    public record CreateSpecGroupCommand(int ProductId, string Title) : IRequest<long>;
    public record UpdateSpecGroupCommand(ProductSpecificationGroup ProductSpecificationGroup) : IRequest;
    public record DeleteSpecGroupCommand(ProductSpecificationGroup ProductSpecificationGroup) : IRequest;

    public record CreateSpecCommand(long GroupId, string Key, string Value) : IRequest<long>;
    public record UpdateSpecCommand(ProductSpecification ProductSpecification) : IRequest;
    public record DeleteSpecCommand(ProductSpecification ProductSpecification) : IRequest;
}
