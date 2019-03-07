using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;
using System.Collections.Generic;

namespace GeometRi_Tests
{
    [TestClass]
    public class Box3dTest
    {
        [TestMethod]
        public void DefaultBoxTest()
        {
            Rotation r = new Rotation(new Vector3d(2, 1, 5), PI / 3);
            Coord3d coord1 = new Coord3d(new Point3d(2, 3, 1), r.ToRotationMatrix.Transpose());
            Box3d b = new Box3d(coord1);

            Assert.AreEqual(b.Center, coord1.Origin);
            Assert.AreEqual(b.P1, new Point3d(-0.5, -0.5, -0.5, coord1));
            Assert.AreEqual(b.P7, new Point3d(0.5, 0.5, 0.5, coord1));
        }

        [TestMethod]
        public void CornerPointsTest()
        {
            Rotation r = new Rotation();
            Point3d p = new Point3d(5, 5, 5);
            Box3d b = new Box3d(p, 2, 2, 2, r);
            Assert.AreEqual(b.P1, new Point3d(4, 4, 4));
            Assert.AreEqual(b.P3, new Point3d(6, 6, 4));
            Assert.AreEqual(b.P8, new Point3d(4, 6, 6));

            r = Rotation.FromEulerAngles(PI / 2, -PI / 2, 0, "zyz");
            b = new Box3d(p, 2, 2, 2, r);
            Assert.AreEqual(b.P1, new Point3d(6, 4, 6));
            Assert.AreEqual(b.P3, new Point3d(6, 6, 4));
            Assert.AreEqual(b.P8, new Point3d(4, 4, 4));

        }

        [TestMethod]
        public void BoxOrientationTest()
        {
            Rotation r = new Rotation(new Vector3d(0, 0, 1), PI / 4);
            Coord3d coord1 = new Coord3d(new Point3d(2, 3, 1), r.ToRotationMatrix.Transpose());
            Box3d b = new Box3d(coord1);

            Assert.AreEqual(b.V1.Normalized, new Vector3d(1, 1, 0).Normalized);
            Assert.AreEqual(b.V2.Normalized, new Vector3d(-1, 1, 0).Normalized);
            Assert.AreEqual(b.V3.Normalized, new Vector3d(0, 0, 1).Normalized);
        }


        [TestMethod]
        public void PointInAlignedBoxTest()
        {
            Point3d p = new Point3d(1, 1, 1);
            Box3d box = new Box3d(p, 8, 6, 10);

            p = new Point3d(2, 2, 2);  // Point inside
            Assert.IsTrue(p.BelongsTo(box));
            Assert.IsTrue(p.IsInside(box));
            Assert.IsFalse(p.IsOutside(box));
            Assert.IsFalse(p.IsOnBoundary(box));

            p = new Point3d(2, 4, 2);  // Point on side
            Assert.IsTrue(p.BelongsTo(box));
            Assert.IsFalse(p.IsInside(box));
            Assert.IsFalse(p.IsOutside(box));
            Assert.IsTrue(p.IsOnBoundary(box));

            p = new Point3d(5, 4, 6);  // Point in corner
            Assert.IsTrue(p.BelongsTo(box));
            Assert.IsFalse(p.IsInside(box));
            Assert.IsFalse(p.IsOutside(box));
            Assert.IsTrue(p.IsOnBoundary(box));

            p = new Point3d(5, -5, 6);  // Point outside
            Assert.IsFalse(p.BelongsTo(box));
            Assert.IsFalse(p.IsInside(box));
            Assert.IsTrue(p.IsOutside(box));
            Assert.IsFalse(p.IsOnBoundary(box));
        }

