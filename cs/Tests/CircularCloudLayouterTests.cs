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
    
}