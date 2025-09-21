using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Commons.Objects.Dtoes;
using Application.Commons.Objects.MessageEventTypes;
using Application.Commons.Interfaces.Localization;
using Application.Commons.Interfaces.Localization.AllMessageKeys;
using Application.Store.AdminSection.PermissionService.Queries;
using Application.Store.AdminSection.RoleService.Queries;

namespace Application.Store.AdminSection.PermissionService.Commands
{
    public interface IResetAndAssignPermissionsService
    {
        Task<ResultDto> Execute(RolePermissionsDto dto);
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

            //Get before AssignedPermissionRoles
            var assignedPermissions = await _mediator.Send(new GetAssignedPermissionIdsQuery(dto.RoleId));

            //Reset and Assign Permissions to Role
            await _mediator.Send(new ResetAndAssignPermissionsCommand(assignedPermissions, dto.RoleId, existingPermissionIds));
            return new ResultDto
            {
                IsSuccess = true
            };
        }
    }
}
