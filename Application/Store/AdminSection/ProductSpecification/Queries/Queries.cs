using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.ProductSpecification.Queries
{
    public record GetSpecGroupsQuery(int ProductId) : IRequest<List<SpecGroupDto>>;

    public class SpecGroupDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public List<SpecDto> Specifications { get; set; }
    }

    public class SpecDto
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