        [TestMethod]
        public void PointInAlignedBoxRelativeTest()
        {
            Point3d p = new Point3d(1, 1, 1);
            Box3d box = new Box3d(p, 8, 6, 10);

            double tol = GeometRi3D.Tolerance;
            bool mode = GeometRi3D.UseAbsoluteTolerance;
            GeometRi3D.Tolerance = 0.01;
            GeometRi3D.UseAbsoluteTolerance = false;

            p = new Point3d(2, 2, 2);  // Point inside
            Assert.IsTrue(p.BelongsTo(box));
            Assert.IsTrue(p.IsInside(box));
            Assert.IsFalse(p.IsOutside(box));
            Assert.IsFalse(p.IsOnBoundary(box));

            p = new Point3d(2, 4.1, 2);  // Point on side
            Assert.IsTrue(p.BelongsTo(box));
            Assert.IsFalse(p.IsInside(box));
            Assert.IsFalse(p.IsOutside(box));
            Assert.IsTrue(p.IsOnBoundary(box));

            p = new Point3d(5.1, 4, 6);  // Point in corner
            Assert.IsTrue(p.BelongsTo(box));
            Assert.IsFalse(p.IsInside(box));
            Assert.IsFalse(p.IsOutside(box));
            Assert.IsTrue(p.IsOnBoundary(box));

            p = new Point3d(2, 4.2, 2);  // Point outside
            Assert.IsFalse(p.BelongsTo(box));
            Assert.IsFalse(p.IsInside(box));
            Assert.IsTrue(p.IsOutside(box));
            Assert.IsFalse(p.IsOnBoundary(box));

            // Resore initial state
            GeometRi3D.UseAbsoluteTolerance = mode;
            GeometRi3D.Tolerance = tol;
        }

        [TestMethod]
        public void ClosestPointTest()
        {
            Rotation r = new Rotation(new Vector3d(2, 1, 5), PI / 3);
            Box3d b = new Box3d(new Point3d(1, 1, 1), 2, 2, 2);
            Point3d p0 = new Point3d(1, 2, 2);
            Point3d p1 = new Point3d(-1, -1, -1);
            Point3d p3 = new Point3d(3, 3, -1);
            Point3d p6 = new Point3d(3, -1, 3);

            b = b.Rotate(r, p0);
            p1 = p1.Rotate(r, p0);
            p3 = p3.Rotate(r, p0);
            p6 = p6.Rotate(r, p0);

            Assert.AreEqual(p0, b.ClosestPoint(p0));
            Assert.AreEqual(b.P1, b.ClosestPoint(p1));
            Assert.AreEqual(b.P3, b.ClosestPoint(p3));
            Assert.AreEqual(b.P6, b.ClosestPoint(p6));
        }

        [TestMethod]
        public void DistanceToPointTest()
        {
            Box3d b = new Box3d();
            Point3d p1 = new Point3d(0.3, 0.4, 0.2); // Point inside box
            Point3d p2 = new Point3d(0.3, 0.4, 0.5); // Point on boundary
            Point3d p3 = new Point3d(0.3, 0.4, 1.0); // Point outside box

            Assert.IsTrue(GeometRi3D.AlmostEqual(b.DistanceTo(p1), 0.0));
            Assert.IsTrue(GeometRi3D.AlmostEqual(b.DistanceTo(p2), 0.0));
            Assert.IsTrue(GeometRi3D.AlmostEqual(b.DistanceTo(p3), 0.5));
        }

        [TestMethod]
        public void LineIntersectionWithBoxTest()
        {
            Rotation rot = new Rotation();
            Point3d p = new Point3d(0, 0, 0);
            Box3d b = new Box3d(p, 2, 2, 2, rot);

            Line3d l = new Line3d(new Point3d(1, 1, 1), new Vector3d(1, 1, 1));
            Segment3d s = (Segment3d)b.IntersectionWith(l);
            Assert.AreEqual(s, new Segment3d(new Point3d(-1, -1, -1), new Point3d(1, 1, 1)));

            l = new Line3d(new Point3d(1, -1, -1), new Vector3d(-1, 1, 1));
            s = (Segment3d)b.IntersectionWith(l);
            Assert.AreEqual(s, new Segment3d(new Point3d(1, -1, -1), new Point3d(-1, 1, 1)));

            l = new Line3d(new Point3d(0, 0, 0), new Vector3d(1, 0, 0));
            s = (Segment3d)b.IntersectionWith(l);
            Assert.AreEqual(s, new Segment3d(new Point3d(-1, 0, 0), new Point3d(1, 0, 0)));

            l = new Line3d(new Point3d(0, 0, 0), new Vector3d(0, 1, 0));
            s = (Segment3d)b.IntersectionWith(l);
            Assert.AreEqual(s, new Segment3d(new Point3d(0, -1, 0), new Point3d(0, 1, 0)));

            l = new Line3d(new Point3d(0, 0, 0), new Vector3d(0, 0, 1));
            s = (Segment3d)b.IntersectionWith(l);
            Assert.AreEqual(s, new Segment3d(new Point3d(0, 0, -1), new Point3d(0, 0, 1)));

            // Intersection is point
            l = new Line3d(new Point3d(-1, -1, 1), new Vector3d(1, 1, 1));
            Assert.AreEqual((Point3d)b.IntersectionWith(l), new Point3d(-1, -1, 1));
        }

