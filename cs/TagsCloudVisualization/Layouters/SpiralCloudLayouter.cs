using System.Drawing;
using TagsCloudVisualization.Geometry;
using TagsCloudVisualization.Geometry.Extensions;

namespace TagsCloudVisualization.Layouters;

public class SpiralCloudLayouter : ICircularCloudLayouter
{
    private readonly ISpiral spiral;
    private readonly List<Rectangle> rectangles = new();
    
    public IReadOnlyList<Rectangle> Rectangles => rectangles.AsReadOnly();

    public SpiralCloudLayouter(ISpiral spiral)
    {
        this.spiral = spiral;
    }
    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        var rectangle = GetValidPosition(rectangleSize);
        rectangle = TryMoveToCenter(rectangle);
        
        rectangles.Add(rectangle);
        
        return rectangle;
    }

    private Rectangle GetValidPosition(Size rectangleSize)
    {
        Rectangle rectangle;
        do
        {
            var candidatePoint = spiral.GetNextPoint();
            rectangle = CreateRectangleWithCenterAt(candidatePoint, rectangleSize);
        } 
        while(HasIntersections(rectangle));

        return rectangle;
    }

    private Rectangle TryMoveToCenter(Rectangle rectangle, int maxIterationsToTry = 1000)
    {
        if(rectangles.Count == 0)
            return rectangle;
        
        var current = rectangle;
        var iterations = 0;

        while(iterations < maxIterationsToTry)
        {
            var movedX = TryMoveAlongAxis(current, Axis.X, out var xCandidate);
            if(movedX) current = xCandidate;
            
            var movedY = TryMoveAlongAxis(current, Axis.Y, out var yCandidate);
            if(movedY) current = yCandidate;
            
            if(!(movedX || movedY))
                break;
            
            iterations++;
        }
        
        return current;
    }

    private bool TryMoveAlongAxis(Rectangle rectangle, Axis axis, out Rectangle candidate)
    {
        var stepSize = 1;
        var direction = GetDirectionToCenter(rectangle.GetCenter(), axis);

        if (direction.IsZero())
        {
            candidate = rectangle;
            return false;
        }
        
        var moved = rectangle.MoveInDirection(direction, stepSize);
        if (!HasIntersections(moved))
        {
            candidate = moved;
            return true;
        }

        candidate = rectangle;
        return false;
    }

    private Point GetDirectionToCenter(Point point, Axis axis)
    {
        return axis switch
        {
            Axis.X => new Point(-Math.Sign(point.X - spiral.Center.X), 0),
            Axis.Y => new Point(0, -Math.Sign(point.Y - spiral.Center.Y)),
            _ => new Point(0, 0)
        };
    }
    
    private bool HasIntersections(Rectangle rectangle)
    {
        return rectangles.Any(r => r.IntersectsWith(rectangle));
    }
    
    private Rectangle CreateRectangleWithCenterAt(Point center, Size size)
    {
        var location = new Point(center.X - size.Width / 2, center.Y - size.Height / 2);
        return new Rectangle(location, size);
    }
}
