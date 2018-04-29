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

            // Resore initial state
            GeometRi3D.UseAbsoluteTolerance = mode;
            GeometRi3D.Tolerance = tol;
        }
    }
}
