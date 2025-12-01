namespace TagsCloudVisualization;

public static class SizesGenerator
{
    public static IEnumerable<Size> Generate(
        int count, int minWidthSize, int maxWidthSize, int minHeightSize, int maxHeightSize)
    {
        var random = new Random();
        for (int i = 0; i < count; i++)
        {
            var width = random.Next(minWidthSize, maxWidthSize);
            var height = random.Next(minHeightSize, maxHeightSize);
            yield return new Size(width, height);
        }
    }
}