        [TestMethod]
        public void RayIntersectionWithBoxTest()
        {
            Rotation rot = new Rotation();
            Point3d p = new Point3d(0, 0, 0);
            Box3d b = new Box3d(p, 2, 2, 2, rot);

            Ray3d r = new Ray3d(new Point3d(-1, -1, -1), new Vector3d(1, 1, 1));
            Segment3d s = (Segment3d)b.IntersectionWith(r);
            Assert.AreEqual(s, new Segment3d(new Point3d(-1, -1, -1), new Point3d(1, 1, 1)));

            r = new Ray3d(new Point3d(0, 0, 0), new Vector3d(1, 1, 1));
            s = (Segment3d)b.IntersectionWith(r);
            Assert.AreEqual(s, new Segment3d(new Point3d(0, 0, 0), new Point3d(1, 1, 1)));

            r = new Ray3d(new Point3d(1, -1, -1), new Vector3d(-1, 1, 1));
            s = (Segment3d)b.IntersectionWith(r);
            Assert.AreEqual(s, new Segment3d(new Point3d(1, -1, -1), new Point3d(-1, 1, 1)));

            r = new Ray3d(new Point3d(0, 0, 0), new Vector3d(1, 0, 0));
            s = (Segment3d)b.IntersectionWith(r);
            Assert.AreEqual(s, new Segment3d(new Point3d(0, 0, 0), new Point3d(1, 0, 0)));

            r = new Ray3d(new Point3d(0, 0, 0), new Vector3d(0, -1, 0));
            s = (Segment3d)b.IntersectionWith(r);
            Assert.AreEqual(s, new Segment3d(new Point3d(0, 0, 0), new Point3d(0, -1, 0)));

            r = new Ray3d(new Point3d(0, 0, 0), new Vector3d(0, 0, 1));
            s = (Segment3d)b.IntersectionWith(r);
            Assert.AreEqual(s, new Segment3d(new Point3d(0, 0, 0), new Point3d(0, 0, 1)));

            // Intersection is point
            r = new Ray3d(new Point3d(-1, -1, 1), new Vector3d(1, 1, 1));
            Assert.AreEqual((Point3d)b.IntersectionWith(r), new Point3d(-1, -1, 1));
        }

        [TestMethod]
        public void SegmentIntersectionWithBoxTest()
        {
            Rotation rot = new Rotation();
            Point3d p = new Point3d(0, 0, 0);
            Box3d b = new Box3d(p, 2, 2, 2, rot);

            Segment3d s = new Segment3d(new Point3d(-1, -1, -1), new Point3d(1, 1, 1));
            Assert.AreEqual((Segment3d)b.IntersectionWith(s), new Segment3d(new Point3d(-1, -1, -1), new Point3d(1, 1, 1)));

            s = new Segment3d(new Point3d(0, 0, 0), new Point3d(1, 1, 1));
            Assert.AreEqual((Segment3d)b.IntersectionWith(s), new Segment3d(new Point3d(0, 0, 0), new Point3d(1, 1, 1)));

            s = new Segment3d(new Point3d(1, -1, -1), new Point3d(-1, 1, 1));
            Assert.AreEqual((Segment3d)b.IntersectionWith(s), new Segment3d(new Point3d(1, -1, -1), new Point3d(-1, 1, 1)));

            s = new Segment3d(new Point3d(0, 0, 0), new Point3d(1, 0, 0));
            Assert.AreEqual((Segment3d)b.IntersectionWith(s), new Segment3d(new Point3d(0, 0, 0), new Point3d(1, 0, 0)));

            s = new Segment3d(new Point3d(0, 0, 0), new Point3d(0, -1, 0));
            Assert.AreEqual((Segment3d)b.IntersectionWith(s), new Segment3d(new Point3d(0, 0, 0), new Point3d(0, -1, 0)));

            s = new Segment3d(new Point3d(0, 0, 0), new Point3d(0, 0, 1));
            Assert.AreEqual((Segment3d)b.IntersectionWith(s), new Segment3d(new Point3d(0, 0, 0), new Point3d(0, 0, 1)));

            // Intersection is point
            s = new Segment3d(new Point3d(-1, -1, 1), new Point3d(0, 0, 2));
            Assert.AreEqual((Point3d)b.IntersectionWith(s), new Point3d(-1, -1, 1));
        }

