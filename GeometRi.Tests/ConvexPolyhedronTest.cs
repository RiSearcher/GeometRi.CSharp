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
    }
}
