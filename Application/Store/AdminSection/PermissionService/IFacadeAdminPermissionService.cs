using Application.Commons.Interfaces.Localization;
using Application.Store.AdminSection.PermissionService.Commands;
using Application.Store.AdminSection.PermissionService.Queries;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.PermissionService
{
    public interface IFacadeAdminPermissionService
    {
        //Commands
        IAssignPermissionsService AssignPermissionsService { get; }
        IUnAssignPermissionsService UnAssignPermissionsService { get; }
        IResetAndAssignPermissionsService ResetAndAssignPermissionsService { get; }
        //Queries
        IGetPermissionsService GetPermissionsService { get; }
        IGetAssignedPermissionsInARoleService GetAssignedPermissionsInARoleService { get; }
        IGetAssignedPermissionsInAllRoleService GetAssignedPermissionsInAllRoleService { get; }
    }
    public class FacadeAdminPermissionService : IFacadeAdminPermissionService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public FacadeAdminPermissionService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }
        //Commands
        #region Commands
        //IAssignPermissionsService
        private IAssignPermissionsService _AssignPermissionsService;
        public IAssignPermissionsService AssignPermissionsService
        {
            get
            {
                return _AssignPermissionsService = _AssignPermissionsService ?? new AssignPermissionService(_mediator, _localizationService);
            }
        }
        //IUnAssignPermissionsService
        private IUnAssignPermissionsService _UnAssignPermissionsService;
        public IUnAssignPermissionsService UnAssignPermissionsService
        {
            get
            {
                return _UnAssignPermissionsService = _UnAssignPermissionsService ?? new UnAssignPermissionService(_mediator, _localizationService);
            }
        }
        //IResetAndAssignPermissionsService
        private IResetAndAssignPermissionsService _ResetAndAssignPermissionsService;
        public IResetAndAssignPermissionsService ResetAndAssignPermissionsService
        {
            get
            {
                return _ResetAndAssignPermissionsService = _ResetAndAssignPermissionsService ?? new ResetAndAssignPermissionsService(_mediator, _localizationService);
            }
        }
        #endregion

        //Queries
        #region Queries
        //IGetPermissionsService
        private IGetPermissionsService _GetPermissionsService;
        public IGetPermissionsService GetPermissionsService
        {
            get
            {
                return _GetPermissionsService = _GetPermissionsService ?? new GetPermissionsService(_mediator);
            }
        }
        //IGetAssignedPermissionsInARoleService
        private IGetAssignedPermissionsInARoleService _GetAssignedPermissionsInARoleService;
        public IGetAssignedPermissionsInARoleService GetAssignedPermissionsInARoleService
        {
            get
            {
                return _GetAssignedPermissionsInARoleService = _GetAssignedPermissionsInARoleService ?? new GetAssignedPermissionsInARoleService(_mediator, _localizationService);
            }
        }
        //IGetAssignedPermissionsInAllRoleService
        private IGetAssignedPermissionsInAllRoleService _GetAssignedPermissionsInAllRoleService;
        public IGetAssignedPermissionsInAllRoleService GetAssignedPermissionsInAllRoleService
        {
            get
            {
                return _GetAssignedPermissionsInAllRoleService = _GetAssignedPermissionsInAllRoleService ?? new GetAssignedPermissionsInAllRoleService(_mediator, _localizationService);
            }
        }
        #endregion
    }
}
