using Application.Store.AdminSection.PermissionService.Commands;
using MediatR;
using Persistence.Contexts;

public class UnAssignPermissionsCommandHandler : IRequestHandler<UnAssignPermissionsCommand>
{
    private readonly DataBaseContext _context;

    public UnAssignPermissionsCommandHandler(DataBaseContext context)
    {
        _context = context;
    }

    public async Task Handle(UnAssignPermissionsCommand request, CancellationToken cancellationToken)
    {
        _context.RolePermissions.RemoveRange(request.RolePermissions);
        await _context.SaveChangesAsync();

        await Task.CompletedTask;
    }
}
