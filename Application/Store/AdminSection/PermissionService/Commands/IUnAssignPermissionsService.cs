using MediatR;
using Application.Commons.Objects.Dtoes;
using Application.Commons.Objects.MessageEventTypes;
using Application.Commons.Interfaces.Localization;
using Application.Commons.Interfaces.Localization.AllMessageKeys;
using Application.Store.AdminSection.RoleService.Queries;
using Application.Store.AdminSection.PermissionService.Queries;

namespace Application.Store.AdminSection.PermissionService.Commands
{
    public interface IUnAssignPermissionsService
    {
        Task<ResultDto> Execute(RolePermissionsDto dto);
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

        public async Task<ResultDto> Execute(RolePermissionsDto dto)
        {
            //check exist Role 
            var role = await _mediator.Send(new GetRoleByIdQuery(dto.RoleId));
            if (role is null)
            {
                return new ResultDto
                {
                    Message = _localizationService.GetMessagePermission(MessageKeysPermission.RoleIdNotFound.ToString()),
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //get existing Permissions
            var receivedPermissionsThatExist = await _mediator.Send(new GetExitingPermissionsByIdsQuery(dto.PermissionIds));
            var existingPermissionIds = receivedPermissionsThatExist.Select(p => p.Id).ToArray();

            //Check for not existsing received PermisionsIds
            var notFoundPermissionIds = dto.PermissionIds.Where(p => !existingPermissionIds.Contains(p)).ToList();
            if (notFoundPermissionIds.Any())
            {
                var str = notFoundPermissionIds.Select(p => "'" + p + "'").Aggregate((p1, p2) => p1 + "," + p2);
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = string.Format(_localizationService.GetMessagePermission(MessageKeysPermission.PermissionIdsNotFound.ToString()), str),
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //Get AssignedPermissionRoles
            var assignedPermissions = await _mediator.Send(new GetAssignedPermissionIdsQuery(dto.RoleId));
            var assignedPermissionsForDelete = assignedPermissions.Where(p => existingPermissionIds.Contains(p.PermissionId)).ToList();

            //UnAssign Permissions to Role
            await _mediator.Send(new UnAssignPermissionsCommand(assignedPermissionsForDelete));
            return new ResultDto
            {
                IsSuccess = true
            };
        }
    }
}
