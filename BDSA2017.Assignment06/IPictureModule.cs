using System.Drawing;

namespace BDSA2017.Assignment06
{
    public interface IPictureModule
    {
        void Resize(string inputFile, string outputFile, Size size);
    }
}
