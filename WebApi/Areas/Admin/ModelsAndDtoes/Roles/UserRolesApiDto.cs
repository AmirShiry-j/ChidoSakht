namespace WebApi.Areas.Admin.ModelsAndDtoes.Permissions
{
    public class UserRolesApiDto
    {
        public string UserId { get; set; }
        public string[] RoleIds { get; set; }
    }
}
