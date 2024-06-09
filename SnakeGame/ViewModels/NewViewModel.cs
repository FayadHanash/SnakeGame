using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SnakeGame.Models;
using SnakeGame.Services;

namespace SnakeGame.ViewModels;

internal partial class NewViewModel :ObservableObject
{
    #region Properties
    /// <summary>
    /// A property that represents the number of players
    /// </summary>
    public NumberOfPlayers NumberOfPlayers { get; private set; } = 0;
    /// <summary>
    /// a field that represents the INewWindowService object
    /// </summary>
    private readonly INewWindowService _windowService;

    #endregion

    #region Constructor
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="windowService"></param>
    public NewViewModel(INewWindowService windowService)
    {
        _windowService = windowService;
    }
    #endregion
    #region Methods
    /// <summary>
    /// Method that sets the number of players to one and closes the window
    /// </summary>
    [RelayCommand]
    async void OnePlayer()
    {
        NumberOfPlayers = NumberOfPlayers.one;
        _windowService.CloseWindow();
    }

    /// <summary>
    /// Method that sets the number of players to two and closes the window
    /// </summary>
    [RelayCommand]
    async void TwoPlayers()
    {
        NumberOfPlayers = NumberOfPlayers.two;
        _windowService.CloseWindow();
    }

    /// <summary>
    /// Method that sets the number of players to three and closes the window
    /// </summary>
    [RelayCommand]
    async void ThreePlayers()
    {
        NumberOfPlayers = NumberOfPlayers.three;
        _windowService.CloseWindow();
    }

    #endregion
}
