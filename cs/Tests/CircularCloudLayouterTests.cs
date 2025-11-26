using FluentAssertions;
using TagsCloudVisualization.Geometry;
using TagsCloudVisualization.Geometry.Extensions;
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
        
        rectangle.Center.Should().BeEquivalentTo(center);
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

    [Test]
    public void PutNextRectangle_ManyRectangles_ShouldTightlyDistribute()
    {
        var densityCoefficient = 0.75;
        
        foreach (var size in GenerateSizes(10, 20, 30))
        {
            layouter.PutNextRectangle(size);
        }
        
        var rectangles = layouter.Rectangles.ToList();
        var rectanglesArea = rectangles.Select(r => r.GetArea()).Sum();
        
        var circumscribedCircleRadius = GetCircumscribedCircleRadius(center, rectangles);
        var circumscribedCircleArea = circumscribedCircleRadius * circumscribedCircleRadius * Math.PI;
        
        var actualDensityCoefficient = rectanglesArea / circumscribedCircleArea;
        
        actualDensityCoefficient.Should().BeGreaterThanOrEqualTo(densityCoefficient);
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

    private double GetCircumscribedCircleRadius(Point center, IEnumerable<Rectangle> rectangles)
    {
        double radius = 0;

        foreach(var rectangle in rectangles)
        {
            foreach(var vertex in rectangle.GetVertices())
            {
                var distance = center.DistanceTo(vertex);
                radius = Math.Max(radius, distance);
            }
        }

        return radius;
    }
}