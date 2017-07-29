using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

namespace GeometRi_Tests
{
    [TestClass]
    public class OtherTest
    {
        //===============================================================
        // Other tests
        //===============================================================

        [TestMethod()]
        public void ToleranceTest()
        {
            Plane3d s1 = new Plane3d(new Point3d(0, 0, 1), new Vector3d(0, 1, 1));
            Plane3d s2 = new Plane3d(-5, 2, 4, 1);
            Plane3d s3 = new Plane3d(2, -3, 2, 4);
            Assert.IsTrue((Point3d)s1.IntersectionWith(s2, s3) == (Point3d)s1.IntersectionWith((Line3d)s2.IntersectionWith(s3)));
            GeometRi3D.Tolerance = 0;
            Assert.IsFalse((Point3d)s1.IntersectionWith(s2, s3) == (Point3d)s1.IntersectionWith((Line3d)s2.IntersectionWith(s3)));
            GeometRi3D.Tolerance = 1E-12;
        }

        [TestMethod()]
        public void CoordSystemTest()
        {
            Coord3d c1 = new Coord3d(new Point3d(), new[] { 2.0, 0.0, 0.0 }, new[] { 1.0, 1.0, 0.0 });
            Assert.IsTrue(c1.Axes == Matrix3d.Identity());

            c1 = new Coord3d(new Point3d(), new Vector3d(2, 0, 0), new Vector3d(0, 0, 5));
            c1.RotateDeg(new Vector3d(1, 0, 0), -90);
            Assert.IsTrue(c1.Axes == Matrix3d.Identity());
        }

        [TestMethod()]
        public void IsParallelToTest()
        {
            Vector3d v = new Vector3d(1, 0, 0);
            Line3d l = new Line3d(new Point3d(), v);
            Ray3d r = new Ray3d(new Point3d(), v.OrthogonalVector);
            Circle3d c = new Circle3d(new Point3d(), 5, v);

            Assert.IsTrue(v.IsParallelTo(l));
            Assert.IsTrue(l.IsNotParallelTo(r));
            Assert.IsTrue(r.IsOrthogonalTo(v));
            Assert.IsTrue(c.IsOrthogonalTo(l));
            Assert.IsTrue(r.IsParallelTo(c));
            Assert.IsTrue(c.IsParallelTo(Coord3d.GlobalCS.YZ_plane));
            Assert.IsTrue(c.IsOrthogonalTo(Coord3d.GlobalCS.XZ_plane));
            Assert.IsTrue(Coord3d.GlobalCS.YZ_plane.IsOrthogonalTo(v));
        }

        [TestMethod()]
        public void AngleToTest()
        {
            // Larger tolerance value is needed for these tests
            double def_tol = GeometRi3D.Tolerance;
            GeometRi3D.Tolerance = 1e-7;

            Vector3d v = new Vector3d(-1, 1, 0);
            Line3d l = new Line3d(new Point3d(), -v);
            Ray3d r = new Ray3d(new Point3d(), v.OrthogonalVector);
            Circle3d c = new Circle3d(new Point3d(), 5, v);

            Assert.IsTrue(Abs(l.AngleTo(v)) <= GeometRi3D.Tolerance);
            Assert.IsTrue(Abs(r.AngleTo(l) - PI / 2) <= GeometRi3D.Tolerance);
            Assert.IsTrue(Abs(c.AngleTo(v) - PI / 2) <= GeometRi3D.Tolerance);

            GeometRi3D.Tolerance = def_tol;
        }
    }
}
