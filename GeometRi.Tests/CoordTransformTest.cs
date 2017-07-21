using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

namespace GeometRi_Tests
{
    [TestClass]
    public class CoordTransformTest
    {
        //===============================================================
        // Coordinate transformation tests
        //===============================================================

        [TestMethod()]
        public void PointConvertToTest()
        {
            Coord3d coord1 = Coord3d.GlobalCS.Copy();
            Coord3d coord2 = Coord3d.GlobalCS.Copy();
            coord2.RotateDeg(new Vector3d(1, 1, 1), 120);

            Assert.IsTrue(new Point3d(1, 2, 3, coord1) == new Point3d(2, 3, 1, coord2));
        }

        [TestMethod()]
        public void PointConvertToTest2()
        {
            Coord3d coord1 = new Coord3d(new Point3d(2, 3, 1), Matrix3d.RotationMatrix(new Vector3d(2, 1, 5), PI / 3));
            Coord3d coord2 = new Coord3d(new Point3d(1, -3, 4), Matrix3d.RotationMatrix(new Vector3d(3, 2, 1), PI / 2));

            Point3d p1 = new Point3d(1, 2, -2, coord1);
            Point3d p2 = p1.ConvertTo(coord2);

            Assert.IsTrue(p2 == p1);
        }

        [TestMethod()]
        public void PointConvertToGlobalTest()
        {
            Coord3d coord1 = Coord3d.GlobalCS.Copy();
            Coord3d coord2 = Coord3d.GlobalCS.Copy();
            coord2.RotateDeg(new Vector3d(1, 1, 1), 120);

            Point3d p1 = new Point3d(1, 2, 3, coord1);
            Point3d p2 = new Point3d(2, 3, 1, coord2);
            p1 = p1.ConvertToGlobal();
            p2 = p2.ConvertToGlobal();

            Assert.IsTrue(p1 == p2);
        }

        [TestMethod()]
        public void VectorConvertToTest()
        {
            Coord3d coord1 = Coord3d.GlobalCS.Copy();
            Coord3d coord2 = Coord3d.GlobalCS.Copy();
            coord2.RotateDeg(new Vector3d(1, 1, 1), 120);

            Assert.IsTrue(new Vector3d(1, 2, 3, coord1) == new Vector3d(2, 3, 1, coord2));
        }

        [TestMethod()]
        public void VectorConvertToTest2()
        {
            Coord3d coord1 = Coord3d.GlobalCS.Copy();
            Coord3d coord2 = Coord3d.GlobalCS.Copy();
            coord2.RotateDeg(new Vector3d(1, 1, 1), 120);

            Vector3d v1 = new Vector3d(2, 3, 4);
            Vector3d v2 = v1.ConvertTo(coord1);
            Vector3d v3 = v1.ConvertTo(coord2);

            Assert.IsTrue(v2 == v3);
        }

        [TestMethod()]
        public void LineConvertToTest()
        {
            Coord3d coord1 = Coord3d.GlobalCS.Copy();
            Coord3d coord2 = Coord3d.GlobalCS.Copy();
            coord2.RotateDeg(new Vector3d(1, 1, 1), 120);

            Point3d p1 = new Point3d(1, 2, 3, coord1);
            Vector3d v1 = new Vector3d(0, 0, 1);
            Line3d l1 = new Line3d(p1, v1);
            l1.Point = l1.Point.ConvertTo(coord2);
            Plane3d s1 = coord2.XZ_plane;
            s1.Point = s1.Point.ConvertTo(coord1);

            Assert.IsTrue((Point3d)l1.IntersectionWith(s1) == new Point3d(1, 2, 0));
        }

        [TestMethod()]
        public void PlaneConvertToTest()
        {
            Coord3d coord1 = Coord3d.GlobalCS.Copy();
            Coord3d coord2 = Coord3d.GlobalCS.Copy();
            coord2.RotateDeg(new Vector3d(1, 1, 1), 120);

            Point3d p1 = new Point3d(1, 2, 3, coord1);
            Vector3d v1 = new Vector3d(0, 0, 1);
            Line3d l1 = new Line3d(p1, v1);
            l1.Point = l1.Point.ConvertTo(coord2);
            Plane3d s1 = coord2.XZ_plane;
            s1.Point = s1.Point.ConvertTo(coord1);

            Assert.IsTrue((Point3d)s1.IntersectionWith(l1) == new Point3d(1, 2, 0));
        }

