using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Vocabu.API.Common;
using Vocabu.BL.Services;
using Vocabu.DAL.Entities;

namespace Vocabu.API.Features.Auth;

public class LoginCommand : IRequest<ApiResponse>
{
    public required string Email { get; set; }
    public required string Password { get; set; }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse>
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly JwtService _jwtService;

        public LoginCommandHandler(JwtService jwtService, SignInManager<User> signInManager, 
            UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<ApiResponse> Handle(LoginCommand command, CancellationToken ct)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user == null)
                return ApiResponse.Error("User does not exist");

            var result = await _signInManager.PasswordSignInAsync(user.Email, command.Password, false, false);

            if (!result.Succeeded)
                return ApiResponse.Error("Error while login: " + result.ToString());

            var token = _jwtService.GenerateToken(user.Id, user.Email);
            return ApiResponse<string>.Ok(token);
        }
    }

    public class SignInCommandValidator : AbstractValidator<LoginCommand>
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
        }
    }
}
