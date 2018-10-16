using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

namespace GeometRi_Tests
{
    [TestClass]
    public class Point3dTest
    {
        //===============================================================
        // Point3d tests
        //===============================================================

        [TestMethod()]
        public void PointAddDivideTest()
        {
            Point3d p1 = new Point3d(1, 2, 3);
            Point3d p2 = new Point3d(3, 2, 1);
            Assert.IsTrue((p1 + p2) / 2 == new Point3d(2, 2, 2));
        }

        [TestMethod()]
        public void PointScaleTest()
        {
            Point3d p1 = new Point3d(1, 2, 3);
            Point3d p2 = new Point3d(2, 4, 6);
            Assert.IsTrue(2 * p1 == p2);
        }

        [TestMethod()]
        public void PointDistanceToPointTest()
        {
            Point3d p1 = new Point3d(0, 0, 0);
            Point3d p2 = new Point3d(0, 3, 4);
            Assert.IsTrue(Abs(p1.DistanceTo(p2) - 5) < GeometRi3D.Tolerance);
        }

        [TestMethod()]
        public void PointDistanceToLineTest()
        {
            Point3d p1 = new Point3d(-4, 3, 5);
            Point3d p2 = new Point3d(1, -5, -1);
            Vector3d v2 = new Vector3d(-2, 3, 4);
            Line3d l1 = new Line3d(p2, v2);
            Assert.IsTrue(Abs(p1.DistanceTo(l1) - 3) < GeometRi3D.Tolerance);
        }

        [TestMethod()]
        public void PointDistanceToPlaneTest()
        {
            Point3d p1 = new Point3d(-4, 3, 5);
            Plane3d s1 = new Plane3d(-1, 2, -2, 9);
            Assert.IsTrue(Abs(p1.DistanceTo(s1) - 3) < GeometRi3D.Tolerance);
        }

        [TestMethod()]
        public void PointDistanceToRayTest()
        {
            Point3d p = new Point3d(0, 0, 0);
            Ray3d r = new Ray3d(new Point3d(1, 1, 0), new Vector3d(1, 0, 0));
            Assert.IsTrue(Abs(p.DistanceTo(r) - Sqrt(2)) < GeometRi3D.Tolerance);

            p = new Point3d(2, 0, 0);
            Assert.IsTrue(Abs(p.DistanceTo(r) - 1) < GeometRi3D.Tolerance);
        }

        [TestMethod()]
        public void PointDistanceToSegmentTest()
        {
            Point3d p = new Point3d(0, 0, 0);
            Segment3d s = new Segment3d(new Point3d(1, 1, 0), new Point3d(3, 3, 0));
            Assert.IsTrue(Abs(p.DistanceTo(s) - Sqrt(2)) < GeometRi3D.Tolerance);

            p = new Point3d(1, 1, 0);
            Assert.IsTrue(Abs(p.DistanceTo(s) - 0) < GeometRi3D.Tolerance);
            p = new Point3d(4, 4, 0);
            Assert.IsTrue(Abs(p.DistanceTo(s) - Sqrt(2)) < GeometRi3D.Tolerance);
            p = new Point3d(1, 3, 0);
            Assert.IsTrue(Abs(p.DistanceTo(s) - Sqrt(2)) < GeometRi3D.Tolerance);
        }

        [TestMethod()]
        public void PointProjectionToPlaneTest()
        {
            Point3d p1 = new Point3d(-4, 3, 5);
            Plane3d s1 = new Plane3d(-1, 2, -2, 9);
            Assert.IsTrue(p1.ProjectionTo(s1) == new Point3d(-3, 1, 7));

            Coord3d coord1 = new Coord3d(new Point3d(2, 3, 1), Matrix3d.RotationMatrix(new Vector3d(2, 1, 5), PI / 3));
            p1 = p1.ConvertTo(coord1);
            Assert.IsTrue(p1.ProjectionTo(s1) == new Point3d(-3, 1, 7));
        }

        [TestMethod()]
        public void PointProjectionToLineTest()
        {
            Point3d p1 = new Point3d(-4, 3, 5);
            Point3d p2 = new Point3d(1, -5, -1);
            Vector3d v2 = new Vector3d(-2, 3, 4);
            Line3d l1 = new Line3d(p2, v2);
            Assert.IsTrue(p1.ProjectionTo(l1) == new Point3d(-3, 1, 7));

            Coord3d coord1 = new Coord3d(new Point3d(2, 3, 1), Matrix3d.RotationMatrix(new Vector3d(2, 1, 5), PI / 3));
            p1 = p1.ConvertTo(coord1);
            Assert.IsTrue(p1.ProjectionTo(l1) == new Point3d(-3, 1, 7));
        }

