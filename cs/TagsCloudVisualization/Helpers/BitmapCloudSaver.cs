using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization.Helpers;

public class BitmapCloudSaver
{
    public void SaveToFile(Bitmap bitmap, string fileName, ImageFormat format)
    {
        bitmap.Save(fileName, format);
    }
}