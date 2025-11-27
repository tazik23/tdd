using System.Drawing;

namespace TagsCloudVisualization.Visualizers;

public class VisualizerSettings
{
    public int Width { get; }
    public int Height { get; }
    public double CloudScaleFactor { get; }
    public Color BackgroundColor { get; }
    public Color RectangleColor { get; }
    public Color RectangleBorderColor { get; }
    public float RectangleBorderWidth { get; }

    public VisualizerSettings(
        int width,
        int height,
        double cloudScaleFactor,
        Color backgroundColor,
        Color rectangleColor,
        Color rectangleBorderColor, 
        float rectangleBorderWidth)
    {
        Width = width;
        Height = height;
        CloudScaleFactor = cloudScaleFactor;
        BackgroundColor = backgroundColor;
        RectangleColor = rectangleColor;
        RectangleBorderColor = rectangleBorderColor;
        RectangleBorderWidth = rectangleBorderWidth;
    }

    public static VisualizerSettings Default()
    {
        return new VisualizerSettings(
            800,
            600,
            0.8,
            Color.White,
            Color.Plum,
            Color.Black,
            4f
        );
    }
}