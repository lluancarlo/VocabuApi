using MediatR;
using Vocabu.API.Common;
using VocabuApi.Services;

namespace Vocabu.API.Features.Auth;

public class LoginCommand : IRequest<ApiResponse<string>>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse<string>>
    {
        private readonly JwtService _jwtService;

        public LoginCommandHandler(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public async Task<ApiResponse<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Simulação de autenticação (substitua por lógica real)
            if (request.Email == "admin" && request.Password == "123")
            {
                var token = await _jwtService.GenerateToken(request.Email);
                return ApiResponse<string>.Ok(token);
            }

            return ApiResponse<string>.Error("Invalid credentials");
        }
    }
}
