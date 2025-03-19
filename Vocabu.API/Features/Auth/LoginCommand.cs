using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Vocabu.API.Common;
using Vocabu.BL.Services;
using Vocabu.DAL.Entities;

namespace Vocabu.API.Features.Auth;

public class LoginCommand : IRequest<CommandResponse>
{
    public required string Email { get; set; }
    public required string Password { get; set; }

    public class LoginCommandHandler(JwtService _jwtService, SignInManager<User> _signInManager, UserManager<User> _userManager) 
        : IRequestHandler<LoginCommand, CommandResponse>
    {
        public async Task<CommandResponse> Handle(LoginCommand command, CancellationToken ct)
        {
            var validatorResult = new LoginCommandValidator().Validate(command);
            if (!validatorResult.IsValid)
                return CommandResponse.ValidatorError(validatorResult.Errors.Select(s => s.ErrorMessage));

            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user == null)
                return CommandResponse.NotFound("User does not exist");

            var result = await _signInManager.CheckPasswordSignInAsync(user, command.Password, false);

            if (!result.Succeeded)
                return CommandResponse.Unauthorized("Error while login: " + result.ToString());

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user.Id, user.Email!, roles);

            return CommandResponse<string>.Ok(token, "Login completed successfully.");
        }
    }

    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty()
                .MaximumLength(254)
                .EmailAddress();

            RuleFor(p => p.Password)
                .NotEmpty()
                .MaximumLength(64);
        }
    }
}
