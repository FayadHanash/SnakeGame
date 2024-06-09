using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace SnakeGame.Models;
public abstract class Snake 
{
    #region Properties
    /// <summary>
    /// A property that represents the width (Segment width) 
    /// </summary>
    public double Width { get; init; }

    /// <summary>
    /// A property that represents the height (Segment height) 
    /// </summary>
    public double Height { get; init; }

    /// <summary>
    /// A property that represents the number of segements (Segments count) 
    /// </summary>
    public int SegmentCount { get; private set; }

    /// <summary>
    /// A property that represents the snake (segements)  
    /// </summary>
    public LinkedList<SnakeEntity> Segments { get; protected set; }

    /// <summary>
    /// A property that represents the snake's head 
    /// </summary>
    public SnakeEntity Head { get; private set; }

    /// <summary>
    /// A property that represents the vector 
    /// </summary>
    public Vector Vector { get; protected set; }
    #endregion

    #region Constructor
    /// <summary>
    /// Constructor with two parameters
    /// </summary>
    /// <param name="head"></param>
    /// <param name="vector"></param>
    public Snake(SnakeEntity head, Vector vector)
    {
        this.Head = head;
        this.Width = head.Width;
        this.Height = head.Height;
        this.Vector = vector;
        this.Segments = new LinkedList<SnakeEntity>();
        this.Segments.AddFirst(Head);
        this.SegmentCount = 1;
    }

    /// <summary>
    /// Constructor with five parameters
    /// </summary>
    /// <param name="position"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="color"></param>
    /// <param name="vector"></param>
    public Snake(Position position, double width, double height, Brush color, Vector vector)
    {
        this.Width = width;
        this.Height = height;
        this.Head = new() { Position = position, Width = width, Height = height, Color = color };
        this.Vector = vector;
        this.Segments = new LinkedList<SnakeEntity>();
        this.Segments.AddFirst(Head);
        this.SegmentCount = 1;
    }

    #endregion

    #region Method
    /// <summary>
    /// Method that moves the snake 
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public virtual void Move(double width, double height)
    {
        var prevPos = new Position(Head.Position.X, Head.Position.Y);
        Head.Position = new Position(Head.Position.X + Vector.X, Head.Position.Y + Vector.Y);

        if (Head.Position.X > width - Width)
        {
            Head.Position.X = 10;
        }
        else if (Head.Position.X <= 0 )
        {
            Head.Position.X = width - Width;
        }
        if (Head.Position.Y > height - Height)
        {
            Head.Position.Y = 10;
        }
        else if (Head.Position.Y < 10)
        {
            Head.Position.Y = height - Height;
        }

        foreach (var entity in this.Segments.Skip(1).ToList()) 
        {
            (entity.Position,prevPos) = (prevPos,entity.Position);
        }

    }

    /// <summary>
    /// Method that checks whether the snake eats a food and updates the score accordingly
    /// </summary>
    /// <param name="food"></param>
    /// <returns></returns>
    public virtual bool IsEat(Food food)
    {

        double headX = Head.Position.X + Head.Width / 2; 
        double headY = Head.Position.Y + Head.Height / 2; 

        double foodX = food.Position.X + food.Width /2;
        double foodY = food.Position.Y + food.Height /2;

       if(CheckPositions(new(headX,headY),new(foodX,foodY),Head.Width/2 + food.Width/2))
        {
            for (int i = 1; i <= food.SnakeSegment; i++)
            {
                var last = Segments.Last();
                Segments.AddLast(new SnakeEntity()
                {
                    Position = new Position(last.Position.X - Vector.X, last.Position.Y - Vector.Y),
                    Color = Head.Color,
                    Height = Head.Height,
                    Width = Head.Width,
                });
                SegmentCount++;
            }
            return true;
        }
        return false;
    }


    /// <summary>
    /// Method that checks whether the distance between two positions is less than a specified value 
    /// </summary>
    protected bool CheckPositions(Position pos1, Position pos2, double distance)
    {
        if (Math.Sqrt(Math.Pow(pos1.X - pos2.X, 2) + Math.Pow(pos1.Y - pos2.Y, 2)) < distance)
        {
            return true;
        }
        return false;
    }

    #endregion 

}
