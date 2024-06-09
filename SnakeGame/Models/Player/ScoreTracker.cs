using System;

namespace SnakeGame.Models;

public class ScoreTracker : EventArgs
{
    #region Properties
    /// <summary>
    /// A property that represents the score
    /// </summary>
    public int Score {  get; set; }

    /// <summary>
    /// A property that represents the id
    /// </summary>
    public int Id { get; set; }
    #endregion
}
