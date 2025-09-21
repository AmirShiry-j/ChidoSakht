using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.CommentService.Commands
{

    public class UpdateCommentCommand : IRequest
    {
        public Comment Comment { get; set; }

        public UpdateCommentCommand(Comment Comment)
        {
            this.Comment = Comment;
        }
    }
}
