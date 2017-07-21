using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

namespace GeometRi_Tests
{
    [TestClass]
    public class TranslateTest
    {
        //===============================================================
        // Object translate tests
        //===============================================================

        [TestMethod()]
        public void TranslatePointTest1()
        {
            Coord3d coord1 = Coord3d.GlobalCS.Copy();
            coord1.RotateDeg(new Vector3d(0, 0, 1), 90);
            coord1.Origin = new Point3d(1, 1, 1);

            Point3d p1 = new Point3d(1, 2, 3, Coord3d.GlobalCS);
            Vector3d v1 = new Vector3d(1, 1, 1, coord1);

            Assert.IsTrue(p1.Translate(v1) == new Point3d(0, 3, 4));
        }

        [TestMethod()]
        public void TranslatePointTest()
        {
            Coord3d coord1 = Coord3d.GlobalCS.Copy();
            Coord3d coord2 = Coord3d.GlobalCS.Copy();
            coord1.RotateDeg(new Vector3d(1, 2, 3), 90);
            coord2.RotateDeg(new Vector3d(1, 1, 1), 120);
            coord1.Origin = new Point3d(1, 1, 1);
            coord2.Origin = new Point3d(10, 2, 5);

            Point3d p1 = new Point3d(1, 2, 3, Coord3d.GlobalCS);
            Point3d p2 = new Point3d(10, -2, 6, coord1);
            Point3d p3 = new Point3d(-3, 5, 1, coord2);
            Vector3d v1 = new Vector3d(p1, p2);
            Vector3d v2 = new Vector3d(p2, p3);
            Vector3d v3 = new Vector3d(p3, p1);

            Point3d p = new Point3d(5, 6, 7);

            Assert.IsTrue(p.Translate(v1).Translate(v2).Translate(v3) == p);
        }

        [TestMethod()]
        public void TranslatePlaneTest()
        {
            Coord3d coord1 = Coord3d.GlobalCS.Copy();
            Coord3d coord2 = Coord3d.GlobalCS.Copy();
            coord1.RotateDeg(new Vector3d(1, 2, 3), 90);
            coord2.RotateDeg(new Vector3d(1, 1, 1), 120);
            coord1.Origin = new Point3d(1, 1, 1);
            coord2.Origin = new Point3d(10, 2, 5);

            Point3d p1 = new Point3d(1, 2, 3, Coord3d.GlobalCS);
            Point3d p2 = new Point3d(10, -2, 6, coord1);
            Point3d p3 = new Point3d(-3, 5, 1, coord2);
            Vector3d v1 = new Vector3d(p1, p2);
            Vector3d v2 = new Vector3d(p2, p3);
            Vector3d v3 = new Vector3d(p3, p1);

            Plane3d s = new Plane3d(1, 2, 3, 4);

            Assert.IsTrue(s.Translate(v1).Translate(v2).Translate(v3) == s);
        }
    }
}
