using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

namespace GeometRi_Tests
{
    [TestClass]
    public class TetrahedronTest
    {
        //===============================================================
        // Tetrahedron tests
        //===============================================================

        [TestMethod()]
        public void TetrahedronBasicTest()
        {
            Tetrahedron t = new Tetrahedron();

            double z = t.Area - Sqrt(3) / 2;
            Assert.IsTrue(Abs(t.Volume - 1.0 / 3.0) < GeometRi3D.Tolerance);
            Assert.IsTrue(Abs(t.Area - Sqrt(3) * 2) < GeometRi3D.Tolerance);

        }

    }
}
