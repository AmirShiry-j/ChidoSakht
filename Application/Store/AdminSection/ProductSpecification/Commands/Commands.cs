using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.ProductSpecification.Commands
{
    public record CreateSpecGroupCommand(int ProductId, string Title) : IRequest<long>;
    public record UpdateSpecGroupCommand(long GroupId, string Title) : IRequest<bool>;
    public record DeleteSpecGroupCommand(long GroupId) : IRequest<bool>;

    public record CreateSpecCommand(long GroupId, string Key, string Value) : IRequest<long>;
    public record UpdateSpecCommand(long SpecId, string Key, string Value) : IRequest<bool>;
    public record DeleteSpecCommand(long SpecId) : IRequest<bool>;
}
