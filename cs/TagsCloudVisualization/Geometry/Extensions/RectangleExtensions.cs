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

    public static Point GetCenter(this Rectangle rectangle)
    {
        return new Point(rectangle.Left + rectangle.Width / 2, rectangle.Top + rectangle.Height / 2);
    }

    public static Rectangle MoveInDirection(this Rectangle rectangle, Point direction, int distance)
    {
        var center = new Point(
            rectangle.GetCenter().X + direction.X * distance,
            rectangle.GetCenter().Y + direction.Y * distance);
        
        return new Rectangle(center, rectangle.Size);
    }
}

