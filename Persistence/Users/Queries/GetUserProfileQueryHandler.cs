using Application.Commons.Services.UserService.Queries;
using MediatR;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Users.Queries
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery,ProfileDto>
    {
        private readonly DataBaseContext _context;
        public GetUserProfileQueryHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<ProfileDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            //get from db
            var profile = (await _context.Users.FindAsync(request.UserId));
            var profileDto = new ProfileDto()
            {
                Id = profile.Id,
                PhoneNumber = profile.PhoneNumber,
                Email = profile.Email,
                FullName = profile.FullName,
            };

            return profileDto;
        }
    }
}
