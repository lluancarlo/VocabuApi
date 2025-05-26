using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vocabu.API.Common;
using Vocabu.DAL.Entities;
using Vocabu.Domain.Interfaces;

namespace Vocabu.API.Features.Profile;

public class AddPlayerExperienceCommand : IRequest<ApiResponse>
{
    public Guid UserId { get; set; }
    public int Experience { get; set; }

    public class ChangePasswordCommandHandler : IRequestHandler<AddPlayerExperienceCommand, ApiResponse>
    {
        public readonly UserManager<User> _userManager;

        public ChangePasswordCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApiResponse> Handle(AddPlayerExperienceCommand command, CancellationToken ct)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(f => f.Id == command.UserId);

            if (user is null)
                return ApiResponse.NotFound("User not found.");
    
            user.Experience += command.Experience;

            return ApiResponse<User>.Ok(user);
        }
    }

    public class ChangePasswordCommandValidator : AbstractValidator<AddPlayerExperienceCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(p => p.Experience)
                .GreaterThan(0)
                .WithMessage("Experience to add should be greater than zero.");
        }
    }
}
