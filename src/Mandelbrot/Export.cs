using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandelbrot
{
    public static class Export
    {
        public static void AsTextWif( string path, IList<Mandelbrot> mandelbrots )
        {
            using ( var file = File.OpenWrite( path ) )
            {
                using ( var writer = new StreamWriter( file ) )
                {
                    foreach ( var mandelbrot in mandelbrots )
                    {
                        ConvertSingle( mandelbrot, writer );
                    }
                }
            }

            void ConvertSingle( Mandelbrot mandelbrot, StreamWriter writer )
            {
                using ( var stream = new MemoryStream() )
                {
                    stream.Write( BitConverter.GetBytes( mandelbrot.Width ) );
                    stream.Write( BitConverter.GetBytes( mandelbrot.Height ) );

                    {
                        var buffer = new byte[3];

                        foreach ( var y in Enumerable.Range( 0, mandelbrot.Height ) )
                        {
                            foreach ( var x in Enumerable.Range( 0, mandelbrot.Width ) )
                            {
                                var n = mandelbrot[x, y];
                                var t = ((double) n) / mandelbrot.MaximumIterations;

                                ConvertToColor( t, buffer );

                                stream.Write( buffer );
                            }
                        }
                    }

                    stream.Seek( 0, SeekOrigin.Begin );

                    {
                        var buffer = new byte[stream.Length];
                        stream.Read( buffer, 0, (int) stream.Length );

                        var base64 = Convert.ToBase64String( buffer );

                        writer.Write( "<<<\n" );
                        writer.Write( base64 );
                        writer.Write( "\n>>>\n" );
                    }
                }
            }
        }

        public static void AsBinaryWif( string path, IList<Mandelbrot> mandelbrots )
        {
            using ( var file = File.OpenWrite( path ) )
            {
                using ( var writer = new BinaryWriter( file ) )
                {
                    foreach ( var mandelbrot in mandelbrots )
                    {
                        ConvertSingle( mandelbrot, writer );
                    }

                    writer.Write( 0 );
                }
            }

            void ConvertSingle( Mandelbrot mandelbrot, BinaryWriter writer )
            {
                writer.Write( BitConverter.GetBytes( mandelbrot.Width ) );
                writer.Write( BitConverter.GetBytes( mandelbrot.Height ) );

                var buffer = new byte[3];

                foreach ( var y in Enumerable.Range( 0, mandelbrot.Height ) )
                {
                    foreach ( var x in Enumerable.Range( 0, mandelbrot.Width ) )
                    {
                        var n = mandelbrot[x, y];
                        var t = ((double) n) / mandelbrot.MaximumIterations;

                        ConvertToColor( t, buffer );

                        writer.Write( buffer );
                    }
                }
            }
        }

        private static void ConvertToColor(double t, byte[] buffer)
        {
            Grayscale( t, buffer );
        }

        private static void Grayscale(double t, byte[] buffer)
        {
            byte c = (byte) (t * 255);

            buffer[0] = buffer[1] = buffer[2] = c;
        }

        private static void Colorful(double t, byte[] buffer)
        {
            byte r = (byte) (255 * (Math.Sin( 2 * Math.PI * t ) * 0.5 + 1));
            byte g = (byte) (255 * (Math.Sin( 3 * Math.PI * t ) * 0.5 + 1));
            byte b = (byte) (255 * (Math.Sin( 4 * Math.PI * t ) * 0.5 + 1));

            buffer[0] = r;
            buffer[1] = g;
            buffer[2] = b;
        }
    }
}
