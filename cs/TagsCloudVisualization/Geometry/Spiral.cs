using System.Drawing;

namespace TagsCloudVisualization.Geometry;

public class Spiral
{
    private readonly Point center;
    private double currentAngle;
    private readonly double angleStep;
    private readonly double distancePerRevolution;

    public Spiral(Point center, double angleStep = 0.1, double distancePerRevolution = 1.0)
    {
        this.center = center;
        this.angleStep = angleStep;
        this.distancePerRevolution = distancePerRevolution;
    }

    public Point GetNextPoint()
    {
        var radius = distancePerRevolution / (2 * Math.PI) * currentAngle;
        var x = center.X + radius * Math.Cos(currentAngle);
        var y = center.Y + radius * Math.Sin(currentAngle);
        
        currentAngle += angleStep;
        
        return new Point(x, y);
    }
}