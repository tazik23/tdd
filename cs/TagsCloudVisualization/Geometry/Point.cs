namespace TagsCloudVisualization.Geometry;

public class Point
{
    public double X { get; }
    public double Y { get; }
    
    public static Point Zero => new(0, 0);

    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }

    
}