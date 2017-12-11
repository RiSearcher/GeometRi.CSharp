using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeometRi.Tests
{
    [TestClass]
    public class RelativeToleranceTest
    {
        [TestMethod]
        public void SegmentTest()
        {
            Segment3d s1 = new Segment3d(new Point3d(0, 0, 0), new Point3d(100, 0, 0));
            Segment3d s2 = new Segment3d(new Point3d(0, 0, 0), new Point3d(100, 0.99, 0));
            Assert.AreNotEqual(s1, s2);

            GeometRi3D.Tolerance = 0.01;
            GeometRi3D.UseAbsoluteTolerance = false;
            Assert.AreEqual(s1, s2);
            GeometRi3D.Tolerance = 1e-12;
            GeometRi3D.UseAbsoluteTolerance = true;
        }

        [TestMethod]
        public void SphereTest()
        {
            Sphere s1 = new Sphere(new Point3d(100, 100, 0), 10);
            Sphere s2 = new Sphere(new Point3d(100, 100.09, 0), 10);
            Assert.AreNotEqual(s1, s2);

            GeometRi3D.Tolerance = 0.01;
            GeometRi3D.UseAbsoluteTolerance = false;
            Assert.AreEqual(s1, s2);
            GeometRi3D.Tolerance = 1e-12;
            GeometRi3D.UseAbsoluteTolerance = true;
        }

        [TestMethod]
        public void Circle3dTest()
        {
            Circle3d s1 = new Circle3d(new Point3d(100, 100, 0), 10, new Vector3d(1, 0, 0));
            Circle3d s2 = new Circle3d(new Point3d(100, 100.09, 0), 10, new Vector3d(10, 0.09, 0));
            Assert.AreNotEqual(s1, s2);

            GeometRi3D.Tolerance = 0.01;
            GeometRi3D.UseAbsoluteTolerance = false;
            Assert.AreEqual(s1, s2);
            GeometRi3D.Tolerance = 1e-12;
            GeometRi3D.UseAbsoluteTolerance = true;
        }
    }
}
