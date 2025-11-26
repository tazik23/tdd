namespace TagsCloudVisualization.Geometry;

public class Rectangle
{
    public Point Center { get; }
    public Size Size { get; }
    
    public double Left => Center.X - Size.Width / 2;
    public double Right => Center.X + Size.Width / 2;
    public double Top => Center.Y - Size.Height / 2;
    public double Bottom => Center.Y + Size.Height / 2;

    public Rectangle(Point center, Size size)
    {
        Center = center;
        Size = size;
    }
}