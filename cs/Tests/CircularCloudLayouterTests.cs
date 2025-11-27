using FluentAssertions;
using TagsCloudVisualization;
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
    
    [TestCaseSource(nameof(RectanglesTestCases))]
    public void PutNextRectangle_ManyRectangles_ShouldNotIntersects(IEnumerable<Size> sizes)
    {
        foreach (var size in sizes)
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

    [TestCaseSource(nameof(RectanglesTestCases))]
    public void PutNextRectangle_ManyRectangles_ShouldTightlyDistribute(IEnumerable<Size> sizes)
    {
        var densityCoefficient = 0.75;
        
        foreach (var size in sizes)
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

    private static IEnumerable<TestCaseData> RectanglesTestCases
    {
        get
        {
            yield return new TestCaseData(
                SizesGenerator.Generate(100, 10, 10, 10, 10))
                .SetName("Squares");
            yield return new TestCaseData(
                    SizesGenerator.Generate(100, 1, 10, 50, 100))
                .SetName("Tall Rectangles");
            yield return new TestCaseData(
                    SizesGenerator.Generate(100, 50, 100, 1, 10))
                .SetName("Long Rectangles");
            yield return new TestCaseData(
                    SizesGenerator.Generate(100, 10, 100, 10, 100))
                .SetName("Random Rectangles");
        }
    }

    private static double GetCircumscribedCircleRadius(Point center, IEnumerable<Rectangle> rectangles)
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