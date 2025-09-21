using Domain.Products;
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
    public class CreateVoteOnCommentCommand : IRequest
    {
        public long CommentId { get; set; }
        public bool WasHelpful { get; set; }
        public string UserId { get; set; }
        public CreateVoteOnCommentCommand(long CommentId, bool WasHelpful, string UserId)
        {
            this.CommentId = CommentId;
            this.WasHelpful = WasHelpful;
            this.UserId = UserId;
        }
    }
    public class UpdateVoteOnCommentCommand : IRequest
    {
        public Helpful Helpful { get; set; }
        public UpdateVoteOnCommentCommand(Helpful Helpful)
        {
            this.Helpful = Helpful;
        }
    }

    public class DeleteVoteOnCommentCommand : IRequest
    {
        public Helpful Helpful { get; set; }
        public DeleteVoteOnCommentCommand(Helpful Helpful)
        {
            this.Helpful = Helpful;
        }
    }

    public class DeleteCommentCommand : IRequest
    {
        public Comment Comment { get; set; }
        public DeleteCommentCommand(Comment Comment)
        {
            this.Comment = Comment;
        }
    }
}
