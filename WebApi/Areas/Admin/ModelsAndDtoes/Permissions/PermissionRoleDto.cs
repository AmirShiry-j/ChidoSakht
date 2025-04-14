namespace WebApi.Areas.Admin.ModelsAndDtoes.Permissions
{
    public class RolePermissionsApiDto
    {
        public string RoleId { get; set; }
        public int[] PermissionIds { get; set; } 
    }
}