        [TestMethod()]
        public void LineConvertProjectionToPlaneTest()
        {
            Coord3d coord1 = Coord3d.GlobalCS.Copy();
            Coord3d coord2 = Coord3d.GlobalCS.Copy();
            coord2.RotateDeg(new Vector3d(1, 1, 1), 120);

            Point3d p1 = new Point3d(1, 2, 3, coord1);
            Vector3d v1 = new Vector3d(0, 0, 1);
            Line3d l1 = new Line3d(p1, v1);
            l1.Point = l1.Point.ConvertTo(coord2);
            Plane3d s1 = coord2.XZ_plane;
            s1.Point = s1.Point.ConvertTo(coord1);

            Assert.IsTrue((Point3d)l1.ProjectionTo(s1) == new Point3d(1, 2, 0));
        }

        [TestMethod()]
        public void LineConvertProjectionToPlaneTest2()
        {
            Coord3d coord1 = Coord3d.GlobalCS.Copy();
            Coord3d coord2 = Coord3d.GlobalCS.Copy();
            coord2.RotateDeg(new Vector3d(1, 1, 1), 120);

            Point3d p1 = new Point3d(1, 2, 3, coord1);
            Vector3d v1 = new Vector3d(0, 2, 1);
            Line3d l1 = new Line3d(p1, v1);
            l1.Point = l1.Point.ConvertTo(coord2);
            Plane3d s1 = coord2.XZ_plane;
            s1.Point = s1.Point.ConvertTo(coord1);

            Assert.IsTrue((Line3d)l1.ProjectionTo(s1) == new Line3d(new Point3d(1, 2, 0), new Vector3d(0, 1, 0)));
        }

        [TestMethod()]
        public void PlaneConvertIntersectionToPlaneTest()
        {
            Coord3d coord1 = Coord3d.GlobalCS.Copy();
            Coord3d coord2 = Coord3d.GlobalCS.Copy();
            coord1.RotateDeg(new Vector3d(1, 1, 1), 90);
            coord2.RotateDeg(new Vector3d(1, 1, 1), 120);


            Plane3d s1 = new Plane3d(1, 1, 1, 0, Coord3d.GlobalCS);
            Plane3d s2 = new Plane3d(1, 3, 6, 0, coord1);
            Plane3d s3 = new Plane3d(100, -1000, 1, 0, coord2);


            Assert.IsTrue((Point3d)s1.IntersectionWith(s2, s3) == new Point3d(0, 0, 0));
        }

        [TestMethod()]
        public void PlaneConvertToGlobalTest()
        {
            Coord3d coord1 = Coord3d.GlobalCS.Copy();
            Coord3d coord2 = Coord3d.GlobalCS.Copy();
            coord1.RotateDeg(new Vector3d(1, 1, 1), 90);
            coord2.RotateDeg(new Vector3d(1, 1, 1), 120);
            coord1.Origin = new Point3d(1, 1, 1);
            coord2.Origin = new Point3d(10, 2, 5);

            Plane3d s1 = new Plane3d(1, 1, 1, 0, Coord3d.GlobalCS);
            s1.Point = s1.Point.ConvertTo(coord1);
            s1.Point = s1.Point.ConvertTo(coord2);
            s1.Point = s1.Point.ConvertToGlobal();

            Assert.IsTrue(s1 == new Plane3d(1, 1, 1, 0, Coord3d.GlobalCS));
        }

        [TestMethod()]
        public void PlaneIntersectionWithPlanesTest()
        {
            Coord3d coord1 = Coord3d.GlobalCS.Copy();
            Coord3d coord2 = Coord3d.GlobalCS.Copy();
            coord1.RotateDeg(new Vector3d(1, 2, 3), 90);
            coord2.RotateDeg(new Vector3d(1, 1, 1), 120);
            coord1.Origin = new Point3d(1, 1, 1);
            coord2.Origin = new Point3d(10, 2, 5);

            Plane3d s1 = new Plane3d(1, 1, 1, 0, Coord3d.GlobalCS);
            Plane3d s2 = new Plane3d(3, -2, 0, 9, Coord3d.GlobalCS);
            Plane3d s3 = new Plane3d(2, 5, 1, -2, Coord3d.GlobalCS);
            Point3d p1 = (Point3d)s1.IntersectionWith(s2, s3);
            s1.Point = s1.Point.ConvertTo(coord2);
            s2.Normal = s2.Normal.ConvertTo(coord1).ConvertTo(coord2);
            s3.Point = s3.Point.ConvertTo(coord2).ConvertTo(coord1);
            Point3d p2 = (Point3d)s2.IntersectionWith(s1, s3);

            Assert.IsTrue(p1 == p2);
        }

    }
}
