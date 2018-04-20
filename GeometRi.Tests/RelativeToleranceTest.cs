using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeometRi.Tests
{
    [TestClass]
    public class RelativeToleranceTest
    {
        [TestMethod]
        public void Segment3dRelativeToleranceTest()
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
        public void SphereRelativeToleranceTest()
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
        public void Circle3dRelativeToleranceTest()
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

        [TestMethod]
        public void Point3dRelativeToleranceTest()
        {
            Point3d p1 = new Point3d(100, 100, 0);
            Point3d p2 = new Point3d(100, 100, 0.9);
            Assert.AreNotEqual(p1, p2);

            GeometRi3D.Tolerance = 0.01;
            GeometRi3D.UseAbsoluteTolerance = false;
            Assert.AreEqual(p1, p2);
            GeometRi3D.Tolerance = 1e-12;
            GeometRi3D.UseAbsoluteTolerance = true;
        }

        [TestMethod]
        public void Vector3dRelativeToleranceTest()
        {
            Vector3d v1 = new Vector3d(100, 100, 0);
            Vector3d v2 = new Vector3d(100, 100, 0.9);
            Assert.AreNotEqual(v1, v2);

            GeometRi3D.Tolerance = 0.01;
            GeometRi3D.UseAbsoluteTolerance = false;
            Assert.AreEqual(v1, v2);
            Assert.IsTrue(v1.IsParallelTo(v2));
            GeometRi3D.Tolerance = 1e-12;
            GeometRi3D.UseAbsoluteTolerance = true;
        }

        [TestMethod]
        public void Vector3dIsParallelToRelativeToleranceTest()
        {
            Vector3d v1 = new Vector3d(-10, -10, -10);
            Vector3d v2 = new Vector3d(199, 198, 198);
            Assert.IsFalse(v1.IsParallelTo(v2));

            GeometRi3D.Tolerance = 0.01;
            GeometRi3D.UseAbsoluteTolerance = false;
            Assert.IsTrue(v1.IsParallelTo(v2));
            GeometRi3D.Tolerance = 1e-12;
            GeometRi3D.UseAbsoluteTolerance = true;
        }

        [TestMethod]
        public void Vector3dIsOrthogonalToRelativeToleranceTest()
        {
            Vector3d v1 = new Vector3d(-10, -10, 0);
            Vector3d v2 = new Vector3d(1, 0, 198);
            Assert.IsFalse(v1.IsOrthogonalTo(v2));

            GeometRi3D.Tolerance = 0.01;
            GeometRi3D.UseAbsoluteTolerance = false;
            Assert.IsTrue(v1.IsOrthogonalTo(v2));
            GeometRi3D.Tolerance = 1e-12;
            GeometRi3D.UseAbsoluteTolerance = true;
        }

        [TestMethod]
        public void Line3dRelativeToleranceTest()
        {
            Line3d l1 = new Line3d(new Point3d(2, 2, 2), new Vector3d(1, 1, 1));
            Line3d l2 = new Line3d(new Point3d(201, 200, 200), new Vector3d(-10, -10, -10));
            Assert.AreNotEqual(l1, l2);

            GeometRi3D.Tolerance = 0.01;
            GeometRi3D.UseAbsoluteTolerance = false;
            Assert.AreEqual(l1, l2);
            Assert.IsTrue(l1.IsParallelTo(l2));
            GeometRi3D.Tolerance = 1e-12;
            GeometRi3D.UseAbsoluteTolerance = true;
        }
    }
}
