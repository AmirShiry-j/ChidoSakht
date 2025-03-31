using Application.Common;
using Application.Interfaces.Localization.AllMessageKeys;
using Application.Interfaces.Localization;
using Application.PermissionService.Queries;
using MediatR;

namespace Application.PermissionService.Commands
{
    public interface IUnAssignPermissionService
    {
        Task<ResultDto> Execute(RolePermissionDto dto);
    }
    public class UnAssignPermissionService : IUnAssignPermissionService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        public UnAssignPermissionService(IMediator mediator, ILocalizationService localizationService)
        {
            _mediator = mediator;
            _localizationService = localizationService;
        }

        public async Task<ResultDto> Execute(RolePermissionDto dto)
        {
            //get recored from db
            var rolePermission = await _mediator.Send(new GetRolePermissionQuery(dto.RoleId, dto.PermissionId));
            if (rolePermission is null)
            {
                return new ResultDto
                {
                    Message = _localizationService.GetMessagePermission(MessageKeysPermission.RoleDoesntHavePermissionBefore.ToString())
                };
            }

            //assin
            await _mediator.Send(new UnAssignPermissionCommand(rolePermission));

            return new ResultDto
            {
                IsSuccess = true
            };
        }
    }
}