        [TestMethod]
        public void SegmentIntersectionWithBoxRelativeTest()
        {

            double tol = GeometRi3D.Tolerance;
            bool mode = GeometRi3D.UseAbsoluteTolerance;
            GeometRi3D.Tolerance = 0.01;
            GeometRi3D.UseAbsoluteTolerance = false;

            Rotation rot = new Rotation();
            Point3d p = new Point3d(0, 0, 0);
            Box3d b = new Box3d(p, 2, 2, 2, rot);

            // Segment aligned with X-axis
            Segment3d s = new Segment3d(new Point3d(-2, 0, 0), new Point3d(-1.01, 0, 0));
            Assert.AreEqual((Point3d)b.IntersectionWith(s), new Point3d(-1, 0, 0));

            s = new Segment3d(new Point3d(1.01, 0, 0), new Point3d(2, 0, 0));
            Assert.AreEqual((Point3d)b.IntersectionWith(s), new Point3d(1, 0, 0));

            s = new Segment3d(new Point3d(-0.5, 0, 0), new Point3d(0.5, 0, 0));
            Assert.AreEqual((Segment3d)b.IntersectionWith(s), s);

            s = new Segment3d(new Point3d(-1.5, 0, 0), new Point3d(1.5, 0, 0));
            Assert.AreEqual((Segment3d)b.IntersectionWith(s), new Segment3d(new Point3d(-1, 0, 0), new Point3d(1, 0, 0)));

            // Segment aligned with Y-axis
            s = new Segment3d(new Point3d(0, -2, 0), new Point3d(0, -1.01, 0));
            Assert.AreEqual((Point3d)b.IntersectionWith(s), new Point3d(0, -1, 0));

            s = new Segment3d(new Point3d(0, 1.01, 0), new Point3d(0, 2, 0));
            Assert.AreEqual((Point3d)b.IntersectionWith(s), new Point3d(0, 1, 0));

            s = new Segment3d(new Point3d(0, -0.5, 0), new Point3d(0, 0.5, 0));
            Assert.AreEqual((Segment3d)b.IntersectionWith(s), s);

            s = new Segment3d(new Point3d(0, -1.5, 0), new Point3d(0, 1.5, 0));
            Assert.AreEqual((Segment3d)b.IntersectionWith(s), new Segment3d(new Point3d(0, -1, 0), new Point3d(0, 1, 0)));

            // Segment aligned with Z-axis
            s = new Segment3d(new Point3d(0, 0, -2), new Point3d(0, 0, -1.01));
            Assert.AreEqual((Point3d)b.IntersectionWith(s), new Point3d(0, 0, -1));

            s = new Segment3d(new Point3d(0, 0, 1.01), new Point3d(0, 0, 2));
            Assert.AreEqual((Point3d)b.IntersectionWith(s), new Point3d(0, 0, 1));

            s = new Segment3d(new Point3d(0, 0, -0.5), new Point3d(0, 0, 0.5));
            Assert.AreEqual((Segment3d)b.IntersectionWith(s), s);

            s = new Segment3d(new Point3d(0, 0, -1.5), new Point3d(0, 0, 1.5));
            Assert.AreEqual((Segment3d)b.IntersectionWith(s), new Segment3d(new Point3d(0, 0, -1), new Point3d(0, 0, 1)));

            // Segment crossing corner
            s = new Segment3d(new Point3d(2.01, 0, 1), new Point3d(0.01, 2, 1));
            Assert.AreEqual((Point3d)b.IntersectionWith(s), new Point3d(1, 1, 1));

            // Resore initial state
            GeometRi3D.UseAbsoluteTolerance = mode;
            GeometRi3D.Tolerance = tol;
        }

        [TestMethod]
        public void BoxRotationTest()
        {
            Rotation r1 = new Rotation(new Vector3d(0, 0, 1), PI / 2);
            Rotation r2 = new Rotation(new Vector3d(1, 0, 0), PI / 2);
            Box3d b = new Box3d();

            b = b.Rotate(r1, new Point3d(-5, 0, 0));
            b = b.Rotate(r2, new Point3d(-5, 0, 0));

            Assert.AreEqual(b.V1.Normalized, new Vector3d(0, 0, 1));
            Assert.AreEqual(b.V2.Normalized, new Vector3d(-1, 0, 0));
            Assert.AreEqual(b.V3.Normalized, new Vector3d(0, -1, 0));
            Assert.AreEqual(b.Center, new Point3d(-5, 0, 5));
        }

