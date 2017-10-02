using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Drawing;

namespace BDSA2017.Assignment06
{
    public class PictureModule : IPictureModule
    {
        public void Resize(string inputFile, string outputFile, Size size)
        {
            using (var image = Image.Load(inputFile))
            {
                var options = new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new SixLabors.Primitives.Size(size.Width, size.Height)
                };

                image.Mutate(x => x
                     .Resize(options));
                image.Save(outputFile);
            }
        }
    }
}
