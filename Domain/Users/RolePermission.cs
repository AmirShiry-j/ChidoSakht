namespace Domain.Users
{
    public class RolePermission
    {
        public string RoleId { get; set; }
        public Role Role { get; set; } = default!;

        public int PermissionId { get; set; }
        public Permission Permission { get; set; } = default!;
    }
}
