namespace TagsCloudVisualization.Geometry;

public class Rectangle
{
    public Point Center { get; }
    public Size Size { get; }

    public Rectangle(Point center, Size size)
    {
        Center = center;
        Size = size;
    }
}