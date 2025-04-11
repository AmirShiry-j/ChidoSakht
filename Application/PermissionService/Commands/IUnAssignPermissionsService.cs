using Application.Interfaces.Localization.AllMessageKeys;
using Application.Interfaces.Localization;
using Application.PermissionService.Queries;
using MediatR;
using Application.RoleService.Queries;
using Application.Common.Dtoes;

namespace Application.PermissionService.Commands
{
    public interface IUnAssignPermissionsService
    {
        Task<ResultDto> Execute(PermissionsRoleDto dto);
    }
    public class UnAssignPermissionService : IUnAssignPermissionsService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public UnAssignPermissionService(IMediator mediator, ILocalizationService localizationService)
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
            var existingPermissions = await _mediator.Send(new GetExitingPermissionsByIdsQuery(dto.PermissionIds));
            if (existingPermissions is null && !existingPermissions.Any())
            {
                return new ResultDto
                {
                    IsSuccess = true
                };
            }
            var existingPermissionIds = existingPermissions.Select(p => p.Id).ToArray();

            //Get AssignedPermissionRoles
            var assignedPermissions = await _mediator.Send(new GetAssignedPermissionIdsQuery(dto.RoleId, existingPermissionIds));
            if (assignedPermissions is not null && assignedPermissions.Count == dto.PermissionIds.Count())
            {
                return new ResultDto
                {
                    IsSuccess = true
                };
            }

            //UnAssign Permissions to Role
            await _mediator.Send(new UnAssignPermissionsCommand(assignedPermissions));
            return new ResultDto
            {
                IsSuccess = true
            };
        }
    }
}
