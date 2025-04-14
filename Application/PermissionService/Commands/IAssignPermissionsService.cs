using Application.Common.Dtoes;
using Application.Interfaces.Localization;
using Application.Interfaces.Localization.AllMessageKeys;
using Application.PermissionService.Queries;
using Application.RoleService.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.PermissionService.Queries;
using Domain.Users;

namespace Application.PermissionService.Commands
{
    public interface IAssignPermissionsService
    {
        Task<ResultDto> Execute(PermissionsRoleDto dto);
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
            if (receivedPermissionsThatExist is null && !receivedPermissionsThatExist.Any())
            {
                return new ResultDto
                {
                    IsSuccess = true
                };
            }
            var existingPermissionIds = receivedPermissionsThatExist.Select(p => p.Id).ToArray();

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
    public class PermissionsRoleDto
    {
        public string RoleId { get; set; }
        public int[] PermissionIds { get; set; }
    }
}
