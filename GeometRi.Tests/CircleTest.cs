using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

namespace GeometRi_Tests
{
    [TestClass]
    public class CircleTest
    {
        //===============================================================
        // Circle3d tests
        //===============================================================

        [TestMethod()]
        public void CircleBy3PointsTest()
        {
            Point3d p1 = new Point3d(-3, 0, 4);
            Point3d p2 = new Point3d(4, 0, 5);
            Point3d p3 = new Point3d(1, 0, -4);

            Circle3d c = new Circle3d(p1, p2, p3);

            Assert.IsTrue(c.Center == new Point3d(1, 0, 1));
            Assert.IsTrue(Abs(c.R - 5) <= GeometRi3D.Tolerance);
        }

        [TestMethod()]
        public void CircleIntersectionWithLineTest()
        {
            // parallel obecjts
            Circle3d c = new Circle3d(new Point3d(0, 0, 0), 5, new Vector3d(0, 0, 1));
            Line3d l = new Line3d(new Point3d(0, 0, 1), new Vector3d(1, 0, 0));
            Assert.AreEqual(c.IntersectionWith(l), null);

            // nonintersecting objects
            c = new Circle3d(new Point3d(0, 0, 0), 5, new Vector3d(0, 0, 1));
            l = new Line3d(new Point3d(10, 0, 0), new Vector3d(1, 1, 0));
            Assert.AreEqual(c.IntersectionWith(l), null);

            // intersection in one point (touching line)
            c = new Circle3d(new Point3d(0, 0, 0), 5, new Vector3d(0, 0, 1));
            l = new Line3d(new Point3d(5, 0, 0), new Vector3d(0, 1, 0));
            Assert.AreEqual(c.IntersectionWith(l), new Point3d(5, 0, 0));

            // intersection in one point (crossing line)
            c = new Circle3d(new Point3d(0, 0, 0), 5, new Vector3d(0, 0, 1));
            l = new Line3d(new Point3d(1, 1, 0), new Vector3d(0, 0, 1));
            Assert.AreEqual(c.IntersectionWith(l), new Point3d(1, 1, 0));

            // intersection in two points
            c = new Circle3d(new Point3d(0, 0, 0), 5, new Vector3d(0, 0, 1));
            l = new Line3d(new Point3d(0, -4, 0), new Vector3d(1, 0, 0));
            Assert.AreEqual(c.IntersectionWith(l), new Segment3d(new Point3d(-3, -4, 0), new Point3d(3, -4, 0)));

        }

        [TestMethod()]
        public void CircleIntersectionWithRayTest()
        {
            // intersection in one point (crossing ray)
            Circle3d c = new Circle3d(new Point3d(0, 0, 0), 5, new Vector3d(0, 0, 1));
            Ray3d r = new Ray3d(new Point3d(0, 0, 0), new Vector3d(1, 0, 0));
            Assert.AreEqual(c.IntersectionWith(r), new Segment3d(new Point3d(5, 0, 0), new Point3d(0, 0, 0)));

            // intersection in one point (touching line)
            r = new Ray3d(new Point3d(5, 0, 0), new Vector3d(1, 0, 0));
            Assert.AreEqual(c.IntersectionWith(r), new Point3d(5, 0, 0));

        }

        [TestMethod()]
        public void CircleIntersectionWithPlaneTest()
        {
            // parallel obecjts
            Circle3d c = new Circle3d(new Point3d(5, 6, 1), 5, new Vector3d(0, 0, 1));
            Plane3d s = new Plane3d(new Point3d(0, 0, 0), new Vector3d(0, 0, 1));
            Assert.AreEqual(c.IntersectionWith(s), null);

            // coplanar objects
            s = new Plane3d(new Point3d(0, 0, 1), new Vector3d(0, 0, 1));
            Assert.AreEqual(c.IntersectionWith(s), c);

            // nonintersecting objects
            c = new Circle3d(new Point3d(5, 6, 10), 5, new Vector3d(0, 0, 1));
            s = new Plane3d(new Point3d(0, 0, 1), new Vector3d(0, 0, 1));
            Assert.AreEqual(c.IntersectionWith(s), null);

            // intersection in one point
            c = new Circle3d(new Point3d(0, 0, 3), 5, new Vector3d(3, 0, 4));
            s = new Plane3d(new Point3d(5, 5, 0), new Vector3d(0, 0, 1));
            Assert.AreEqual(c.IntersectionWith(s), new Point3d(4, 0, 0));

            // intersection in two points
            c = new Circle3d(new Point3d(0, 0, 3), 5, new Vector3d(3, 0, 0));
            s = new Plane3d(new Point3d(5, 5, 0), new Vector3d(0, 0, 1));
            Assert.AreEqual(c.IntersectionWith(s), new Segment3d(new Point3d(0, 4, 0), new Point3d(0, -4, 0)));

        }

