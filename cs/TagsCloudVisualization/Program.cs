using System.Drawing.Imaging;
using TagsCloudVisualization;
using TagsCloudVisualization.Layouters;
using TagsCloudVisualization.Savers;
using TagsCloudVisualization.Visualizers;
using Point = TagsCloudVisualization.Geometry.Point;

var visualizer = new BitmapCloudVisualizer(VisualizerSettings.Default());
var saver = new BitmapCloudSaver();

var cloudsFolder = "Clouds";

var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
var fullCloudsPath = Path.Combine(baseDirectory, cloudsFolder);

Directory.CreateDirectory(fullCloudsPath);


var center = new Point(0, 0);
var layouter1 = new SpiralCloudLayouter(center);

foreach (var size in SizesGenerator.Generate(100, 10, 10, 10, 10))
{
    layouter1.PutNextRectangle(size);
}

var image = visualizer.CreateImage(layouter1.Rectangles, center);
saver.SaveToFile(image, Path.Combine(fullCloudsPath, "squares.png"), ImageFormat.Png);

var layouter2 = new SpiralCloudLayouter(center);

foreach (var size in SizesGenerator.Generate(1000, 20, 50, 20, 50))
{
    layouter2.PutNextRectangle(size);
}

var image2 = visualizer.CreateImage(layouter2.Rectangles, center);
saver.SaveToFile(image2, Path.Combine(fullCloudsPath, "random_rectangles.png"), ImageFormat.Png);

