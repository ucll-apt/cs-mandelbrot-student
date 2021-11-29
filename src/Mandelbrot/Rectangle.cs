using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandelbrot
{
    public class Rectangle
    {
        public double Left { get; init; }

        public double Right { get; init; }

        public double Top { get; init; }

        public double Bottom { get; init; }


        public Point FromRelative(Point relative)
        {
            var x = Left + relative.X * (Right - Left);
            var y = Bottom + relative.Y * (Top - Bottom);

            return new Point { X = x, Y = y };
        }

        public static Rectangle CreateCenteredAt(Point center, double width, double height)
        {
            return new Rectangle {
                Left = center.X - width / 2,
                Right = center.X + width / 2,
                Top = center.Y + height / 2,
                Bottom = center.Y - height / 2
            };
        }

        public static Rectangle CreateCenteredAtByRatio( Point center, double ratio, double width )
        {
            var height = width / ratio;

            return CreateCenteredAt( center, width, height );
        }
    }
}