        [TestMethod()]
        public void CircleIntersectionWithCircle2DTest()
        {
            // parallel obecjts
            Circle3d c1 = new Circle3d(new Point3d(0, 0, 0), 5, new Vector3d(0, 0, 1));
            Circle3d c2 = new Circle3d(new Point3d(5, 0, 1), 5, new Vector3d(0, 0, 1));
            Assert.AreEqual(c1.IntersectionWith(c2), null);

            // Coincided circles
            c2 = new Circle3d(new Point3d(0, 0, 0), 5, new Vector3d(0, 0, 1));
            Assert.AreEqual(c1.IntersectionWith(c2), c1);

            // Separated circles
            c2 = new Circle3d(new Point3d(10, 0, 0), 2, new Vector3d(0, 0, 1));
            Assert.AreEqual(c1.IntersectionWith(c2), null);

            // Outer tangency
            c2 = new Circle3d(new Point3d(10, 0, 0), 5, new Vector3d(0, 0, 1));
            Assert.AreEqual(c1.IntersectionWith(c2), new Point3d(5, 0, 0));

            // Inner tangency 1
            c2 = new Circle3d(new Point3d(3, 0, 0), 2, new Vector3d(0, 0, 1));
            Assert.AreEqual(c1.IntersectionWith(c2), new Point3d(5, 0, 0));

            // Inner tangency 2
            c2 = new Circle3d(new Point3d(-2, 0, 0), 7, new Vector3d(0, 0, 1));
            Assert.AreEqual(c1.IntersectionWith(c2), new Point3d(5, 0, 0));

            // Intersection
            c2 = new Circle3d(new Point3d(6, 0, 0), 5, new Vector3d(0, 0, 1));
            Segment3d s = new Segment3d(new Point3d(3, 4, 0), new Point3d(3, -4, 0));
            Assert.AreEqual(c1.IntersectionWith(c2), s);

        }

        [TestMethod()]
        public void CircleIntersectionWithCircle3DTest()
        {
            // Touching circles
            Circle3d c1 = new Circle3d(new Point3d(0, 0, 0), 5, new Vector3d(0, 0, 1));
            Circle3d c2 = new Circle3d(new Point3d(5, 0, 0), 5, new Vector3d(1, 0, 0));
            Assert.AreEqual(c1.IntersectionWith(c2), new Point3d(5, 0, 0));

            // Touching circles
            c2 = new Circle3d(new Point3d(10, 0, 0), 5, new Vector3d(0, 1, 0));
            Assert.AreEqual(c1.IntersectionWith(c2), new Point3d(5, 0, 0));

            // Intersecting circles
            c2 = new Circle3d(new Point3d(0, 0, 0), 5, new Vector3d(0, 1, 0));
            Segment3d s = new Segment3d(new Point3d(-5, 0, 0), new Point3d(5, 0, 0));
            Assert.AreEqual(c1.IntersectionWith(c2), s);

            // Intersecting circles
            c2 = new Circle3d(new Point3d(5, 0, 0), 5, new Vector3d(0, 1, 0));
            s = new Segment3d(new Point3d(0, 0, 0), new Point3d(5, 0, 0));
            Assert.AreEqual(c1.IntersectionWith(c2), s);

            // Intersecting circles
            c2 = new Circle3d(new Point3d(0, 0, 4), 5, new Vector3d(0, 1, 0));
            s = new Segment3d(new Point3d(-3, 0, 0), new Point3d(3, 0, 0));
            Assert.AreEqual(c1.IntersectionWith(c2), s);

        }

