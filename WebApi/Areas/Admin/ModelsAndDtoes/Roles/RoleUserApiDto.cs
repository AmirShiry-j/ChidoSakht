using Application.UserService.Queries;

namespace WebApi.Areas.Admin.ModelsAndDtoes.Roles
{
    public class RoleUserApiDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<UserDto> Users { get; set; }
    }
}
