using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

namespace GeometRi_Tests
{
    [TestClass]
    public class TriangleTest
    {
        //===============================================================
        // Triangle tests
        //===============================================================

        [TestMethod()]
        public void TriangleEqualTest()
        {
            Point3d p1 = new Point3d(-3, 0, 4);
            Point3d p2 = new Point3d(4, 0, 5);
            Point3d p3 = new Point3d(1, 0, -4);

            Triangle t1 = new Triangle(p1, p2, p3);
            Triangle t2 = new Triangle(p1, p3, p2);
            Triangle t3 = new Triangle(p3, p2, p1);

            Assert.AreEqual(t1, t2);
            Assert.AreEqual(t1, t3);
            Assert.AreEqual(t3, t2);
        }

        [TestMethod()]
        public void TriangleAreaTest()
        {
            Point3d p1 = new Point3d(0, 0, 0);
            Point3d p2 = new Point3d(1, 0, 0);
            Point3d p3 = new Point3d(0, 1, 0);

            Triangle t = new Triangle(p1, p2, p3);
            Assert.IsTrue(Abs(t.Area - 0.5) < GeometRi3D.Tolerance);
        }

        [TestMethod()]
        public void TriangleBisectorTest()
        {
            Point3d p1 = new Point3d(0, 0, 0);
            Point3d p2 = new Point3d(1, 0, 0);
            Point3d p3 = new Point3d(0, 1, 0);
            Triangle t = new Triangle(p1, p2, p3);
            Assert.AreEqual(t.Bisector_A, new Segment3d(p1, new Point3d(0.5, 0.5, 0)));

            t = new Triangle(p2, p3, p1);
            Assert.AreEqual(t.Bisector_C, new Segment3d(p1, new Point3d(0.5, 0.5, 0)));

            t = new Triangle(p3, p1, p2);
            Assert.AreEqual(t.Bisector_B, new Segment3d(p1, new Point3d(0.5, 0.5, 0)));
        }

        [TestMethod()]
        public void TriangleIncenterTest()
        {
            Point3d p1 = new Point3d(0, 0, 0);
            Point3d p2 = new Point3d(14, 0, 0);
            Point3d p3 = new Point3d(5, 12, 0);

            Triangle t = new Triangle(p1, p2, p3);
            Assert.AreEqual(t.Incenter, new Point3d(6, 4, 0));
        }

        [TestMethod()]
        public void TriangleOrthocenterTest()
        {
            Point3d p1 = new Point3d(0, 0, 0);
            Point3d p2 = new Point3d(14, 0, 0);
            Point3d p3 = new Point3d(5, 1, 0);

            Triangle t = new Triangle(p1, p2, p3);
            Assert.AreEqual(t.Orthocenter, new Point3d(5, 45, 0));
        }

        [TestMethod()]
        public void TriangleCentroidTest()
        {
            Point3d p1 = new Point3d(0, 0, 0);
            Point3d p2 = new Point3d(14, 0, 0);
            Point3d p3 = new Point3d(4, 12, 0);

            Triangle t = new Triangle(p1, p2, p3);
            Assert.AreEqual(t.Centroid, new Point3d(6, 4, 0));
        }

        [TestMethod()]
        public void TriangleCircumcenterTest()
        {
            Point3d p1 = new Point3d(0, 0, 0);
            Point3d p2 = new Point3d(14, 0, 0);
            Point3d p3 = new Point3d(4, 12, 0);

            Triangle t = new Triangle(p1, p2, p3);
            Assert.AreEqual(t.Circumcenter, new Point3d(7.0, 13.0 / 3, 0.0));
        }

        [TestMethod()]
        public void TriangleGeometryTest()
        {
            Point3d p1 = new Point3d(0, 0, 0);
            Point3d p2 = new Point3d(6, 0, 0);
            Point3d p3 = new Point3d(3, 6, 0);

            Triangle t = new Triangle(p1, p2, p3);
            Assert.IsTrue(t.IsAcute);
            Assert.IsTrue(t.IsIsosceles);

            t.C = new Point3d(3, 2, 0);
            Assert.IsTrue(t.IsObtuse);

            t.C = new Point3d(2, 2, 0);
            Assert.IsTrue(t.IsScalene);

            t.C = new Point3d(0, 2, 0);
            Assert.IsTrue(t.IsRight);
        }

    }
}
