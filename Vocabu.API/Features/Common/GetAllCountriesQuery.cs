using MediatR;
using Vocabu.API.Common;
using Vocabu.DAL.Entities;
using Vocabu.Domain.Interfaces;

namespace Vocabu.API.Features.Common;

public class GetAllCountriesQuery : IRequest<ApiResponse>
{
    public class GetAllCountriesQueryHandler : IRequestHandler<GetAllCountriesQuery, ApiResponse>
    {
        public readonly IRepository<Country> _countryRepo;

        public GetAllCountriesQueryHandler(IRepository<Country> countryRepo)
        {
            _countryRepo = countryRepo;
        }

        public async Task<ApiResponse> Handle(GetAllCountriesQuery request, CancellationToken ct) =>
            CommandResponse<IEnumerable<Country>>.Ok(await _countryRepo.GetAllAsync(false));
    }
}