using MediatR;
using Microsoft.EntityFrameworkCore;
using Vocabu.API.Common;
using Vocabu.DAL.Entities;
using Vocabu.Domain.DTOs.Games;
using Vocabu.Domain.Interfaces;

namespace Vocabu.API.Features.Player;

public class GetPrepositionsQuery : IRequest<ApiResponse>
{
    public Guid Id { get; set; }

    public class GetPrepositionsQueryHandler : IRequestHandler<GetPrepositionsQuery, ApiResponse>
    {
        private readonly ILogger<GetPrepositionsQueryHandler> _log;
        private readonly IRepository<WordTypeOfSpeech> _wordTypeOfSpeechRepo;

        public GetPrepositionsQueryHandler(ILogger<GetPrepositionsQueryHandler> loggerService, 
                IRepository<WordTypeOfSpeech> wordRepo)
        {
            _log = loggerService;
            _wordTypeOfSpeechRepo = wordRepo;
        }

        public async Task<ApiResponse> Handle(GetPrepositionsQuery request, CancellationToken ct)
        {
            List<PrepositionWordToImage> lista = new List<PrepositionWordToImage>();

            var listPrepositionWords = await _wordTypeOfSpeechRepo.AsQueryable().AsNoTracking()
                .Include(i => i.Word)
                .FirstOrDefaultAsync(f => f.PartOfSpeech == Domain.Enums.PartsOfSpeech.Preposition);    

            if (listPrepositionWords == null)
                _log.LogError(string.Format("GetPrepositionsQuery is null for request: Id={0}", request.Id));

            return ApiResponse<IEnumerable<PrepositionWordToImage>>.Ok(lista);
        }
    }
}