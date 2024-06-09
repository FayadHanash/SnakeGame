using SnakeGame.Models;
using System.Windows;
using System.Windows.Controls;

namespace SnakeGame.ViewModels;

public class ShapeTemplateSelector : DataTemplateSelector
{
    #region Properties
    /// <summary>
    /// A property that represents snake template / entity
    /// </summary>
    public DataTemplate SnakeTemplate { get; set; }

    /// <summary>
    /// A property that represents food template / entity
    /// </summary>
    public DataTemplate FoodTemplate { get; set; }

    #endregion

    #region Methods
    /// <summary>
    /// Method that selects a template based on the item
    /// </summary>
    /// <param name="item"></param>
    /// <param name="container"></param>
    /// <returns></returns>
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (item is SnakeEntity) return SnakeTemplate;
        if(item is FoodEntity) return FoodTemplate;
        return base.SelectTemplate(item, container);
    }
    #endregion
}
