using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Products
{
    public class Comment
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public bool Confirmation { get; set; }
        public byte Star { get; set; }
        //Navs and Rel
        public string UserId { get; set; }
        public User User { get; set; }
        //
        public int ProductId { get; set; }
        public Product Product { get; set; }
        //
        public ICollection<Helpful> Helpfuls { get; set; }
    }
    public class Helpful
    {
        public long Id { get; set; }
        public bool WasHelpful { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public Comment Comment { get; set; }
        public long CommentId { get; set; }
    }
}
