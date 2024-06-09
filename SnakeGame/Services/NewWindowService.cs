using SnakeGame.Models;
using SnakeGame.ViewModels;
using SnakeGame.Views;
using System.Windows;

namespace SnakeGame.Services;

/// <summary>
/// A service that opens the NewWindow
/// </summary>
public class NewWindowService : INewWindowService
{
    /// <summary>
    /// The window that will be opened
    /// </summary>
    private Window _window;

    /// <summary>
    /// Method that shows the window and returns the result of the window as an enum
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    public NumberOfPlayers ShowWindow(object viewModel)
    {
        _window = new NewWindow();
        _window.DataContext = viewModel;
        _window.ShowDialog();
        return (_window.DataContext as NewViewModel).NumberOfPlayers;
    }

    /// <summary>
    /// Method that closes the window
    /// </summary>
    public void CloseWindow()
    {
        _window.Close();
    }
}


