namespace TagsCloudVisualization.Geometry.Extensions;

public static class PointExtensions
{
    public static double DistanceTo(this Point point, Point other)
    {
        var dx = point.X - other.X;
        var dy = point.Y - other.Y;
        
        return Math.Sqrt(dx * dx + dy * dy);
    }
}