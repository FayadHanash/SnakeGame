
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace SnakeGame.Models;
#region Delegates
/// <summary>
/// A delegate for an event that notifies when the items have changed 
/// </summary>
public delegate void ItemsChanged();

/// <summary>
/// A delegate for an event that notifies when the game is over
/// </summary
public delegate void GameOver();

/// <summary>
/// A delegate for an event that notifies when the speed has changed
/// </summary
public delegate void Speed(int speed);

#endregion
public class Engine
{
    #region Properties
    /// <summary>
    /// A property that represents a list of entities
    /// </summary>
    public List<Entity>? ItemsSource { get; private set; }

    /// <summary>
    /// A field that represents a list of players
    /// </summary>
    private List<Player> players;

    /// <summary>
    /// A field that represents a list of foods
    /// </summary>
    private List<Food> foods;

    /// <summary>
    /// A field that represents the random object
    /// </summary>
    private Random random;

    /// <summary>
    /// A field that represents the timer
    /// </summary>
    private Timer timer;

    /// <summary>
    /// A property that represents the width of canvas
    /// </summary>
    public double Width;

    /// <summary>
    /// A property that represents the height canvas
    /// </summary>
    public double Height;

    /// <summary>
    /// A field that represents number of players
    /// </summary>
    private NumberOfPlayers playerNum;
    #endregion
    #region Events
    /// <summary>
    /// An event that is raised when the items have changed 
    /// </summary>
    public event ItemsChanged ItemsChanged = delegate { };

    /// <summary>
    /// An event that is raised when the game is over
    /// </summary
    public event GameOver IsGameOver = delegate { };

    /// <summary>
    /// An event that is raised when the speed has changed
    /// </summary
    public event Speed SpeedChanged = delegate { };

    /// <summary>
    /// An event handler that is raised when the score has changed
    /// </summary
    public event EventHandler<ScoreTracker> ScoreChanged = delegate { };
    #endregion
    #region Constructor
    /// <summary>
    /// Constructor
    /// </summary>
    public Engine(NumberOfPlayers PlayersNum,double width, double height)
    {
        this.Width = width;
        this.Height = height;
        this.playerNum = PlayersNum;
        random = new Random();
        ItemsSource = new();
        (players,foods) = Initialize();
        timer = new Timer();
        timer.Interval = 10000;
        timer.Elapsed += Timer_Elapsed;
        OnItemsChanged();
    }
    #endregion

    #region Methods
    /// <summary>
    /// Method that is raised when the timer has elapsed 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        OnSpeed(300);
        timer.Stop();
    }

    /// <summary>
    /// Method that initializes the foods and players
    /// </summary>
    /// <returns></returns>
    private (List<Player>,List<Food>) Initialize()
    {
        List<Player?> tempPlayers = new();
        List<Food?> tempFoods = new();

        for (int i = 0; i < (int)playerNum; i++)
        {
            tempPlayers.Add(PlayerFactory.CreatePlayer((NumberOfPlayers)i+1));
            int offset = (i + 1) * 10;
            tempFoods.Add(new(new(random.Next(offset, (int)(Width - 2 * offset )), random.Next(offset, (int)(Height - 2 * offset)))));
        }
        return (tempPlayers,tempFoods);
    }

    /// <summary>
    /// Method that triggers the ItemsChanged event
    /// </summary>
    public void OnItemsChanged() => ItemsChanged();

    /// <summary>
    /// Method that triggers the ScoreChanged event
    /// </summary>
    public void OnScoreChanged(ScoreTracker st)
    {
        if(st != null) ScoreChanged(this, st);  
    }

    /// <summary>
    /// Method that triggers the ScoreChanged event
    /// </summary>
    public void OnScoreChanged()
    {
        for (int i = 0; i < (int)playerNum; i++)
        {
            OnScoreChanged(new() { Score = players[i].Score, Id = i + 1 });
        }
    }

    /// <summary>
    /// Method that triggers the GameOver event
    /// </summary>
    public void OnGameOver() => IsGameOver();

    /// <summary>
    /// Method that triggers the SpeedChanged event
    /// </summary>
    public void OnSpeed(int speed) => SpeedChanged(speed);
    
    /// <summary>
    /// Method that checks whether all players are alive
    /// </summary>
    /// <returns></returns>
    private bool IsPlayersAlive()
    {
        foreach (var player in players) 
        {
            if (player.IsAlive)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Method that updates the Entities list 
    /// </summary>
    private void Update()
    {
        if (ItemsSource == null)
        {
            ItemsSource = new();
        }
        else
        {
            ItemsSource.Clear();
        }

        players.ForEach(p => 
        {
            IEnumerator<SnakeEntity> enumerator = p.Segments.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ItemsSource.Add(enumerator.Current);
            }
        });

        foods.ForEach(f => { ItemsSource.Add(f); });
    }

    /// <summary>
    /// Method that runs the game
    /// </summary>
    /// <returns></returns>
    public async Task Run()
    {
        Collision();
        foreach (var player in players)
        {
            if (player.IsAlive)
            {
                player.Move(Width, Height);

                foreach (var food in foods)
                {
                    if (player.IsEat(food))
                    {
                        food.GenerateNewFood(new Position(random.Next(20, (int)Width - 40), random.Next(20, (int)Height - 40)));
                        
                        if (food.FoodType == FoodTypes.Speed)
                        {

                            OnSpeed(100);
                            timer.Start();
                        }
                        else if (food.FoodType == FoodTypes.Slow)
                        {
                            OnSpeed(500);
                            timer.Start();
                        }
                    }
                }
            }
            else
            {
                player.Segments.Clear();
            }
        }
 
        OnItemsChanged();
        OnScoreChanged();
        Update();
        if (!IsPlayersAlive())
        {
            OnGameOver();
        }
    }

    /// <summary>
    /// Method that direct the players based on pressed keys
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        foreach (var player in players)
        {
            player.Redirect(e);
        }
    }

    /// <summary>
    /// Method that checks collisions
    /// </summary>
    private void Collision()
    {
        if(players.Count < 2) return;
        for (int i = 0; i < players.Count; i++) 
        {
            if (!players[i].IsAlive) continue;
            
            for (int j = i + 1; j < players.Count; j++)
            {
                if(!players[j].IsAlive) continue;
                bool collision = players[i].IsCollide(players[j]);
                if (collision)
                {
                    if (players[i].IsAlive && !players[j].IsAlive)
                    {
                        players[i].Score += 5;
                        OnScoreChanged();
                    }
                    else if (!players[i].IsAlive && players[j].IsAlive)
                    {
                        players[j].Score += 5;
                        OnScoreChanged();
                    }
                }
            } 
        }
    }

    /// <summary>
    /// Method that returns the winners
    /// </summary>
    /// <returns></returns>
   public ((int,int),(int,int)) GetWinners()
   {
        (int, int) winner = (0, 0);
        (int, int) winner2 =(0,0);
        if (playerNum >= 0)
        {
            var sorted = players.OrderByDescending(x => x.Score).ToList();

            winner = (sorted[0].Id, sorted[0].Score);


            if (playerNum == NumberOfPlayers.two || playerNum == NumberOfPlayers.three)
            {
                winner2 = (sorted[1].Id, sorted[1].Score);
            } 
        }

        return (winner, winner2);
   }
    #endregion
}
