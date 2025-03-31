using Application.Common;
using Application.Interfaces.Localization;
using Application.Interfaces.Localization.AllMessageKeys;
using Application.PermissionService.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PermissionService.Commands
{
    public interface IAssignPermissionService
    {
        Task<ResultDto> Execute(RolePermissionDto dto);
    }
    public class AssignPermissionService : IAssignPermissionService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public AssignPermissionService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }

        public async Task<ResultDto> Execute(RolePermissionDto dto)
        {
            //check has any record in db
            var beforeIsExist = await _mediator.Send(new CheckRoleHasPermissionQuery(dto.RoleId, dto.PermissionId));
            if (beforeIsExist)
            {
                return new ResultDto
                {
                    Message = _localizationService.GetMessagePermission(MessageKeysPermission.RoleHasPermissionBefore.ToString())
                };
            }

            //assin
            await _mediator.Send(new AssignPermissionCommand(dto.RoleId, dto.PermissionId));

            return new ResultDto
            {
                IsSuccess = true
            };
        }
    }
    public class RolePermissionDto
    {
        public string RoleId { get; set; }
        public int PermissionId { get; set; }
    }
}
