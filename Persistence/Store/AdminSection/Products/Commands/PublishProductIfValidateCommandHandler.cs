using Application.Store.AdminSection.ProductService.Commands;
using Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Store.AdminSection.Products.Commands
{
    public class PublishProductIfValidateCommandHandler : IRequestHandler<PublishProductIfValidateCommand, Tuple<bool, string>>
    {
        private readonly DataBaseContext _context;
        public PublishProductIfValidateCommandHandler(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<Tuple<bool, string>> Handle(PublishProductIfValidateCommand request, CancellationToken cancellationToken)
        {
            List<string> messages = new List<string>();
            bool canPublish = true;


            //شروط
            //هر محصولی

            //حتما باید دارای یک عکس باشه

            //فعلا دسته بندی کامنت
            //////حتما باید جز یک دسته بندی باشد

            //اگه محصول ساده بود
            //حتما باید دارای قیمت باشه

            //محصول متغیر
            //حتما باید دارای یک واریانت باشه


            // در این صورت قابل پابلیش میشه

            var product = await _context.Products.Where(p => p.Id.Equals(request.ProductId))
                .Include(p => p.ProductVariants)
                .FirstOrDefaultAsync();

            if (product.NameIndexImage is null)
            {
                messages.Add("حتما باید یک تصویر شاخص برای محصول در نظر گرفته شود");
                canPublish = false;
            }

            //if (product.CategoryId is null)
            //{
            //    messages.Add("محصول حتما باید جزوه یک دسته بندی باشد");
            //    canPublish = false;
            //}

            if (product.ProductType == ProductType.Sample)
            {
                if (!product.ProductVariants.Where(p => p.ProductType == ProductType.Sample).Any())
                {
                    messages.Add("قیمت محصول حتما باید ثبت شود");
                    canPublish = false;
                }
            }
            else
            {
                if (!product.ProductVariants.Where(p => p.ProductType == ProductType.Variable).Any())
                {
                    messages.Add("محصول متغیر حتما باید دارای یک واریانت باشد");
                    canPublish = false;
                }
            }

            if (canPublish)
            {
                product.IsPublished = true;

                //set publish true
                _context.Update(product);
                _context.SaveChanges();

                return new Tuple<bool, string>(true, null);
            }
            else
            {
                string message = messages.Aggregate((p1, p2) => p1 + "," + p2).ToString();
                return new Tuple<bool, string>(false, message);
            }
        }
    }
}
