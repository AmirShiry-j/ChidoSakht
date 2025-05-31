using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProductVariant.Commands
{
    public class CreateProductVariantCommand : IRequest<int>
    {
        public CreateProductVariantDto ProductVariantDto { get; set; }
        public CreateProductVariantCommand(CreateProductVariantDto productVariantDto)
        {
            ProductVariantDto = productVariantDto;
        }
    }
}
