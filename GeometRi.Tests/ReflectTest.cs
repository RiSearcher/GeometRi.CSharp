using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

namespace GeometRi_Tests
{
    [TestClass]
    public class ReflectTest
    {
        //===============================================================
        // Object reflection tests
        //===============================================================

        [TestMethod()]
        public void PointReflectInPointTest()
        {
            Point3d p1 = new Point3d(2, 3, 4);
            Point3d p2 = new Point3d(1, 1, 1);

            Assert.IsTrue(p1.ReflectIn(p2) == new Point3d(0, -1, -2));
        }

        [TestMethod()]
        public void PointReflectInLineTest()
        {
            Point3d p1 = new Point3d(2, 3, 4);
            Point3d p2 = new Point3d(1, 1, 1);
            Line3d l = new Line3d(p2, new Vector3d(0, 0, 1));

            Assert.IsTrue(p1.ReflectIn(l) == new Point3d(0, -1, 4));
        }

        [TestMethod()]
        public void PointReflectInPlaneTest()
        {
            Point3d p1 = new Point3d(3, 3, 3);
            Plane3d s = new Plane3d(1, 1, 1, -3);

            Assert.IsTrue(p1.ReflectIn(s) == new Point3d(-1, -1, -1));
        }

        [TestMethod()]
        public void LineReflectInPointTest()
        {
            Point3d p1 = new Point3d(1, 1, 1);
            Line3d l1 = new Line3d(new Point3d(2, 3, 0), new Vector3d(0, 0, 1));
            Line3d l2 = new Line3d(new Point3d(0, -1, 2), new Vector3d(0, 0, 1));

            Assert.IsTrue(l1.ReflectIn(p1) == l2);
        }

        [TestMethod()]
        public void PlaneReflectInPlaneTest()
        {
            Coord3d coord1 = new Coord3d(new Point3d(2, 3, 1), Matrix3d.RotationMatrix(new Vector3d(2, 1, 5), PI / 3));
            Coord3d coord2 = new Coord3d(new Point3d(1, -3, 4), Matrix3d.RotationMatrix(new Vector3d(3, 2, 1), PI / 2));

            Plane3d s1 = new Plane3d(1, 2, -2, -3, coord1);
            Plane3d s2 = new Plane3d(2, -1, 3, -1, coord2);
            Plane3d s3 = s1.ReflectIn(s2);
            s3.Point = s3.Point.ConvertTo(coord2);
            Plane3d s4 = s3.ReflectIn(s2);

            bool bol = s4.Equals(s1);

            Assert.IsTrue(s4 == s1);
        }

        [TestMethod()]
        public void LineReflectInLineTest()
        {
            Coord3d coord1 = new Coord3d(new Point3d(2, 3, 1), Matrix3d.RotationMatrix(new Vector3d(2, 1, 5), PI / 3));
            Coord3d coord2 = new Coord3d(new Point3d(1, -3, 4), Matrix3d.RotationMatrix(new Vector3d(3, 2, 1), PI / 2));

            Line3d l1 = new Line3d(new Point3d(2, 3, 4, coord1), new Vector3d(2, 1, -3));
            Line3d l2 = new Line3d(new Point3d(2, 3, 4, coord2), new Vector3d(2, 1, -3));
            Line3d lt = l1.ReflectIn(l2);
            lt.Point = lt.Point.ConvertTo(coord2);

            Assert.IsTrue(lt.ReflectIn(l2) == l1);
        }

    }
}
