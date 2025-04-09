using Application.CategoryService.Commands;
using Application.CategoryService.Queries;
using Application.Interfaces.Localization;
using Application.UserService.Queries;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CategoryService
{
    public interface IFacadeUserService
    {
        //Commands
        //Queries
        IGetAllUserService GetAllUserService { get; }
        IGetUserProfileService GetUserProfileService { get; }
    }
    public class FacadeUserService : IFacadeUserService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public FacadeUserService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }
        //Commands

        //Queries
        #region Queries
        private IGetAllUserService _getAllUserService;
        public IGetAllUserService GetAllUserService
        {
            get
            {
                return _getAllUserService = _getAllUserService ?? new GetAllUserService(_mediator);
            }
        }
        private IGetUserProfileService _GetUserProfileService;
        public IGetUserProfileService GetUserProfileService
        {
            get
            {
                return _GetUserProfileService = _GetUserProfileService ?? new GetUserProfileService(_mediator);
            }
        }
        #endregion
    }
}