        [TestMethod()]
        public void CircleIntersectionWithCircle3DTest_2()
        {
            double tol = GeometRi3D.Tolerance;
            bool mode = GeometRi3D.UseAbsoluteTolerance;
            GeometRi3D.Tolerance = 0.01;
            GeometRi3D.UseAbsoluteTolerance = true;

            Circle3d c1 = new Circle3d(new Point3d(-0.28776, -0.29482, -0.16311), 0.2, new Vector3d(-0.44759, -0.3224, -0.8341));
            Circle3d c2 = new Circle3d(new Point3d(-0.35134, -0.27228, -0.12871), 0.2, new Vector3d(0.84394, -0.416, -0.33868));
            Assert.IsTrue(c1.IntersectionWith(c2) != null);
            Assert.IsTrue(c2.IntersectionWith(c1) != null);

            // Restore initial state
            GeometRi3D.UseAbsoluteTolerance = mode;
            GeometRi3D.Tolerance = tol;
        }

        [TestMethod()]
        public void CircleIntersectionWithCircle3DTest_3()
        {
            double tol = GeometRi3D.Tolerance;
            bool mode = GeometRi3D.UseAbsoluteTolerance;
            GeometRi3D.Tolerance = 0.04;
            GeometRi3D.UseAbsoluteTolerance = true;

            Circle3d c1 = new Circle3d(new Point3d(0.36335, -0.46836, -0.11003), 0.25, new Vector3d(0.89975, -0.12088, -0.41932));
            Circle3d c2 = new Circle3d(new Point3d(0.18967, -0.14709, 0.081927), 0.25, new Vector3d(0.90756, -0.16092, -0.38787));
            Assert.IsTrue(c1.IntersectionWith(c2) == null);
            Assert.IsTrue(c2.IntersectionWith(c1) == null);

            // Restore initial state
            GeometRi3D.UseAbsoluteTolerance = mode;
            GeometRi3D.Tolerance = tol;
        }

        [TestMethod()]
        public void CircleIntersectionWithCircle3DTest_4()
        {
            double tol = GeometRi3D.Tolerance;
            bool mode = GeometRi3D.UseAbsoluteTolerance;
            GeometRi3D.Tolerance = 0.03;
            GeometRi3D.UseAbsoluteTolerance = true;

            Circle3d c1 = new Circle3d(new Point3d(0.21512, -0.00082439, 0.17926), 0.3, new Vector3d(0.62821, -0.68096, 0.37636));
            Circle3d c2 = new Circle3d(new Point3d(-0.038202, -0.090672, -0.078966), 0.3, new Vector3d(-0.060788, -0.026431, 0.9978));
            Assert.IsTrue(c1.IntersectionWith(c2) != null);
            Assert.IsTrue(c2.IntersectionWith(c1) != null);

            // Restore initial state
            GeometRi3D.UseAbsoluteTolerance = mode;
            GeometRi3D.Tolerance = tol;
        }

        [TestMethod()]
        public void CircleIntersectionWithCircle3DTest_5()
        {
            double tol = GeometRi3D.Tolerance;
            bool mode = GeometRi3D.UseAbsoluteTolerance;
            GeometRi3D.Tolerance = 0.01;
            GeometRi3D.UseAbsoluteTolerance = true;

            Circle3d c1 = new Circle3d(new Point3d(-0.15988, 0.3074, 0.11761), 0.1, new Vector3d(-0.14315, -0.33678, 0.93064));
            Circle3d c2 = new Circle3d(new Point3d(-0.13031, 0.2539, 0.11499), 0.1, new Vector3d(0.70155, 0.62669, -0.33924));
            Assert.IsTrue(c1.IntersectionWith(c2) != null);
            Assert.IsTrue(c2.IntersectionWith(c1) != null);

            // Restore initial state
            GeometRi3D.UseAbsoluteTolerance = mode;
            GeometRi3D.Tolerance = tol;
        }

        [TestMethod()]
        public void CircleDistanceToPlaneTest()
        {
            // Parallel circle
            Plane3d p = new Plane3d(new Point3d(), new Vector3d(0, 0, 1));
            Circle3d c = new Circle3d(new Point3d(10, 10, 10), 5, new Vector3d(0, 0, 1));
            Assert.IsTrue(GeometRi3D.AlmostEqual(c.DistanceTo(p), 10));

            // Orthogonal circle
            c = new Circle3d(new Point3d(10, 10, 10), 5, new Vector3d(1, 1, 0));
            Assert.IsTrue(GeometRi3D.AlmostEqual(c.DistanceTo(p), 5));

            // Inclined circle
            c = new Circle3d(new Point3d(10, 10, 10), 5, new Vector3d(3, 0, 4));
            Assert.IsTrue(GeometRi3D.AlmostEqual(c.DistanceTo(p), 7));
        }

