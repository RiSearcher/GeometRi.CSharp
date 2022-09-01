using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

namespace GeometRi_Tests
{
    [TestClass]
    public class TetrahedronTest
    {
        //===============================================================
        // Tetrahedron tests
        //===============================================================

        [TestMethod()]
        public void TetrahedronBasicTest()
        {
            Tetrahedron t = new Tetrahedron();

            Assert.IsTrue(Abs(t.Volume - 1.0 / 3.0) < GeometRi3D.Tolerance);
            Assert.IsTrue(Abs(t.Area - Sqrt(3) * 2) < GeometRi3D.Tolerance);
        }

        [TestMethod()]
        public void TetrahedronPointLocationTest()
        {
            Tetrahedron t = new Tetrahedron();

            Point3d p = new Point3d(0.5, 0.5, 0.5);  // Point inside
            Assert.IsTrue(p.BelongsTo(t));
            Assert.IsTrue(p.IsInside(t));
            Assert.IsFalse(p.IsOutside(t));
            Assert.IsFalse(p.IsOnBoundary(t));

            p = new Point3d(0.0, 0.0, 0.0);  // Point on vertex
            Assert.IsTrue(p.BelongsTo(t));
            Assert.IsFalse(p.IsInside(t));
            Assert.IsFalse(p.IsOutside(t));
            Assert.IsTrue(p.IsOnBoundary(t));

            p = new Point3d(0.5, 0.0, 0.5);  // Point on edge
            Assert.IsTrue(p.BelongsTo(t));
            Assert.IsFalse(p.IsInside(t));
            Assert.IsFalse(p.IsOutside(t));
            Assert.IsTrue(p.IsOnBoundary(t));

            p = new Point3d(0.5, 0.25, 0.75);  // Point on face
            Assert.IsTrue(p.BelongsTo(t));
            Assert.IsFalse(p.IsInside(t));
            Assert.IsFalse(p.IsOutside(t));
            Assert.IsTrue(p.IsOnBoundary(t));

            p = new Point3d(1, 1, 1);  // Point outside
            Assert.IsFalse(p.BelongsTo(t));
            Assert.IsFalse(p.IsInside(t));
            Assert.IsTrue(p.IsOutside(t));
            Assert.IsFalse(p.IsOnBoundary(t));

        }

        [TestMethod()]
        public void TetrahedronClosestPointTest()
        {
            Tetrahedron t = new Tetrahedron();

            Point3d p = new Point3d(0.5, 0.5, 0.5);  // Point inside
            Point3d closest_point = t.ClosestPoint(p);
            Assert.AreEqual(p, closest_point);

            p = new Point3d(0.0, 0.0, 0.0);  // Point on vertex
            closest_point = t.ClosestPoint(p);
            Assert.AreEqual(p, closest_point);

            p = new Point3d(0.5, 0.0, 0.5);  // Point on edge
            closest_point = t.ClosestPoint(p);
            Assert.AreEqual(p, closest_point);

            p = new Point3d(0.5, 0.25, 0.75);  // Point on face
            closest_point = t.ClosestPoint(p);
            Assert.AreEqual(p, closest_point);

            p = new Point3d(-1, -1, -1);  // Point outside
            closest_point = t.ClosestPoint(p);
            Assert.AreEqual(new Point3d(0,0,0), closest_point);

            p = new Point3d(0.5, -1, 0.5);  // Point outside
            closest_point = t.ClosestPoint(p);
            Assert.AreEqual(new Point3d(0.5, 0, 0.5), closest_point);
        }

        [TestMethod()]
        public void TetrahedronScaleTest()
        {
            Tetrahedron t = new Tetrahedron();
            Tetrahedron s = t.Scale(0.5);

            Assert.IsTrue(t.Center == s.Center);
            Assert.IsTrue(Abs(t.Volume * 0.5 * 0.5 * 0.5 - s.Volume) < GeometRi3D.Tolerance);
        }

        [TestMethod()]
        public void TetrahedronIntersectsTetrahedronTest_01()
        {
            Tetrahedron t = new Tetrahedron();
            // tetrahedron inside
            Tetrahedron s = t.Scale(0.5);
            Assert.IsTrue(t.Intersects(s));
            Assert.IsTrue(s.Intersects(t));
        }

        [TestMethod()]
        public void TetrahedronIntersectsTetrahedronTest_02()
        {
            Tetrahedron t = new Tetrahedron();
            // tetrahedron outside
            Tetrahedron s = t.Scale(1.5);
            Assert.IsTrue(t.Intersects(s));
            Assert.IsTrue(s.Intersects(t));
        }

        [TestMethod()]
        public void TetrahedronIntersectsTetrahedronTest_03()
        {
            Tetrahedron t = new Tetrahedron();
            // vertex-to-vertex contact
            Tetrahedron s = t.ReflectIn(new Point3d(0, 0, 0));
            Assert.IsTrue(t.Intersects(s));
            Assert.IsTrue(s.Intersects(t));
        }

