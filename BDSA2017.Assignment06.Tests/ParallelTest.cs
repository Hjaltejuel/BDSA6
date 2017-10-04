using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BDSA2017.Assignment06.Tests
{
    class ParallelTest
    {
        [Fact]
        public void TestSquared()
        {
            Assert.Equal(new long[] { 1, 4, 9, 16, 25 },ParallelOperations.Squares(1,5));
        }
    }
}
