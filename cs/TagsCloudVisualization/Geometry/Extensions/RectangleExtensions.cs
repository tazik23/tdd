using System.Drawing;

namespace TagsCloudVisualization.Geometry.Extensions;

public static class RectangleExtensions
{
    public static bool IntersectsWith(this Rectangle rectangle, Rectangle other)
    {
        return rectangle.Left < other.Right
               && rectangle.Right > other.Left
               && rectangle.Top < other.Bottom
               && rectangle.Bottom > other.Top;
    }

    public static Point[] GetVertices(this Rectangle rectangle)
    {
        return
        [
            new Point(rectangle.Left, rectangle.Top),    
            new Point(rectangle.Left, rectangle.Bottom), 
            new Point(rectangle.Right, rectangle.Top),  
            new Point(rectangle.Right, rectangle.Bottom)
        ];
    }

    public static double GetArea(this Rectangle rectangle)
    {
        return rectangle.Size.Height * rectangle.Size.Width;
    }

    public static Rectangle MoveInDirection(this Rectangle rectangle, Point direction, double distance)
    {
        var center = new Point(
            rectangle.Center.X + direction.X * distance,
            rectangle.Center.Y + direction.Y * distance);
        
        return new Rectangle(center, rectangle.Size);
    }
}

