using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;

namespace SnakeGame.Models;

public partial class Entity : ObservableObject
{
    #region Properties
    /// <summary>
    /// A property that represents the position
    /// </summary>
    [ObservableProperty]
    private Position? position;

    /// <summary>
    /// A property that represents the width
    /// </summary>
    [ObservableProperty]
    private double width;

    /// <summary>
    /// A property that represents the height
    /// </summary>
    [ObservableProperty]
    private double height;

    /// <summary>
    /// A property that represents the color
    /// </summary>
    [ObservableProperty]
    private Brush? color;

    #endregion
}

