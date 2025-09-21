using Application.Commons.Interfaces.Localization;
using Application.Store.UserSection.CommentService.Commands;
using Application.Store.UserSection.CommentService.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.CommentService
{
    public interface IFacadeCommentService
    {
        ICommentCommandsService CommentCommandsService { get; }
        ICommentQueriesService CommentQueriesService { get; }
    }

    public class FacadeCommentService : IFacadeCommentService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public FacadeCommentService(IMediator mediator, ILocalizationService localizationService)
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