        [TestMethod()]
        public void CircleDistanceToPointTest()
        {
            Circle3d c = new Circle3d(new Point3d(0, 0, 0), 5, new Vector3d(0, 0, 1));
            Point3d p = new Point3d(2, 3, 4);
            Assert.IsTrue(GeometRi3D.AlmostEqual(c.DistanceTo(p), 4));

            p = new Point3d(4, 3, 0);
            Assert.IsTrue(GeometRi3D.AlmostEqual(c.DistanceTo(p), 0));

            p = new Point3d(2, 3, 0);
            Assert.IsTrue(GeometRi3D.AlmostEqual(c.DistanceTo(p), 0));

            p = new Point3d(8, 0, 4);
            Assert.IsTrue(GeometRi3D.AlmostEqual(c.DistanceTo(p), 5));
        }

        [TestMethod()]
        public void CircleDistanceToCircleTest()
        {
            Circle3d c1 = new Circle3d(new Point3d(0, 0, 0), 5, new Vector3d(0, 0, 1));
            Circle3d c2 = new Circle3d(new Point3d(11, 0, 0), 5, new Vector3d(0, 0, 1));
            Assert.IsTrue(GeometRi3D.AlmostEqual(c1.DistanceTo(c2), 1));

            c2 = new Circle3d(new Point3d(11, 0, 0), 5, new Vector3d(0, 2, 1));
            Assert.IsTrue(GeometRi3D.AlmostEqual(c1.DistanceTo(c2), 1));
        }

        [TestMethod()]
        public void CircleToCircleClosestPointTest()
        {
            Point3d p1, p2;

            // Nonintersecting circles in one plane
            Circle3d c1 = new Circle3d(new Point3d(0, 0, 0), 5, new Vector3d(0, 0, 1));
            Circle3d c2 = new Circle3d(new Point3d(11, 0, 0), 5, new Vector3d(0, 0, 1));
            double dist = c1.DistanceTo(c2, out p1, out p2);
            Assert.IsTrue(GeometRi3D.AlmostEqual(dist, p1.DistanceTo(p2)));

            // Intersecting circles in one plane
            c1 = new Circle3d(new Point3d(0, 0, 0), 5, new Vector3d(0, 0, 1));
            c2 = new Circle3d(new Point3d(8, 0, 0), 5, new Vector3d(0, 0, 1));
            dist = c1.DistanceTo(c2, out p1, out p2);
            Assert.IsTrue(GeometRi3D.AlmostEqual(dist, p1.DistanceTo(p2)));

            // Intersecting circles in one plane
            c1 = new Circle3d(new Point3d(0, 0, 0), 5, new Vector3d(0, 0, 1));
            c2 = new Circle3d(new Point3d(0, 0, 0), 3, new Vector3d(0, 0, 1));
            dist = c1.DistanceTo(c2, out p1, out p2);
            Assert.IsTrue(GeometRi3D.AlmostEqual(dist, p1.DistanceTo(p2)));

            // Touching circles
            c1 = new Circle3d(new Point3d(0, 0, 0), 5, new Vector3d(0, 0, 1));
            c2 = new Circle3d(new Point3d(8, 0, 0), 3, new Vector3d(0, 0, 1));
            dist = c1.DistanceTo(c2, out p1, out p2);
            Assert.IsTrue(GeometRi3D.AlmostEqual(dist, p1.DistanceTo(p2)));

            // Coplanar circles
            c1 = new Circle3d(new Point3d(0, 0, 0), 5, new Vector3d(0, 0, 1));
            c2 = new Circle3d(new Point3d(5, 0, 5), 3, new Vector3d(0, 0, 1));
            dist = c1.DistanceTo(c2, out p1, out p2);
            Assert.IsTrue(GeometRi3D.AlmostEqual(dist, p1.DistanceTo(p2)));

            // Coplanar circles
            c1 = new Circle3d(new Point3d(0, 0, 0), 5, new Vector3d(0, 0, 1));
            c2 = new Circle3d(new Point3d(15, 0, 5), 3, new Vector3d(0, 0, 1));
            dist = c1.DistanceTo(c2, out p1, out p2);
            Assert.IsTrue(GeometRi3D.AlmostEqual(dist, p1.DistanceTo(p2)));

            // Random circles
            c1 = new Circle3d(new Point3d(2, 3, 0), 5, new Vector3d(4, -2, 1));
            c2 = new Circle3d(new Point3d(15, 7, 5), 3, new Vector3d(2, 3, 1));
            dist = c1.DistanceTo(c2, out p1, out p2);
            Assert.IsTrue(GeometRi3D.AlmostEqual(dist, p1.DistanceTo(p2)));

        }

