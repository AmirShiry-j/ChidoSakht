using Application.Store.AdminSection.ProductVariant.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.ProductVariant.Queries
{
    public class GetValidSendedProductAttributeValueIdsByProductIdQuery : IRequest<List<int>>
    {
        public int ProductId { get; set; }
        public List<int> ProductAttributeValueIds { get; set; }
        public GetValidSendedProductAttributeValueIdsByProductIdQuery(int productId, List<int> productAttributeValueIds)
        {
            ProductId = productId;
            ProductAttributeValueIds = productAttributeValueIds;
        }
    }
    public class GetProductVariantByIdQuery : IRequest<Domain.Products.ProductVariant>
    {
        public int ProductVariantId { get; set; }
        public GetProductVariantByIdQuery(int ProductVariantId)
        {
            this.ProductVariantId = ProductVariantId;
        }
    }

    public class CheckUsedVariantInAnySefareshQuery : IRequest<bool>
    {
        public int ProductVariantId { get; set; }
        public CheckUsedVariantInAnySefareshQuery(int ProductVariantId)
        {
            this.ProductVariantId = ProductVariantId;
        }
    }
    public class GetProductVariantTransportationByProductVariantIdQuery : IRequest<Domain.Products.ProductVariantTransportation>
    {
        public int ProductVariantId { get; set; }
        public GetProductVariantTransportationByProductVariantIdQuery(int ProductVariantId)
        {
            this.ProductVariantId = ProductVariantId;
        }
    }
    public class GetProductVariantsByProductIdQuery : IRequest<List<ProductVariantDto>>
    {
        public int ProductId { get; set; }
        public GetProductVariantsByProductIdQuery(int productId)
        {
            ProductId = productId;
        }
    }

    public class GetProductVariantsTypeVariableByProductIdQuery : IRequest<List<ProductVariantDto>>
    {
        public int ProductId { get; set; }
        public GetProductVariantsTypeVariableByProductIdQuery(int productId)
        {
            ProductId = productId;
        }
    }

    public class GetProductVariantTypeSampleByProductIdQuery : IRequest<ProductVariantDto>
    {
        public int ProductId { get; set; }
        public GetProductVariantTypeSampleByProductIdQuery(int productId)
        {
            ProductId = productId;
        }
    }

    public class GroupbySendedProductAttributeValueIds_ByAttributeId_Query : IRequest<List<GroupBy_Values_By_AttributeId_Dto>>
    {
        public List<int> ProductAttributeValueIds { get; set; }
        public GroupbySendedProductAttributeValueIds_ByAttributeId_Query(List<int> productAttributeValueIds)
        {
            ProductAttributeValueIds = productAttributeValueIds;
        }
    }

    public class CheckNotExistVariantLikeThisBeforeQuery : IRequest<bool>
    {
        public int ProductId { get; set; }
        public List<int> ProductAttributeValueIds { get; set; }
        public CheckNotExistVariantLikeThisBeforeQuery(int ProductId, List<int> productAttributeValueIds)
        {
            this.ProductId = ProductId;
            ProductAttributeValueIds = productAttributeValueIds;
        }
    }
}
