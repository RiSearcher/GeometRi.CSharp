using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

namespace GeometRi_Tests
{
    [TestClass]
    public class EllipsoidTest
    {
        [TestMethod()]
        public void EllipsoidIntersectionWithLineTest()
        {
            Point3d p = new Point3d(0, 0, 0);
            Vector3d v1 = new Vector3d(4, 0, 0);
            Vector3d v2 = new Vector3d(0, 6, 0);
            Vector3d v3 = new Vector3d(0, 0, 9);
            Ellipsoid e = new Ellipsoid(p, v1, v2, v3);


            Line3d l = new Line3d(new Point3d(0, 0, 0), v1 = new Vector3d(1, 0, 0));
            Assert.IsTrue((Segment3d)e.IntersectionWith(l) == new Segment3d(new Point3d(-4, 0, 0), new Point3d(4, 0, 0)));

            l = new Line3d(new Point3d(0, 0, 0), v1 = new Vector3d(0, 1, 0));
            Assert.IsTrue((Segment3d)e.IntersectionWith(l) == new Segment3d(new Point3d(0, -6, 0), new Point3d(0, 6, 0)));

            l = new Line3d(new Point3d(0, 0, 0), v1 = new Vector3d(0, 0, 1));
            Assert.IsTrue((Segment3d)e.IntersectionWith(l) == new Segment3d(new Point3d(0, 0, -9), new Point3d(0, 0, 9)));


            p = new Point3d(0, 2, 1);
            v1 = new Vector3d(-1, 1, 3);
            l = new Line3d(p, v1);

            Segment3d s = (Segment3d)e.IntersectionWith(l);

            Assert.IsTrue(s.P1.IsOnBoundary(e));
            Assert.IsTrue(s.P2.IsOnBoundary(e));

        }

        [TestMethod()]
        public void EllipsoidIntersectionWithSegmentRelativeTest()
        {
            double tol = GeometRi3D.Tolerance;
            bool mode = GeometRi3D.UseAbsoluteTolerance;
            GeometRi3D.Tolerance = 0.01;
            GeometRi3D.UseAbsoluteTolerance = false;

            Point3d p = new Point3d(0, 0, 0);
            Vector3d v1 = new Vector3d(4, 0, 0);
            Vector3d v2 = new Vector3d(0, 6, 0);
            Vector3d v3 = new Vector3d(0, 0, 9);
            Ellipsoid e = new Ellipsoid(p, v1, v2, v3);

            Segment3d s = new Segment3d(new Point3d(-5, 0.01, 0), new Point3d(5, 0, 0));
            Assert.IsTrue((Segment3d)e.IntersectionWith(s) == new Segment3d(new Point3d(-4, 0, 0), new Point3d(4, 0, 0)));

            s = new Segment3d(new Point3d(0, -7, 0), new Point3d(0.01, 7, 0));
            Assert.IsTrue((Segment3d)e.IntersectionWith(s) == new Segment3d(new Point3d(0, -6, 0), new Point3d(0, 6, 0)));

            s = new Segment3d(new Point3d(0, 0, 0), new Point3d(0.01, 0, 10));
            Assert.IsTrue((Segment3d)e.IntersectionWith(s) == new Segment3d(new Point3d(0, 0, 0), new Point3d(0, 0, 9)));

            // Resore initial state
            GeometRi3D.UseAbsoluteTolerance = mode;
            GeometRi3D.Tolerance = tol;
        }

        [TestMethod()]
        public void EllipsoidIntersectionWithPlaneTest()
        {
            Point3d p = new Point3d(0, 0, 0);
            Vector3d v1 = new Vector3d(5, 0, 0);
            Vector3d v2 = new Vector3d(0, 4, 0);
            Vector3d v3 = new Vector3d(0, 0, 3);
            Ellipsoid e = new Ellipsoid(p, v1, v2, v3);

            Plane3d s = new Plane3d(1, 2, 3, 4);

            Ellipse res = (Ellipse)e.IntersectionWith(s);

            Assert.IsTrue(res.Center.IsInside(e));
            Assert.IsTrue(res.Center.Translate(res.MajorSemiaxis).BelongsTo(e));
            Assert.IsTrue(res.Center.Translate(res.MinorSemiaxis).BelongsTo(e));
            Assert.IsTrue(res.Center.Translate(-res.MajorSemiaxis).BelongsTo(e));
            Assert.IsTrue(res.Center.Translate(-res.MinorSemiaxis).BelongsTo(e));
            Assert.IsTrue(res.ParametricForm(0.01).BelongsTo(e));
            Assert.IsTrue(res.ParametricForm(0.11).BelongsTo(e));
            Assert.IsTrue(res.ParametricForm(0.55).BelongsTo(e));
            Assert.IsTrue(res.ParametricForm(0.876).BelongsTo(e));
        }

