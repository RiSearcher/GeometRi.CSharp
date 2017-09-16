using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

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
    }
}
