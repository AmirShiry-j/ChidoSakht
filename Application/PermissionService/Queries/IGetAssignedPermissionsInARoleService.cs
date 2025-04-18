using Application.CategoryService.Queries;
using Application.Common.Dtoes;
using Application.Common.MessageEventTypes;
using Application.Interfaces.Localization;
using Application.Interfaces.Localization.AllMessageKeys;
using Application.PermissionService.Commands;
using Application.RoleService.Queries;
using Domain.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PermissionService.Queries
{
    public interface IGetAssignedPermissionsInARoleService
    {
        Task<ResultDto<List<PermissionDto>>> Execute(string RoleId);
    }
    public class GetAssignedPermissionsInARoleService : IGetAssignedPermissionsInARoleService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public GetAssignedPermissionsInARoleService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }

        public async Task<ResultDto<List<PermissionDto>>> Execute(string RoleId)
        {
            //check exist Role 
            var role = await _mediator.Send(new GetRoleByIdQuery(RoleId));
            if (role is null)
            {
                return new ResultDto<List<PermissionDto>>
                {
                    Message = _localizationService.GetMessagePermission(MessageKeysPermission.RoleIdNotFound.ToString()),
                    MessageEventType = MessageEventType.NotFound
                };
            }

            //get from db
            var permissons = await _mediator.Send(new GetAssignedPermissionsInARoleQuery(RoleId));

            //Check exist and return
            if (permissons is not null)
            {
                return new ResultDto<List<PermissionDto>>
                {
                    IsSuccess = true,
                    Data = permissons,
                };
            }
            else
            {
                return new ResultDto<List<PermissionDto>>
                {
                    IsSuccess = true,
                    Data = new List<PermissionDto> { },
                };
            }
        }
    }
}
