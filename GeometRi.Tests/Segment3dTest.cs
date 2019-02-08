using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

namespace GeometRi_Tests
{
    [TestClass]
    public class Segment3dTest
    {
        //===============================================================
        // Segment3d tests
        //===============================================================

        [TestMethod()]
        public void SegmentProjectionToLineTest()
        {
            Line3d l = new Line3d(new Point3d(1, 1, 1), new Vector3d(0, 0, 1));
            Segment3d r = new Segment3d(new Point3d(-1, -3, -2), new Point3d(-5, 1, -3));
            Assert.IsTrue((Segment3d)r.ProjectionTo(l) == new Segment3d(new Point3d(1, 1, -2), new Point3d(1, 1, -3)));
        }

        [TestMethod()]
        public void SegmentProjectionToLineTest2()
        {
            Line3d l = new Line3d(new Point3d(1, 1, 1), new Vector3d(0, 0, 1));
            Segment3d r = new Segment3d(new Point3d(-1, -3, -2), new Point3d(-5, 1, -2));
            Assert.IsTrue((Point3d)r.ProjectionTo(l) == new Point3d(1, 1, -2));
        }

        [TestMethod()]
        public void SegmentProjectionToPlaneTest()
        {
            Plane3d s = new Plane3d(0, 0, 1, -1);
            Segment3d r = new Segment3d(new Point3d(-1, -3, -2), new Point3d(-5, 1, -3));
            Assert.IsTrue((Segment3d)r.ProjectionTo(s) == new Segment3d(new Point3d(-1, -3, 1), new Point3d(-5, 1, 1)));
        }

        [TestMethod()]
        public void SegmentIntersectionWithPlaneTest()
        {
            Plane3d s = new Plane3d(0, 0, 1, -1);
            Segment3d r = new Segment3d(new Point3d(-1, -3, -2), new Point3d(-5, 1, -3));
            Assert.IsTrue(r.IntersectionWith(s) == null);

            r = new Segment3d(new Point3d(-1, -3, 1), new Point3d(-5, 1, 6));
            Assert.IsTrue((Point3d)r.IntersectionWith(s) == new Point3d(-1, -3, 1));

            r = new Segment3d(new Point3d(-1, -3, -5), new Point3d(-5, 1, 1));
            Assert.IsTrue((Point3d)r.IntersectionWith(s) == new Point3d(-5, 1, 1));

            r = new Segment3d(new Point3d(-1, -3, 1), new Point3d(-5, 1, 1));
            Assert.IsTrue((Segment3d)r.IntersectionWith(s) == r);
        }

        [TestMethod()]
        public void SegmentDistanceToPlaneTest()
        {
            Plane3d s = new Plane3d(0, 0, 1, -1);
            Segment3d r = new Segment3d(new Point3d(-1, -3, -2), new Point3d(-5, 1, -3));
            Assert.IsTrue(Abs(r.DistanceTo(s) - 3) < GeometRi3D.Tolerance);

            r = new Segment3d(new Point3d(-1, -3, 1), new Point3d(-5, 1, 6));
            Assert.IsTrue(Abs(r.DistanceTo(s)) < GeometRi3D.Tolerance);

            r = new Segment3d(new Point3d(-1, -3, -5), new Point3d(-5, 1, 1));
            Assert.IsTrue(Abs(r.DistanceTo(s)) < GeometRi3D.Tolerance);

            r = new Segment3d(new Point3d(-1, -3, 1), new Point3d(-5, 1, 1));
            Assert.IsTrue(Abs(r.DistanceTo(s)) < GeometRi3D.Tolerance);
        }

        [TestMethod()]
        public void SegmentDistanceToLineTest()
        {
            Line3d l = new Line3d(new Point3d(), new Vector3d(1, 0, 0));
            Segment3d r = new Segment3d(new Point3d(-2, -3, 1), new Point3d(-5, 6, 1));
            Assert.IsTrue(Abs(r.DistanceTo(l) - 1) < GeometRi3D.Tolerance);

            r = new Segment3d(new Point3d(-2, 0, 1), new Point3d(-5, 6, 5));
            Assert.IsTrue(Abs(r.DistanceTo(l) - 1) < GeometRi3D.Tolerance);

            r = new Segment3d(new Point3d(-5, -6, 0), new Point3d(-2, -2, 0));
            Assert.IsTrue(Abs(r.DistanceTo(l) - 2) < GeometRi3D.Tolerance);

            r = new Segment3d(new Point3d(-5, -6, 0), new Point3d(0, 0, 0));
            Assert.IsTrue(Abs(r.DistanceTo(l)) < GeometRi3D.Tolerance);
        }

