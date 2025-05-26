using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Vocabu.API.Common;
using Vocabu.BL.Services;
using Vocabu.DAL.Entities;

namespace Vocabu.API.Features.Profile;

public class UpdatePlayerCommand : IRequest<ApiResponse>
{
    public required string Email { get; set; }
    public required string CurrentPassword { get; set; }
    public required string NewPassword { get; set; }

    public class ChangePasswordCommandHandler : IRequestHandler<UpdatePlayerCommand, ApiResponse>
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;

        public ChangePasswordCommandHandler(SignInManager<User> _signInManager, 
            UserManager<User> _userManager)
        {
            signInManager = _signInManager;
            userManager = _userManager;
        }

        public async Task<ApiResponse> Handle(UpdatePlayerCommand command, CancellationToken ct)
        {
            var validatorResult = new ChangePasswordCommandValidator().Validate(command);
            if (!validatorResult.IsValid)
                return ApiResponse.ValidatorError(validatorResult.Errors.Select(s => s.ErrorMessage));
    
            var user = await userManager.FindByEmailAsync(command.Email);
            if (user == null)
                return ApiResponse.NotFound("User not found.");

            var passwordCheck = await userManager.CheckPasswordAsync(user, command.CurrentPassword);
            if (!passwordCheck)
                return ApiResponse.BadRequest("Current password is incorrect.");

            var result = await userManager.ChangePasswordAsync(user, command.CurrentPassword, command.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ApiResponse.InternalServerError($"Failed to change password: {errors}");
            }

            // Optional: Sign the user out everywhere or force re-login
            await signInManager.RefreshSignInAsync(user);

            return ApiResponse.Ok("Password changed successfully.");
        }
    }

    public class ChangePasswordCommandValidator : AbstractValidator<UpdatePlayerCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty()
                .MaximumLength(254)
                .EmailAddress()
                .WithMessage("Email is invalid");

            RuleFor(p => p.CurrentPassword)
                .NotEmpty()
                .MaximumLength(64)
                .WithMessage("CurrentPassword is invalid");

            RuleFor(p => p.NewPassword)
                .NotEmpty()
                .MaximumLength(64)
                .WithMessage("NewPassword is invalid");
        }
    }
}
