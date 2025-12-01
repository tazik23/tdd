using System.Drawing;

namespace TagsCloudVisualization.Geometry.Extensions;

public static class RectangleExtensions
{
    public static IEnumerable<Point> GetVertices(this Rectangle rectangle)
    {
        yield return new Point(rectangle.Left, rectangle.Top);
        yield return new Point(rectangle.Left, rectangle.Bottom);
        yield return new Point(rectangle.Right, rectangle.Top);
        yield return new Point(rectangle.Right, rectangle.Bottom);
    }

    public static double GetArea(this Rectangle rectangle)
    {
        return rectangle.Height * rectangle.Width;
    }

    public static Rectangle MoveInDirection(this Rectangle rectangle, Point direction, double distance)
    {
        var center = new Point(
            rectangle.Center.X + direction.X * distance,
            rectangle.Center.Y + direction.Y * distance);
        
        return new Rectangle(center, rectangle.Size);
    }
}

