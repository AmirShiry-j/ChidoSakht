using Application.CategoryService.Queries;
using Application.Common.Dtoes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PermissionService.Queries
{
    public interface IGetPermissionsService
    {
        Task<ResultDto<List<PermissionDto>>> Execute();
    }
    public class GetPermissionsService : IGetPermissionsService
    {
        private readonly IMediator _mediator;
        public GetPermissionsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ResultDto<List<PermissionDto>>> Execute()
        {
            //get them from db
            var permissions = await _mediator.Send(new GetPermissionsQuery());

            //Check exist and return
            if (permissions is not null)
            {
                return new ResultDto<List<PermissionDto>>
                {
                    IsSuccess = true,
                    Data = permissions
                };
            }
            else
            {
                return new ResultDto<List<PermissionDto>>
                {
                    IsSuccess = true,
                    Data = new List<PermissionDto>()
                };
            }
        }
    }
    public class PermissionDto
    {
        public int Id { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
    }
    public class AssignPermissionDto
    {
        public string RoleId { get; set; }
        public int PermissionId { get; set; }
    }
}
