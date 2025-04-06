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

    public class SignInCommandHandler : IRequestHandler<SignInCommand, CommandResponse>
    {
        private readonly JwtService jwtService;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;

        public SignInCommandHandler(JwtService _jwtService, 
            SignInManager<User> _signInManager, 
            UserManager<User> _userManager)
        {
            jwtService = _jwtService;
            signInManager = _signInManager;
            userManager = _userManager;
        }

        public async Task<CommandResponse> Handle(SignInCommand command, CancellationToken ct)
        {
            var validatorResult = new SignInCommandValidator().Validate(command);
            if (!validatorResult.IsValid)
                return CommandResponse.ValidatorError(validatorResult.Errors.Select(s => s.ErrorMessage));

            var parseResult = Guid.TryParse(command.CountryId, out var countryId);
            if (!parseResult)
                return CommandResponse.Conflict("CountryId is not a valid guid.");

            var user = await userManager.FindByEmailAsync(command.Email);
            if (user != null)
                return CommandResponse.Conflict("Email already in use");

            user = new User
            {
                Name = command.Name,
                UserName = command.Email,
                Email = command.Email,
                CountryId = countryId,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, command.Password);

            if (!result.Succeeded)
                return CommandResponse.InternalServerError("Error while creating user: " + string.Join(" | ", result.Errors.Select(s => $"{s.Code} - {s.Description}")));

            var signInResult = await signInManager.PasswordSignInAsync(user, command.Password, false, false);
            if (!signInResult.Succeeded)
            {
                if (signInResult.IsLockedOut)
                    return CommandResponse.Forbidden("User account is locked.");
                if (signInResult.IsNotAllowed)
                    return CommandResponse.Forbidden("User is not allowed to sign in.");
                return CommandResponse.Unauthorized("Invalid login attempt.");
            }

            var roles = await userManager.GetRolesAsync(user);
            var serviceReponse = jwtService.GenerateToken(user.Id, user.Email, roles);

            if (!serviceReponse.Success)
                return CommandResponse.Error("Error while login: " + result.ToString(), System.Net.HttpStatusCode.InternalServerError);

            return CommandResponse<string>.Ok(serviceReponse.Data!, "Login completed successfully.");
        }
    }

    public class SignInCommandValidator : AbstractValidator<SignInCommand>
    {
        public SignInCommandValidator()
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

            RuleFor(p => p.Name)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Name is invalid");
        }
    }
}
