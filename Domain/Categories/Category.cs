using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Categories
{
    public class Category
    {
        public Category()
        {
                
        }
        public Category(string Name, int? ParentCategoryId)
        {
            _Name = Name;
            this.ParentCategoryId = ParentCategoryId;
        }
        public int Id { get; set; }
        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(value));
                if (value.Length > 50)
                    throw new InvalidDataException($"{nameof(_Name)} is more than 50 character");

                _Name = value;
            }
        }
        //Navigations
        public Category ParentCategory { get; set; }
        public int? ParentCategoryId { get; set; }
        public ICollection<Category> ChildCategories { get; set; }
    }
}
