using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

namespace GeometRi_Tests
{
    [TestClass]
    public class EllipseTest
    {
        //===============================================================
        // Ellipse tests
        //===============================================================

        [TestMethod()]
        public void PointBelongsToEllipseTest()
        {
            Vector3d v1 = new Vector3d(3, 0, 1);
            Vector3d v2 = 3 * v1.OrthogonalVector;
            Ellipse e = new Ellipse(new Point3d(5, 6, 1), v1, v2);
            Assert.IsTrue(e.ParametricForm(0.5).BelongsTo(e));
        }

        [TestMethod()]
        public void EllipseProjectionToPlaneTest()
        {
            Vector3d v1 = new Vector3d(3, 0, 1);
            Vector3d v2 = 3 * v1.OrthogonalVector;
            Ellipse e = new Ellipse(new Point3d(5, 6, 1), v1, v2);
            Plane3d s = new Plane3d(5, 2, 3, -3);

            Point3d p = e.ParametricForm(0.5).ProjectionTo(s);
            Assert.IsTrue(p.BelongsTo(e.ProjectionTo(s)));
            p = e.ParametricForm(0.725).ProjectionTo(s);
            Assert.IsTrue(p.BelongsTo(e.ProjectionTo(s)));
            p = e.ParametricForm(2.7122).ProjectionTo(s);
            Assert.IsTrue(p.BelongsTo(e.ProjectionTo(s)));
        }

        [TestMethod()]
        public void EllipseIntersectionWithLineTest()
        {
            Point3d p = new Point3d(0, 0, 0);
            Vector3d v1 = new Vector3d(3, 0, 0);
            Vector3d v2 = new Vector3d(0, 2, 0);
            Ellipse e = new Ellipse(p, v1, v2);

            Line3d l = new Line3d(new Point3d(0, -2, 0), new Vector3d(1, 2, 0));
            Segment3d res = new Segment3d(new Point3d(0, -2, 0), new Point3d(9.0/5.0, 8.0/5.0, 0));
            Assert.AreEqual((Segment3d)e.IntersectionWith(l), res);

            l = new Line3d(new Point3d(0, -2, 0), new Vector3d(1, 0, 0));
            Assert.AreEqual((Point3d)e.IntersectionWith(l), new Point3d(0, -2, 0));
        }

        [TestMethod()]
        public void EllipseIntersectionWithPlaneTest()
        {
            Point3d p = new Point3d(0, 0, 0);
            Vector3d v1 = new Vector3d(4, 0, 0);
            Vector3d v2 = new Vector3d(0, 6, 0);
            Ellipse e = new Ellipse(p, v1, v2);

            p = new Point3d(0, 50, 0);
            v1 = new Vector3d(-1000, 1, 0);
            Plane3d s = new Plane3d(p, v1);

            Segment3d res = new Segment3d(new Point3d(-0.0440003630169716, 5.99963698302842, 0), new Point3d(-0.0559994119835347, -5.99941198353467, 0));
            Assert.AreEqual(e.IntersectionWith(s), res);
        }

        [TestMethod]
        public void PointInEllipseTest()
        {
            Point3d p = new Point3d(1, 1, 0);
            Ellipse s = new Ellipse(p, new Vector3d(10, 0, 0), new Vector3d(0, 5, 0));

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
        public void PointInEllipseRelativeTest()
        {
            Point3d p = new Point3d(1, 1, 0);
            Ellipse s = new Ellipse(p, new Vector3d(10, 0, 0), new Vector3d(0, 5, 0));

            double tol = GeometRi3D.Tolerance;
            bool mode = GeometRi3D.UseAbsoluteTolerance;
            GeometRi3D.Tolerance = 0.01;
            GeometRi3D.UseAbsoluteTolerance = false;

            p = new Point3d(2, 2, 0.1);  // Point inside
            Assert.IsTrue(p.BelongsTo(s));
            Assert.IsTrue(p.IsInside(s));
            Assert.IsFalse(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));

            p = new Point3d(1, 6, 0);  // Point on boundary
            Assert.IsTrue(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsFalse(p.IsOutside(s));
            Assert.IsTrue(p.IsOnBoundary(s));

            p = new Point3d(1, 6.2, 0);  // Point outside
            Assert.IsFalse(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsTrue(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));

            p = new Point3d(1, 2, 0.2);  // Point outside
            Assert.IsFalse(p.BelongsTo(s));
            Assert.IsFalse(p.IsInside(s));
            Assert.IsTrue(p.IsOutside(s));
            Assert.IsFalse(p.IsOnBoundary(s));

            // Resore initial state
            GeometRi3D.UseAbsoluteTolerance = mode;
            GeometRi3D.Tolerance = tol;
        }

        [TestMethod()]
        public void ClosestPointTest()
        {
            Point3d p = new Point3d(0, 0, 0);
            Vector3d v1 = new Vector3d(10, 0, 0);
            Vector3d v2 = new Vector3d(0, 5, 0);
            Ellipse e = new Ellipse(p, v1, v2);

            p = new Point3d(11, 0, 0);
            Assert.AreEqual(e.ClosestPoint(p), new Point3d(10, 0, 0));

            p = new Point3d(-11, 0, 0);
            Assert.AreEqual(e.ClosestPoint(p), new Point3d(-10, 0, 0));

            p = new Point3d(0, 6, 0);
            Assert.AreEqual(e.ClosestPoint(p), new Point3d(0, 5, 0));

            p = new Point3d(0, -6, 0);
            Assert.AreEqual(e.ClosestPoint(p), new Point3d(0, -5, 0));

            p = new Point3d(0, 0, 0);
            Assert.AreEqual(e.ClosestPoint(p), new Point3d(0, 5, 0));

            p = new Point3d(0, 0, 0);
            v1 = new Vector3d(10, 0, 0);
            v2 = new Vector3d(0, 10, 0);
            e = new Ellipse(p, v1, v2);

            p = new Point3d(11, 11, 0);
            Assert.AreEqual(e.ClosestPoint(p), new Point3d(Sqrt(50), Sqrt(50), 0));

            p = new Point3d(-11, 11, 0);
            Assert.AreEqual(e.ClosestPoint(p), new Point3d(-Sqrt(50), Sqrt(50), 0));

            p = new Point3d(-11, -11, 0);
            Assert.AreEqual(e.ClosestPoint(p), new Point3d(-Sqrt(50), -Sqrt(50), 0));
        }
    }
}
