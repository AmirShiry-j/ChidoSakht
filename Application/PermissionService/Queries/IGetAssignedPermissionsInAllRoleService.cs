using Application.Common.Dtoes;
using Application.Interfaces.Localization;
using Domain.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PermissionService.Queries
{
    public interface IGetAssignedPermissionsInAllRoleService
    {
        Task<ResultDto<List<RoleDto>>> Execute();
    }
    public class GetAssignedPermissionsInAllRoleService : IGetAssignedPermissionsInAllRoleService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public GetAssignedPermissionsInAllRoleService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }
        public async Task<ResultDto<List<RoleDto>>> Execute()
        {
            //get from db
            var rolesAndTheirPermissons = await _mediator.Send(new GetAssignedPermissionsInAllRoleQuery());

            //Check exist and return
            if (rolesAndTheirPermissons is not null)
            {
                return new ResultDto<List<RoleDto>>
                {
                    IsSuccess = true,
                    Data = rolesAndTheirPermissons,
                };
            }
            else
            {
                return new ResultDto<List<RoleDto>>
                {
                    IsSuccess = true,
                    Data = new List<RoleDto> { },
                };
            }
        }
    }
    public class RoleDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<PermissionDto> Permissions { get; set; }
    }
}