        [TestMethod()]
        public void SegmentIntersectionWithLineTest()
        {
            Line3d l = new Line3d(new Point3d(), new Vector3d(1, 0, 0));
            Segment3d s = new Segment3d(new Point3d(-2, -3, 1), new Point3d(-5, 6, 1));
            Assert.IsTrue(s.IntersectionWith(l) == null);

            s = new Segment3d(new Point3d(1, 1, 0), new Point3d(1, 5, 0));
            Assert.IsTrue(s.IntersectionWith(l) == null);

            s = new Segment3d(new Point3d(5, 0, 0), new Point3d(10, 0, 0));
            Assert.IsTrue((Segment3d)s.IntersectionWith(l) == s);

            s = new Segment3d(new Point3d(5, 0, 0), new Point3d(10, 10, 10));
            Assert.IsTrue((Point3d)s.IntersectionWith(l) == new Point3d(5, 0, 0));

            s = new Segment3d(new Point3d(5, -1, 0), new Point3d(7, 1, 0));
            Assert.IsTrue((Point3d)s.IntersectionWith(l) == new Point3d(6, 0, 0));
        }

        [TestMethod()]
        public void SegmentIntersectionWithSegmentTest()
        {
            Segment3d s1 = new Segment3d(new Point3d(0, 0, 0), new Point3d(5, 5, 0));

            // Coincided segments
            Segment3d s2 = new Segment3d(new Point3d(5, 5, 0), new Point3d(0, 0, 0));
            Assert.IsTrue((Segment3d)s1.IntersectionWith(s2) == s1);

            // Parallel segments
            s2 = new Segment3d(new Point3d(5, 5, 1), new Point3d(0, 0, 1));
            Assert.IsNull(s1.IntersectionWith(s2));

            // Collinear nonintersecting segments
            s2 = new Segment3d(new Point3d(6, 6, 0), new Point3d(7, 7, 0));
            Assert.IsNull(s1.IntersectionWith(s2));

            // Nonintersecting segments
            s2 = new Segment3d(new Point3d(2, 1, 0), new Point3d(5, 0, 0));
            Assert.IsNull(s1.IntersectionWith(s2));

            // Touching segments
            s2 = new Segment3d(new Point3d(2, 2, 0), new Point3d(5, 0, 0));
            Assert.IsTrue((Point3d)s1.IntersectionWith(s2) == new Point3d(2, 2, 0));

            // Intersecting at one end
            s2 = new Segment3d(new Point3d(3, 4, 0), new Point3d(0, 0, 0));
            Assert.IsTrue((Point3d)s1.IntersectionWith(s2) == new Point3d(0, 0, 0));

            // Intersecting at the middle
            s2 = new Segment3d(new Point3d(2, 4, 0), new Point3d(4, 2, 0));
            Assert.IsTrue((Point3d)s1.IntersectionWith(s2) == new Point3d(3, 3, 0));

            // Overlaping segments, s1 > s2
            s2 = new Segment3d(new Point3d(2, 2, 0), new Point3d(3, 3, 0));
            Assert.IsTrue((Segment3d)s1.IntersectionWith(s2) == new Segment3d(new Point3d(2, 2, 0), new Point3d(3, 3, 0)));

            // Overlaping segments, s2 > s1
            s2 = new Segment3d(new Point3d(-2, -2, 0), new Point3d(7, 7, 0));
            Assert.IsTrue((Segment3d)s1.IntersectionWith(s2) == new Segment3d(new Point3d(0, 0, 0), new Point3d(5, 5, 0)));

            // Partly overlaping segments
            s2 = new Segment3d(new Point3d(3, 3, 0), new Point3d(-2, -2, 0));
            Assert.IsTrue((Segment3d)s1.IntersectionWith(s2) == new Segment3d(new Point3d(0, 0, 0), new Point3d(3, 3, 0)));

        }


