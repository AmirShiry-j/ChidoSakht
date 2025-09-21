using MediatR;
using Domain.Users;
using Application.Commons.Objects.Dtoes;
using Application.Commons.Objects.MessageEventTypes;
using Application.Commons.Interfaces.Localization;
using Application.Commons.Interfaces.Localization.AllMessageKeys;
using Application.Store.AdminSection.PermissionService.Queries;
using Application.Store.AdminSection.RoleService.Queries;

namespace Application.Store.AdminSection.PermissionService.Commands
{
    public interface IAssignPermissionsService
    {
        Task<ResultDto> Execute(RolePermissionsDto dto);
    }
    public class AssignPermissionService : IAssignPermissionsService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public AssignPermissionService(IMediator mediator, ILocalizationService localizationService)
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
            var assinedPermissionsIds = assignedPermissions.Select(p => p.PermissionId).ToArray();

            //UnAssign Permissions to Role
            var newPermissonIdsForAssign = existingPermissionIds.Where(p => !assinedPermissionsIds.Contains(p)).ToArray();
            if (newPermissonIdsForAssign.Count() > 0)
                await _mediator.Send(new AssignPermissionsCommand(dto.RoleId, newPermissonIdsForAssign));
            return new ResultDto
            {
                IsSuccess = true
            };
        }
    }
    public class RolePermissionsDto
    {
        public string RoleId { get; set; }
        public int[] PermissionIds { get; set; }
    }
}
