using System;
using System.Windows.Media;
using System.Windows.Input;
namespace SnakeGame.Models;

public class PlayerFactory
{
    #region Properties
    /// <summary>
    /// A property that represents the segment width of a snake 
    /// </summary>
    public readonly static int SegmentWidth = 20;

    /// <summary>
    /// A property that represents the segment height of a snake 
    /// </summary>
    public readonly static int SegmentHeight = 20;
    #endregion

    #region Method
    /// <summary>
    /// Mathod that returns a player/snake with its color and control keys configured
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static Player? CreatePlayer(NumberOfPlayers num)
    {
        Player? player = null;
        Random random = new Random();

        switch (num)
        {
            case NumberOfPlayers.one:
                player = new(1, SegmentWidth, SegmentHeight, new(random.Next(0, 100), random.Next(0, 100)),
                Brushes.Green,
                new() { Up = Key.Up, Down = Key.Down, Right = Key.Right, Left = Key.Left });
                break;
            case NumberOfPlayers.two:
                player = new(2, SegmentWidth, SegmentHeight, new(random.Next(100, 200), random.Next(100, 200)),
                         Brushes.Cyan,
                         new() { Up = Key.W, Down = Key.S, Right = Key.D, Left = Key.A });
                break;
            case NumberOfPlayers.three:
                player = new(3, SegmentWidth, SegmentHeight, new(random.Next(200, 300), random.Next(200, 300)),
                         Brushes.LightGreen,
                         new() { Up = Key.I, Down = Key.K, Right = Key.L, Left = Key.J });
                break;
        }
        return player;
    }
    #endregion
}
