using MediatR;
using VocabuApi.Common;
using VocabuApi.Services;

namespace VocabuApi.Features.Auth;

public class LoginCommand : IRequest<ApiResponse<string>>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse<string>>
    {
        private readonly IJwtService _jwtService;

        public LoginCommandHandler(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public async Task<ApiResponse<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Simulação de autenticação (substitua por lógica real)
            if (request.Email == "user@example.com" && request.Password == "password")
            {
                var token = await _jwtService.GenerateToken(request.Email);
                return ApiResponse<string>.Ok(token);
            }

            throw new UnauthorizedAccessException("Credenciais inválidas");
        }
    }
}
