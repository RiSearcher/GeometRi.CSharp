using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

namespace GeometRi_Tests
{
    [TestClass]
    public class CoplanarityTest
    {
        [TestMethod]
        public void PlanarObjectsCoplanarityTest()
        {
            Plane3d s1 = new Plane3d(new Point3d(1, 1, 1), new Vector3d(1, 0, 0));
            Plane3d s2 = new Plane3d(new Point3d(-1, 1, 1), new Vector3d(1, 0, 0));
            Plane3d s3 = new Plane3d(new Point3d(1, 2, 3), new Vector3d(-1, 0, 0));
            Plane3d s4 = new Plane3d(new Point3d(1, 1, 1), new Vector3d(-1, 2, 3));

            Assert.IsTrue(s1.IsCoplanarTo(s3));
            Assert.IsTrue(s1.IsCoplanarTo(s1));
            Assert.IsFalse(s1.IsCoplanarTo(s2));
            Assert.IsFalse(s1.IsCoplanarTo(s4));

            Circle3d c = new Circle3d(new Point3d(1, 2, 2), 5, new Vector3d(1, 0, 0));
            Assert.IsTrue(c.IsCoplanarTo(c));
            Assert.IsTrue(s1.IsCoplanarTo(c));
            Assert.IsFalse(s2.IsCoplanarTo(c));
            Assert.IsTrue(c.IsCoplanarTo(s1));
            Assert.IsFalse(c.IsCoplanarTo(s2));

            Triangle t = new Triangle(new Point3d(1, 2, 2), new Point3d(1, -5, 0), new Point3d(1, 3, -2));
            Assert.IsTrue(t.IsCoplanarTo(t));
            Assert.IsTrue(s1.IsCoplanarTo(t));
            Assert.IsFalse(s2.IsCoplanarTo(t));
            Assert.IsTrue(t.IsCoplanarTo(s1));
            Assert.IsFalse(t.IsCoplanarTo(s2));
            Assert.IsTrue(t.IsCoplanarTo(c));
        }

        [TestMethod]
        public void LinearObjectsCoplanarityTest()
        {
            Line3d l1 = new Line3d(new Point3d(1, 1, 1), new Vector3d(0, 1, 1));
            Line3d l2 = new Line3d(new Point3d(-1, 2, 1), new Vector3d(0, 1, 1));
            Line3d l3 = new Line3d(new Point3d(1, 2, 3), new Vector3d(0, 2, 1));
            Line3d l4 = new Line3d(new Point3d(1, 1, 1), new Vector3d(1, 2, 3));

            // Self-coplanarity
            Assert.IsTrue(l1.IsCoplanarTo(l1));
            // Parallel lines
            Assert.IsTrue(l1.IsCoplanarTo(l2));
            // Intersecting lines
            Assert.IsTrue(l1.IsCoplanarTo(l3));
            // Intersecting lines
            Assert.IsTrue(l1.IsCoplanarTo(l4));
            // Not coplanar
            Assert.IsFalse(l3.IsCoplanarTo(l4));

            Segment3d s1 = new Segment3d(new Point3d(1, 2, 4), new Point3d(1, -2, 5));
            Assert.IsTrue(s1.IsCoplanarTo(s1));
            Assert.IsTrue(s1.IsCoplanarTo(l1));
            Assert.IsFalse(s1.IsCoplanarTo(l4));
        }

        [TestMethod]
        public void PlanarLinearObjectsCoplanarityTest()
        {
            Plane3d p1 = new Plane3d(new Point3d(1, 1, 1), new Vector3d(1, 0, 0));
            Line3d l1 = new Line3d(new Point3d(1, 1, 1), new Vector3d(0, 1, 1));
            Segment3d s1 = new Segment3d(new Point3d(1, 2, 4), new Point3d(2, -2, 0));

            Assert.IsTrue(p1.IsCoplanarTo(l1));
            Assert.IsTrue(l1.IsCoplanarTo(p1));
            Assert.IsFalse(p1.IsCoplanarTo(s1));
            Assert.IsFalse(s1.IsCoplanarTo(p1));

        }
    }
}
