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

    public class LoginCommandHandler : IRequestHandler<LoginCommand, CommandResponse>
    {
        private readonly JwtService jwtService;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;

        public LoginCommandHandler(JwtService _jwtService,
            SignInManager<User> _signInManager,
            UserManager<User> _userManager)
        {
            jwtService = _jwtService;
            signInManager = _signInManager;
            userManager = _userManager;
        }

        public async Task<CommandResponse> Handle(LoginCommand command, CancellationToken ct)
        {
            var validatorResult = new LoginCommandValidator().Validate(command);
            if (!validatorResult.IsValid)
                return CommandResponse.ValidatorError(validatorResult.Errors.Select(s => s.ErrorMessage));

            var user = await userManager.FindByEmailAsync(command.Email);
            if (user == null)
                return CommandResponse.NotFound("User does not exist");

            var result = await signInManager.CheckPasswordSignInAsync(user, command.Password, false);

            if (!result.Succeeded)
                return CommandResponse.Unauthorized("Error while login: " + result.ToString());

            var roles = await userManager.GetRolesAsync(user);
            var serviceReponse = jwtService.GenerateToken(user.Id, user.Email!, roles);

            if (!serviceReponse.Success)
                return CommandResponse.Error("Error while login: " + result.ToString(), System.Net.HttpStatusCode.InternalServerError);

            return CommandResponse<string>.Ok(serviceReponse.Data!, "Login completed successfully.");
        }
    }

    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty()
                .MaximumLength(254)
                .EmailAddress()
                .WithMessage("Email is invalid");

            RuleFor(p => p.Password)
                .NotEmpty()
                .MaximumLength(64)
                .WithMessage("Password is invalid");
        }
    }
}