        [TestMethod()]
        public void SegmentIntersectionWithSegmentRelativeTest()
        {
            Segment3d s1 = new Segment3d(new Point3d(0, 0, 0), new Point3d(5, 5, 0));

            double tol = GeometRi3D.Tolerance;
            bool mode = GeometRi3D.UseAbsoluteTolerance;
            GeometRi3D.Tolerance = 0.01;
            GeometRi3D.UseAbsoluteTolerance = false;

            // Coincided segments
            Segment3d s2 = new Segment3d(new Point3d(5, 5, 0.0), new Point3d(0, 0, 0.01));
            Assert.IsTrue((Segment3d)s1.IntersectionWith(s2) == s1);

            // Parallel segments
            s2 = new Segment3d(new Point3d(5, 5, 1.01), new Point3d(0, 0, 1));
            Assert.IsNull(s1.IntersectionWith(s2));

            // Collinear nonintersecting segments
            s2 = new Segment3d(new Point3d(6, 6, 0.01), new Point3d(7, 7, 0));
            Assert.IsNull(s1.IntersectionWith(s2));

            // Nonintersecting segments
            s2 = new Segment3d(new Point3d(2, 1, 0.01), new Point3d(5, 0, 0));
            Assert.IsNull(s1.IntersectionWith(s2));

            // Touching segments
            s2 = new Segment3d(new Point3d(2, 2, 0.01), new Point3d(5, 0, 0));
            Assert.IsTrue((Point3d)s1.IntersectionWith(s2) == new Point3d(2, 2, 0));

            // Intersecting at one end
            s2 = new Segment3d(new Point3d(3, 4, 0), new Point3d(0, 0, 0.001));
            Assert.IsTrue((Point3d)s1.IntersectionWith(s2) == new Point3d(0, 0, 0));

            // Intersecting at the middle
            s2 = new Segment3d(new Point3d(2, 4, 0.01), new Point3d(4, 2, 0));
            Assert.IsTrue((Point3d)s1.IntersectionWith(s2) == new Point3d(3, 3, 0));

            // Overlaping segments, s1 > s2
            s2 = new Segment3d(new Point3d(2, 2, 0.01), new Point3d(3, 3, 0));
            Assert.IsTrue((Segment3d)s1.IntersectionWith(s2) == new Segment3d(new Point3d(2, 2, 0), new Point3d(3, 3, 0)));

            // Overlaping segments, s2 > s1
            s2 = new Segment3d(new Point3d(-2, -2, 0.01), new Point3d(7, 7, 0));
            Assert.IsTrue((Segment3d)s1.IntersectionWith(s2) == new Segment3d(new Point3d(0, 0, 0), new Point3d(5, 5, 0)));

            // Partly overlaping segments
            s2 = new Segment3d(new Point3d(3, 3, 0.01), new Point3d(-2, -2, 0));
            Assert.IsTrue((Segment3d)s1.IntersectionWith(s2) == new Segment3d(new Point3d(0, 0, 0), new Point3d(3, 3, 0)));

            // Resore initial state
            GeometRi3D.UseAbsoluteTolerance = mode;
            GeometRi3D.Tolerance = tol;
        }

        [TestMethod()]
        public void SegmentBelongsToLineTest()
        {
            Line3d l = new Line3d(new Point3d(), new Vector3d(1, 0, 0));
            Segment3d r = new Segment3d(new Point3d(5, 0, 0), new Point3d(10, 0, 0));
            Assert.IsTrue(r.BelongsTo(l));

            r = new Segment3d(new Point3d(5, 0, 0), new Point3d(5, 5, 5));
            Assert.IsFalse(r.BelongsTo(l));

            r = new Segment3d(new Point3d(-5, -6, 0), new Point3d(-2, -2, 0));
            Assert.IsFalse(r.BelongsTo(l));
        }

        [TestMethod()]
        public void SegmentDistanceToSegmentTest()
        {
            Segment3d s1 = new Segment3d(new Point3d(-5, 0, 0), new Point3d(5, 0, 0));
            Segment3d s2 = new Segment3d(new Point3d(-2, -3, 1), new Point3d(5, 6, 1));
            Assert.IsTrue(Abs(s1.DistanceTo(s2) - 1) < GeometRi3D.Tolerance);

            s2 = new Segment3d(new Point3d(6, -3, 1), new Point3d(6, 6, 1));
            Assert.IsTrue(Abs(s1.DistanceTo(s2) - Sqrt(2)) < GeometRi3D.Tolerance);

            s2 = new Segment3d(new Point3d(2, 4, 0), new Point3d(6, 8, 0));
            Assert.IsTrue(Abs(s1.DistanceTo(s2) - 4) < GeometRi3D.Tolerance);

            s2 = new Segment3d(new Point3d(2, 4, 5), new Point3d(4, 0, 2));
            Assert.IsTrue(Abs(s1.DistanceTo(s2) - 2) < GeometRi3D.Tolerance);

            s2 = new Segment3d(new Point3d(-7, -6, 0), new Point3d(-5, -2, 0));
            Assert.IsTrue(Abs(s1.DistanceTo(s2) - 2) < GeometRi3D.Tolerance);
        }

