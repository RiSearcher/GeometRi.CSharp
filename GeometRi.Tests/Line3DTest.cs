using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

namespace GeometRi_Tests
{
    [TestClass]
    public class Line3dTest
    {
        //===============================================================
        // Line3d tests
        //===============================================================

        [TestMethod()]
        public void LineDistanceToCrossingLineTest()
        {
            Point3d p2 = new Point3d(1, -5, -1);
            Vector3d v2 = new Vector3d(-2, 3, 4);
            Line3d l1 = new Line3d(p2, v2);
            p2 = new Point3d(-2, 1, 2);
            v2 = new Vector3d(-2, 2, 3);
            Line3d l2 = new Line3d(p2, v2);
            dynamic zzz = l1.DistanceTo(l2);
            Assert.IsTrue(Abs(l1.DistanceTo(l2) - 3) < GeometRi3D.Tolerance);
        }

        [TestMethod()]
        public void LineDistanceToParallelLineTest()
        {
            Point3d p2 = new Point3d(1, -5, -1);
            Vector3d v2 = new Vector3d(-2, 3, 4);
            Line3d l1 = new Line3d(p2, v2);
            p2 = new Point3d(-4, 3, 5);
            v2 = new Vector3d(4, -6, -8);
            Line3d l2 = new Line3d(p2, v2);
            Assert.IsTrue(Abs(l1.DistanceTo(l2) - 3) < GeometRi3D.Tolerance);
        }

        [TestMethod()]
        public void LinePerpendicularToLineTest()
        {
            Point3d p1 = new Point3d(1, -5, -1);
            Vector3d v1 = new Vector3d(-2, 3, 4);
            Line3d l1 = new Line3d(p1, v1);

            Point3d p2 = new Point3d(-2, 1, 2);
            Vector3d v2 = new Vector3d(-2, 2, 3);
            Line3d l2 = new Line3d(p2, v2);

            Assert.IsTrue(l1.PerpendicularTo(l2) == new Point3d(-4, 3, 5));
        }

        [TestMethod()]
        public void LineIntersectionWithLineTest()
        {
            Line3d l1 = new Line3d(new Point3d(), new Vector3d(1, 0, 0));
            Line3d l2 = new Line3d(new Point3d(8,0,0), new Vector3d(-5, 0, 0));
            Assert.IsTrue((Line3d)l1.IntersectionWith(l2) == l1);

            l2 = new Line3d(new Point3d(5, 5, 0), new Vector3d(1, -1, 0));
            Assert.IsTrue((Point3d)l1.IntersectionWith(l2) == new Point3d(10,0,0));

            l2 = new Line3d(new Point3d(5, 5, 1), new Vector3d(1, -1, 0));
            Assert.IsTrue((Object)l1.IntersectionWith(l2) == null);

        }

        [TestMethod()]
        public void LineIntersectionWithPlaneTest()
        {
            Point3d p1 = new Point3d(1, -5, -1);
            Vector3d v1 = new Vector3d(-2, 3, 4);
            Line3d l1 = new Line3d(p1, v1);
            Plane3d s1 = new Plane3d(-2, 2, 3, -29);

            Assert.IsTrue((Point3d)l1.IntersectionWith(s1) == new Point3d(-3, 1, 7));
        }

        [TestMethod()]
        public void LineIntersectionWithPlaneTest2()
        {
            // Parallel line
            Point3d p1 = new Point3d(1, 0, 0);
            Vector3d v1 = new Vector3d(1, 1, 1);
            Plane3d s1 = new Plane3d(p1, v1);
            p1 = new Point3d(0, 2, 0);
            Point3d p2 = new Point3d(0, 0, 2);
            Line3d l1 = new Line3d(p1, new Vector3d(p1, p2));

            Assert.IsTrue(l1.IntersectionWith(s1) == null);
        }

        [TestMethod()]
        public void LineIntersectionWithPlaneTest3()
        {
            // Line lies in the plane
            Point3d p1 = new Point3d(1, 0, 0);
            Vector3d v1 = new Vector3d(1, 1, 1);
            Plane3d s1 = new Plane3d(p1, v1);
            p1 = new Point3d(0, 1, 0);
            Point3d p2 = new Point3d(0, 0, 1);
            Line3d l1 = new Line3d(p1, new Vector3d(p1, p2));

            Assert.IsTrue((Line3d)l1.IntersectionWith(s1) == l1);
        }

        [TestMethod()]
        public void LineProjectionToPlaneTest()
        {
            Point3d p1 = new Point3d(1, -5, -1);
            Vector3d v1 = new Vector3d(-2, 3, 4);
            Line3d l1 = new Line3d(p1, v1);
            Plane3d s1 = new Plane3d(-2, 2, 3, -29);

            Point3d p2 = (Point3d)l1.IntersectionWith(s1);
            Line3d l2 = (Line3d)l1.ProjectionTo(s1);

            Assert.IsTrue(p2.BelongsTo(l2));
        }
    }
}
