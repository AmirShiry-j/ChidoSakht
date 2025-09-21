using Application.Store.AdminSection.ProductVariant.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.AdminSection.ProductVariant.Queries
{
    public class CheckNotExistVariantLikeThisBeforeQueryHandler : IRequestHandler<CheckNotExistVariantLikeThisBeforeQuery, bool>
    {
        private readonly DataBaseContext _context;
        public CheckNotExistVariantLikeThisBeforeQueryHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(CheckNotExistVariantLikeThisBeforeQuery request, CancellationToken cancellationToken)
        {
            var valuesOfVariatsProduct = await _context.ProductVariantAttributeValues.IgnoreQueryFilters()
                .Include(p => p.ProductVariant)
                .Where(p => p.ProductVariant.ProductId.Equals(request.ProductId))
                .ToListAsync();


            var results = valuesOfVariatsProduct.GroupBy(
    p => p.ProductVariantId,
    (key, g) => new GroupBy_Values_By_VariantId_Dto { VariantId = key, ValueIds = g.Select(p => p.ProductAttributeValueId).ToList() })
                .ToList();

            foreach (var variantWithValues in results)
            {
                if (variantWithValues.ValueIds.Count == request.ProductAttributeValueIds.Count)
                {
                    var a_ValueId_Not_In_list = variantWithValues.ValueIds.Where(p => !request.ProductAttributeValueIds.Contains(p)).Any();
                    if (a_ValueId_Not_In_list == false)
                        return false;
                }
            }

            return true;
        }
    }

    public class GroupBy_Values_By_VariantId_Dto
    {
        public int VariantId { get; set; }
        public List<int> ValueIds { get; set; }
    }
}
