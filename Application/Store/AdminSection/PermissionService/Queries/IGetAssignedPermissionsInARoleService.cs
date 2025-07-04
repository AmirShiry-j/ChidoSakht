using Application.Commons.Interfaces.Localization;
using Application.Commons.Interfaces.Localization.AllMessageKeys;
using Application.Commons.Objects.Dtoes;
using Application.Commons.Objects.MessageEventTypes;
using Application.Store.AdminSection.RoleService.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.AdminSection.PermissionService.Queries
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
