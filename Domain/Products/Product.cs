using Domain.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Products
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string UniCode { get; set; }
        public long Price { get; set; }
        public long? SpecialPrice { get; set; }
        public StatusInWarehouse StatusInWarehouse { get; set; }
        public double Weight { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        //Nav
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
    public enum StatusInWarehouse
    {
        NonExistent,
        Existent,
        InAdvancePurchase
    }
}
