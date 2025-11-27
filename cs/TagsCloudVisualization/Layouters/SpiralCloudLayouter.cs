using TagsCloudVisualization.Geometry;
using TagsCloudVisualization.Geometry.Extensions;

namespace TagsCloudVisualization.Layouters;

public class SpiralCloudLayouter : ICircularCloudLayouter
{
    private readonly Point center;
    private readonly Spiral spiral;
    private readonly List<Rectangle> rectangles = new();
    
    public IEnumerable<Rectangle> Rectangles => rectangles;

    public SpiralCloudLayouter(Point center)
    {
        this.center = center;
        spiral = new Spiral(center);
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
            rectangle = new Rectangle(candidatePoint, rectangleSize);
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
        var stepSize = 0.1;
        var direction = GetDirectionToCenter(rectangle.Center, axis);

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
            Axis.X => new Point(-Math.Sign(point.X - center.X), 0),
            Axis.Y => new Point(0, -Math.Sign(point.Y - center.Y)),
            _ => Point.Zero
        };
    }
    
    private bool HasIntersections(Rectangle rectangle)
    {
        return rectangles.Any(r => r.IntersectsWith(rectangle));
    }
}
