using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

namespace GeometRi.Tests
{
    [TestClass]
    public class RotationTest
    {
        [TestMethod]
        public void RotationMatrixTest()
        {
            Vector3d v = new Vector3d(4, 111, 6);
            double angle = 0.0515;

            Rotation r = new Rotation(v, angle);

            Assert.AreEqual(Matrix3d.RotationMatrix(v, angle), r.RotationMatrix);
        }
    }
}
