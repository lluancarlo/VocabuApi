using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Vocabu.API.Common;
using Vocabu.DAL.Entities;

namespace Vocabu.API.Features.Auth;

public class SignInCommand : IRequest<ApiResponse>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Name { get; set; }
    public Guid? CountryId { get; set; }

    public class SignInCommandHandler : IRequestHandler<SignInCommand, ApiResponse>
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public SignInCommandHandler(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<ApiResponse> Handle(SignInCommand command, CancellationToken ct)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user != null)
                return ApiResponse.Error("Email already in use");

            var result = await _userManager.CreateAsync(new User
            {
                Name = command.Name,
                UserName = command.Email,
                Email = command.Email,
                CountryId = command.CountryId,
            }, command.Password);

            if (!result.Succeeded)
                return ApiResponse.Error("Error while creating user: " 
                    + string.Join(" | ", result.Errors.Select(s=> $"{s.Code} - {s.Description}")));

            return ApiResponse.Ok();
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
