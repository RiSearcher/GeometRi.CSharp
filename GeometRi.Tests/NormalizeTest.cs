using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

namespace GeometRi_Tests
{
    [TestClass]
    public class TestHighNumberTolerances
    {
        [TestMethod]
        public void testTriangleIntersection()
        {

            GeometRi3D.UseAbsoluteTolerance = false;
            Point3d a = new Point3d(18508, 7020, 3193);
            Point3d b = new Point3d(-925, -162, -233);
            Point3d c = new Point3d(-858, -349, -221);

            Triangle t = new Triangle(a, b, c);

            Plane3d plane = new Plane3d(new Point3d(0, 200, 0), new Vector3d(0, 1, 0));

            Segment3d s0 = new Segment3d(a, b);
            Segment3d s1 = new Segment3d(a, c);


            object result = t.IntersectionWith(plane); //while we expect a segment3d, it can be null, or point3d like here

            object inter1 = s0.IntersectionWith(plane); //returns Point3d
            object inter2 = s1.IntersectionWith(plane);//returns another distinct Point3d 
            Segment3d expectedResult = new Segment3d((Point3d)inter1, (Point3d)inter2); //segment3d with a >0 length

            Assert.IsTrue(expectedResult.Equals(result)); //Error
        }

        [TestMethod]
        public void testCoplanar()
        {
            Point3d a = new Point3d(-1000, 180, -70);
            Point3d b = new Point3d(20000, 180, -70);
            Point3d c = new Point3d(20000, 180, 70);
            Point3d d = new Point3d(-1000, 180, 70);

            Triangle t0 = new Triangle(a, b, c);
            Triangle t1 = new Triangle(b, c, d);

            Rotation rz = new Rotation(Coord3d.GlobalCS.Zaxis, 30 * System.Math.PI / 180);
            Rotation ry = new Rotation(Coord3d.GlobalCS.Yaxis, 10 * System.Math.PI / 180);

            Point3d p = new Point3d(0, 0, 0);

            t0 = t0.Rotate(rz, p);
            t1 = t1.Rotate(rz, p);
            t0 = t0.Rotate(ry, p);
            t1 = t1.Rotate(ry, p);

            bool areEqual = t0.ToPlane.Equals(t1.ToPlane); //returns false (expected true)
            bool areCoplanar = t0.IsCoplanarTo(t1);//returns false (expected true)
            Assert.IsTrue(areEqual); //Error
            Assert.IsTrue(areCoplanar); //Error

            //bool areEqual_fix = Equals_fix(t0.ToPlane,t1.ToPlane); //returns true
            //bool areCoplanar_fix = IsCoplanarTo_fix(t0,t1); //returns true
        }

        [TestMethod]
        public void SimpleAngleTest()
        {
            Point3d p0 = new Point3d(0, 0, 0, Coord3d.GlobalCS);
            Point3d p1 = new Point3d(1, 1, 0, Coord3d.GlobalCS);

            Segment3d s0 = new Segment3d(p0, p1);

            double angle = s0.AngleToDeg(Coord3d.GlobalCS.Zaxis);
            Assert.AreEqual(90, angle);

            angle = s0.AngleToDeg(Coord3d.GlobalCS.Yaxis);
            Assert.IsTrue(Abs(45 - angle) < 1e-14); //error, angle is 0, expected 45
        }

        //public static bool Equals_fix(this Plane3d p0, Plane3d p1)
        //{
        //    bool isCoplanar;
        //    if (p1.Normal.IsParallelTo(p0.Normal))
        //    {
        //        if (p1.Point.Equals(p0.Point))
        //            isCoplanar = true;
        //        else
        //        {
        //            var v = new Vector3d(p1.Point, p0.Point).Normalized;
        //            double a = v.Dot(p0.Normal.Normalized);
        //            isCoplanar = a <= GeometRi3D.DefaultTolerance;
        //        }
        //    }
        //    else
        //        isCoplanar = false;
        //    return isCoplanar;
        //}

        //public static bool IsCoplanarTo_fix(this IPlanarObject p1, IPlanarObject p2)
        //{
        //    return Equals_fix(p1.ToPlane, p2.ToPlane);
        //}
    }
}
