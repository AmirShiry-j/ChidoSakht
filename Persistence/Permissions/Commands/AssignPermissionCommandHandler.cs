using Application.PermissionService.Commands;
using Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System.Threading;
using System.Threading.Tasks;

public class AssignPermissionCommandHandler : IRequestHandler<AssignPermissionCommand>
{
    private readonly DataBaseContext _context;

    public AssignPermissionCommandHandler(DataBaseContext context)
    {
        _context = context;
    }

    public async Task Handle(AssignPermissionCommand request, CancellationToken cancellationToken)
    {
        //Map
        var rolePermission = new RolePermission
        {
            RoleId = request.RoleId,
            PermissionId = request.PermissionId
        };

        _context.RolePermissions.Add(rolePermission);
        await _context.SaveChangesAsync();

        await Task.CompletedTask;
    }
}
