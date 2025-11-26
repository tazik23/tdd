using TagsCloudVisualization.Geometry;

namespace TagsCloudVisualization.Layouters;

public interface ICircularCloudLayouter
{
    IEnumerable<Rectangle> Rectangles { get; }
    Rectangle PutNextRectangle(Size rectangleSize);
}