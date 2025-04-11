namespace WebApi.Areas.Admin.ModelsAndDtoes.Permissions
{
    public class PermissionsRoleApiDto
    {
        public string RoleId { get; set; }
        public int[] PermissionIds { get; set; } 
    }
}
