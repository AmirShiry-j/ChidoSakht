using Application.PermissionService.Commands;
using Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System.Threading;
using System.Threading.Tasks;

public class AssignPermissionsCommandHandler : IRequestHandler<AssignPermissionsCommand>
{
    private readonly DataBaseContext _context;

    public AssignPermissionsCommandHandler(DataBaseContext context)
    {
        _context = context;
    }

    public async Task Handle(AssignPermissionsCommand request, CancellationToken cancellationToken)
    {
        //Map
        var rolePermissions = request.PermissionIds.Select(p => new
        RolePermission
        {
            RoleId = request.RoleId,
            PermissionId = p
        })
            .ToList();

        await _context.RolePermissions.AddRangeAsync(rolePermissions);
        await _context.SaveChangesAsync();

        await Task.CompletedTask;
    }
}
