using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SnakeGame.Models;
using SnakeGame.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace SnakeGame.ViewModels;

public partial class MainViewModel : ObservableObject
{
    #region Properties
    /// <summary>
    /// A property that represents the author
    /// </summary>
    [ObservableProperty]
    private string? author;

    /// <summary>
    /// A property that represents the entities list
    /// </summary>
    [ObservableProperty]
    private List<Entity>? itemsSource;

    /// <summary>
    /// A property that represents the player 1
    /// </summary>
    [ObservableProperty]
    private string player1;

    /// <summary>
    /// A property that represents the player 2
    /// </summary>
    [ObservableProperty] 
    private string player2;

    /// <summary>
    /// A property that represents the player 3
    /// </summary>
    [ObservableProperty] 
    private string player3;

    /// <summary>
    /// A property that represents the canvas' width
    /// </summary>
    [ObservableProperty]
    private double canvasWidth;

    /// <summary>
    /// A property that represents the canvas' height
    /// </summary>
    [ObservableProperty]
    private double canvasHeight;

    /// <summary>
    /// A property that represents the visibility of overlay border
    /// </summary>
    [ObservableProperty]
    private Visibility overlayBorder;

    /// <summary>
    /// A property that represents the overlay text
    /// </summary>
    [ObservableProperty]
    private string overlayText;

    /// <summary>
    /// A property that represents whether the start button is enabled
    /// </summary>
    [ObservableProperty]
    private bool isStartButtonEnabled;

    /// <summary>
    /// A property that represents whether the stop button is enabled
    /// </summary>
    [ObservableProperty]
    private bool isStopButtonEnabled;
    /// <summary>
    /// A field that represents the number of players
    /// </summary>
    private NumberOfPlayers NumberOfPlayer = NumberOfPlayers.one;
    /// <summary>
    /// A field that represents the window object
    /// </summary>
    private Window window;
    /// <summary>
    /// A field that represents whether the game is running
    /// </summary>
    private bool IsRunning;
    
    /// <summary>
    /// a field that represents the NewWindowService object
    /// </summary>
    private readonly INewWindowService _windowService = new NewWindowService();

    /// <summary>
    /// a field that represents the engine object
    /// </summary>
    private Engine _engine;

    /// <summary>
    /// a ffield that represents the speed
    /// </summary>
    private int speed;

    #endregion

    #region Constructor 
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="window"></param>
    public MainViewModel(Window window)
    {
        this.window = window;
        this.window.Width = 800;
        this.window.Height = 600;
        CanvasHeight = 450;
        CanvasWidth = 800;
        Initialize();
    }

    #endregion

    #region Methods
    /// <summary>
    /// Method that loops the game
    /// </summary>
    /// <returns></returns>
    private async Task Loop()
    {
        try
        {
            while (IsRunning)
            {
                await _engine.Run();
                await Task.Delay(speed);
            }
        }
        catch (Exception ex) 
        {
            MessageBox.Show(ex.ToString());
        }
    }

    /// <summary>
    /// Method that initializes the GUI and game
    /// </summary>
    private void Initialize()
    {
        try
        {
            Author = "Snake Game By Fayad";
            OverlayBorder = Visibility.Hidden;
            OverlayText = string.Empty;
            _engine = new(NumberOfPlayer, CanvasWidth, CanvasHeight);
            IsRunning = true;
            speed = 300;
            Loop();
            ItemsSource = _engine.ItemsSource;

            _engine.ItemsChanged += _engine_ItemsChanged;
            _engine.ScoreChanged += _engine_ScoreChanged;
            _engine.IsGameOver += _engine_IsGameOver;
            _engine.SpeedChanged += _engine_SpeedChanged;

            this.window.PreviewKeyDown += Window_PreviewKeyDown;
            this.window.SizeChanged += Window_SizeChanged;

            IsStartButtonEnabled = true;
            IsStopButtonEnabled = true;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.ToString()); 
        }
    }

    /// <summary>
    /// Method that updates the scores
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void _engine_ScoreChanged(object? sender, ScoreTracker e)
    {
        switch (e.Id)
        {
            case 1:
                Player1 = $"Player 1\nScore: {e.Score}\nKeys: Up, Right, Down, Left";
                break;
            case 2:
                Player2 = $"Player 2\nScore: {e.Score}\nKeys: W, D, S, A";
                break;
            case 3:
                Player3 = $"Player 3\nScore: {e.Score}\nKeys: I, L, K, J";
                break;
        }
    }

    /// <summary>
    /// Method that adjusts the speed
    /// </summary>
    /// <param name="speed"></param>
    private void _engine_SpeedChanged(int speed)
    {
        this.speed = speed;
    }

    /// <summary>
    /// Method that adjusts the GUI when the game is over
    /// </summary>
    private void _engine_IsGameOver()
    {
        IsRunning = false;
        OverlayBorder = Visibility.Visible;
        var winners = _engine.GetWinners();
        string strOut = string.Empty;
        strOut = $"The winner is the player: {winners.Item1.Item1} with score: {winners.Item1.Item2}\n";
        if(winners.Item2.Item1 !=0)
        {
            strOut += $"The second winner is the player: {winners.Item2.Item1} with score: {winners.Item2.Item2}\n";
        }

        OverlayText += strOut;
        IsStartButtonEnabled = false;
        IsStopButtonEnabled = false;

    }

    /// <summary>
    /// Method that resizes the window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        CanvasWidth = (double)e.NewSize.Width;
        CanvasHeight =(double)e.NewSize.Height * 0.75;
        _engine.Width = CanvasWidth;
        _engine.Height = CanvasHeight;
    }

    /// <summary>
    /// Method that updates the entities 
    /// </summary>
    private void _engine_ItemsChanged()
    {
        ItemsSource = new();
        ItemsSource = _engine.ItemsSource;
    }

    /// <summary>
    /// Method that direct the players based on pressed keys
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        _engine.Window_PreviewKeyDown(sender, e);
    }
    #endregion

    #region Commands


    /// <summary>
    /// Method that initialize a new game
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    async Task NewGame()
    {
        NewViewModel nVmodel = new(_windowService);
        NumberOfPlayers NumberOfPlayers = _windowService.ShowWindow(nVmodel);
        this.NumberOfPlayer = NumberOfPlayers;
        Initialize();
        
    }

    /// <summary>
    /// Method that starts the game
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    async Task Start()
    {
        if (!IsRunning)
        {
            IsRunning = true;
            await Loop();
        }
    }

    /// <summary>
    /// Method that stops the game
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    async Task Stop()
    {
       IsRunning = false;
    }
    #endregion Commands



}
