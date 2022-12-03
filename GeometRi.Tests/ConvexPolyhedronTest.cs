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
        public void ConvexPolyhedronIntersectionCheckTest_01()
        {
            Box3d box = new Box3d(new Point3d(0.5, 0.5, 0.5), 1, 1, 1);

            Point3d A = new Point3d(0.62055, 1.2952, 0.89845);
            Point3d B = new Point3d(0.47681, 1.1381, 0.91592);
            Point3d C = new Point3d(0.60429, 0.96027, 1.0405);
            Point3d D = new Point3d(0.54816, 1.256, 1.2097);
            Tetrahedron t = new Tetrahedron(A, B, C, D);

            ConvexPolyhedron cp_box = ConvexPolyhedron.FromBox(box);
            ConvexPolyhedron cp_t = ConvexPolyhedron.FromTetrahedron(t);

            Assert.IsFalse(cp_t.Intersects(cp_box));
        }

        [TestMethod]
        public void ConvexPolyhedronIntersectionCheckTest_02()
        {
            Point3d A = new Point3d(0.75189, 0.047671, 0.64089);
            Point3d B = new Point3d(0.69441, 0.26369, 0.8156);
            Point3d C = new Point3d(1.0126, 0.26195, 0.71716);
            Point3d D = new Point3d(0.73275, 0.41057, 0.61468);
            Tetrahedron t1 = new Tetrahedron(A, B, C, D);

            A = new Point3d(0.91294, 0.095756, 0.74814);
            B = new Point3d(0.95606, 0.28099, 1.0639);
            C = new Point3d(1.2819, 0.22197, 0.84984);
            D = new Point3d(0.9913, 0.4657, 0.74708);
            Tetrahedron t2 = new Tetrahedron(A, B, C, D);

            ConvexPolyhedron cp_t1 = ConvexPolyhedron.FromTetrahedron(t1);
            ConvexPolyhedron cp_t2 = ConvexPolyhedron.FromTetrahedron(t2);

            Assert.IsFalse(cp_t1.Intersects(cp_t2));
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