        [TestMethod()]
        public void CircleParametricFormTest()
        {
            Circle3d c = new Circle3d(new Point3d(5, 6, 1), 5, new Vector3d(3, 0, 1));
            Assert.IsTrue(c.ParametricForm(0.5).BelongsTo(c));
        }

        [TestMethod()]
        public void CircleToEllipseTest()
        {
            Circle3d c = new Circle3d(new Point3d(5, 6, 1), 5, new Vector3d(3, 0, 1));
            Ellipse e = c.ToEllipse;

            Assert.IsTrue(c.ParametricForm(0.5).BelongsTo(e));
            Assert.IsTrue(c.ParametricForm(0.725).BelongsTo(e));
            Assert.IsTrue(c.ParametricForm(2.7215).BelongsTo(e));

            Assert.IsTrue(e.ParametricForm(0.5).BelongsTo(c));
            Assert.IsTrue(e.ParametricForm(0.725).BelongsTo(c));
            Assert.IsTrue(e.ParametricForm(2.7215).BelongsTo(c));
        }

        [TestMethod()]
        public void CircleProjectionToPlaneTest()
        {
            Vector3d v1 = new Vector3d(3, 5, 1);
            Circle3d c = new Circle3d(new Point3d(5, 6, 1), 5, v1);
            Plane3d s = new Plane3d(5, 2, 3, -3);

            Point3d p = c.ParametricForm(0.5).ProjectionTo(s);
            Assert.IsTrue(p.BelongsTo(c.ProjectionTo(s)));
            p = c.ParametricForm(0.725).ProjectionTo(s);
            Assert.IsTrue(p.BelongsTo(c.ProjectionTo(s)));
            p = c.ParametricForm(2.7122).ProjectionTo(s);
            Assert.IsTrue(p.BelongsTo(c.ProjectionTo(s)));
        }

        [TestMethod]
        public void PointInCircleTest()
        {
            Point3d p = new Point3d(1, 1, 0);
            Circle3d s = new Circle3d(p, 5, new Vector3d(0,0,1));

            p = new Point3d(2, 2, 0);  // Point inside
            Assert.IsTrue(p.BelongsTo(s));
            Assert.IsTrue(p.IsInside(s));
            Assert.IsFalse(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));

            p = new Point3d(1, 6, 0);  // Point on boundary
            Assert.IsTrue(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsFalse(p.IsOutside(s));
            Assert.IsTrue(p.IsOnBoundary(s));

            p = new Point3d(1, 6.005, 0);  // Point outside
            Assert.IsFalse(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsTrue(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));

            p = new Point3d(1, 2, 0.01);  // Point outside
            Assert.IsFalse(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsTrue(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));
        }

        [TestMethod]
        public void PointInCircleRelativeTest()
        {
            Point3d p = new Point3d(1, 1, 0);
            Circle3d s = new Circle3d(p, 5, new Vector3d(0, 0, 1));

            double tol = GeometRi3D.Tolerance;
            bool mode = GeometRi3D.UseAbsoluteTolerance;
            GeometRi3D.Tolerance = 0.01;
            GeometRi3D.UseAbsoluteTolerance = false;

            p = new Point3d(2, 2, 0.04);  // Point inside
            Assert.IsTrue(p.BelongsTo(s));
            Assert.IsTrue(p.IsInside(s));
            Assert.IsFalse(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));

            p = new Point3d(1, 6.04, 0);  // Point on boundary
            Assert.IsTrue(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsFalse(p.IsOutside(s));
            Assert.IsTrue(p.IsOnBoundary(s));

            p = new Point3d(1, 6.06, 0);  // Point outside
            Assert.IsFalse(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsTrue(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));

            p = new Point3d(1, 2, 0.06);  // Point outside
            Assert.IsFalse(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsTrue(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));

            // Restore initial state
            GeometRi3D.UseAbsoluteTolerance = mode;
            GeometRi3D.Tolerance = tol;
        }

