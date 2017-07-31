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

            p = new Point3d(0, 2, 1);
            v1 = new Vector3d(-1, 1, 3);
            Line3d l = new Line3d(p, v1);

            Segment3d s = (Segment3d)e.IntersectionWith(l);

            Assert.IsTrue(s.P1.BelongsTo(e));
            Assert.IsTrue(s.P2.BelongsTo(e));
        }
    }
}
