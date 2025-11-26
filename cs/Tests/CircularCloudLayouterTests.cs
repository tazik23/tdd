using FluentAssertions;
using TagsCloudVisualization.Geometry;
using TagsCloudVisualization.Layouters;

namespace Tests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private Point center;
    private ICircularCloudLayouter layouter;

    [SetUp]
    public void SetUp()
    {
        center = new Point(0, 0);
        layouter = new SpiralCloudLayouter(center);
    }
    
    
    [Test]
    public void PutNextRectangle_FirstRectangle_ShouldPlaceInCenter()
    {
        var rectangle = layouter.PutNextRectangle(new Size(10, 10));
        
        rectangle.Center.Should().Be(center);
    }
    
    [Test]
    public void PutNextRectangle_ManyRectangles_ShouldNotIntersects()
    {
        foreach (var size in GenerateSizes(10, 20, 30))
        {
            layouter.PutNextRectangle(size);
        }
        
        var rectangles = layouter.Rectangles.ToList();

        foreach (var r1 in rectangles)
        {
            foreach (var r2 in rectangles.Where(r2 => r2 != r1))
            {
                r1.IntersectsWith(r2).Should().BeFalse();
            }
        }
    }
    
    private IEnumerable<Size> GenerateSizes(int count, int minSize, int maxSize)
    {
        var random = new Random();
        for (int i = 0; i < count; i++)
        {
            var width = random.Next(minSize, maxSize);
            var height = random.Next(minSize, maxSize);
            yield return new Size(width, height);
        }
    }
}