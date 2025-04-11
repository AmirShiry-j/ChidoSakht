using Application.CategoryService.Commands;
using Application.CategoryService.Queries;
using Application.Interfaces.Localization;
using Application.PermissionService.Commands;
using Application.PermissionService.Queries;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CategoryService
{
    public interface IFacadePermissionService
    {
        //Commands
        IAssignPermissionsService AssignPermissionsService { get; }
        IUnAssignPermissionsService UnAssignPermissionsService { get; }
        IResetAndAssignPermissionsService ResetAndAssignPermissionsService { get; }
        //Queries
        IGetPermissionsService GetPermissionsService { get; }
    }
    public class FacadePermissionService : IFacadePermissionService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public FacadePermissionService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }
        //Commands
        private IAssignPermissionsService _AssignPermissionsService;
        public IAssignPermissionsService AssignPermissionsService
        {
            get
            {
                return _AssignPermissionsService = _AssignPermissionsService ?? new AssignPermissionService(_mediator, _localizationService);
            }
        }
        private IUnAssignPermissionsService _UnAssignPermissionsService;
        public IUnAssignPermissionsService UnAssignPermissionsService
        {
            get
            {
                return _UnAssignPermissionsService = _UnAssignPermissionsService ?? new UnAssignPermissionService(_mediator, _localizationService);
            }
        }
        private IResetAndAssignPermissionsService _ResetAndAssignPermissionsService;
        public IResetAndAssignPermissionsService ResetAndAssignPermissionsService
        {
            get
            {
                return _ResetAndAssignPermissionsService = _ResetAndAssignPermissionsService ?? new ResetAndAssignPermissionsService(_mediator, _localizationService);
            }
        }
        //Queries
        #region Queries
        private IGetPermissionsService _GetPermissionsService;
        public IGetPermissionsService GetPermissionsService
        {
            get
            {
                return _GetPermissionsService = _GetPermissionsService ?? new GetPermissionsService(_mediator);
            }
        }


        #endregion
    }
}
