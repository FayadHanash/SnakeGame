using SnakeGame.Models;

namespace SnakeGame.Services;

/// <summary>
/// A service that opens new windows
/// </summary>
public interface INewWindowService
{
    NumberOfPlayers ShowWindow(object viewModel);
    void CloseWindow();
}