using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

namespace GeometRi_Tests
{
    [TestClass]
    public class Vector3dTest
    {
        //===============================================================
        // Vector3d tests
        //===============================================================

        [TestMethod()]
        public void VectorAngleTest()
        {
            // Angle < 90 
            Vector3d v1 = new Vector3d(1, 0, 0);
            Vector3d v2 = new Vector3d(1, 1, 0);
            Assert.IsTrue(Abs(v1.AngleToDeg(v2) - 45) < GeometRi3D.Tolerance);
        }

        [TestMethod()]
        public void VectorAngleTest2()
        {
            // Angle > 90 
            Vector3d v1 = new Vector3d(1, 0, 0);
            Vector3d v2 = new Vector3d(-1, 1, 0);
            Assert.IsTrue(Abs(v1.AngleToDeg(v2) - 135) < GeometRi3D.Tolerance);
        }

        [TestMethod()]
        public void VectorAngleToPlaneTest()
        {
            Plane3d s = new Plane3d();
            // XY plane

            Vector3d v = new Vector3d(1, 0, 0);
            Assert.IsTrue(Abs(v.AngleTo(s)) < GeometRi3D.Tolerance);
            Assert.IsTrue(Abs(s.AngleTo(v)) < GeometRi3D.Tolerance);

            v = new Vector3d(0, 0, 1);
            Assert.IsTrue(Abs(v.AngleTo(s) - PI / 2) < GeometRi3D.Tolerance);
            Assert.IsTrue(Abs(s.AngleTo(v) - PI / 2) < GeometRi3D.Tolerance);

            v = new Vector3d(0, 0, -1);
            Assert.IsTrue(Abs(v.AngleTo(s) - PI / 2) < GeometRi3D.Tolerance);
            Assert.IsTrue(Abs(s.AngleTo(v) - PI / 2) < GeometRi3D.Tolerance);

            v = new Vector3d(1, 0, 1);
            Assert.IsTrue(Abs(v.AngleTo(s) - PI / 4) < GeometRi3D.Tolerance);
            Assert.IsTrue(Abs(s.AngleTo(v) - PI / 4) < GeometRi3D.Tolerance);

            v = new Vector3d(1, 0, -1);
            Assert.IsTrue(Abs(v.AngleTo(s) - PI / 4) < GeometRi3D.Tolerance);
            Assert.IsTrue(Abs(s.AngleTo(v) - PI / 4) < GeometRi3D.Tolerance);

        }
    }
}
