using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

namespace GeometRi_Tests
{
    [TestClass]
    public class SphereTest
    {
        //===============================================================
        // Sphere tests
        //===============================================================

        [TestMethod()]
        public void SphereEqualTest()
        {
            Sphere s1 = new Sphere(new Point3d(0, 0, 0), 5);
            Sphere s2 = new Sphere(new Point3d(0, 0, 0), 6);
            Assert.IsTrue(s1 != s2);

            s1 = new Sphere(new Point3d(0, 0, 0), 5);
            s2 = new Sphere(new Point3d(1, 0, 0), 5);
            Assert.IsTrue(s1 != s2);

            s1 = new Sphere(new Point3d(0, 0, 0), 5);
            s2 = new Sphere(new Point3d(0, 0, 0), 5);
            Assert.IsTrue(s1 == s2);
        }

        [TestMethod()]
        public void SphereIntersectionWithLineTest()
        {
            Line3d l = new Line3d(new Point3d(5, 0, -6), new Vector3d(1, 0, 0));
            Sphere s = new Sphere(new Point3d(0, 0, 0), 5);

            Assert.IsTrue(s.IntersectionWith(l) == null);

            l.Point = new Point3d(5, 0, -5);
            Assert.IsTrue((Point3d)s.IntersectionWith(l) == new Point3d(0, 0, -5));

            l.Point = new Point3d(0, 0, 0);
            l.Direction = new Vector3d(1, 0, 0);
            Assert.IsTrue((Segment3d)s.IntersectionWith(l) == new Segment3d(new Point3d(-5, 0, 0), new Point3d(5, 0, 0)));

            l.Direction = new Vector3d(1, 3, 4);
            Assert.IsTrue(((Segment3d)s.IntersectionWith(l)).Length == 10);
        }

        [TestMethod()]
        public void SphereIntersectionWithSegmentRelativeTest()
        {

            double tol = GeometRi3D.Tolerance;
            bool mode = GeometRi3D.UseAbsoluteTolerance;
            GeometRi3D.Tolerance = 0.01;
            GeometRi3D.UseAbsoluteTolerance = false;

            Sphere s = new Sphere(new Point3d(0, 0, 0), 5);
            Segment3d l = new Segment3d(new Point3d(10, 12, 13), new Point3d(22, 23, 24));
            Assert.IsTrue(s.IntersectionWith(l) == null);

            l = new Segment3d(new Point3d(-2, -2, -1), new Point3d(1, 2, 2));
            Assert.IsTrue((Segment3d)s.IntersectionWith(l) == l);

            l = new Segment3d(new Point3d(5.01, 0, 0), new Point3d(25, 0, 0));
            Assert.IsTrue((Point3d)s.IntersectionWith(l) == new Point3d(5, 0, 0));

            l = new Segment3d(new Point3d(0, 0, 0), new Point3d(25, 0, 0));
            Assert.IsTrue((Segment3d)s.IntersectionWith(l) == new Segment3d(new Point3d(0, 0, 0), new Point3d(5, 0, 0)));

            // Resore initial state
            GeometRi3D.UseAbsoluteTolerance = mode;
            GeometRi3D.Tolerance = tol;
        }

        [TestMethod()]
        public void SphereIntersectionWithRayTest()
        {

            Sphere s = new Sphere(new Point3d(0, 0, 0), 5);
            Ray3d r = new Ray3d(new Point3d(0, 0, 0), new Vector3d(1, 0, 0));
            Assert.AreEqual(r.IntersectionWith(s), new Segment3d(new Point3d(0, 0, 0), new Point3d(5, 0, 0)));

            r = new Ray3d(new Point3d(0, 5, 0), new Vector3d(1, 0, 0));
            Assert.AreEqual(s.IntersectionWith(r), new Point3d(0, 5, 0));
        }

        [TestMethod()]
        public void SphereIntersectionWithPlaneTest()
        {
            Sphere s = new Sphere(new Point3d(1, -1, 3), 3);
            Plane3d p = new Plane3d(1, 4, 5, 6);
            Circle3d c = (Circle3d)s.IntersectionWith(p);
            Assert.IsTrue(Abs(c.R - 1.13) < 0.005);
            Assert.IsTrue(c.Center.DistanceTo(new Point3d(0.57, -2.71, 0.86)) < 0.01);
        }

        [TestMethod()]
        public void SphereIntersectionWithSphereTest()
        {
            Sphere s1 = new Sphere(new Point3d(-2, 2, 4), 5);
            Sphere s2 = new Sphere(new Point3d(3, 7, 3), 5);
            Circle3d c1 = (Circle3d)s1.IntersectionWith(s2);
            Assert.IsTrue(Abs(c1.R - 3.5) < GeometRi3D.Tolerance);
            Assert.IsTrue(c1.Center == new Point3d(0.5, 4.5, 3.5));

            Circle3d c2 = (Circle3d)s2.IntersectionWith(s1);
            Assert.IsTrue(c1 == c2);
        }

