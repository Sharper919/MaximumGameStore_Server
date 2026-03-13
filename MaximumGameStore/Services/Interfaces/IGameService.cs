using MaximumGameStore.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MaximumGameStore.Services.Interfaces
{
    public interface IGameService
    {
        // user functions
        Task<List<GameListItemDto>> GetGamesAsync(int take = 8);
        Task<List<GameListItemDto>> GetGamesByNameAsync(string name);
        Task<List<GameListItemDto>> FilterAsync(int? genreId,
            int? developerId, int? engineId,
            int? serieId, int? publisherId,
            int? modeId);
        Task<GameDetailsDto?> GetGameDetailsByIdAsync(int gameId);
        Task<List<GameSystemRequirementsDto>> GetGamesRequirementsByIdAsync(int gameId);
        Task<List<string>> GetGameImagesByIdAsync(int gameId);

        // admin functions
        Task<int?> CreateGameAsync(CreateGameDto dto);
        Task<bool> UpdateGameAsync(int gameId, CreateGameDto dto);
        Task<bool> DeleteGameAsync(int gameId);
    }
}