        [TestMethod()]
        public void TetrahedronIntersectsTetrahedronTest_04()
        {
            Tetrahedron t = new Tetrahedron();
            // vertex-to-edge contact
            Tetrahedron s = t.Translate(new Vector3d(-1, 0.5, -0.5));
            Assert.IsTrue(t.Intersects(s));
            Assert.IsTrue(s.Intersects(t));
        }

        [TestMethod()]
        public void TetrahedronIntersectsTetrahedronTest_05()
        {
            Tetrahedron t = new Tetrahedron();
            // edge-to-edge contact
            Tetrahedron s = t.Translate(new Vector3d(-1, 0, 0));
            Assert.IsTrue(t.Intersects(s));
            Assert.IsTrue(s.Intersects(t));
        }

        [TestMethod()]
        public void TetrahedronIntersectsTetrahedronTest_06()
        {
            Tetrahedron t = new Tetrahedron();
            // face-to-face contact
            Plane3d p = new Plane3d(new Point3d(1, 0, 1), new Vector3d(1, 1, 1));
            Tetrahedron s = t.ReflectIn(p);
            Assert.IsTrue(t.Intersects(s));
            Assert.IsTrue(s.Intersects(t));
        }

        [TestMethod()]
        public void TetrahedronIntersectsTetrahedronTest_07()
        {
            Tetrahedron t = new Tetrahedron();
            // face-to-face partial contact
            Plane3d p = new Plane3d(new Point3d(1, 0, 1), new Vector3d(1, 1, 1));
            Tetrahedron s = t.ReflectIn(p);
            Rotation r = new Rotation(new Vector3d(1, 1, 1), Math.PI / 6);
            s.Rotate(r, s.Center);
            Assert.IsTrue(t.Intersects(s));
            Assert.IsTrue(s.Intersects(t));
        }

        [TestMethod()]
        public void TetrahedronIntersectsTetrahedronTest_08()
        {
            Tetrahedron t = new Tetrahedron();
            // vertex-to-face contact
            Plane3d p = new Plane3d(new Point3d(1, 0, 1), new Vector3d(1, 1, 1));
            Point3d projection = new Point3d(0, 0, 0).ProjectionTo(p);
            Vector3d v = new Vector3d(new Point3d(0, 0, 0), projection);
            Tetrahedron s = t.Translate(v);
            Assert.IsTrue(t.Intersects(s));
            Assert.IsTrue(s.Intersects(t));
        }

        [TestMethod()]
        public void TetrahedronIntersectsTetrahedronTest_09()
        {
            Tetrahedron t = new Tetrahedron();
            // one vertex inside
            Plane3d p = new Plane3d(new Point3d(1, 0, 1), new Vector3d(1, 1, 1));
            Point3d projection = new Point3d(0, 0, 0).ProjectionTo(p);
            Vector3d v = new Vector3d(new Point3d(0, 0, 0), projection);
            Tetrahedron s = t.Translate(0.9 * v);
            Assert.IsTrue(t.Intersects(s));
            Assert.IsTrue(s.Intersects(t));
        }

        [TestMethod()]
        public void TetrahedronIntersectsTetrahedronTest_10()
        {
            Tetrahedron t = new Tetrahedron();
            // not in contact
            Plane3d p = new Plane3d(new Point3d(1, 0, 1), new Vector3d(1, 1, 1));
            Point3d projection = new Point3d(0, 0, 0).ProjectionTo(p);
            Vector3d v = new Vector3d(new Point3d(0, 0, 0), projection);
            Tetrahedron s = t.Translate(1.1 * v);
            Assert.IsFalse(t.Intersects(s));
            Assert.IsFalse(s.Intersects(t));
        }

        [TestMethod()]
        public void TetrahedronDistanceToTetrahedronTest_01()
        {
            Tetrahedron t = new Tetrahedron();
            // tetrahedron inside
            Tetrahedron s = t.Scale(0.5);
            Assert.IsTrue(t.DistanceTo(s) == 0);
            Assert.IsTrue(s.DistanceTo(t) == 0);
        }

        [TestMethod()]
        public void TetrahedronDistanceToTetrahedronTest_02()
        {
            Tetrahedron t = new Tetrahedron();
            // tetrahedron outside
            Tetrahedron s = t.Scale(1.5);
            Assert.IsTrue(t.DistanceTo(s) == 0);
            Assert.IsTrue(s.DistanceTo(t) == 0);
        }

        [TestMethod()]
        public void TetrahedronDistanceToTetrahedronTest_03()
        {
            Tetrahedron t = new Tetrahedron();
            // vertex-to-vertex contact
            Tetrahedron s = t.ReflectIn(new Point3d(0, 0, 0));
            Assert.IsTrue(t.DistanceTo(s) == 0);
            Assert.IsTrue(s.DistanceTo(t) == 0);
        }

        [TestMethod()]
        public void TetrahedronDistanceToTetrahedronTest_04()
        {
            Tetrahedron t = new Tetrahedron();
            // vertex-to-edge contact
            Tetrahedron s = t.Translate(new Vector3d(-1, 0.5, -0.5));
            Assert.IsTrue(t.DistanceTo(s) == 0);
            Assert.IsTrue(s.DistanceTo(t) == 0);
        }

