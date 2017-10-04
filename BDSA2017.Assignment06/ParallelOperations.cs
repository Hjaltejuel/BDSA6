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
            BlockingCollection<long> blockingCollection = new BlockingCollection<long>();
            Parallel.For(0, upperBound, i =>
             {

                 long x = (long)Math.Pow(i, 2);
                 blockingCollection.Add((long)Math.Pow(i, 2));
               
             });
            return blockingCollection.ToArray();
        }

        public static void CreateThumbnails(IPictureModule resizer, IEnumerable<string> imageFiles, string outputFolder, Size size)
        {
            throw new NotImplementedException();
        }
    }
}
