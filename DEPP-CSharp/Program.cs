using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageMagick;

namespace DEPP_CSharp
{
    class Program
    {

        static void Main(string[] args)
        {

            int magicRatio = 200;
            int imageSize = 1024;

            using (MagickImage image = new MagickImage(MagickColor.FromRgb(255, 255, 255), imageSize, imageSize))
            {

                var pixels = image.GetPixels();

                for (int a = 1; a < imageSize * magicRatio; ++a)
                {
                    var range = Enumerable.Range(a + 1, (imageSize * magicRatio) - (a + 1));

                    Parallel.ForEach(range, new ParallelOptions { MaxDegreeOfParallelism = 3 }, b =>
                    {
                        double c = Math.Pow((Math.Pow(a, 2) + Math.Pow(b, 2)), 1.0 / 2);
                        int check = (int)c;
                        //double a_k = int(a);

                        double a_k = (double)a / magicRatio;
                        double b_k = (double)b / magicRatio;


                        if (check == c)
                        {

                            int stepSize = 10000;

                            var pixel = pixels.GetPixel((int)a_k, (int)b_k);
                            //Console.WriteLine(a.ToString() + "," + b.ToString());
                            var x = (ushort)(pixel.GetChannel(0));
                            if (x >= stepSize)
                            {
                                x = (ushort)(pixel.GetChannel(0) - stepSize);
                            }
                            pixel.SetChannel(0, (ushort)(x));
                            pixel.SetChannel(1, (ushort)(x));
                            pixel.SetChannel(2, (ushort)(x));
                            pixels.Set(pixel);

                            pixel = pixels.GetPixel((int)b_k, (int)a_k);
                            //Console.WriteLine(a.ToString() + "," + b.ToString());
                            pixel.SetChannel(0, (ushort)(x));
                            pixel.SetChannel(1, (ushort)(x));
                            pixel.SetChannel(2, (ushort)(x));
                            pixels.Set(pixel);

                        }
                    });

                    //for (int b = a + 1; b < imageSize * magicRatio; ++b)
                    //{
                        
                    //}
                }
                image.Write("output.png");
                //Console.WriteLine("Done");
                //Console.ReadLine();
            }
        }
    }
}
