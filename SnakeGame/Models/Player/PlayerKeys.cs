
using System.Windows.Input;

namespace SnakeGame.Models;

public class PlayerKeys
{
    #region Properties
    /// <summary>
    /// A property that represents the Up key
    /// </summary>
    public Key Up { get; set; }

    /// <summary>
    /// A property that represents the Down key
    /// </summary>
    public Key Down { get; set; }

    /// <summary>
    /// A property that represents the Right key
    /// </summary>
    public Key Right { get; set; }

    /// <summary>
    /// A property that represents the Left key
    /// </summary>
    public Key Left { get; set; }

    #endregion

    #region Methods
    /// <summary>
    /// Method that returns a direction 
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public Directions GetDirection(KeyEventArgs e) => e.Key switch
    {
        Key key when key == Up => Directions.Up,
        Key key when key == Down => Directions.Down,
        Key key when key == Right => Directions.Right,
        Key key when key == Left => Directions.Left,
        _ => Directions.None,
    };

    #endregion
}

