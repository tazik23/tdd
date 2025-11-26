using TagsCloudVisualization.Geometry;

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
        } while (rectangles.Any(r => r.IntersectsWith(rectangle)));

        return rectangle;
    }
}