        [TestMethod()]
        public void IsInsideBoxTest()
        {
            Box3d box = new Box3d();
            Circle3d c = new Circle3d(new Point3d(0, 0, 0), 0.2, new Vector3d(0, 0, 1));
            Assert.IsTrue(c.IsInside(box));
            c = new Circle3d(new Point3d(5, 0, 0), 0.2, new Vector3d(0, 0, 1));
            Assert.IsFalse(c.IsInside(box));
            c = new Circle3d(new Point3d(0.9, 0, 0), 0.2, new Vector3d(0, 0, 1));
            Assert.IsFalse(c.IsInside(box));
        }

        [TestMethod()]
        public void CircleIntersectsSphereTest()
        {
            Point3d p = new Point3d();
            Circle3d c = new Circle3d(p, 1.0, new Vector3d(0, 0, 1));

            // Intersecting objects
            Sphere s = new Sphere(new Point3d(0, 0, 1), 1.1);
            Assert.IsTrue(c.Intersects(s));

            s = new Sphere(new Point3d(0, 0, 0), 10);
            Assert.IsTrue(c.Intersects(s));

            s = new Sphere(new Point3d(2, 0, 0), 1.1);
            Assert.IsTrue(c.Intersects(s));

            // Touching objects
            s = new Sphere(new Point3d(0, 0, 1), 1.0);
            Assert.IsTrue(c.Intersects(s));

            s = new Sphere(new Point3d(2, 0, 0), 1.0);
            Assert.IsTrue(c.Intersects(s));

            // Non-intersecting objects
            s = new Sphere(new Point3d(3, 0, 0), 1.0);
            Assert.IsFalse(c.Intersects(s));

        }

        [TestMethod()]
        public void CircleDistanceToSphereTest()
        {
            Point3d p = new Point3d();
            Circle3d c = new Circle3d(p, 1.0, new Vector3d(0, 0, 1));
            Point3d p1, p2;

            // Intersecting objects
            Sphere s = new Sphere(new Point3d(0, 0, 1), 1.1);
            double dist = c.DistanceTo(s, out p1, out p2);
            Assert.AreEqual(dist, 0.0);
            Assert.AreEqual(p1, new Point3d(0, 0, 0));
            Assert.AreEqual(p2, new Point3d(0, 0, -0.1));

            s = new Sphere(new Point3d(0, 0, 0), 10);
            Assert.AreEqual(c.DistanceTo(s), 0.0);

            s = new Sphere(new Point3d(2, 0, 0), 1.1);
            dist = c.DistanceTo(s, out p1, out p2);
            Assert.AreEqual(dist, 0.0);
            Assert.AreEqual(p1, new Point3d(1, 0, 0));
            Assert.AreEqual(p2, new Point3d(0.9, 0, 0));

            // Touching objects
            s = new Sphere(new Point3d(0, 0, 1), 1.0);
            dist = c.DistanceTo(s, out p1, out p2);
            Assert.AreEqual(dist, 0.0);
            Assert.AreEqual(p1, new Point3d(0, 0, 0));
            Assert.AreEqual(p2, new Point3d(0, 0, 0));

            s = new Sphere(new Point3d(2, 0, 0), 1.0);
            dist = c.DistanceTo(s, out p1, out p2);
            Assert.AreEqual(dist, 0.0);
            Assert.AreEqual(p1, new Point3d(1, 0, 0));
            Assert.AreEqual(p2, new Point3d(1, 0, 0));

            // Non-intersecting objects
            s = new Sphere(new Point3d(3, 0, 0), 1.0);
            dist = c.DistanceTo(s, out p1, out p2);
            Assert.IsTrue(GeometRi3D.AlmostEqual(dist, 1.0));
            Assert.AreEqual(p1, new Point3d(1, 0, 0));
            Assert.AreEqual(p2, new Point3d(2, 0, 0));
        }

