using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Vocabu.API.Common;
using Vocabu.BL.Services;
using Vocabu.DAL.Entities;

namespace Vocabu.API.Features.Auth;

public class SignInCommand : IRequest<ApiResponse>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Name { get; set; }
    public int CountryId { get; set; }

    public class SignInCommandHandler : IRequestHandler<SignInCommand, ApiResponse>
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

        public async Task<ApiResponse> Handle(SignInCommand command, CancellationToken ct)
        {
            var validatorResult = new SignInCommandValidator().Validate(command);
            if (!validatorResult.IsValid)
                return ApiResponse.ValidatorError(validatorResult.Errors.Select(s => s.ErrorMessage));

            var user = await userManager.FindByEmailAsync(command.Email);
            if (user != null)
                return ApiResponse.Conflict("Email already in use");

            user = new User
            {
                Name = command.Name,
                UserName = command.Email,
                Email = command.Email,
                CountryId = command.CountryId,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, command.Password);

            if (!result.Succeeded)
                return ApiResponse.InternalServerError("Error while creating user: " + string.Join(" | ", result.Errors.Select(s => $"{s.Code} - {s.Description}")));

            var signInResult = await signInManager.PasswordSignInAsync(user, command.Password, false, false);
            if (!signInResult.Succeeded)
            {
                if (signInResult.IsLockedOut)
                    return ApiResponse.Forbidden("User account is locked.");
                if (signInResult.IsNotAllowed)
                    return ApiResponse.Forbidden("User is not allowed to sign in.");
                return ApiResponse.Unauthorized("Invalid login attempt.");
            }

            var roles = await userManager.GetRolesAsync(user);
            var serviceReponse = jwtService.GenerateToken(user.Id, user.Email, roles);

            if (!serviceReponse.Success)
                return ApiResponse.Error("Error while login: " + result.ToString(), System.Net.HttpStatusCode.InternalServerError);

            return ApiResponse<string>.Ok(serviceReponse.Data!, "Login completed successfully.");
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
