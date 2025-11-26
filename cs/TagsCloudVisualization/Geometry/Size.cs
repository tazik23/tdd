namespace TagsCloudVisualization.Geometry;

public class Size
{
    public double Width { get; }
    public double Height { get; }

    public Size(double width, double height)
    {
        if (width < 0)
            throw new ArgumentException("Width must not be negative");
        if (height < 0)
            throw new ArgumentException("Height must not be negative");
        
        Width = width;
        Height = height;
    }
}