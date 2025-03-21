using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

namespace GeometRi_Tests
{
    [TestClass]
    public class AABBTest
    {
        [TestMethod]
        public void AABB_DistanceTo_AABB_1()
        {
            AABB box1 = new AABB(new Point3d(1, 1, 1), new Point3d(2, 2, 2));
            AABB box2 = new AABB(new Point3d(3, 3, 3), new Point3d(4, 4, 4));
            double dist = Sqrt(3);
            Assert.IsTrue(GeometRi3D.AlmostEqual(dist, box1.DistanceTo(box2)));
        }

        [TestMethod]
        public void AABB_DistanceTo_AABB_2()
        {
            AABB box1 = new AABB(new Point3d(1, 1, 1), new Point3d(2, 2, 2));
            AABB box2 = new AABB(new Point3d(3, 1, 1), new Point3d(4, 2, 2));
            double dist = 1;
            Assert.IsTrue(GeometRi3D.AlmostEqual(dist, box1.DistanceTo(box2)));
        }

        [TestMethod]
        public void AABB_IntersectionWith_AABB_01()
        {
            // Fully overlapping boxes
            AABB box1 = new AABB(new Point3d(1, 1, 1), new Point3d(2, 2, 2));
            AABB box2 = new AABB(new Point3d(0, 0, 0), new Point3d(4, 4, 4));

            Assert.IsTrue(box1.IntersectionWith(box2) == box1);
            Assert.IsTrue(box2.IntersectionWith(box1) == box1);
            Assert.IsTrue(box1.Intersects(box2));
            Assert.IsTrue(box2.Intersects(box1));

        }

        [TestMethod]
        public void AABB_IntersectionWith_AABB_02()
        {
            // Partially overlapping boxes
            AABB box1 = new AABB(new Point3d(1, 1, 1), new Point3d(3, 3, 3));
            AABB box2 = new AABB(new Point3d(2, 2, 2), new Point3d(4, 4, 4));

            AABB res = new AABB(new Point3d(2, 2, 2), new Point3d(3, 3, 3));

            Assert.IsTrue(box1.IntersectionWith(box2) == res);
            Assert.IsTrue(box2.IntersectionWith(box1) == res);
            Assert.IsTrue(box1.Intersects(box2));
            Assert.IsTrue(box2.Intersects(box1));
        }

        [TestMethod]
        public void AABB_IntersectionWith_AABB_03()
        {
            // Non-overlapping boxes
            AABB box1 = new AABB(new Point3d(1, 1, 1), new Point3d(3, 3, 3));
            AABB box2 = new AABB(new Point3d(4, 4, 4), new Point3d(5, 5, 5));

            Assert.IsTrue(box1.IntersectionWith(box2) == null);
            Assert.IsTrue(box2.IntersectionWith(box1) == null);
            Assert.IsFalse(box1.Intersects(box2));
            Assert.IsFalse(box2.Intersects(box1));
        }

        [TestMethod]
        public void PointInAABBTest()
        {
            Point3d p = new Point3d(1, 1, 1);
            AABB box = new AABB(p, 8, 6, 10);

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
    }
}
