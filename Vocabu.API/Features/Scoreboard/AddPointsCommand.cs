using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vocabu.API.Common;
using Vocabu.DAL.Entities;
using Vocabu.Domain.Interfaces;

namespace Vocabu.API.Features.Auth;

public class AddPointsCommand : IRequest<CommandResponse>
{
    public required Guid UserId { get; set; }
    public required Guid GameId { get; set; }
    public required int Points { get; set; }

    public class AddPointsCommandHandler : IRequestHandler<AddPointsCommand, CommandResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IRepository<Game> _gameRepo;
        private readonly IRepository<Score> _scoreRepo;
        private readonly IRepository<ScoreTransaction> _scoreTransactionRepo;

        public AddPointsCommandHandler(
            UserManager<User> userManager,
            IRepository<Game> gameRepo,
            IRepository<Score> scoreRepo,
            IRepository<ScoreTransaction> scoreTransactionRepo)
        {
            _userManager = userManager;
            _gameRepo = gameRepo;
            _scoreRepo = scoreRepo;
            _scoreTransactionRepo = scoreTransactionRepo;
        }

        public async Task<CommandResponse> Handle(AddPointsCommand command, CancellationToken ct)
        {
            var validatorResult = new AddPointsCommandValidator().Validate(command);
            if (!validatorResult.IsValid)
                return CommandResponse.ValidatorError(validatorResult.Errors.Select(s => s.ErrorMessage));

            if (!await _userManager.Users.AsNoTracking().AnyAsync(u => u.Id == command.UserId, ct))
                return CommandResponse.Error($"user {command.UserId} not found", System.Net.HttpStatusCode.NotFound);

            if (!await _gameRepo.AsQueryable().AsNoTracking().AnyAsync(a => a.Id == command.GameId, ct))
                return CommandResponse.Error($"game {command.GameId} not found", System.Net.HttpStatusCode.NotFound);

            var score = await _scoreRepo.AsQueryable()
                .FirstOrDefaultAsync(f => f.UserId.Equals(command.UserId) && f.GameId.Equals(command.GameId), ct);

            if (score == null)
            {
                score = new Score(command.Points, command.UserId, command.GameId);
                await _scoreRepo.AddAsync(score);
            }
            else
            {
                score.Points += command.Points;
                _scoreRepo.Update(score);
            }

            await _scoreTransactionRepo.AddAsync(new ScoreTransaction(command.Points, DateTime.Now, score.Id));

            await _scoreTransactionRepo.SaveChangesAsync();
            await _scoreRepo.SaveChangesAsync();

            return CommandResponse<Score>.Ok(score);
        }
    }

    public class AddPointsCommandValidator : AbstractValidator<AddPointsCommand>
    {
        public AddPointsCommandValidator()
        {
            RuleFor(p => p.UserId)
                .NotNull()
                .WithMessage("UserId cannot be null");

            RuleFor(p => p.GameId)
                .NotNull()
                .WithMessage("GameId cannot be null");

            RuleFor(p => p.Points)
                .NotEqual(0)
                .WithMessage("Points should be different than 0");
        }
    }
}