        [TestMethod()]
        public void CircleDistanceToLineTest()
        {
            Point3d p = new Point3d();
            Circle3d c = new Circle3d(p, 1.0, new Vector3d(0, 0, 1));
            Point3d p1, p2;

            // Parallel objects
            Line3d l= new Line3d(new Point3d(0, 0, 1), new Vector3d(1, 0, 0));
            double dist = c.DistanceTo(l, out p1, out p2);
            Assert.AreEqual(dist, 1.0);
            Assert.AreEqual(p1, p);
            Assert.AreEqual(p2, new Point3d(0, 0, 1));

            // Coplanar intersecting objects
            l = new Line3d(new Point3d(0, 0.5, 0), new Vector3d(1, 0, 0));
            dist = c.DistanceTo(l, out p1, out p2);
            Assert.AreEqual(dist, 0.0);
            Assert.AreEqual(p1, new Point3d(0, 0.5, 0));
            Assert.AreEqual(p2, new Point3d(0, 0.5, 0));

            // Coplanar non-intersecting objects
            l = new Line3d(new Point3d(0, 1.5, 0), new Vector3d(1, 0, 0));
            dist = c.DistanceTo(l, out p1, out p2);
            Assert.AreEqual(dist, 0.5);
            Assert.AreEqual(p1, new Point3d(0, 1, 0));
            Assert.AreEqual(p2, new Point3d(0, 1.5, 0));

            // Intersecting objects
            l = new Line3d(new Point3d(0, 0.5, 0), new Vector3d(0, 0, 1));
            dist = c.DistanceTo(l, out p1, out p2);
            Assert.AreEqual(dist, 0.0);
            Assert.AreEqual(p1, new Point3d(0, 0.5, 0));
            Assert.AreEqual(p2, new Point3d(0, 0.5, 0));

            // Non-intersecting objects
            l = new Line3d(new Point3d(0, 1.5, 0), new Vector3d(0, 0, 1));
            dist = c.DistanceTo(l, out p1, out p2);
            Assert.AreEqual(dist, 0.5);
            Assert.AreEqual(p1, new Point3d(0, 1, 0));
            Assert.AreEqual(p2, new Point3d(0, 1.5, 0));
        }

        [TestMethod()]
        public void CircleclosestPointToPlaneTest()
        {
            Point3d p = new Point3d(0, 0, 5);
            Circle3d c = new Circle3d(p, 1.0, new Vector3d(0, 0, 1));
            Plane3d plane = new Plane3d(new Point3d(), new Vector3d(0, 0, 1));
            Point3d p1, p2;

            // Parallel objects
            double dist = c.DistanceTo(plane, out p1, out p2);
            Assert.AreEqual(dist, 5.0);
            Assert.AreEqual(p1, p);
            Assert.AreEqual(p2, new Point3d(0, 0, 0));

            // Non-parallel objects
            c = new Circle3d(p, 1.0, new Vector3d(1, 0, 0));
            dist = c.DistanceTo(plane, out p1, out p2);
            Assert.AreEqual(dist, 4.0);
            Assert.AreEqual(p1, new Point3d(0, 0, 4));
            Assert.AreEqual(p2, new Point3d(0, 0, 0));
        }

        [TestMethod()]
        public void CircleclosestPointToTriangleTest()
        {
            Point3d p1 = new Point3d(0, 0, 0);
            Point3d p2 = new Point3d(5, 0, 0);
            Point3d p3 = new Point3d(0, 5, 0);
            Triangle t = new Triangle(p1, p2, p3);

            Point3d pc, pt;

            // Circle in triangle
            Circle3d c = new Circle3d(new Point3d(2, 2, 0), 1, new Vector3d(0, 0, 1));
            double dist = c.DistanceTo(t, out pc, out pt);
            Assert.AreEqual(dist, 0.0);
            Assert.AreEqual(pc, new Point3d(2, 2, 0));
            Assert.AreEqual(pt, new Point3d(2, 2, 0));

            // Triangle in circle
            c = new Circle3d(new Point3d(5, 5, 0), 10, new Vector3d(0, 0, 1));
            dist = c.DistanceTo(t, out pc, out pt);
            Assert.AreEqual(dist, 0.0);
            Assert.AreEqual(pc, pt);
            Assert.IsTrue(pc.BelongsTo(c));
            Assert.IsTrue(pt.BelongsTo(t));

            // Closest point at vertex
            c = new Circle3d(new Point3d(-2, -2, 0), Sqrt(2), new Vector3d(0, 0, 1));
            dist = c.DistanceTo(t, out pc, out pt);
            Assert.AreEqual(dist, Sqrt(2.0));
            Assert.AreEqual(pc, new Point3d(-1, -1, 0));
            Assert.AreEqual(pt, p1);

            // Closest point at edge
            c = new Circle3d(new Point3d(2, -2, 0), 1, new Vector3d(0, 0, 1));
            dist = c.DistanceTo(t, out pc, out pt);
            Assert.AreEqual(dist, 1.0);
            Assert.AreEqual(pc, new Point3d(2, -1, 0));
            Assert.AreEqual(pt, new Point3d(2, 0, 0));

        }


    }
}
