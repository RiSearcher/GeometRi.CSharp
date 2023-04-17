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
    }
}
