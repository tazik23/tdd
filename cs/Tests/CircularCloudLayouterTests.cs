using System.Drawing.Imaging;
using FluentAssertions;
using TagsCloudVisualization;
using TagsCloudVisualization.Geometry;
using TagsCloudVisualization.Geometry.Extensions;
using TagsCloudVisualization.Layouters;
using TagsCloudVisualization.Savers;
using TagsCloudVisualization.Visualizers;

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

    [TearDown]
    public void TearDown()
    {
        if(TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            var failuresFolder = Path.Combine(TestContext.CurrentContext.WorkDirectory, "TestFailures");
            Directory.CreateDirectory(failuresFolder); 

            var testName = TestContext.CurrentContext.Test.Name;
            var fileName = $"test_failure_{testName}.png";
            var fullPath = Path.Combine(failuresFolder, fileName);
        
            SaveCloudVisualization(fullPath);
            TestContext.WriteLine($"Test {testName} failed. Tag cloud visualization saved to {fullPath}");
        }
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
                .SetName("TallRectangles");
            yield return new TestCaseData(
                    SizesGenerator.Generate(100, 50, 100, 1, 10))
                .SetName("LongRectangles");
            yield return new TestCaseData(
                    SizesGenerator.Generate(100, 10, 100, 10, 100))
                .SetName("RandomRectangles");
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

    private void SaveCloudVisualization(string fileName)
    {
        var visualizer = new BitmapCloudVisualizer(VisualizerSettings.Default());
        var image = visualizer.CreateImage(layouter.Rectangles, center);
        new BitmapCloudSaver().SaveToFile(image, fileName, ImageFormat.Png);
    }
}