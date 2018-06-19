using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

namespace GeometRi_Tests
{
    [TestClass]
    public class Plane3dTest
    {
        //===============================================================
        // Plane3d tests
        //===============================================================

        [TestMethod()]
        public void PlaneIntersectionWithLineTest()
        {
            // Inclined line
            Point3d p1 = new Point3d(1, -5, -1);
            Vector3d v1 = new Vector3d(-2, 3, 4);
            Line3d l1 = new Line3d(p1, v1);
            Plane3d s1 = new Plane3d(-2, 2, 3, -29);

            Assert.IsTrue((Point3d)s1.IntersectionWith(l1) == new Point3d(-3, 1, 7));
        }

        [TestMethod()]
        public void PlaneIntersectionWithLineTest2()
        {
            // Parallel line
            Point3d p1 = new Point3d(1, 0, 0);
            Vector3d v1 = new Vector3d(1, 1, 1);
            Plane3d s1 = new Plane3d(p1, v1);
            p1 = new Point3d(0, 2, 0);
            Point3d p2 = new Point3d(0, 0, 2);
            Line3d l1 = new Line3d(p1, new Vector3d(p1, p2));

            Assert.IsTrue(s1.IntersectionWith(l1) == null);
        }

        [TestMethod()]
        public void PlaneIntersectionWithLineTest3()
        {
            // Line lies in the plane
            Point3d p1 = new Point3d(1, 0, 0);
            Vector3d v1 = new Vector3d(1, 1, 1);
            Plane3d s1 = new Plane3d(p1, v1);
            p1 = new Point3d(0, 1, 0);
            Point3d p2 = new Point3d(0, 0, 1);
            Line3d l1 = new Line3d(p1, new Vector3d(p1, p2));

            Assert.IsTrue((Line3d)s1.IntersectionWith(l1) == l1);
        }

        //===============================================================
        // Intersection of three planes
        //===============================================================

        [TestMethod()]
        public void PlaneIntersectionWithTwoPlanesTest1()
        {
            // Three coplanar planes
            // Planes do not coincide
            Plane3d s1 = new Plane3d(new Point3d(0, 0, 1), new Vector3d(1, 1, 1));
            Plane3d s2 = new Plane3d(new Point3d(2, 0, 1), new Vector3d(1, 1, 1));
            Plane3d s3 = new Plane3d(new Point3d(0, 3, 1), new Vector3d(-1, -1, -1));

            Assert.IsTrue(s1.IntersectionWith(s2, s3) == null);
        }

        [TestMethod()]
        public void PlaneIntersectionWithTwoPlanesTest2()
        {
            // Three coplanar planes
            // Two planes are coincide
            Plane3d s1 = new Plane3d(new Point3d(0, 0, 1), new Vector3d(1, 1, 1));
            Plane3d s2 = new Plane3d(new Point3d(2, 0, 1), new Vector3d(1, 1, 1));
            Plane3d s3 = new Plane3d(new Point3d(1, 0, 0), new Vector3d(-1, -1, -1));

            Assert.IsTrue(s1.IntersectionWith(s2, s3) == null);
        }

        [TestMethod()]
        public void PlaneIntersectionWithTwoPlanesTest3()
        {
            // Three coplanar planes
            // Three planes are coincide
            Plane3d s1 = new Plane3d(new Point3d(0, 0, 1), new Vector3d(1, 1, 1));
            Plane3d s2 = new Plane3d(new Point3d(0, 1, 0), new Vector3d(1, 1, 1));
            Plane3d s3 = new Plane3d(new Point3d(1, 0, 0), new Vector3d(-1, -1, -1));

            Assert.IsTrue((Plane3d)s1.IntersectionWith(s2, s3) == s1);
        }