        [TestMethod()]
        public void TetrahedronDistanceToTetrahedronTest_05()
        {
            Tetrahedron t = new Tetrahedron();
            // edge-to-edge contact
            Tetrahedron s = t.Translate(new Vector3d(-1, 0, 0));
            Assert.IsTrue(t.DistanceTo(s) == 0);
            Assert.IsTrue(s.DistanceTo(t) == 0);
        }

        [TestMethod()]
        public void TetrahedronDistanceToTetrahedronTest_06()
        {
            Tetrahedron t = new Tetrahedron();
            // face-to-face contact
            Plane3d p = new Plane3d(new Point3d(1, 0, 1), new Vector3d(1, 1, 1));
            Tetrahedron s = t.ReflectIn(p);
            Assert.IsTrue(t.DistanceTo(s) == 0);
            Assert.IsTrue(s.DistanceTo(t) == 0);
        }

        [TestMethod()]
        public void TetrahedronDistanceToTetrahedronTest_07()
        {
            Tetrahedron t = new Tetrahedron();
            // face-to-face partial contact
            Plane3d p = new Plane3d(new Point3d(1, 0, 1), new Vector3d(1, 1, 1));
            Tetrahedron s = t.ReflectIn(p);
            Rotation r = new Rotation(new Vector3d(1, 1, 1), Math.PI / 6);
            s.Rotate(r, s.Center);
            Assert.IsTrue(t.DistanceTo(s) == 0);
            Assert.IsTrue(s.DistanceTo(t) == 0);
        }

        [TestMethod()]
        public void TetrahedronDistanceToTetrahedronTest_08()
        {
            Tetrahedron t = new Tetrahedron();
            // vertex-to-face contact
            Plane3d p = new Plane3d(new Point3d(1, 0, 1), new Vector3d(1, 1, 1));
            Point3d projection = new Point3d(0, 0, 0).ProjectionTo(p);
            Vector3d v = new Vector3d(new Point3d(0, 0, 0), projection);
            Tetrahedron s = t.Translate(v);
            Assert.IsTrue(t.DistanceTo(s) == 0);
            Assert.IsTrue(s.DistanceTo(t) == 0);
        }

        [TestMethod()]
        public void TetrahedronDistanceToTetrahedronTest_09()
        {
            Tetrahedron t = new Tetrahedron();
            // one vertex inside
            Plane3d p = new Plane3d(new Point3d(1, 0, 1), new Vector3d(1, 1, 1));
            Point3d projection = new Point3d(0, 0, 0).ProjectionTo(p);
            Vector3d v = new Vector3d(new Point3d(0, 0, 0), projection);
            Tetrahedron s = t.Translate(0.9 * v);
            Assert.IsTrue(t.DistanceTo(s) == 0);
            Assert.IsTrue(s.DistanceTo(t) == 0);
        }

        [TestMethod()]
        public void TetrahedronDistanceToTetrahedronTest_10()
        {
            Tetrahedron t = new Tetrahedron();
            // not in contact
            Vector3d v = new Vector3d(2,0,0);
            Tetrahedron s = t.Translate(v);
            Assert.IsTrue(Abs(t.DistanceTo(s) - 1) < GeometRi3D.Tolerance);
            Assert.IsTrue(Abs(s.DistanceTo(t) - 1) < GeometRi3D.Tolerance);
        }

        [TestMethod()]
        public void TetrahedronDistanceToTetrahedronTest_11()
        {
            Tetrahedron t = new Tetrahedron();
            // in contact
            Tetrahedron s = t.ReflectIn(t.Center);
            s = s.Scale(1.1);
            Assert.IsTrue(Abs(t.DistanceTo(s) - 0) < GeometRi3D.Tolerance);
            Assert.IsTrue(Abs(s.DistanceTo(t) - 0) < GeometRi3D.Tolerance);
        }

        [TestMethod()]
        public void TetrahedronAABBTest()
        {
            Tetrahedron t = new Tetrahedron();

            Box3d aabb = t.BoundingBox();

            Assert.IsTrue(aabb.Center == new Point3d(0.5, 0.5, 0.5));
            Assert.IsTrue(Abs(aabb.L1 - 1) < GeometRi3D.Tolerance);
            Assert.IsTrue(Abs(aabb.L2 - 1) < GeometRi3D.Tolerance);
            Assert.IsTrue(Abs(aabb.L3 - 1) < GeometRi3D.Tolerance);
        }

        [TestMethod()]
        public void TetrahedronAABBTest_01()
        {
            Tetrahedron t = new Tetrahedron().Translate(new Vector3d(-1,0,0));

            Box3d aabb = t.BoundingBox();

            Assert.IsTrue(aabb.Center == new Point3d(-0.5, 0.5, 0.5));
            Assert.IsTrue(Abs(aabb.L1 - 1) < GeometRi3D.Tolerance);
            Assert.IsTrue(Abs(aabb.L2 - 1) < GeometRi3D.Tolerance);
            Assert.IsTrue(Abs(aabb.L3 - 1) < GeometRi3D.Tolerance);
        }

    }
}
