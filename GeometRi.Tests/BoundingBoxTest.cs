using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

namespace GeometRi_Tests
{
    [TestClass]
    public class BoundingBoxTest
    {
        [TestMethod]
        public void EllipsoidAABBTest()
        {
            Point3d p = new Point3d(0, 0, 0);
            Vector3d v1 = new Vector3d(3, 0, 0);
            Vector3d v2 = new Vector3d(0, 2, 0);
            Vector3d v3 = new Vector3d(0, 0, 4);
            Ellipsoid e = new Ellipsoid(p, v1, v2, v3);
            Rotation r = new Rotation(new Vector3d(1, 2, 3), PI / 4);
            e = e.Rotate(r, p);

            Box3d b = e.BoundingBox();

            Plane3d s = new Plane3d(b.P1, b.P2, b.P3);
            Assert.IsTrue(e.IntersectionWith(s).GetType() == typeof(Point3d));

            s = new Plane3d(b.P8, b.P7, b.P6);
            Assert.IsTrue(e.IntersectionWith(s).GetType() == typeof(Point3d));

            s = new Plane3d(b.P1, b.P2, b.P5);
            Assert.IsTrue(e.IntersectionWith(s).GetType() == typeof(Point3d));

            s = new Plane3d(b.P2, b.P3, b.P6);
            Assert.IsTrue(e.IntersectionWith(s).GetType() == typeof(Point3d));

            s = new Plane3d(b.P3, b.P4, b.P7);
            Assert.IsTrue(e.IntersectionWith(s).GetType() == typeof(Point3d));

            s = new Plane3d(b.P1, b.P4, b.P8);
            Assert.IsTrue(e.IntersectionWith(s).GetType() == typeof(Point3d));
        }

        [TestMethod]
        public void SphereAABBTest()
        {
            Point3d p = new Point3d(0, 0, 0);
            Sphere e = new Sphere(p, 5);
            Rotation r = new Rotation(new Vector3d(1, 2, 3), PI / 4);
            Coord3d cs = new Coord3d(new Point3d(1, 2, 4), r.ToRotationMatrix.Transpose());

            Box3d b = e.BoundingBox(cs);

            Plane3d s = new Plane3d(b.P1, b.P2, b.P3);
            Assert.IsTrue(e.IntersectionWith(s).GetType() == typeof(Point3d));

            s = new Plane3d(b.P8, b.P7, b.P6);
            Assert.IsTrue(e.IntersectionWith(s).GetType() == typeof(Point3d));

            s = new Plane3d(b.P1, b.P2, b.P5);
            Assert.IsTrue(e.IntersectionWith(s).GetType() == typeof(Point3d));

            s = new Plane3d(b.P2, b.P3, b.P6);
            Assert.IsTrue(e.IntersectionWith(s).GetType() == typeof(Point3d));

            s = new Plane3d(b.P3, b.P4, b.P7);
            Assert.IsTrue(e.IntersectionWith(s).GetType() == typeof(Point3d));

            s = new Plane3d(b.P1, b.P4, b.P8);
            Assert.IsTrue(e.IntersectionWith(s).GetType() == typeof(Point3d));
        }

        [TestMethod]
        public void TriangleAABBTest()
        {
            Point3d p1 = new Point3d(1, 0, 2);
            Point3d p2 = new Point3d(4, 2, 6);
            Point3d p3 = new Point3d(0, 4, 0);
            Triangle t = new Triangle(p1, p2, p3);

            Box3d res = new Box3d(new Point3d(2, 2, 3), 4, 4, 6);

            Assert.AreEqual(t.BoundingBox(), res);
        }

        [TestMethod]
        public void CircleAABBTest()
        {
            Point3d p = new Point3d(1, 1, 1);
            Vector3d v = new Vector3d(3, 4, 0);
            Circle3d c = new Circle3d(p, 5, v);

            Box3d res = new Box3d(p, 8, 6, 10);

            Assert.AreEqual(c.BoundingBox(), res);
        }
    }
}
