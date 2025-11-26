using TagsCloudVisualization.Geometry;

namespace TagsCloudVisualization.Layouters;

public class SpiralCloudLayouter : ICircularCloudLayouter
{
    private readonly Point center;
    
    public IEnumerable<Rectangle> Rectangles { get; }

    public SpiralCloudLayouter(Point center)
    {
        this.center = center;
    }
    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        throw new NotImplementedException();
    }
}