        [TestMethod()]
        public void SegmentDistanceToRayTest()
        {
            Segment3d s1 = new Segment3d(new Point3d(-5, 0, 0), new Point3d(5, 0, 0));
            Ray3d r = s1.ToRay;

            Segment3d s2 = new Segment3d(new Point3d(-2, -3, 1), new Point3d(5, 6, 1));
            Assert.IsTrue(Abs(s2.DistanceTo(r) - 1) < GeometRi3D.Tolerance);

            s2 = new Segment3d(new Point3d(6, -3, 1), new Point3d(6, 6, 1));
            Assert.IsTrue(Abs(s2.DistanceTo(r) - 1) < GeometRi3D.Tolerance);

            s2 = new Segment3d(new Point3d(2, 4, 0), new Point3d(6, 8, 0));
            Assert.IsTrue(Abs(s2.DistanceTo(r) - 4) < GeometRi3D.Tolerance);

            s2 = new Segment3d(new Point3d(2, 4, 5), new Point3d(4, 0, 2));
            Assert.IsTrue(Abs(s2.DistanceTo(r) - 2) < GeometRi3D.Tolerance);

            s2 = new Segment3d(new Point3d(-7, -6, 0), new Point3d(-5, -2, 0));
            Assert.IsTrue(Abs(s2.DistanceTo(r) - 2) < GeometRi3D.Tolerance);
        }

        [TestMethod()]
        public void SegmentAngleToPlaneTest()
        {
            Plane3d s = new Plane3d(0, 0, 1, -1);
            Segment3d r1 = new Segment3d(new Point3d(0, 0, 3), new Point3d(1, 0, 4));
            Segment3d r2 = new Segment3d(new Point3d(0, 0, -3), new Point3d(1, 0, -4));
            Assert.IsTrue(Abs(r1.AngleToDeg(s) - 45) < GeometRi3D.Tolerance && Abs(r1.AngleTo(s) - r2.AngleTo(s)) < GeometRi3D.Tolerance);
        }

        [TestMethod()]
        public void SegmentEqualsTest()
        {
            Point3d p1 = new Point3d(1, 4, 6);
            Point3d p2 = new Point3d(8, -4, 0);
            Segment3d r1 = new Segment3d(p1, p2);
            Segment3d r2 = new Segment3d(p2, p1);
            Assert.IsTrue(r1 == r2);
        }

        [TestMethod]
        public void PointInSegmentTest()
        {
            Point3d p = new Point3d(0, 0, 0);
            Segment3d s = new Segment3d(p, new Point3d(10,0,0));

            p = new Point3d(5, 0, 0);  // Point inside
            Assert.IsTrue(p.BelongsTo(s));
            Assert.IsTrue(p.IsInside(s));
            Assert.IsFalse(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));

            p = new Point3d(10, 0, 0);  // Point on boundary
            Assert.IsTrue(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsFalse(p.IsOutside(s));
            Assert.IsTrue(p.IsOnBoundary(s));

            p = new Point3d(1, 3, 0);  // Point outside
            Assert.IsFalse(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsTrue(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));

            p = new Point3d(11, 0, 0);  // Point outside
            Assert.IsFalse(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsTrue(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));
        }

        [TestMethod]
        public void PointInSegmentRelativeTest()
        {
            Point3d p = new Point3d(0, 0, 0);
            Segment3d s = new Segment3d(p, new Point3d(10, 0, 0));

            double tol = GeometRi3D.Tolerance;
            bool mode = GeometRi3D.UseAbsoluteTolerance;
            GeometRi3D.Tolerance = 0.01;
            GeometRi3D.UseAbsoluteTolerance = false;

            p = new Point3d(5, 0, 0.05);  // Point inside
            Assert.IsTrue(p.BelongsTo(s));
            Assert.IsTrue(p.IsInside(s));
            Assert.IsFalse(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));

            p = new Point3d(10.05, 0, 0);  // Point on boundary
            Assert.IsTrue(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsFalse(p.IsOutside(s));
            Assert.IsTrue(p.IsOnBoundary(s));

            p = new Point3d(1, 0.2, 0);  // Point outside
            Assert.IsFalse(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsTrue(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));

            p = new Point3d(10.2, 0, 0);  // Point outside
            Assert.IsFalse(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsTrue(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));

            // Resore initial state
            GeometRi3D.UseAbsoluteTolerance = mode;
            GeometRi3D.Tolerance = tol;
        }

        [TestMethod()]
        public void SegmentDistanceToCircleTest()
        {
            Point3d p1 = new Point3d(-0.5, -0.5, -0.5);
            Point3d p2 = new Point3d(0.5, 0.5, -0.5);
            Segment3d s = new Segment3d(p1, p2);

            Circle3d c = new Circle3d(new Point3d(-1.3195, -1.0435, -0.70047), 0.35, new Vector3d(0.83694, -0.13208, -0.53112));

            double dist = s.DistanceTo(c);
            Assert.IsTrue(dist > 0);


        }
    }
}
