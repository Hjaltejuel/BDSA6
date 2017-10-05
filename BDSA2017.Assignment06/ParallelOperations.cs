using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace BDSA2017.Assignment06
{
    public class ParallelOperations
    {
        public static ICollection<long> Squares(long lowerBound, long upperBound)
        {
            BlockingCollection<long> Collection = new BlockingCollection<long>();
            Parallel.For(lowerBound, upperBound+1, i =>
             {

                 long x = (long)Math.Pow(i, 2);
                 Collection.Add((long)Math.Pow(i, 2));
               
             });
            long[] converted = Collection.ToArray();
            Array.Sort(converted);
            return converted;
        }

        public static void CreateThumbnails(IPictureModule resizer, IEnumerable<string> imageFiles, string outputFolder, Size size)
        {
            Parallel.ForEach(imageFiles, file =>
             {
                 resizer.Resize(file, outputFolder, size);
             });
        }
    }
}
