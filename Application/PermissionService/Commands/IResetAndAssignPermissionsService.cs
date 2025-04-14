using Application.Common.Dtoes;
using Application.Interfaces.Localization.AllMessageKeys;
using Application.Interfaces.Localization;
using Application.PermissionService.Queries;
using Application.RoleService.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PermissionService.Commands
{
    public interface IResetAndAssignPermissionsService
    {
        Task<ResultDto> Execute(PermissionsRoleDto dto);
    }
    public class ResetAndAssignPermissionsService : IResetAndAssignPermissionsService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public ResetAndAssignPermissionsService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }

        public async Task<ResultDto> Execute(PermissionsRoleDto dto)
        {
            //check exist Role 
            var role = await _mediator.Send(new GetRoleByIdQuery(dto.RoleId));
            if (role is null)
            {
                return new ResultDto
                {
                    Message = _localizationService.GetMessagePermission(MessageKeysPermission.RoleIdNotFound.ToString())
                };
            }

            //get existing Permissions
            var receivedPermissionsThatExist = await _mediator.Send(new GetExitingPermissionsByIdsQuery(dto.PermissionIds));
            var existingPermissionIds = receivedPermissionsThatExist.Select(p => p.Id).ToArray();

            //Get before AssignedPermissionRoles
            var assignedPermissions = await _mediator.Send(new GetAssignedPermissionIdsQuery(dto.RoleId));

            //Reset and Assign Permissions to Role
            await _mediator.Send(new ResetAndAssignPermissionsCommand(assignedPermissions,dto.RoleId, existingPermissionIds));
            return new ResultDto
            {
                IsSuccess = true
            };
        }
    }
}
