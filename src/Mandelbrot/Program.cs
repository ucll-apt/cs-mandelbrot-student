using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Mandelbrot
{
    class Program
    {
        #region Parameters

        // Path of output file
        public const string PATH = "G:/temp/mandel.wif";

        // Set the scheduler here
        public static readonly IScheduler SCHEDULER = new ThreadproolScheduler();

        // Set the planner factory here
        public static readonly Func<IList<Mandelbrot>, IPlanner> PLANNER_FACTORY = x => new RowPlanner( x );

        // Set the exporter here, use AsBinaryWif for the .NET Viewer, use AsTextWif for Python viewer
        public static readonly Action<string, IList<Mandelbrot>> EXPORTER = Export.AsBinaryWif;

        // Horizontal resolution of images
        public const int HORIZONTAL_RESOLUTION = 1920;

        // Vertical resolution of images
        public const int VERTICAL_RESOLUTION = 1080;

        // No reason to touch this, it impacts the fractal rendering
        public const double MAXIMUM_MAGNITUDE = 5.0;

        // By how much each frame zooms in. Must be 0 < factor < 1. Higher numbers yield a more smooth animation (and more frames).
        public const double ZOOM_FACTOR = 0.98;

        // X-coordinate of center point
        public const double X_COORDINATE = -0.761574;

        // Y-coordinate of center point
        public const double Y_COORDINATE = -0.0847596;

        // Initial width of rectangle in complex plane
        public const double START_WIDTH = 3;

        // Final width of rectangle in complex plane
        public const double END_WIDTH = 0.0001;

        #endregion


        // Nothing should be changed down here
        public static void Main( string[] args )
        {
            // Create list of fractals to be rendered
            var mandelbrots = CreateFrames().ToList();
            Console.WriteLine( $"Rendering {mandelbrots.Count} frames..." );

            // Create planner that divides rendering into many small jobs
            var planner = PLANNER_FACTORY( mandelbrots );

            // Render fractals
            Benchmark( () => SCHEDULER.Schedule( planner ) );
            //RenderFractals( planner );

            // Write to file
            Console.WriteLine( $"Writing results to {PATH}" );
            EXPORTER( PATH, mandelbrots );
        }

        public static IEnumerable<Mandelbrot> CreateFrames()
        {
            // Calculate aspect ratio
            var ratio = ((double) HORIZONTAL_RESOLUTION) / VERTICAL_RESOLUTION;

            // Each iteration produces a Mandelbrot object
            for ( double width = START_WIDTH; width > END_WIDTH; width *= ZOOM_FACTOR )
            {
                // Create rectangle in complex plane
                var rectangle = Rectangle.CreateCenteredAtByRatio( new Point { X = X_COORDINATE, Y = Y_COORDINATE }, ratio, width );

                // Yield Mandelbrot object
                yield return new Mandelbrot( HORIZONTAL_RESOLUTION, VERTICAL_RESOLUTION, rectangle, maximumMagnitude: MAXIMUM_MAGNITUDE );
            }
        }

        public static void Benchmark(Action action)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            action();
            stopwatch.Stop();
            Console.WriteLine( $"Used {stopwatch.ElapsedMilliseconds}ms" );
        }
    }
}