        [TestMethod]
        public void BoxReflectInPointTest()
        {

            Box3d b = new Box3d();
            List<Point3d> original_points = b.ListOfPoints;

            Point3d p = new Point3d(-4.1, 7.876, -8);
            Box3d reflected_box = b.ReflectIn(p);
            List<Point3d> reflected_points = reflected_box.ListOfPoints;

            foreach (Point3d op in original_points)
            {
                Point3d reflected_p = op.ReflectIn(p);
                foreach (Point3d rp in reflected_points)
                {
                    if (reflected_p == rp)
                    {
                        reflected_points.Remove(rp);
                        break;
                    }
                }
            }

            Assert.IsTrue(reflected_points.Count == 0);

        }

        [TestMethod]
        public void BoxReflectInLineTest()
        {

            Box3d b = new Box3d();
            List < Point3d > original_points = b.ListOfPoints;

            Line3d l = new Line3d(new Point3d(-4.1, 7.876, -8), new Vector3d(1.25, -8, -22));
            Box3d reflected_box = b.ReflectIn(l);
            List  < Point3d > reflected_points = reflected_box.ListOfPoints;

            foreach (Point3d op in original_points)
            {
                Point3d reflected_p = op.ReflectIn(l);
                foreach (Point3d rp in reflected_points)
                {
                    if (reflected_p == rp)
                    {
                        reflected_points.Remove(rp);
                        break;
                    }
                }
            }

            Assert.IsTrue(reflected_points.Count == 0);

        }

        [TestMethod]
        public void BoxReflectInPlaneTest()
        {

            Box3d b = new Box3d();
            List<Point3d> original_points = b.ListOfPoints;

            Plane3d s = new Plane3d(new Point3d(-4.1, 7.876, -8), new Vector3d(1.25, -8, -22));
            Box3d reflected_box = b.ReflectIn(s);
            List<Point3d> reflected_points = reflected_box.ListOfPoints;

            foreach (Point3d op in original_points)
            {
                Point3d reflected_p = op.ReflectIn(s);
                foreach (Point3d rp in reflected_points)
                {
                    if (reflected_p == rp)
                    {
                        reflected_points.Remove(rp);
                        break;
                    }
                }
            }

            Assert.IsTrue(reflected_points.Count == 0);

        }

        [TestMethod()]
        public void BoxDistanceToCircleTest()
        {
            Box3d box = new Box3d();
            Circle3d c = new Circle3d(new Point3d(-1.3195, -1.0435, -0.70047), 0.35, new Vector3d(0.83694, -0.13208, -0.53112));

            double dist = box.DistanceTo(c);
            Assert.IsTrue(dist > 0);

        }

        [TestMethod()]
        public void BoxIntersectsCircleTest()
        {
            Box3d box = new Box3d();
            Circle3d c = new Circle3d(new Point3d(), 1, new Vector3d(0, 0, 1));
            Assert.IsTrue(c.Intersects(box));

            c = new Circle3d(new Point3d(1.5, 0, 0), 1, new Vector3d(0, 0, 1));
            Assert.IsTrue(c.Intersects(box));

            c = new Circle3d(new Point3d(0.5, 0.5, 0.5), 1, new Vector3d(1, 1, 1));
            Assert.IsTrue(c.Intersects(box));

            c = new Circle3d(new Point3d(0.6, 0.6, 0.6), 1, new Vector3d(1, 1, 1));
            Assert.IsFalse(c.Intersects(box));

        }

        [TestMethod()]
        public void BoxIntersectsCircleTest2()
        {
            Box3d box = new Box3d(new Point3d(0.5, 0.5, 0.5), 1, 1, 1);
            Circle3d c = new Circle3d(new Point3d(0.957494668177094, 1.08987119472114, -0.11622424522239),
                                      0.154926580712558, 
                                      new Vector3d(0.362303959251271, 0.267138656415756, 0.892957322249635));
            Assert.IsFalse(c.Intersects(box));


        }


    }


}
