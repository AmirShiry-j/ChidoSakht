using Application.Commons.Interfaces.Localization;
using Application.Store.AdminSection.CommentService.Commands;
using Application.Store.AdminSection.CommentService.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.CommentService
{
    public interface IFacadeAdminCommentService
    {
        ICommentCommandsService CommentCommandsService { get; }
        ICommentQueriesService CommentQueriesService { get; }
    }
    public class FacadeAdminCommentService : IFacadeAdminCommentService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public FacadeAdminCommentService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }

        //Commands
        private ICommentCommandsService _CommentCommandsService;
        public ICommentCommandsService CommentCommandsService
        {
            get
            {
                return _CommentCommandsService = _CommentCommandsService ?? new CommentCommandsService(_mediator, _localizationService);
            }
        }

        //Queries
        private ICommentQueriesService _CommentQueriesService;
        public ICommentQueriesService CommentQueriesService
        {
            get
            {
                return _CommentQueriesService = _CommentQueriesService ?? new CommentQueriesService(_mediator, _localizationService);
            }
        }
    }
}
