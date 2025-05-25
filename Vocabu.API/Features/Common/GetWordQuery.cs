using MediatR;
using Vocabu.API.Common;
using Vocabu.DAL.Entities;

namespace Vocabu.API.Features.Common;

public class GetWordQuery : IRequest<ApiResponse>
{
    public required string Word { get; set; }

    public class GetWordQueryHandler : IRequestHandler<GetWordQuery, ApiResponse>
    {
        public async Task<ApiResponse> Handle(GetWordQuery request, CancellationToken ct)
        {
            return CommandResponse<Word>.Ok("Not implemented");
        }
    }
}