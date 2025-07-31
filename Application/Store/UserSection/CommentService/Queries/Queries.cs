using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.CommentService.Queries
{
    public class GetCommentByIdQuery : IRequest<Comment>
    {
        public long Id { get; set; }

        public GetCommentByIdQuery(long id)
        {
            Id = id;
        }
    }

    public class GetVoteAUserOnCommentQuery : IRequest<Helpful>
    {
        public long CommentId { get; set; }
        public string UserId { get; set; }
        public GetVoteAUserOnCommentQuery(long CommentId, string UserId)
        {
            this.CommentId = CommentId;
            this.UserId = UserId;
        }
    }
}
