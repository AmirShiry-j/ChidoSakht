using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProductService.Commands
{
    public class InsertProductCommand : IRequest<int?>
    {
        public string Name { get; set; }

        public InsertProductCommand(string name)
        {
            Name = name;
        }
    }
}
