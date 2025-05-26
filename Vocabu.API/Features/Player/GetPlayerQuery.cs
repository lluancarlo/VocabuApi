using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vocabu.API.Common;
using Vocabu.DAL.Entities;

namespace Vocabu.API.Features.Player;

public class GetPlayerQuery : IRequest<ApiResponse>
{
    public Guid Id { get; set; }

    public class GetPlayerQueryHandler : IRequestHandler<GetPlayerQuery, ApiResponse>
    {
        public readonly UserManager<User> _userManager;

        public GetPlayerQueryHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApiResponse> Handle(GetPlayerQuery request, CancellationToken ct) =>
            ApiResponse<User?>.Ok(await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(f => f.Id == request.Id));
    }
}