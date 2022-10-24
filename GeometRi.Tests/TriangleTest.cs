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
        public void TriangleExternalBisectorTest()
        {
            Point3d p1 = new Point3d(0, 1, 0);
            Point3d p2 = new Point3d(1, 0, 0);
            Point3d p3 = new Point3d(-1, 0, 0);
            Line3d line = new Line3d(p1, new Vector3d(1, 0, 0));

            Triangle t = new Triangle(p1, p2, p3);
            Assert.AreEqual(t.ExternalBisector_A, line);

            t = new Triangle(p2, p1, p3);
            Assert.AreEqual(t.ExternalBisector_B, line);

            t = new Triangle(p3, p2, p1);
            Assert.AreEqual(t.ExternalBisector_C, line);
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

        [TestMethod]
        public void PointInTriangleTest()
        {
            Point3d a = new Point3d(0, 0, 0);
            Point3d b = new Point3d(15, 0, 0);
            Point3d p = new Point3d(0, 15, 0);
            Triangle s = new Triangle(a, b, p);

            p = new Point3d(1, 1, 0);  // Point inside
            Assert.IsTrue(p.BelongsTo(s));
            Assert.IsTrue(p.IsInside(s));
            Assert.IsFalse(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));

            p = new Point3d(10, 0, 0);  // Point on boundary
            Assert.IsTrue(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsFalse(p.IsOutside(s));
            Assert.IsTrue(p.IsOnBoundary(s));

            p = new Point3d(1, 3, 1);  // Point outside
            Assert.IsFalse(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsTrue(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));

            p = new Point3d(16, 0, 0);  // Point outside
            Assert.IsFalse(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsTrue(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));
        }

        [TestMethod]
        public void PointInsideRelativeTest()
        {
            Point3d a = new Point3d(0, 0, 0);
            Point3d b = new Point3d(15, 0, 0);
            Point3d p = new Point3d(0, 15, 0);
            Triangle s = new Triangle(a, b, p);

            double tol = GeometRi3D.Tolerance;
            bool mode = GeometRi3D.UseAbsoluteTolerance;
            GeometRi3D.Tolerance = 0.01;
            GeometRi3D.UseAbsoluteTolerance = false;

            p = new Point3d(1, 1, 0.1);  // Point inside
            Assert.IsTrue(p.BelongsTo(s));
            Assert.IsTrue(p.IsInside(s));
            Assert.IsFalse(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));

            p = new Point3d(0.2, 5, 0.0);  // Point inside
            Assert.IsTrue(p.BelongsTo(s));
            Assert.IsTrue(p.IsInside(s));
            Assert.IsFalse(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));

            // Resore initial state
            GeometRi3D.UseAbsoluteTolerance = mode;
            GeometRi3D.Tolerance = tol;
        }

        [TestMethod]
        public void PointOnBoundaryRelativeTest()
        {
            Point3d a = new Point3d(0, 0, 0);
            Point3d b = new Point3d(15, 0, 0);
            Point3d p = new Point3d(0, 15, 0);
            Triangle s = new Triangle(a, b, p);

            double tol = GeometRi3D.Tolerance;
            bool mode = GeometRi3D.UseAbsoluteTolerance;
            GeometRi3D.Tolerance = 0.01;
            GeometRi3D.UseAbsoluteTolerance = false;

            p = new Point3d(10, 0.1, 0);  // Point on boundary
            Assert.IsTrue(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsFalse(p.IsOutside(s));
            Assert.IsTrue(p.IsOnBoundary(s));

            p = new Point3d(0.1, 0.1, 0);  // Point on boundary
            Assert.IsTrue(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsFalse(p.IsOutside(s));
            Assert.IsTrue(p.IsOnBoundary(s));

            p = new Point3d(-0.1, -0.1, 0);  // Point on boundary
            Assert.IsTrue(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsFalse(p.IsOutside(s));
            Assert.IsTrue(p.IsOnBoundary(s));

            p = new Point3d(7.6, 7.6, 0);  // Point on boundary
            Assert.IsTrue(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsFalse(p.IsOutside(s));
            Assert.IsTrue(p.IsOnBoundary(s));

            // Resore initial state
            GeometRi3D.UseAbsoluteTolerance = mode;
            GeometRi3D.Tolerance = tol;
        }

        [TestMethod]
        public void PointOutsideRelativeTest()
        {
            Point3d a = new Point3d(0, 0, 0);
            Point3d b = new Point3d(15, 0, 0);
            Point3d p = new Point3d(0, 15, 0);
            Triangle s = new Triangle(a, b, p);

            double tol = GeometRi3D.Tolerance;
            bool mode = GeometRi3D.UseAbsoluteTolerance;
            GeometRi3D.Tolerance = 0.01;
            GeometRi3D.UseAbsoluteTolerance = false;

            p = new Point3d(1, 1, 0.2);  // Point outside
            Assert.IsFalse(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsTrue(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));

            p = new Point3d(15.2, 0, 0);  // Point outside
            Assert.IsFalse(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsTrue(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));

            p = new Point3d(7.65, 7.65, 0);  // Point outside
            Assert.IsFalse(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsTrue(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));

            p = new Point3d(-0.2, 10, 0);  // Point outside
            Assert.IsFalse(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsTrue(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));

            // Resore initial state
            GeometRi3D.UseAbsoluteTolerance = mode;
            GeometRi3D.Tolerance = tol;
        }

        [TestMethod()]
        public void TriangleIntersectionWithLineTest()
        {
            Point3d p1 = new Point3d(0, 0, 0);
            Point3d p2 = new Point3d(6, 0, 0);
            Point3d p3 = new Point3d(0, 6, 0);

            Triangle t = new Triangle(p1, p2, p3);

            // Line coincides with one side
            Line3d l = new Line3d(new Point3d(), new Vector3d(1, 0, 0));
            Assert.IsTrue((Segment3d)t.IntersectionWith(l) == new Segment3d(p1, p2));

            // Line crosses one corner
            l = new Line3d(new Point3d(), new Vector3d(1, -1, 0));
            Assert.IsTrue((Point3d)t.IntersectionWith(l) == p1);

            // Line crosses one corner and one side
            l = new Line3d(new Point3d(), new Vector3d(1, 1, 0));
            Assert.IsTrue((Segment3d)t.IntersectionWith(l) == new Segment3d(p1, new Point3d(3, 3, 0)));

            // Line crosses two sides
            l = new Line3d(new Point3d(0, 3, 0), new Vector3d(1, -1, 0));
            Assert.IsTrue((Segment3d)t.IntersectionWith(l) == new Segment3d(new Point3d(0, 3, 0), new Point3d(3, 0, 0)));

            // Line crosses triangle
            l = new Line3d(new Point3d(1, 1, 1), new Vector3d(0, 0, 1));
            Assert.IsTrue((Point3d)t.IntersectionWith(l) == new Point3d(1, 1, 0));

            // Line crosses corner
            l = new Line3d(new Point3d(0, 0, 1), new Vector3d(0, 0, 1));
            Assert.IsTrue((Point3d)t.IntersectionWith(l) == p1);

            l = new Line3d(new Point3d(-10, -10, 1), new Vector3d(0, 0, 1));
            Assert.IsNull(t.IntersectionWith(l));

        }

        [TestMethod()]
        public void TriangleIntersectionWithLineRelativeTest()
        {

            double tol = GeometRi3D.Tolerance;
            bool mode = GeometRi3D.UseAbsoluteTolerance;
            GeometRi3D.Tolerance = 0.01;
            GeometRi3D.UseAbsoluteTolerance = false;

            Point3d p1 = new Point3d(0, 0, 0);
            Point3d p2 = new Point3d(6, 0, 0);
            Point3d p3 = new Point3d(0, 6, 0);

            Triangle t = new Triangle(p1, p2, p3);

            // Line coincides with one side
            Line3d l = new Line3d(new Point3d(0, 0, 0.01), new Vector3d(1, 0, 0));
            Assert.IsTrue((Segment3d)t.IntersectionWith(l) == new Segment3d(p1, p2));

            // Line crosses one corner and one side
            l = new Line3d(new Point3d(), new Vector3d(1, 1, 0.001));
            Assert.IsTrue((Segment3d)t.IntersectionWith(l) == new Segment3d(p1, new Point3d(3, 3, 0)));


            // Resore initial state
            GeometRi3D.UseAbsoluteTolerance = mode;
            GeometRi3D.Tolerance = tol;
        }

        [TestMethod()]
        public void TriangleIntersectionWithPlaneTest()
        {
            Point3d p1 = new Point3d(0, 0, 0);
            Point3d p2 = new Point3d(6, 0, 0);
            Point3d p3 = new Point3d(0, 6, 0);

            Triangle t = new Triangle(p1, p2, p3);

            // Plane is parallel
            Plane3d s = new Plane3d(new Point3d(0, 0, 1), new Vector3d(0, 0, 1));
            Assert.IsNull(t.IntersectionWith(s));

            s = new Plane3d(new Point3d(), new Vector3d(0, 0, 1));
            Assert.IsTrue((Triangle)t.IntersectionWith(s) == t);

            // Plane crosses triangle
            p1 = new Point3d(0, 0, 0);
            p2 = new Point3d(6, 0, 1);
            p3 = new Point3d(0, 6, -1);

            t = new Triangle(p1, p2, p3);
            Assert.IsTrue((Segment3d)t.IntersectionWith(s) == new Segment3d(p1, new Point3d(3, 3, 0)));
        }

        [TestMethod()]
        public void TriangleIntersectionWithPlaneRelativeTest()
        {

            double tol = GeometRi3D.Tolerance;
            bool mode = GeometRi3D.UseAbsoluteTolerance;
            GeometRi3D.Tolerance = 0.01;
            GeometRi3D.UseAbsoluteTolerance = false;

            Point3d p1 = new Point3d(0, 0, 0);
            Point3d p2 = new Point3d(6, 0, 0);
            Point3d p3 = new Point3d(0, 6, 0);

            Triangle t = new Triangle(p1, p2, p3);

            Plane3d s = new Plane3d(new Point3d(), new Vector3d(0.001, 0, 1));
            Assert.IsTrue((Triangle)t.IntersectionWith(s) == t);

            // Plane crosses triangle
            p1 = new Point3d(0, 0, 0);
            p2 = new Point3d(6, 0.01, 1);
            p3 = new Point3d(0, 6, -1);

            t = new Triangle(p1, p2, p3);
            Assert.IsTrue((Segment3d)t.IntersectionWith(s) == new Segment3d(p1, new Point3d(3, 3, 0)));

            // Resore initial state
            GeometRi3D.UseAbsoluteTolerance = mode;
            GeometRi3D.Tolerance = tol;
        }

        [TestMethod()]
        public void TriangleIntersectionWithSegmentRelativeTest()
        {

            double tol = GeometRi3D.Tolerance;
            bool mode = GeometRi3D.UseAbsoluteTolerance;
            GeometRi3D.Tolerance = 0.01;
            GeometRi3D.UseAbsoluteTolerance = false;

            Point3d p1 = new Point3d(0, 0, 0);
            Point3d p2 = new Point3d(6, 0, 0);
            Point3d p3 = new Point3d(0, 6, 0);

            Triangle t = new Triangle(p1, p2, p3);

            Segment3d s = new Segment3d(new Point3d(0, 0, 0.01), new Point3d(3.01, 3, 0));
            Assert.IsTrue((Segment3d)t.IntersectionWith(s) == s);

            s = new Segment3d(new Point3d(2, 2, 0.01), new Point3d(5.01, 5, 0));
            Assert.IsTrue((Segment3d)t.IntersectionWith(s) == new Segment3d(new Point3d(2, 2, 0), new Point3d(3, 3, 0)));

            // Resore initial state
            GeometRi3D.UseAbsoluteTolerance = mode;
            GeometRi3D.Tolerance = tol;
        }

        [TestMethod()]
        public void TriangleIntersectionWithRayTest()
        {

            Point3d p1 = new Point3d(0, 0, 0);
            Point3d p2 = new Point3d(6, 0, 0);
            Point3d p3 = new Point3d(0, 6, 0);

            Triangle t = new Triangle(p1, p2, p3);

            Ray3d r = new Ray3d(new Point3d(1, 1, 0), new Vector3d(1, 1, 0));
            Assert.IsTrue((Segment3d)t.IntersectionWith(r) == new Segment3d(new Point3d(1, 1, 0), new Point3d(3, 3, 0)));

            t = new Triangle(p1, p3, p2);
            r = new Ray3d(new Point3d(1, 1, 0), new Vector3d(1, 1, 0));
            Assert.IsTrue((Segment3d)t.IntersectionWith(r) == new Segment3d(new Point3d(1, 1, 0), new Point3d(3, 3, 0)));

            r = new Ray3d(new Point3d(0, 0, 10), new Vector3d(0, 0, -1));
            Assert.IsTrue((Point3d)t.IntersectionWith(r) == new Point3d(0, 0, 0));

            r = new Ray3d(new Point3d(0, 0, -10), new Vector3d(0, 0, 1));
            Assert.IsTrue((Point3d)t.IntersectionWith(r) == new Point3d(0, 0, 0));

            r = new Ray3d(new Point3d(4, 4, -10), new Vector3d(0, 0, 1));
            Assert.IsNull(t.IntersectionWith(r));

        }

        [TestMethod()]
        public void PointInTriangleTest2()
        {

            Point3d p1 = new Point3d(0, 0, 0);
            Point3d p2 = new Point3d(6, 0, 0);
            Point3d p3 = new Point3d(0, 6, 0);

            Triangle t = new Triangle(p1, p2, p3);
            Point3d p = new Point3d(4, 4, 0);
            Assert.IsFalse(p.BelongsTo(t));

            p = new Point3d(6, 0.5, 0);
            Assert.IsFalse(p.BelongsTo(t));

            p = new Point3d(5, -0.5, 0);
            Assert.IsFalse(p.BelongsTo(t));

            p = new Point3d(2, 5, 0);
            Assert.IsFalse(p.BelongsTo(t));

            p = new Point3d(2, 2, 0.1);
            Assert.IsFalse(p.BelongsTo(t));

            p = new Point3d(2, 2, 0);
            Assert.IsTrue(p.BelongsTo(t));

            p = new Point3d(5, 1, 0);
            Assert.IsTrue(p.BelongsTo(t));

            p = new Point3d(0, 1, 0);
            Assert.IsTrue(p.BelongsTo(t));

            p = new Point3d(2, 0, 0);
            Assert.IsTrue(p.BelongsTo(t));

        }

        [TestMethod()]
        public void TriangleDistanceToCircleTest()
        {
            Point3d p1 = new Point3d(-0.5, -0.5, -0.5);
            Point3d p2 = new Point3d(0.5, 0.5, -0.5);
            Point3d p3 = new Point3d(0.5, -0.5, -0.5);
            Triangle t = new Triangle(p1, p2, p3);

            Circle3d c = new Circle3d(new Point3d(-1.3195, -1.0435, -0.70047), 0.35, new Vector3d(0.83694, -0.13208, -0.53112));

            double dist = t.DistanceTo(c);
            Assert.IsTrue(dist > 0);

        }

        [TestMethod()]
        public void TriangleIntersectsCircleTest()
        {

            Circle3d c = new Circle3d(new Point3d(), 1.0, new Vector3d(0, 0, 1));

            // Coplanar objects
            // Triangle's vertecx inside circle
            Point3d p1 = new Point3d(0.5, 0.5, 0);
            Point3d p2 = new Point3d(2, 3, 0);
            Point3d p3 = new Point3d(-3, 1, 0);
            Triangle t = new Triangle(p1, p2, p3);
            Assert.IsTrue(c.Intersects(t));

            // circle's center inside triangle
            p1 = new Point3d(-5, -5, 0);
            p2 = new Point3d(5, -5, 0);
            p3 = new Point3d(0, 5, 0);
            t = new Triangle(p1, p2, p3);
            Assert.IsTrue(c.Intersects(t));

            // circle touch triangle
            p1 = new Point3d(1, -1, 0);
            p2 = new Point3d(1, 1, 0);
            p3 = new Point3d(3, 0, 0);
            t = new Triangle(p1, p2, p3);
            Assert.IsTrue(c.Intersects(t));

            // Non-intersecting objects
            p1 = new Point3d(1.5, -1, 0);
            p2 = new Point3d(1.5, 1, 0);
            p3 = new Point3d(3, 0, 0);
            t = new Triangle(p1, p2, p3);
            Assert.IsFalse(c.Intersects(t));

            // Non-coplanar objects
            p1 = new Point3d(0.5, 0.5, -1);
            p2 = new Point3d(0.5, -0.5, 1);
            p3 = new Point3d(0, 0, 1);
            t = new Triangle(p1, p2, p3);
            Assert.IsTrue(c.Intersects(t));

            // Non-intersecting objects
            p1 = new Point3d(1.5, -1, -1);
            p2 = new Point3d(1.5, 1, -2);
            p3 = new Point3d(3, 0, -1);
            t = new Triangle(p1, p2, p3);
            Assert.IsFalse(c.Intersects(t));

            // Non-intersecting objects
            p1 = new Point3d(1, 1, -1);
            p2 = new Point3d(1, 1, -2);
            p3 = new Point3d(1, 5, -1);
            t = new Triangle(p1, p2, p3);
            Assert.IsFalse(c.Intersects(t));
        }


        [TestMethod()]
        public void TriangleIntersectsTriangleTest()
        {
            Triangle t1 = new Triangle(new Point3d(0, 0, 0), new Point3d(10, 0, 0), new Point3d(0, 10, 0));

            // Coplanar objects
            // Triangle 2 is inside triangle 1
            Triangle t2 = new Triangle(new Point3d(1, 1, 0), new Point3d(2, 1, 0), new Point3d(1, 2, 0));
            Assert.IsTrue(t2.Intersects(t1));
            Assert.IsTrue(t1.Intersects(t2));

            // Triangle 1 is inside triangle 2
            t2 = new Triangle(new Point3d(-1, -1, 0), new Point3d(20, -1, 0), new Point3d(-1, 20, 0));
            Assert.IsTrue(t2.Intersects(t1));
            Assert.IsTrue(t1.Intersects(t2));

            // Triangle 2 touch vertex of triangle 1
            t2 = new Triangle(new Point3d(10, 0, 0), new Point3d(20, 0, 0), new Point3d(10, -10, 0));
            Assert.IsTrue(t2.Intersects(t1));
            Assert.IsTrue(t1.Intersects(t2));

            // Triangle 2 touch edge of triangle 1
            t2 = new Triangle(new Point3d(5, 0, 0), new Point3d(10, -5, 0), new Point3d(0, -5, 0));
            Assert.IsTrue(t2.Intersects(t1));
            Assert.IsTrue(t1.Intersects(t2));

            // Triangle 2 intersects triangle 1
            t2 = new Triangle(new Point3d(5, -1, 0), new Point3d(5, 20, 0), new Point3d(6, -1, 0));
            Assert.IsTrue(t2.Intersects(t1));
            Assert.IsTrue(t1.Intersects(t2));

            // Non-intersecting objects
            t2 = new Triangle(new Point3d(-5, -1, 0), new Point3d(-5, -20, 0), new Point3d(-6, -1, 0));
            Assert.IsFalse(t2.Intersects(t1));
            Assert.IsFalse(t1.Intersects(t2));


            // Non-coplanar objects
            t2 = new Triangle(new Point3d(5, 1, 1), new Point3d(6, 1, -1), new Point3d(5, 2, 1));
            Assert.IsTrue(t2.Intersects(t1));
            Assert.IsTrue(t1.Intersects(t2));

            t2 = new Triangle(new Point3d(5, 1, 1), new Point3d(6, 1, 0), new Point3d(5, 2, 1));
            Assert.IsTrue(t2.Intersects(t1));
            Assert.IsTrue(t1.Intersects(t2));

            // Non-intersecting objects
            t2 = new Triangle(new Point3d(15, 1, 0), new Point3d(15, -20, -1), new Point3d(15, -1, 5));
            Assert.IsFalse(t2.Intersects(t1));
            Assert.IsFalse(t1.Intersects(t2));

        }

        [TestMethod()]
        public void TriangleClosestPointTest()
        {
            Point3d p1 = new Point3d(0, 0, 0);
            Point3d p2 = new Point3d(10, 0, 0);
            Point3d p3 = new Point3d(5, 5, 0);
            Triangle t = new Triangle(p1, p2, p3);

            Point3d p = new Point3d(1, 1, 1);
            Assert.AreEqual(p.ClosestPoint(t), new Point3d(1, 1, 0));

            p = new Point3d(-3, -1, 0);
            Assert.AreEqual(p.ClosestPoint(t), new Point3d(0, 0, 0));
        }

        [TestMethod()]
        public void TriangleDistancetoPointTest()
        {
            Point3d p1 = new Point3d(0, 0, 0);
            Point3d p2 = new Point3d(2, 0, 0);
            Point3d p3 = new Point3d(0, 2, 0);
            Triangle t = new Triangle(p1, p2, p3);

            Point3d p = new Point3d(1, 1, 8);
            Assert.IsTrue(Abs(t.DistanceTo(p) - 8) < GeometRi3D.Tolerance);

            p = new Point3d(5, 0, 4);
            Assert.IsTrue(Abs(t.DistanceTo(p) - 5) < GeometRi3D.Tolerance);

        }

        [TestMethod()]
        public void TriangleDistancetoSegmentTest_01()
        {
            Point3d p1 = new Point3d(0, 0, 0);
            Point3d p2 = new Point3d(2, 0, 0);
            Point3d p3 = new Point3d(0, 2, 0);
            Triangle t = new Triangle(p1, p2, p3);

            // One point belongs to triangle
            Segment3d s = new Segment3d(new Point3d(0, 0, 0), new Point3d(-1, -1, -1));
            double dist = t.DistanceTo(s, out Point3d point_on_triangle, out Point3d point_on_segment);
            Assert.IsTrue(Abs(dist - 0) < GeometRi3D.Tolerance);
            Assert.IsTrue(point_on_triangle == t.A);
            Assert.IsTrue(point_on_segment == s.P1);
        }

        [TestMethod()]
        public void TriangleDistancetoSegmentTest_02()
        {
            Point3d p1 = new Point3d(0, 0, 0);
            Point3d p2 = new Point3d(2, 0, 0);
            Point3d p3 = new Point3d(0, 2, 0);
            Triangle t = new Triangle(p1, p2, p3);

            // Segment is coplanar and belongs to triangle
            Segment3d s = new Segment3d(new Point3d(0.5, 0.5, 0), new Point3d(1,1,0));
            double dist = t.DistanceTo(s, out Point3d point_on_triangle, out Point3d point_on_segment);
            Assert.IsTrue(Abs(dist - 0) < GeometRi3D.Tolerance);
            Assert.IsTrue(point_on_triangle == new Point3d(0.5, 0.5, 0));
            Assert.IsTrue(point_on_segment == s.P1);
        }

        [TestMethod()]
        public void TriangleDistancetoSegmentTest_03()
        {
            Point3d p1 = new Point3d(0, 0, 0);
            Point3d p2 = new Point3d(2, 0, 0);
            Point3d p3 = new Point3d(0, 2, 0);
            Triangle t = new Triangle(p1, p2, p3);

            // Segment is coplanar and does not belong to triangle
            Segment3d s = new Segment3d(new Point3d(3, 0, 0), new Point3d(3, 5, 0));
            double dist = t.DistanceTo(s, out Point3d point_on_triangle, out Point3d point_on_segment);
            Assert.IsTrue(Abs(dist - 1) < GeometRi3D.Tolerance);
            Assert.IsTrue(point_on_triangle == new Point3d(2, 0, 0));
            Assert.IsTrue(point_on_segment == s.P1);
        }

        [TestMethod()]
        public void TriangleDistancetoSegmentTest_04()
        {
            Point3d p1 = new Point3d(0, 0, 0);
            Point3d p2 = new Point3d(2, 0, 0);
            Point3d p3 = new Point3d(0, 2, 0);
            Triangle t = new Triangle(p1, p2, p3);

            // Segment intersects triangle
            Segment3d s = new Segment3d(new Point3d(0, 0, -1), new Point3d(2, 2, 1));
            double dist = t.DistanceTo(s, out Point3d point_on_triangle, out Point3d point_on_segment);
            Assert.IsTrue(Abs(dist - 0) < GeometRi3D.Tolerance);
            Assert.IsTrue(point_on_triangle == new Point3d(1, 1, 0));
            Assert.IsTrue(point_on_segment == new Point3d(1, 1, 0));
        }

        [TestMethod()]
        public void TriangleDistancetoSegmentTest_05()
        {
            Point3d p1 = new Point3d(0, 0, 0);
            Point3d p2 = new Point3d(2, 0, 0);
            Point3d p3 = new Point3d(0, 2, 0);
            Triangle t = new Triangle(p1, p2, p3);

            // Projection point belongs to triangle and is closest
            Segment3d s = new Segment3d(new Point3d(0.5, 0.5, 1), new Point3d(5, 5, 10));
            double dist = t.DistanceTo(s, out Point3d point_on_triangle, out Point3d point_on_segment);
            Assert.IsTrue(Abs(dist - 1) < GeometRi3D.Tolerance);
            Assert.IsTrue(point_on_triangle == new Point3d(0.5, 0.5, 0));
            Assert.IsTrue(point_on_segment == s.P1);
        }

        [TestMethod()]
        public void TriangleDistancetoSegmentTest_06()
        {
            Point3d p1 = new Point3d(0, 0, 0);
            Point3d p2 = new Point3d(2, 0, 0);
            Point3d p3 = new Point3d(0, 2, 0);
            Triangle t = new Triangle(p1, p2, p3);

            // General case
            Segment3d s = new Segment3d(new Point3d(0, -1, -1), new Point3d(2, -1, 1));
            double dist = t.DistanceTo(s, out Point3d point_on_triangle, out Point3d point_on_segment);
            Assert.IsTrue(Abs(dist - 1) < GeometRi3D.Tolerance);
            Assert.IsTrue(point_on_triangle == new Point3d(1, 0, 0));
            Assert.IsTrue(point_on_segment == new Point3d(1, -1, 0));
        }

        [TestMethod()]
        public void TriangleIntersectsSphereTest_01()
        {
            Point3d p1 = new Point3d(0, 0, 0);
            Point3d p2 = new Point3d(2, 0, 0);
            Point3d p3 = new Point3d(0, 2, 0);
            Triangle t = new Triangle(p1, p2, p3);

            Sphere s = new Sphere(new Point3d(0.5, 0.5, 1.1), 1);
            Assert.IsFalse(t.Intersects(s));

            s = new Sphere(new Point3d(2, 2, 0.5), 1);
            Assert.IsFalse(t.Intersects(s));

            s = new Sphere(new Point3d(0.5, 0.5, 0.9), 1);
            Assert.IsTrue(t.Intersects(s));

            s = new Sphere(new Point3d(1.1, 1.1, 0.7), 1);
            Assert.IsTrue(t.Intersects(s));


        }


    }
}