        [TestMethod()]
        public void PlaneIntersectionWithTwoPlanesTest4()
        {
            // Three vertical planes (relative to the XY-plane) with common line
            Plane3d s1 = new Plane3d(new Point3d(0, 0, 0), new Vector3d(1, 1, 0));
            Plane3d s2 = new Plane3d(new Point3d(0, 0, 2), new Vector3d(4, 2, 0));
            Plane3d s3 = new Plane3d(new Point3d(0, 0, 4), new Vector3d(-1, 1, 0));

            object obj = s1.IntersectionWith(s2, s3);

            if (obj != null && obj.GetType() == typeof(Line3d))
            {
                Line3d l1 = (Line3d)obj;
                Assert.IsTrue(l1 == new Line3d(new Point3d(0, 0, 0), new Vector3d(0, 0, 1)));
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void PlaneIntersectionWithTwoPlanesTest5()
        {
            // Three vertical planes (relative to the XY-plane) with NO common line
            Plane3d s1 = new Plane3d(new Point3d(1, 0, 0), new Vector3d(1, 1, 0));
            Plane3d s2 = new Plane3d(new Point3d(0, 2, 2), new Vector3d(4, 2, 0));
            Plane3d s3 = new Plane3d(new Point3d(3, 3, 4), new Vector3d(-1, 1, 0));

            object obj = s1.IntersectionWith(s2, s3);

            Assert.IsTrue(obj == null);
        }

        [TestMethod()]
        public void PlaneIntersectionWithTwoPlanesTest6()
        {
            // Three intersecting planes with common point
            Plane3d s1 = new Plane3d(new Point3d(1, 0, 0), new Vector3d(0, 1, 5));
            Plane3d s2 = new Plane3d(new Point3d(0, 2, 0), new Vector3d(4, 0, 0));
            Plane3d s3 = new Plane3d(new Point3d(0, 0, 0), new Vector3d(-1, 1, 3));

            object obj = s1.IntersectionWith(s2, s3);

            if (obj != null && obj.GetType() == typeof(Point3d))
            {
                Point3d p1 = (Point3d)obj;
                Assert.IsTrue(p1 == new Point3d(0, 0, 0));
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void PlaneIntersectionWithTwoPlanesTest()
        {
            Plane3d s1 = new Plane3d(new Point3d(0, 0, 1), new Vector3d(0, 1, 1));
            Plane3d s2 = new Plane3d(-5, 2, 4, 1);
            Plane3d s3 = new Plane3d(2, -3, 2, 4);
            Assert.IsTrue((Point3d)s1.IntersectionWith(s2, s3) == (Point3d)s1.IntersectionWith((Line3d)s2.IntersectionWith(s3)));
        }

        //===============================================================

        [TestMethod()]
        public void PlaneEqualsToPlaneTest()
        {
            Point3d p1 = new Point3d(1, 0, 0);
            Vector3d v1 = new Vector3d(1, 1, 1);
            Plane3d s1 = new Plane3d(p1, v1);

            Point3d p2 = new Point3d(0, 0, 1);
            Vector3d v2 = new Vector3d(-1, -1, -1);
            Plane3d s2 = new Plane3d(p2, v2);

            Assert.IsTrue(s1 == s2);
        }

        [TestMethod()]
        public void PlaneAngleToPlaneTest()
        {
            Point3d p1 = new Point3d(1, 0, 0);
            Vector3d v1 = new Vector3d(0, 1, 1);
            Plane3d s1 = new Plane3d(p1, v1);

            Matrix3d m = Matrix3d.RotationMatrix(new Vector3d(0, -1, 1), 10 * PI / 180);
            v1 = m * v1;
            Plane3d s2 = new Plane3d(p1, v1);

            Assert.IsTrue(Abs(s1.AngleToDeg(s2) - 10) < GeometRi3D.Tolerance);
        }

        [TestMethod()]
        public void PlaneAngleToPlaneTest2()
        {
            Point3d p1 = new Point3d(1, 0, 0);
            Vector3d v1 = new Vector3d(0, 1, 1);
            Plane3d s1 = new Plane3d(p1, v1);

            Matrix3d m = Matrix3d.RotationMatrix(new Vector3d(0, -1, 1), 10 * PI / 180);
            v1 = m * v1;
            Plane3d s2 = new Plane3d(p1, -v1);

            Assert.IsTrue(Abs(s1.AngleToDeg(s2) - 10) < GeometRi3D.Tolerance);
        }
    }
}
