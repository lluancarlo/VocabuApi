using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Vocabu.API.Common;
using Vocabu.BL.Services;
using Vocabu.DAL.Entities;

namespace Vocabu.API.Features.Auth;

public class SignInCommand : IRequest<CommandResponse>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Name { get; set; }
    public string? CountryId { get; set; }

    public class SignInCommandHandler(JwtService _jwtService, SignInManager<User> _signInManager, UserManager<User> _userManager)
        : IRequestHandler<SignInCommand, CommandResponse>
    {
        public async Task<CommandResponse> Handle(SignInCommand command, CancellationToken ct)
        {
            var validatorResult = new SignInCommandValidator().Validate(command);
            if (!validatorResult.IsValid)
                return CommandResponse.ValidatorError(validatorResult.Errors.Select(s => s.ErrorMessage));

            var parseResult = Guid.TryParse(command.CountryId, out var countryId);
            if (!parseResult)
                return CommandResponse.Conflict("CountryId is not a valid guid.");

            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user != null)
                return CommandResponse.Conflict("Email already in use");

            user = new User
            {
                Name = command.Name,
                UserName = command.Email,
                Email = command.Email,
                CountryId = countryId,
            };

            var result = await _userManager.CreateAsync(user, command.Password);

            if (!result.Succeeded)
                return CommandResponse.InternalServerError("Error while creating user: " + string.Join(" | ", result.Errors.Select(s => $"{s.Code} - {s.Description}")));

            var signInResult = await _signInManager.PasswordSignInAsync(user, command.Password, false, false);
            if (!signInResult.Succeeded)
            {
                if (signInResult.IsLockedOut)
                    return CommandResponse.Forbidden("User account is locked.");
                if (signInResult.IsNotAllowed)
                    return CommandResponse.Forbidden("User is not allowed to sign in.");
                return CommandResponse.Unauthorized("Invalid login attempt.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user.Id, user.Email, roles);

            return CommandResponse<string>.Ok(token, "Login completed successfully.");
        }
    }

    public class SignInCommandValidator : AbstractValidator<SignInCommand>
    {
        public SignInCommandValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty()
                .MaximumLength(254)
                .EmailAddress();

            RuleFor(p => p.Password)
                .NotEmpty()
                .MaximumLength(64);

            RuleFor(p => p.Name)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
