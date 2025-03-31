using Application.CategoryService.Queries;
using Domain.Categories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserService.Queries
{
    public interface IGetAllUserService
    {
        public Task<List<UserDto>> Execute();
    }
    public class GetAllUserService : IGetAllUserService
    {
        private readonly IMediator _mediator;
        public GetAllUserService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<List<UserDto>> Execute()
        {
            //Get category from db
            var users = await _mediator.Send(new GetAllUsersQuery());

            return users;
        }
    }

    public class UserDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
