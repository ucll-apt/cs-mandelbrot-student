using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Mandelbrot
{
    public class Mandelbrot
    {
        private readonly int[][] iterations;

        private readonly double maximumMagnitude;

        public Mandelbrot( int hpixels, int vpixels, Rectangle rectangle, double maximumMagnitude = 10.0, int maximumIterations = 255 )
        {
            this.Width = hpixels;
            this.Height = vpixels;
            this.Rectangle = rectangle;
            this.maximumMagnitude = maximumMagnitude;
            this.MaximumIterations = maximumIterations;
            this.iterations = Enumerable.Range( 0, Height ).Select( y => Enumerable.Range( 0, Width ).Select( x => 0 ).ToArray() ).ToArray();
        }

        public int MaximumIterations { get; }

        public int Width { get; }

        public int Height { get; }

        public Rectangle Rectangle { get; }

        public void ComputeSingle(int x, int y)
        {
            var z = ComputeInitialValue(x, y);
            var c = z;
            var n = 0;

            while ( z.Magnitude < maximumMagnitude && n < MaximumIterations )
            {
                z = z * z + c;
                n++;
            }

            iterations[y][x] = n;
        }

        public void ComputeRow(int y)
        {
            for ( var x = 0; x != Width; ++x )
            {
                ComputeSingle( x, y );
            }
        }

        public void ComputeAll()
        {
            for ( var y = 0; y != Height; ++y )
            {
                ComputeRow( y );
            }
        }

        public int this[int x, int y] => iterations[y][x];

        private Complex ComputeInitialValue( int x, int y )
        {
            var p = new Point { X = x / (double) Width, Y = y / (double) Height };
            var q = Rectangle.FromRelative( p );
            return new Complex( q.X, q.Y );
        }
    }
}
