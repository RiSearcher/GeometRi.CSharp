using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

namespace GeometRi_Tests
{
    [TestClass]
    public class RotateTest
    {
        //===============================================================
        // Object rotation tests
        //===============================================================

        [TestMethod()]
        public void PointRotationTest()
        {
            Matrix3d r = Matrix3d.RotationMatrix(new Vector3d(0, 0, 1), PI / 2);
            Point3d p = new Point3d(1, 2, 0);

            Assert.IsTrue(p.Rotate(r) == new Point3d(-2, 1, 0));

            Rotation q = new Rotation(new Vector3d(0, 0, 1), PI / 2);
            Assert.IsTrue(p.Rotate(q) == new Point3d(-2, 1, 0));

        }

        [TestMethod()]
        public void PointRotationAroundPointTest()
        {
            Matrix3d r = Matrix3d.RotationMatrix(new Vector3d(0, 0, 1), PI / 2);
            Point3d p = new Point3d(3, 3, 0);
            Point3d pc = new Point3d(2, 3, 5);

            Assert.IsTrue(p.Rotate(r, pc) == new Point3d(2, 4, 0));

            Rotation q = new Rotation(new Vector3d(0, 0, 1), PI / 2);
            Assert.IsTrue(p.Rotate(q, pc) == new Point3d(2, 4, 0));
        }

        [TestMethod()]
        public void PointRotationAroundPointTest2()
        {
            Matrix3d r = Matrix3d.RotationMatrix(new Vector3d(1, 1, 1), 2 * PI / 3);
            Point3d p = new Point3d(5, 0, 2);
            Point3d pc = new Point3d(1, 1, 3);

            Assert.IsTrue(p.Rotate(r, pc) == new Point3d(0, 5, 2));

            Rotation q = new Rotation(new Vector3d(1, 1, 1), 2 * PI / 3);
            Assert.IsTrue(p.Rotate(q, pc) == new Point3d(0, 5, 2));
        }

        [TestMethod()]
        public void VectorRotationTest()
        {
            Matrix3d r = Matrix3d.RotationMatrix(new Vector3d(1, 1, 1), 2 * PI / 3);
            Vector3d v1 = new Vector3d(1, 1, 1);
            Vector3d v2 = new Vector3d(1, 0, 0);

            Assert.IsTrue(v1.Rotate(r) == v1 && v2.Rotate(r) == new Vector3d(0, 1, 0));

            Rotation q = new Rotation(new Vector3d(1, 1, 1), 2 * PI / 3);
            Assert.IsTrue(v1.Rotate(q) == v1 && v2.Rotate(q) == new Vector3d(0, 1, 0));
        }

        [TestMethod()]
        public void LineRotationTest()
        {
            Matrix3d r = Matrix3d.RotationMatrix(new Vector3d(1, 1, 1), 2 * PI / 3);
            Coord3d coord = new Coord3d(new Point3d(3, 2, 1), r);
            Point3d p = new Point3d(5, 0, 2, coord);
            Line3d l = new Line3d(new Point3d(4, 1, 2), new Vector3d(1, 2, 6));

            Assert.IsTrue(l.Rotate(r, p).Rotate(r, p).Rotate(r, p) == l);

            Rotation q = new Rotation(new Vector3d(1, 1, 1), 2 * PI / 3);
            Assert.IsTrue(l.Rotate(q, p).Rotate(q, p).Rotate(q, p) == l);
        }

        [TestMethod()]
        public void PlaneRotationTest()
        {
            Matrix3d r = Matrix3d.RotationMatrix(new Vector3d(1, 1, 1), 2 * PI / 3);
            Coord3d coord = new Coord3d(new Point3d(3, 2, 1), r);
            Point3d p = new Point3d(5, 0, 2, coord);
            Plane3d s = new Plane3d(1, 2, 3, 4);

            Assert.IsTrue(s.Rotate(r, p).Rotate(r, p).Rotate(r, p) == s);

            Rotation q = new Rotation(new Vector3d(1, 1, 1), 2 * PI / 3);
            Assert.IsTrue(s.Rotate(q, p).Rotate(q, p).Rotate(q, p) == s);
        }
    }
}
