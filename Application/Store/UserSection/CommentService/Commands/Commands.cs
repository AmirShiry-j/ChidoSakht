using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.CommentService.Commands
{
    public class CreateCommentCommand : IRequest<long>
    {
        public string Text { get; set; }
        public byte Star { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public CreateCommentCommand(string text, byte star, int productId, string userId)
        {
            Text = text;
            Star = star;
            ProductId = productId;
            UserId = userId;
        }
    }
    public class VoteOnCommentCommand : IRequest
    {
        public long CommentId { get; set; }
        public bool WasHelpful { get; set; }
        public string UserId { get; set; }
        public VoteOnCommentCommand(long CommentId, bool WasHelpful, string UserId)
        {
            this.CommentId = CommentId;
            this.WasHelpful = WasHelpful;
            this.UserId = UserId;
        }
    }
}
