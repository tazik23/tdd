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

    private Rectangle TryMoveToCenter(Rectangle rectangle,int  maxIterationsToTry = 1000)
    {
        if(rectangles.Count == 0)
            return rectangle;
        
        var current = rectangle;
        bool isMoved;
        var iterations = 0;
        var moveStep = 0.1;
        
        do
        {
            isMoved = false;
            var directionX = new Point(-Math.Sign(current.Center.X - center.X), 0);
            if(directionX.X != 0)
            {
                var candidate = current.MoveInDirection(directionX, moveStep);
                if(!HasIntersections(candidate))
                {
                    current = candidate;
                    isMoved = true;
                }
            }

            var directionY = new Point(0, -Math.Sign(current.Center.Y - center.Y));
            if(directionY.Y != 0)
            {
                var candidate = current.MoveInDirection(directionY, moveStep);
                if (!HasIntersections(candidate))
                {
                    current = candidate;
                    isMoved = true;
                }
            }

            iterations++;
        } while(isMoved && iterations < maxIterationsToTry);

        return current;
    }

    private bool HasIntersections(Rectangle rectangle)
    {
        return rectangles.Any(r => r.IntersectsWith(rectangle));
    }
}