        [TestMethod()]
        public void EllipsoidProjectionToLineTest_1()
        {
            Point3d p = new Point3d(0, 0, 0);
            Vector3d v1 = new Vector3d(4, 0, 0);
            Vector3d v2 = new Vector3d(0, 6, 0);
            Vector3d v3 = new Vector3d(0, 0, 9);
            Ellipsoid e = new Ellipsoid(p, v1, v2, v3);

            p = new Point3d(1, 1, 1);
            v1 = new Vector3d(0, 1, 0);
            Line3d l = new Line3d(p, v1);
            Segment3d s = e.ProjectionTo(l);

            Segment3d res = new Segment3d(new Point3d(1, 6, 1), new Point3d(1, -6, 1));

            Assert.AreEqual(s,res);
        }

        [TestMethod()]
        public void EllipsoidProjectionToLineTest_2()
        {
            Point3d p = new Point3d(0, 0, 0);
            Vector3d v1 = new Vector3d(4, 0, 0);
            Vector3d v2 = new Vector3d(0, 6, 0);
            Vector3d v3 = new Vector3d(0, 0, 9);
            Ellipsoid e = new Ellipsoid(p, v1, v2, v3);

            p = new Point3d(1, 1, 1);
            v1 = new Vector3d(1, 1, 3);
            Line3d l = new Line3d(p, v1);
            Segment3d s = e.ProjectionTo(l);

            // Construct plane orthogonal to line and passing through segment end point
            // And check if it is touching ellipsoid
            Plane3d pl1 = new Plane3d(s.P1, v1);
            object obj = e.IntersectionWith(pl1);

            if (obj.GetType() == typeof(Point3d))
            {
                p = (Point3d)obj;
                if (p.BelongsTo(e)) {
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.Fail();
                }
            }
            else
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void PointInEllipsoidTest()
        {
            Point3d p = new Point3d(1, 1, 1);
            Ellipsoid s = new Ellipsoid(p, new Vector3d(3,0,0), new Vector3d(0, 5, 0), new Vector3d(0, 0, 2));

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

        [TestMethod()]
        public void ClosestPointTest()
        {
            Point3d p = new Point3d(0, 0, 0);
            Vector3d v1 = new Vector3d(10, 0, 0);
            Vector3d v2 = new Vector3d(0, 5, 0);
            Vector3d v3 = new Vector3d(0, 0, 3);
            Ellipsoid e = new Ellipsoid(p, v1, v2, v3);

            p = new Point3d(11, 0, 0);
            Assert.AreEqual(e.ClosestPoint(p), new Point3d(10, 0, 0));

            p = new Point3d(-11, 0, 0);
            Assert.AreEqual(e.ClosestPoint(p), new Point3d(-10, 0, 0));

            p = new Point3d(0, 6, 0);
            Assert.AreEqual(e.ClosestPoint(p), new Point3d(0, 5, 0));

            p = new Point3d(0, -6, 0);
            Assert.AreEqual(e.ClosestPoint(p), new Point3d(0, -5, 0));

            p = new Point3d(0, 0, 0);
            Assert.AreEqual(e.ClosestPoint(p), new Point3d(0, 3, 0));

            p = new Point3d(0, 0, 0);
            v1 = new Vector3d(10, 0, 0);
            v2 = new Vector3d(0, 10, 0);
            v3 = new Vector3d(0, 0, 10);
            e = new Ellipsoid(p, v1, v2, v3);

            p = new Point3d(11, 11, 0);
            Assert.AreEqual(e.ClosestPoint(p), new Point3d(Sqrt(50), Sqrt(50), 0));

            p = new Point3d(-11, 11, 0);
            Assert.AreEqual(e.ClosestPoint(p), new Point3d(-Sqrt(50), Sqrt(50), 0));

            p = new Point3d(-11, -11, 0);
            Assert.AreEqual(e.ClosestPoint(p), new Point3d(-Sqrt(50), -Sqrt(50), 0));

            p = new Point3d(-6, -6, 6);
            double dist = Sqrt(3 * 6 * 6) - 10;
            Assert.IsTrue(Abs(e.ClosestPoint(p).DistanceTo(p) - dist) <= GeometRi3D.Tolerance);

            // Internal point
            p = new Point3d(-5, -5, 5);
            dist = Abs(Sqrt(3 * 5 * 5) - 10);
            Assert.IsTrue(Abs(e.ClosestPoint(p).DistanceTo(p) - dist) <= GeometRi3D.Tolerance);
        }
    }
}
