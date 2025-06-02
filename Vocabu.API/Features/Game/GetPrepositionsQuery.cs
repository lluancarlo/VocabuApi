using MediatR;
using Microsoft.EntityFrameworkCore;
using Vocabu.API.Common;
using Vocabu.DAL.Entities;
using Vocabu.Domain.DTOs.Games;
using Vocabu.Domain.Interfaces;

namespace Vocabu.API.Features.Player;

public class GetPrepositionsQuery : IRequest<ApiResponse>
{
    public int LanguageId { get; set; }

    public class GetPrepositionsQueryHandler : IRequestHandler<GetPrepositionsQuery, ApiResponse>
    {
        private readonly ILogger<GetPrepositionsQueryHandler> _log;
        private readonly IRepository<WordPreposition> _wordPrepositionRepo;

        public GetPrepositionsQueryHandler(ILogger<GetPrepositionsQueryHandler> loggerService, 
                IRepository<WordPreposition> wordRepo)
        {
            _log = loggerService;
            _wordPrepositionRepo = wordRepo;
        }

        public async Task<ApiResponse> Handle(GetPrepositionsQuery request, CancellationToken ct)
        {
            var listPrepositionsReturn = new List<PrepositionWord>();

            var listWordPrepositions = await _wordPrepositionRepo.AsQueryable().AsNoTracking()
                .Include(i => i.Word)
                .Where(w => w.Word.LanguageId == request.LanguageId)
                .ToListAsync();

            var rnd = new Random();
            listWordPrepositions = listWordPrepositions.OrderBy(i => rnd.Next()).Take(4).ToList();

            for (int i = 0; i < listWordPrepositions.Count; i++)
            {
                var word = PrepositionWord.FromWordEntity(listWordPrepositions[i].Word!);

                if (i == 0)
                    word.isCorrect = true;

                listPrepositionsReturn.Add(word);
            }

            if (listPrepositionsReturn.Count == 0)
            {
                _log.LogError(string.Format("GetPrepositionsQuery is null for request: Id={0}", request.LanguageId));
                return ApiResponse.BadRequest("Id not found");
            }

            return ApiResponse<IEnumerable<PrepositionWord>>.Ok(listPrepositionsReturn);
        }
    }
}