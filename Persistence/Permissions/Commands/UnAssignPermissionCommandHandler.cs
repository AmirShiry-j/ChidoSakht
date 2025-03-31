using Application.PermissionService.Commands;
using MediatR;
using Persistence.Contexts;

public class UnAssignPermissionCommandHandler : IRequestHandler<UnAssignPermissionCommand>
{
    private readonly DataBaseContext _context;

    public UnAssignPermissionCommandHandler(DataBaseContext context)
    {
        _context = context;
    }

    public async Task Handle(UnAssignPermissionCommand request, CancellationToken cancellationToken)
    {
        _context.RolePermissions.Remove(request.RolePermission);
        await _context.SaveChangesAsync();

        await Task.CompletedTask;
    }
}