        [TestMethod()]
        public void SphereProjectionToPlaneTest()
        {
            Sphere s = new Sphere(new Point3d(-2, -2, -2), 5);
            Plane3d p = new Plane3d(new Point3d(1, 1, 1), new Vector3d(1, 1, 1));
            Circle3d c = s.ProjectionTo(p);
            Circle3d res = new Circle3d(new Point3d(1, 1, 1), 5, new Vector3d(-1, -1, -1));
            Assert.AreEqual(c, res);
        }

        [TestMethod()]
        public void SphereProjectionToLineTest()
        {
            Sphere s = new Sphere(new Point3d(-4, -3, -2), 5);
            Line3d l = new Line3d(new Point3d(0, 0, 0), new Vector3d(4, 3, 0));
            Segment3d c = s.ProjectionTo(l);
            Segment3d res = new Segment3d(new Point3d(0, 0, 0), new Point3d(-8, -6, 0));
            Assert.AreEqual(c, res);
        }

        [TestMethod]
        public void PointInSphereTest()
        {
            Point3d p = new Point3d(1, 1, 1);
            Sphere s = new Sphere(p, 5);

            p = new Point3d(2, 2, 2);  // Point inside
            Assert.IsTrue(p.BelongsTo(s));
            Assert.IsTrue(p.IsInside(s));
            Assert.IsFalse(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));

            p = new Point3d(1, 6, 1);  // Point on surface
            Assert.IsTrue(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsFalse(p.IsOutside(s));
            Assert.IsTrue(p.IsOnBoundary(s));

            p = new Point3d(1, 6.005, 1);  // Point outside
            Assert.IsFalse(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsTrue(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));
        }

        [TestMethod]
        public void PointInSphereRelativeTest()
        {
            Point3d p = new Point3d(1, 1, 1);
            Sphere s = new Sphere(p, 5);

            double tol = GeometRi3D.Tolerance;
            bool mode = GeometRi3D.UseAbsoluteTolerance;
            GeometRi3D.Tolerance = 0.01;
            GeometRi3D.UseAbsoluteTolerance = false;

            p = new Point3d(1, 5.9, 1);  // Point inside
            Assert.IsTrue(p.BelongsTo(s));
            Assert.IsTrue(p.IsInside(s));
            Assert.IsFalse(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));

            p = new Point3d(1, 6.04, 1);  // Point on surface
            Assert.IsTrue(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsFalse(p.IsOutside(s));
            Assert.IsTrue(p.IsOnBoundary(s));

            p = new Point3d(1, 6.06, 1);  // Point outside
            Assert.IsFalse(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsTrue(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));

            // Resore initial state
            GeometRi3D.UseAbsoluteTolerance = mode;
            GeometRi3D.Tolerance = tol;
        }

        [TestMethod]
        public void SphereInBoxTest()
        {
            Box3d b = new Box3d(new Point3d(), 1.0, 1.0, 1.0);

            Point3d p = new Point3d(5, 0, 0);
            Sphere s = new Sphere(p, 0.1);
            Assert.IsFalse(s.IsInside(b));

            p = new Point3d(0.4, 0, 0);
            s = new Sphere(p, 0.1);
            Assert.IsFalse(s.IsInside(b));

            p = new Point3d(0.3, 0, 0);
            s = new Sphere(p, 0.1);
            Assert.IsTrue(s.IsInside(b));
        }

        [TestMethod()]
        public void SphereDistanceToSphereTest()
        {
            Sphere s1 = new Sphere(new Point3d(0, 0, 0), 1.0);
            Point3d p1, p2;

            // Intersecting objects
            Sphere s2 = new Sphere(new Point3d(1, 0, 0), 1.0);
            double dist = s1.DistanceTo(s2, out p1, out p2);
            Assert.AreEqual(dist, 0.0);
            Assert.AreEqual(p1, new Point3d(1, 0, 0));
            Assert.AreEqual(p2, new Point3d(0, 0, 0));

            // Touching objects
            s2 = new Sphere(new Point3d(2, 0, 0), 1.0);
            dist = s1.DistanceTo(s2, out p1, out p2);
            Assert.AreEqual(dist, 0.0);
            Assert.AreEqual(p1, new Point3d(1, 0, 0));
            Assert.AreEqual(p2, new Point3d(1, 0, 0));


            // Non-intersecting objects
            s2 = new Sphere(new Point3d(3, 0, 0), 1.0);
            dist = s1.DistanceTo(s2, out p1, out p2);
            Assert.AreEqual(dist, 1.0);
            Assert.AreEqual(p1, new Point3d(1, 0, 0));
            Assert.AreEqual(p2, new Point3d(2, 0, 0));

        }

    }
}