        [TestMethod()]
        public void PointProjectionToSphereTest()
        {
            Point3d p1 = new Point3d(1, 1, 1);
            Sphere s = new Sphere(p1, 2);
            Point3d p2 = new Point3d(5, 5, 5);

            Assert.IsTrue(p2.ProjectionTo(s) == new Point3d(1 + 2 / Sqrt(3), 1 + 2 / Sqrt(3), 1 + 2 / Sqrt(3)));

            Coord3d coord1 = new Coord3d(new Point3d(2, 3, 1), Matrix3d.RotationMatrix(new Vector3d(2, 1, 5), PI / 3));
            p2 = p2.ConvertTo(coord1);
            Assert.IsTrue(p2.ProjectionTo(s) == new Point3d(1 + 2 / Sqrt(3), 1 + 2 / Sqrt(3), 1 + 2 / Sqrt(3)));
        }

        [TestMethod()]
        public void PointBelongsToLineTest()
        {
            Point3d p1 = new Point3d(1, -5, -1);
            Vector3d v1 = new Vector3d(-2, 3, 4);
            Line3d l1 = new Line3d(p1, v1);
            Point3d p2 = p1.Translate(3 * v1);
            Assert.IsTrue(p2.BelongsTo(l1));
        }

        [TestMethod()]
        public void PointBelongsToPlaneTest()
        {
            Plane3d s1 = new Plane3d(-1, 2, -2, 9);
            Point3d p1 = s1.Point;
            Assert.IsTrue(p1.BelongsTo(s1));

            s1 = new Plane3d(new Point3d(0,0,10), new Vector3d(0,0,1));
            p1 = new Point3d(0, 0, 10);
            Assert.IsTrue(p1.BelongsTo(s1));

            double tol = GeometRi3D.Tolerance;
            bool mode = GeometRi3D.UseAbsoluteTolerance;
            GeometRi3D.Tolerance = 0.01;
            GeometRi3D.UseAbsoluteTolerance = false;

            s1 = new Plane3d(new Point3d(0, 0, 10), new Vector3d(0, 0, 100));
            p1 = new Point3d(0, 0, 10.05);
            Assert.IsTrue(p1.BelongsTo(s1));
            p1 = new Point3d(0, 0, 10.15);
            Assert.IsFalse(p1.BelongsTo(s1));

            // Resore initial state
            GeometRi3D.UseAbsoluteTolerance = mode;
            GeometRi3D.Tolerance = tol;
        }

        [TestMethod()]
        public void PointBelongsToRayTest()
        {
            Point3d p = new Point3d(0, 0, 0);
            Ray3d r = new Ray3d(new Point3d(1, 1, 0), new Vector3d(1, 0, 0));
            Assert.IsFalse(p.BelongsTo(r));

            p = new Point3d(1, 1, 0);
            Assert.IsTrue(p.BelongsTo(r));

            p = new Point3d(3, 1, 0);
            Assert.IsTrue(p.BelongsTo(r));
        }

        [TestMethod()]
        public void PointBelongsToSegmentTest()
        {
            Point3d p = new Point3d(0, 0, 0);
            Segment3d s = new Segment3d(new Point3d(1, 1, 0), new Point3d(3, 3, 0));
            Assert.IsFalse(p.BelongsTo(s));

            p = new Point3d(1, 1, 0);
            Assert.IsTrue(p.BelongsTo(s));
            p = new Point3d(2, 2, 0);
            Assert.IsTrue(p.BelongsTo(s));
            p = new Point3d(3, 3, 0);
            Assert.IsTrue(p.BelongsTo(s));
        }

        [TestMethod()]
        public void PointEqualsNullTest()
        {
            Point3d p1 = null;
            Point3d p2 = new Point3d(0, 0, 0);

            Assert.IsTrue(p1 != p2);
            Assert.IsFalse(p1 == p2);

            p2 = null;
            Assert.IsTrue(p1 == p2);
            Assert.IsFalse(p1 != p2);
        }
    }
}
