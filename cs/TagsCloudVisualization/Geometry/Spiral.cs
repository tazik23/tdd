using System.Drawing;

namespace TagsCloudVisualization.Geometry;

public class ArchimedeanSpiral : ISpiral
{
    private double currentAngle;
    private readonly double angleStep;
    private readonly int distancePerRevolution;
    
    public Point Center { get; }

    public ArchimedeanSpiral(Point center, double angleStep = 0.1, int distancePerRevolution = 1)
    {
        Center = center;
        this.angleStep = angleStep;
        this.distancePerRevolution = distancePerRevolution;
    }

    public Point GetNextPoint()
    {
        var radius = distancePerRevolution / (2 * Math.PI) * currentAngle;
        var x = (int)(Center.X + radius * Math.Cos(currentAngle));
        var y = (int)(Center.Y + radius * Math.Sin(currentAngle));
        
        currentAngle += angleStep;
        
        return new Point(x, y);
    }
}