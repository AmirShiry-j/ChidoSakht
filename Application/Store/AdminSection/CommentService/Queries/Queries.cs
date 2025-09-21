using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.CommentService.Queries
{
    public class GetCommentsByFilterAdminQuery : IRequest<ResultFilterDto>
    {
        public CommentFilterForAdminSectionDto Filter { get; set; }
        public GetCommentsByFilterAdminQuery(CommentFilterForAdminSectionDto Filter)
        {
            this.Filter = Filter;
        }
    }
}
