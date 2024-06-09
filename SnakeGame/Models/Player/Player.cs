using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;

namespace SnakeGame.Models;

public class Player : Snake
{
    #region Properties
    /// <summary>
    /// A property that represents the id of a player 
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// A property that represents whether the player is alive
    /// </summary>
    public bool IsAlive { get; private set; }

    /// <summary>
    /// A property that represents the score
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// A field that represents the direction keys of a player
    /// </summary>

    private PlayerKeys keys;

    /// <summary>
    /// A field that represents the random obejct 
    /// </summary>
    private Random random;

    #endregion
    #region Constructor
    /// <summary>
    /// Constructor 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="position"></param>
    /// <param name="color"></param>
    /// <param name="keys"></param>
    public Player(int id, double width, double height, Position position, Brush color, PlayerKeys keys)
    : base(new() { Position = position, Width = width, Height = height, Color = color }, new(20, 0))
    {
        Id = id;
        Score = 0;
        IsAlive = true;
        this.keys = keys;
        random = new();
    }
    #endregion

    #region Methods
    /// <summary>
    /// Method that moves the snake 
    /// Calls IsHit() to check whether the player hits itself 
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public override void Move(double width, double height)
    {
        if (IsAlive)
        {
            base.Move(width,height);
            IsAlive = !IsHit();
        }
    }


    /// <summary>
    /// Method that checks whether the snake eats a food and updates the score accordingly
    /// </summary>
    /// <param name="food"></param>
    /// <returns></returns>
    public override bool IsEat(Food food)
    {
        if (base.IsEat(food))
        {
            Score += food.FoodValue;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Method that redirects the snake based on the pressed key
    /// </summary>
    /// <param name="key"></param>
    public void Redirect(KeyEventArgs key) => Redirect(keys.GetDirection(key));

    /// <summary>
    /// Method that redirects the snake 
    /// </summary>
    /// <param name="dir"></param>
    public void Redirect(Directions dir)
    {
        if (Vector.X == 0)
        {
            switch (dir)
            {
                case Directions.Right:
                    Vector = new Vector(Width, 0);
                    break;
                case Directions.Left:
                    Vector = new Vector(-Width, 0);
                    break;
                default:
                    break;
            }
        }
        if (Vector.Y == 0)
        {
            switch (dir)
            {
                case Directions.Up:
                    Vector = new Vector(0, -Height);
                    break;
                case Directions.Down:
                    Vector = new Vector(0, Height);
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Method that checks whether the payer hits itself
    /// </summary>
    /// <returns></returns>
    public bool IsHit()
    {
        IEnumerator<SnakeEntity> enumerator = Segments.GetEnumerator();
        if (enumerator.MoveNext())
        {
            var head = enumerator.Current;
            while (enumerator.MoveNext())
            {
                if (CheckPositions(head.Position, enumerator.Current.Position, 5))
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Method that checks whether the player collides with another player
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public bool IsCollide(Player player)
    {
        bool isCollide = checkcollide(player.Segments);
        IsAlive = !isCollide;

        bool checkcollide(LinkedList<SnakeEntity> segments)
        {
            IEnumerator<SnakeEntity> enumerator = segments.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (CheckPositions(Head.Position, enumerator.Current.Position, Head.Width / 2 + enumerator.Current.Width / 2))
                {
                    return true;
                }
            }
            return false;
        }
        return isCollide;
    }
    #endregion
}
