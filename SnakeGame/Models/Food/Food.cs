using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace SnakeGame.Models;
/// <summary>
/// Class that represents the food 
/// </summary>
public class Food : FoodEntity
{
    #region Fields
    /// <summary>
    /// A property that represents the type of a food 
    /// </summary>
    public FoodTypes FoodType { get; private set; }

    /// <summary>
    /// A property that represents the SnakeSegment. 
    /// The number of segments to be increased / decreased 
    /// </summary>
    public int SnakeSegment { get; private set; }

    /// <summary>
    /// A property that represents the food value
    /// </summary>
    public int FoodValue { get; private set; }

    /// <summary>
    /// A field that represents the random object
    /// </summary>

    private static Random _random = new();

    /// <summary>
    /// A dictionary that specifies the properties of a food entity
    /// </summary>

    private Dictionary<FoodTypes, (Brush color, int snakeSegment, int foodValue)> foodSpecification = new()
    {
        {FoodTypes.Normal, (Brushes.Blue,1,1)},
        {FoodTypes.Value,  (Brushes.Yellow,2,10)},
        {FoodTypes.Poison, (Brushes.Red,-1,1)},
        {FoodTypes.Speed, (Brushes.Orange,1,5)},
        {FoodTypes.Slow , (Brushes.Pink,1,1)},
    };
    #endregion

    #region Constructor
    /// <summary>
    /// Constructor that creates a food entity
    /// </summary>
    /// <param name="position"></param>
    public Food(Position position)
    {
        Position = position;
        Width = 20;
        Height = 20;
        //this.Color = Brushes.Blue; 
        RandomizeFood();
    }

    #endregion

    #region Methods
    /// <summary>
    /// Method that geneates a food entity
    /// </summary>
    /// <param name="position"></param>
    public void GenerateNewFood(Position position)
    {
        Position = position;
        RandomizeFood();
    }

    /// <summary>
    /// Method that randomly creates a food entity
    /// </summary>
    private void RandomizeFood()
    {

        var foodTypes = Enum.GetValues(typeof(FoodTypes));
        FoodType = (FoodTypes)foodTypes.GetValue(_random.Next(foodTypes.Length));
        var properties = foodSpecification[FoodType];
        Color = properties.color;
        SnakeSegment = properties.snakeSegment;
        FoodValue = properties.foodValue;
       
    }
    #endregion
}
