using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Vocabu.API.Common;
using Vocabu.BL.Services;
using Vocabu.DAL.Entities;

namespace Vocabu.API.Features.Profile;

public class ChangePasswordCommand : IRequest<CommandResponse>
{
    public required string Email { get; set; }
    public required string CurrentPassword { get; set; }
    public required string NewPassword { get; set; }

    public class ChangePasswordCommandHandler(JwtService _jwtService, SignInManager<User> _signInManager,  UserManager<User> _userManager) 
        : IRequestHandler<ChangePasswordCommand, CommandResponse>
    {
        public async Task<CommandResponse> Handle(ChangePasswordCommand command, CancellationToken ct)
        {
            var validatorResult = new ChangePasswordCommandValidator().Validate(command);
            if (!validatorResult.IsValid)
                return CommandResponse.ValidatorError(validatorResult.Errors.Select(s => s.ErrorMessage));
    
            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user == null)
                return CommandResponse.NotFound("User not found.");

            var passwordCheck = await _userManager.CheckPasswordAsync(user, command.CurrentPassword);
            if (!passwordCheck)
                return CommandResponse.BadRequest("Current password is incorrect.");

            var result = await _userManager.ChangePasswordAsync(user, command.CurrentPassword, command.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return CommandResponse.InternalServerError($"Failed to change password: {errors}");
            }

            // Optional: Sign the user out everywhere or force re-login
            await _signInManager.RefreshSignInAsync(user);

            return CommandResponse.Ok("Password changed successfully.");
        }
    }

    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty()
                .MaximumLength(254)
                .EmailAddress();

            RuleFor(p => p.CurrentPassword)
                .NotEmpty()
                .MaximumLength(64);

            RuleFor(p => p.NewPassword)
                .NotEmpty()
                .MaximumLength(64);
        }
    }
}
