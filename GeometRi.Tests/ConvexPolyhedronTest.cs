using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

namespace GeometRi_Tests
{
    [TestClass]
    public class ConvexPolyhedronTest
    {
        [TestMethod]
        public void AreaVolumeTest()
        {
            int count = 200;
            for (int i = 0; i < count; i++)
            {
                Tetrahedron t1 = Tetrahedron.Random();
                ConvexPolyhedron c1 = ConvexPolyhedron.FromTetrahedron(t1);
                Assert.AreEqual(t1.Area, c1.Area);
                Assert.IsTrue(Abs(t1.Volume - c1.Volume) < GeometRi3D.Tolerance);
            }
        }

        [TestMethod]
        public void TetrahedronIntersectionCheckTest()
        {
            int count = 200;

            // Likely intersecting tetrahedrones
            for (int i=0; i < count; i++)
            {
                Tetrahedron t1 = Tetrahedron.Random();
                Tetrahedron t2 = Tetrahedron.Random();
                // Exclude close to degenerate tetrahednones
                if (t1.Volume > 0.1 && t2.Volume > 0.1)
                {
                    ConvexPolyhedron c1 = ConvexPolyhedron.FromTetrahedron(t1);
                    ConvexPolyhedron c2 = ConvexPolyhedron.FromTetrahedron(t2);
                    Assert.AreEqual(t1.Intersects(t2), c1.Intersects(c2));
                }
            }

            // Close non-intersecting tetrahedrones
            for (int i = 0; i < count; i++)
            {
                Tetrahedron t1 = Tetrahedron.Random();
                Tetrahedron t2 = Tetrahedron.Random().Translate(new Vector3d(1, 0, 0));
                // Exclude close to degenerate tetrahednones
                if (t1.Volume > 0.05 && t2.Volume > 0.05)
                {
                    ConvexPolyhedron c1 = ConvexPolyhedron.FromTetrahedron(t1);
                    ConvexPolyhedron c2 = ConvexPolyhedron.FromTetrahedron(t2);
                    Assert.AreEqual(t1.Intersects(t2), c1.Intersects(c2));
                }
            }

            // Far non-intersecting tetrahedrones
            for (int i = 0; i < count; i++)
            {
                Tetrahedron t1 = Tetrahedron.Random();
                Tetrahedron t2 = Tetrahedron.Random().Translate(new Vector3d(100, 0, 0));
                // Exclude close to degenerate tetrahednones
                if (t1.Volume > 0.05 && t2.Volume > 0.05)
                {
                    ConvexPolyhedron c1 = ConvexPolyhedron.FromTetrahedron(t1);
                    ConvexPolyhedron c2 = ConvexPolyhedron.FromTetrahedron(t2);
                    Assert.AreEqual(t1.Intersects(t2), c1.Intersects(c2));
                }
            }

        }

        [TestMethod]
        public void TetrahedronDistanceTest()
        {
            int count = 200;

            // Likely intersecting tetrahedrones
            for (int i = 0; i < count; i++)
            {
                Tetrahedron t1 = Tetrahedron.Random();
                Tetrahedron t2 = Tetrahedron.Random();
                // Exclude close to degenerate tetrahednones
                if (t1.Volume > 0.1 && t2.Volume > 0.1)
                {
                    ConvexPolyhedron c1 = ConvexPolyhedron.FromTetrahedron(t1);
                    ConvexPolyhedron c2 = ConvexPolyhedron.FromTetrahedron(t2);
                    double dist1 = t1.DistanceTo(t2);
                    double dist2 = c1.DistanceTo(c2);
                    Assert.IsTrue(Abs(dist1-dist2) < 1e-10);
                }
            }

            // Close non-intersecting tetrahedrones
            for (int i = 0; i < count; i++)
            {
                Tetrahedron t1 = Tetrahedron.Random();
                Tetrahedron t2 = Tetrahedron.Random().Translate(new Vector3d(1, 0, 0));
                // Exclude close to degenerate tetrahednones
                if (t1.Volume > 0.05 && t2.Volume > 0.05)
                {
                    ConvexPolyhedron c1 = ConvexPolyhedron.FromTetrahedron(t1);
                    ConvexPolyhedron c2 = ConvexPolyhedron.FromTetrahedron(t2);
                    double dist1 = t1.DistanceTo(t2);
                    double dist2 = c1.DistanceTo(c2);
                    Assert.IsTrue(Abs(dist1 - dist2) < 1e-10);
                }
            }

            // Far non-intersecting tetrahedrones
            for (int i = 0; i < count; i++)
            {
                Tetrahedron t1 = Tetrahedron.Random();
                Tetrahedron t2 = Tetrahedron.Random().Translate(new Vector3d(10, 0, 0));
                // Exclude close to degenerate tetrahednones
                if (t1.Volume > 0.05 && t2.Volume > 0.05)
                {
                    ConvexPolyhedron c1 = ConvexPolyhedron.FromTetrahedron(t1);
                    ConvexPolyhedron c2 = ConvexPolyhedron.FromTetrahedron(t2);
                    double dist1 = t1.DistanceTo(t2);
                    double dist2 = c1.DistanceTo(c2);
                    Assert.IsTrue(Abs(dist1 - dist2) < 1e-10);
                }
            }

        }

        [TestMethod]
        public void BoxIntersectionTest()
        {
            Box3d b = new Box3d();
            Tetrahedron t = new Tetrahedron();

            ConvexPolyhedron cb = ConvexPolyhedron.FromBox(b);
            ConvexPolyhedron ct = ConvexPolyhedron.FromTetrahedron(t);
            Assert.IsTrue(ct.Intersects(cb));

            cb = cb.Translate(new Vector3d(1, 0, 0));
            Assert.IsTrue(ct.Intersects(cb));

            cb = cb.Translate(new Vector3d(1, 0, 0));
            Assert.IsFalse(ct.Intersects(cb));
        }

        [TestMethod]
        public void CopyTest()
        {
            Tetrahedron t = new Tetrahedron();
            ConvexPolyhedron ct = ConvexPolyhedron.FromTetrahedron(t);
            ConvexPolyhedron copy = ct.Copy();
            Assert.AreEqual(ct.Volume, copy.Volume);
        }
    }
}
