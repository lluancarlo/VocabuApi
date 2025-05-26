using Vocabu.DAL.Entities;
using Vocabu.Domain.DTOs;
using Vocabu.Domain.Interfaces;

namespace Vocabu.BL.Services;

public class GameService
{
    private readonly IRepository<Word> _wordRepo;

    public GameService(IRepository<Word> wordRepo)
    {
        _wordRepo = wordRepo;
    }

    public ServiceResponse<IEnumerable<GamePreposition>> GetPrepositions(Guid userId, string email, ICollection<string> roles)
    {
        var gameList = new List<GamePreposition>();

        return ServiceResponse<IEnumerable<GamePreposition>>.Ok(gameList);
    }
}