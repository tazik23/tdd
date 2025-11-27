    using TagsCloudVisualization.Geometry;
    using TagsCloudVisualization.Geometry.Extensions;

    namespace TagsCloudVisualization.Layouters;

    public class SpiralCloudLayouter : ICircularCloudLayouter
    {
        private readonly Point center;
        private readonly Spiral spiral;
        private readonly List<Rectangle> rectangles = new();
        
        public IEnumerable<Rectangle> Rectangles => rectangles;

        public SpiralCloudLayouter(Point center)
        {
            this.center = center;
            spiral = new Spiral(center);
        }
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = GetValidPosition(rectangleSize);
            rectangle = TryMoveToCenter(rectangle);
            
            rectangles.Add(rectangle);
            
            return rectangle;
        }

        private Rectangle GetValidPosition(Size rectangleSize)
        {
            Rectangle rectangle;
            do
            {
                var candidatePoint = spiral.GetNextPoint();
                rectangle = new Rectangle(candidatePoint, rectangleSize);
            } while(rectangles.Any(r => r.IntersectsWith(rectangle)));

            return rectangle;
        }

        private Rectangle TryMoveToCenter(Rectangle rectangle)
        {
            if(rectangles.Count == 0)
                return rectangle;
            
            var current = rectangle;
            bool isMoved;
            var iterations = 0;
            var maxIterations = 1000;
            var moveStep = 0.1;
            
            do
            {
                isMoved = false;
                var directionX = new Point(-Math.Sign(current.Center.X - center.X), 0);
                if(directionX.X != 0)
                {
                    var candidate = current.MoveInDirection(directionX, moveStep);
                    if(!rectangles.Any(r => r.IntersectsWith(candidate)))
                    {
                        current = candidate;
                        isMoved = true;
                    }
                }

                var directionY = new Point(0, -Math.Sign(current.Center.Y - center.Y));
                if(directionY.Y != 0)
                {
                    var candidate = current.MoveInDirection(directionY, moveStep);
                    if (!rectangles.Any(r => r.IntersectsWith(candidate)))
                    {
                        current = candidate;
                        isMoved = true;
                    }
                }

                iterations++;
            } while(isMoved && iterations < maxIterations);

            return current;
        }
    }