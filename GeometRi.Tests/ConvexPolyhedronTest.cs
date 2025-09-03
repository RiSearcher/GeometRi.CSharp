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
                Assert.IsTrue(Abs(t1.Area - c1.Area) < GeometRi3D.Tolerance);
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
        public void ConvexPolyhedronIntersectionCheckTest_03()
        {
            Point3d A = new Point3d(0.051634, 0.391, 0.75902);
            Point3d B = new Point3d(0.056938, 0.50361, 0.89111);
            Point3d C = new Point3d(0.27042, 0.38715, 0.89053);
            Point3d D = new Point3d(0.27694, 0.50134, 0.76028);
            Tetrahedron t1 = new Tetrahedron(A, B, C, D);

            A = new Point3d(-0.074466, 0.46362, 1.1418);
            B = new Point3d(0.13639, 0.66447, 0.99967);
            C = new Point3d(0.06711, 0.44386, 0.77189);
            D = new Point3d(-0.19346, 0.62704, 0.84602);
            Tetrahedron t2 = new Tetrahedron(A, B, C, D);

            ConvexPolyhedron cp_t1 = ConvexPolyhedron.FromTetrahedron(t1);
            ConvexPolyhedron cp_t2 = ConvexPolyhedron.FromTetrahedron(t2);

            Assert.IsTrue(cp_t1.Intersects(cp_t2));
        }

        [TestMethod]
        public void ConvexPolyhedronIntersectionCheckTest_04()
        {
            Point3d A = new Point3d(0.251097013104677, 0.546703145796848, 0.495434613743138);
            Point3d B = new Point3d(0.291709439728301, 0.578882461419393, 0.650931851386847);
            Point3d C = new Point3d(0.128566597389787, 0.440248968316735, 0.605189321180178);
            Point3d D = new Point3d(0.350216056201755, 0.391985311884308, 0.582693942204766);
            Tetrahedron t1 = new Tetrahedron(A, B, C, D);

            A = new Point3d(0.121116388215459, 0.361753267278295, 0.898825185911191);
            B = new Point3d(-0.00140768874670161, 0.573928160961876, 0.666186244206548);
            C = new Point3d(0.175804665188155, 0.740808551308965, 0.844898958033819);
            D = new Point3d(0.222857195023536, 0.471956639099317, 0.598778318291477);
            Tetrahedron t2 = new Tetrahedron(A, B, C, D);

            ConvexPolyhedron cp_t1 = ConvexPolyhedron.FromTetrahedron(t1);
            ConvexPolyhedron cp_t2 = ConvexPolyhedron.FromTetrahedron(t2);

            Assert.IsTrue(cp_t1.Intersects(cp_t2));
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

        [TestMethod()]
        public void TetrahedronDistanceToTetrahedronTest_01()
        {
            Point3d p0 = new Point3d(1.18847041209228, 0.104193542059284, 0.35298291672622);
            Point3d p1 = new Point3d(0.649467768945706, 0.170331494522815, 0.72174959083238);
            Point3d p2 = new Point3d(1.15947687723363, 0.573922428456402, 0.810586551138034);
            Point3d p3 = new Point3d(0.793812982683988, 0.621719485201377, 0.267546685570428);
            Tetrahedron t1 = new Tetrahedron(p0, p1, p2, p3);

            Point3d s0 = new Point3d(0.216267039810085, 0.169909202890096, 0.859064204525464);
            Point3d s1 = new Point3d(0.205044294164905, 0.659002041871181, 0.42140088318818);
            Point3d s2 = new Point3d(0.66494259923399, 0.648073860241189, 0.889654085897164);
            Point3d s3 = new Point3d(0.684153131501953, 0.210778895931041, 0.400480284835266);
            Tetrahedron t2 = new Tetrahedron(s0, s1, s2, s3);

            ConvexPolyhedron c1 = ConvexPolyhedron.FromTetrahedron(t1);
            ConvexPolyhedron c2 = ConvexPolyhedron.FromTetrahedron(t2);

            double dist = c1.DistanceTo(c2);
            double dist2 = c2.DistanceTo(c1);

            Assert.IsTrue(dist > 0.02562);
            Assert.IsTrue(dist2 > 0.02562);

            bool check1 = c1.Intersects(c2);
            bool check2 = c2.Intersects(c1);
            Assert.IsFalse(c1.Intersects(c2));
            Assert.IsFalse(c2.Intersects(c1));
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
        public void OctahedronIntersectionCheckTest_03()
        {

            ConvexPolyhedron cp1 = ConvexPolyhedron.Octahedron();
            ConvexPolyhedron cp2 = ConvexPolyhedron.Octahedron();

            cp1 = cp1.Translate(new Vector3d(0.9, 0, 0));

            Assert.IsTrue(cp1.Intersects(cp2));
        }

        [TestMethod]
        public void IcosahedronIntersectionCheckTest_03()
        {

            ConvexPolyhedron cp1 = ConvexPolyhedron.Icosahedron();
            ConvexPolyhedron cp2 = ConvexPolyhedron.Icosahedron();

            cp1 = cp1.Translate(new Vector3d(1.5, 0, 0));

            Assert.IsTrue(cp1.Intersects(cp2));
        }

        [TestMethod]
        public void DodecaahedronIntersectionCheckTest_03()
        {

            ConvexPolyhedron cp1 = ConvexPolyhedron.Dodecahedron();
            ConvexPolyhedron cp2 = ConvexPolyhedron.Dodecahedron();

            cp2 = cp2.Translate(new Vector3d(1.5, 0, 0));
            Assert.IsTrue(cp1.Intersects(cp2));

            cp2 = cp2.Translate(new Vector3d(1.5, 0, 0));
            Assert.IsTrue(cp1.Intersects(cp2));
        }

        [TestMethod]
        public void CopyTest()
        {
            Tetrahedron t = new Tetrahedron();
            ConvexPolyhedron ct = ConvexPolyhedron.FromTetrahedron(t);
            ConvexPolyhedron copy = ct.Copy();
            Assert.AreEqual(ct.Volume, copy.Volume);
        }

        [TestMethod]
        public void BoxBoxDistanceTest_01()
        {
            Box3d b1 = new Box3d(new Point3d(0, 0, 0), 1, 1, 1);
            Box3d b2 = new Box3d(new Point3d(2, 0, 0), 1, 1, 1);
            ConvexPolyhedron cp1 = ConvexPolyhedron.FromBox(b1);
            ConvexPolyhedron cp2 = ConvexPolyhedron.FromBox(b2);
            Point3d p1, p2;
            double dist = cp1.DistanceTo(cp2, out p1, out p2);
            Assert.AreEqual(dist, 1);
        }

        [TestMethod]
        public void BoxBoxDistanceTest_02()
        {
            // Touching boxes
            Box3d b1 = new Box3d(new Point3d(0, 0, 0), 1, 1, 1);
            Box3d b2 = new Box3d(new Point3d(1, 0.5, 0.5), 1, 1, 1);
            ConvexPolyhedron cp1 = ConvexPolyhedron.FromBox(b1);
            ConvexPolyhedron cp2 = ConvexPolyhedron.FromBox(b2);
            Point3d p1, p2;
            double dist = cp1.DistanceTo(cp2, out p1, out p2);
            Assert.AreEqual(dist, 0);
            Assert.IsTrue(p1.BelongsTo(cp1));
            Assert.IsTrue(p2.BelongsTo(cp2));
        }

        [TestMethod]
        public void BoxBoxDistanceTest_03()
        {
            // Partially intersecting boxes
            Box3d b1 = new Box3d(new Point3d(0, 0, 0), 1, 1, 1);
            Box3d b2 = new Box3d(new Point3d(0.5, 0.5, 0.5), 1, 1, 1);
            ConvexPolyhedron cp1 = ConvexPolyhedron.FromBox(b1);
            ConvexPolyhedron cp2 = ConvexPolyhedron.FromBox(b2);
            Point3d p1, p2;
            double dist = cp1.DistanceTo(cp2, out p1, out p2);
            Assert.AreEqual(dist, 0);
            Assert.IsTrue(p1.BelongsTo(cp1));
            Assert.IsTrue(p2.BelongsTo(cp2));
        }

        [TestMethod]
        public void TetTetDistanceTest_01()
        {
            // Vertex-Vertex distance
            Tetrahedron t1 = new Tetrahedron(new Point3d(0, 0, 0), new Point3d(-1, -1, -1), new Point3d(-1, -1, 1), new Point3d(-1, 1, 1));
            Tetrahedron t2 = new Tetrahedron(new Point3d(0.5, 0, 0), new Point3d(1, -1, -1), new Point3d(1, -1, 1), new Point3d(1, 1, 1));
            ConvexPolyhedron cp1 = ConvexPolyhedron.FromTetrahedron(t1);
            ConvexPolyhedron cp2 = ConvexPolyhedron.FromTetrahedron(t2);
            Point3d p1 = new Point3d();
            Point3d p2 = new Point3d();
            double dist = cp1.DistanceTo(cp2, out p1, out p2);
            Assert.IsTrue(GeometRi3D.AlmostEqual(dist, 0.5));
            Assert.IsTrue(p1 == new Point3d(0, 0, 0));
            Assert.IsTrue(p2 == new Point3d(0.5, 0, 0));
        }

        [TestMethod]
        public void TetTetDistanceTest_02()
        {
            // Vertex-Edge distance
            Tetrahedron t1 = new Tetrahedron(new Point3d(0, 0, 0), new Point3d(-1, -1, -1), new Point3d(-1, -1, 1), new Point3d(-1, 1, 1));
            Tetrahedron t2 = new Tetrahedron(new Point3d(0.5, -1, 0), new Point3d(0.5, 1, 0), new Point3d(1, 0, -1), new Point3d(1, 0, 1));
            ConvexPolyhedron cp1 = ConvexPolyhedron.FromTetrahedron(t1);
            ConvexPolyhedron cp2 = ConvexPolyhedron.FromTetrahedron(t2);
            Point3d p1 = new Point3d();
            Point3d p2 = new Point3d();
            double dist = cp1.DistanceTo(cp2, out p1, out p2);
            Assert.IsTrue(GeometRi3D.AlmostEqual(dist, 0.5));
            Assert.IsTrue(p1 == new Point3d(0, 0, 0));
            Assert.IsTrue(p2 == new Point3d(0.5, 0, 0));
        }

        [TestMethod]
        public void TetTetDistanceTest_03()
        {
            // Vertex-Face distance
            Tetrahedron t1 = new Tetrahedron(new Point3d(0, 0, 0), new Point3d(-1, -1, -1), new Point3d(-1, -1, 1), new Point3d(-1, 1, 1));
            Tetrahedron t2 = new Tetrahedron(new Point3d(0.5, -1, -1), new Point3d(0.5, 1, -1), new Point3d(0.5, 0, 1), new Point3d(1, 0, 0));
            ConvexPolyhedron cp1 = ConvexPolyhedron.FromTetrahedron(t1);
            ConvexPolyhedron cp2 = ConvexPolyhedron.FromTetrahedron(t2);
            Point3d p1 = new Point3d();
            Point3d p2 = new Point3d();
            double dist = cp1.DistanceTo(cp2, out p1, out p2);
            Assert.IsTrue(GeometRi3D.AlmostEqual(dist, 0.5));
            Assert.IsTrue(p1 == new Point3d(0, 0, 0));
            Assert.IsTrue(p2 == new Point3d(0.5, 0, 0));
        }

        [TestMethod]
        public void TetTetDistanceTest_04()
        {
            // Edge-Edge distance
            Tetrahedron t1 = new Tetrahedron(new Point3d(0, 0, -1), new Point3d(0, 0, 1), new Point3d(-1, -1, 0), new Point3d(-1, 1, 0));
            Tetrahedron t2 = new Tetrahedron(new Point3d(0.5, -1, 0), new Point3d(0.5, 1, 0), new Point3d(1, 0, -1), new Point3d(1, 0, 1));
            ConvexPolyhedron cp1 = ConvexPolyhedron.FromTetrahedron(t1);
            ConvexPolyhedron cp2 = ConvexPolyhedron.FromTetrahedron(t2);
            Point3d p1 = new Point3d();
            Point3d p2 = new Point3d();
            double dist = cp1.DistanceTo(cp2, out p1, out p2);
            Assert.IsTrue(GeometRi3D.AlmostEqual(dist, 0.5));
            Assert.IsTrue(p1 == new Point3d(0, 0, 0));
            Assert.IsTrue(p2 == new Point3d(0.5, 0, 0));
        }

        [TestMethod]
        public void TetTetDistanceTest_05()
        {
            // Vertex-Vertex touching
            Tetrahedron t1 = new Tetrahedron(new Point3d(0, 0, 0), new Point3d(-1, -1, -1), new Point3d(-1, -1, 1), new Point3d(-1, 1, 1));
            Tetrahedron t2 = new Tetrahedron(new Point3d(0, 0, 0), new Point3d(1, -1, -1), new Point3d(1, -1, 1), new Point3d(1, 1, 1));
            ConvexPolyhedron cp1 = ConvexPolyhedron.FromTetrahedron(t1);
            ConvexPolyhedron cp2 = ConvexPolyhedron.FromTetrahedron(t2);
            double dist = cp1.DistanceTo(cp2);
            Assert.IsTrue(GeometRi3D.AlmostEqual(dist, 0));
        }

        [TestMethod]
        public void TetTetDistanceTest_06()
        {
            // Vertex-Edge touching
            Tetrahedron t1 = new Tetrahedron(new Point3d(0, 0, 0), new Point3d(-1, -1, -1), new Point3d(-1, -1, 1), new Point3d(-1, 1, 1));
            Tetrahedron t2 = new Tetrahedron(new Point3d(0, -1, 0), new Point3d(0, 1, 0), new Point3d(1, 0, -1), new Point3d(1, 0, 1));
            ConvexPolyhedron cp1 = ConvexPolyhedron.FromTetrahedron(t1);
            ConvexPolyhedron cp2 = ConvexPolyhedron.FromTetrahedron(t2);
            double dist = cp1.DistanceTo(cp2);
            Assert.IsTrue(GeometRi3D.AlmostEqual(dist, 0));
        }

        [TestMethod]
        public void TetTetDistanceTest_07()
        {
            // Vertex-Face touching
            Tetrahedron t1 = new Tetrahedron(new Point3d(0, 0, 0), new Point3d(-1, -1, -1), new Point3d(-1, -1, 1), new Point3d(-1, 1, 1));
            Tetrahedron t2 = new Tetrahedron(new Point3d(0, -1, -1), new Point3d(0, 1, -1), new Point3d(0, 0, 1), new Point3d(1, 0, 0));
            ConvexPolyhedron cp1 = ConvexPolyhedron.FromTetrahedron(t1);
            ConvexPolyhedron cp2 = ConvexPolyhedron.FromTetrahedron(t2);
            double dist = cp1.DistanceTo(cp2);
            Assert.IsTrue(GeometRi3D.AlmostEqual(dist, 0));
        }

        [TestMethod]
        public void TetTetDistanceTest_08()
        {
            // Edge-Edge touching
            Tetrahedron t1 = new Tetrahedron(new Point3d(0, 0, -1), new Point3d(0, 0, 1), new Point3d(-1, -1, 0), new Point3d(-1, 1, 0));
            Tetrahedron t2 = new Tetrahedron(new Point3d(0, -1, 0), new Point3d(0, 1, 0), new Point3d(1, 0, -1), new Point3d(1, 0, 1));
            ConvexPolyhedron cp1 = ConvexPolyhedron.FromTetrahedron(t1);
            ConvexPolyhedron cp2 = ConvexPolyhedron.FromTetrahedron(t2);
            double dist = cp1.DistanceTo(cp2);
            Assert.IsTrue(GeometRi3D.AlmostEqual(dist, 0));
        }

        [TestMethod]
        public void TetTetDistanceTest_09()
        {
            // Vertex-Vertex penetration
            Tetrahedron t1 = new Tetrahedron(new Point3d(0.01, 0, 0), new Point3d(-1, -1, -1), new Point3d(-1, -1, 1), new Point3d(-1, 1, 1));
            Tetrahedron t2 = new Tetrahedron(new Point3d(0, 0, 0), new Point3d(1, -1, -1), new Point3d(1, -1, 1), new Point3d(1, 1, 1));
            ConvexPolyhedron cp1 = ConvexPolyhedron.FromTetrahedron(t1);
            ConvexPolyhedron cp2 = ConvexPolyhedron.FromTetrahedron(t2);
            double dist = cp1.DistanceTo(cp2);
            Assert.IsTrue(GeometRi3D.AlmostEqual(dist, 0));
        }

        [TestMethod]
        public void TetTetDistanceTest_10()
        {
            // Vertex-Edge penetration
            Tetrahedron t1 = new Tetrahedron(new Point3d(0.01, 0, 0), new Point3d(-1, -1, -1), new Point3d(-1, -1, 1), new Point3d(-1, 1, 1));
            Tetrahedron t2 = new Tetrahedron(new Point3d(0, -1, 0), new Point3d(0, 1, 0), new Point3d(1, 0, -1), new Point3d(1, 0, 1));
            ConvexPolyhedron cp1 = ConvexPolyhedron.FromTetrahedron(t1);
            ConvexPolyhedron cp2 = ConvexPolyhedron.FromTetrahedron(t2);
            double dist = cp1.DistanceTo(cp2);
            Assert.IsTrue(GeometRi3D.AlmostEqual(dist, 0));
        }

        [TestMethod]
        public void TetTetDistanceTest_11()
        {
            // Vertex-Face penetration
            Tetrahedron t1 = new Tetrahedron(new Point3d(0.1, 0, 0), new Point3d(-1, -1, -1), new Point3d(-1, -1, 1), new Point3d(-1, 1, 1));
            Tetrahedron t2 = new Tetrahedron(new Point3d(0, -1, -1), new Point3d(0, 1, -1), new Point3d(0, 0, 1), new Point3d(1, 0, 0));
            ConvexPolyhedron cp1 = ConvexPolyhedron.FromTetrahedron(t1);
            ConvexPolyhedron cp2 = ConvexPolyhedron.FromTetrahedron(t2);
            double dist = cp1.DistanceTo(cp2);
            Assert.IsTrue(GeometRi3D.AlmostEqual(dist, 0));
        }

        [TestMethod]
        public void TetTetDistanceTest_12()
        {
            // Edge-Edge penetration
            Tetrahedron t1 = new Tetrahedron(new Point3d(0.1, 0, -1), new Point3d(0.1, 0, 1), new Point3d(-1, -1, 0), new Point3d(-1, 1, 0));
            Tetrahedron t2 = new Tetrahedron(new Point3d(0, -1, 0), new Point3d(0, 1, 0), new Point3d(1, 0, -1), new Point3d(1, 0, 1));
            ConvexPolyhedron cp1 = ConvexPolyhedron.FromTetrahedron(t1);
            ConvexPolyhedron cp2 = ConvexPolyhedron.FromTetrahedron(t2);
            double dist = cp1.DistanceTo(cp2);
            Assert.IsTrue(GeometRi3D.AlmostEqual(dist, 0));
        }

        [TestMethod]
        public void SegmentDistanceTest_01()
        {
            // Segment inside CP
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(new Box3d());
            Segment3d s = new Segment3d(new Point3d(0.1, 0.1, 0.1), new Point3d(0.2, 0.2, 0.2));

            Assert.IsTrue(cp.DistanceTo(s) == 0);
        }

        [TestMethod]
        public void SegmentDistanceTest_02()
        {
            // Segment touch face
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(new Box3d());
            Segment3d s = new Segment3d(new Point3d(0.3, 0.3, 0.0), new Point3d(0.7, 0.7, -1));

            Assert.IsTrue(GeometRi3D.AlmostEqual(cp.DistanceTo(s), 0));
        }

        [TestMethod]
        public void SegmentDistanceTest_03()
        {
            // Segment outside of CP
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(new Box3d());
            Segment3d s = new Segment3d(new Point3d(1, 0.3, 0.3), new Point3d(2, 0.3, 0.3));

            Assert.IsTrue(GeometRi3D.AlmostEqual(cp.DistanceTo(s), 0.5));
        }

        [TestMethod]
        public void CircleDistanceTest_01()
        {
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(new Box3d());
            Circle3d s = new Circle3d(new Point3d(1.5, 1.5, 1.5), 1, new Vector3d(1, 1, 1));
            double d = cp.DistanceTo(s);
            Assert.IsTrue(GeometRi3D.AlmostEqual(cp.DistanceTo(s), Sqrt(3)));
        }

        [TestMethod]
        public void ExtrudeTest_01()
        {
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(new Box3d());
            ConvexPolyhedron extrude = cp.face[0].Extrude(cp.face[0].normal, 0.2);
            Assert.IsTrue(extrude.Center == new Point3d(0.0, 0.0, -0.6));
        }

        [TestMethod]
        public void ExtrudeTest_02()
        {
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(new Box3d());
            ConvexPolyhedron extrude = cp.face[0].Extrude(cp.face[0].normal, 0.2, true);
            Assert.IsTrue(extrude.Center == new Point3d(0.0, 0.0, -0.5));
            Assert.IsTrue(extrude.Center == cp.face[0].Center);
        }

        [TestMethod]
        public void PointDistanceTest_01()
        {
            // Point inside CP
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(new Box3d());
            Point3d p  = new Point3d(0.1, 0.1, 0.1);

            Assert.IsTrue(cp.DistanceTo(p) == 0);
        }

        [TestMethod]
        public void PointDistanceTest_02()
        {
            // Point on face
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(new Box3d());
            Point3d p = new Point3d(0.1, 0.1, 0.5);
            Assert.IsTrue(cp.DistanceTo(p) == 0);

            p = new Point3d(-0.1, 0.1, -0.5);
            Assert.IsTrue(cp.DistanceTo(p) == 0);

            p = new Point3d(-0.1, -0.5, -0.2);
            Assert.IsTrue(cp.DistanceTo(p) == 0);
        }

        [TestMethod]
        public void PointDistanceTest_03()
        {
            // Point on edge
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(new Box3d());
            Point3d p = new Point3d(0.1, 0.5, 0.5);
            Assert.IsTrue(cp.DistanceTo(p) == 0);

            p = new Point3d(-0.1, -0.5, 0.5);
            Assert.IsTrue(cp.DistanceTo(p) == 0);

            p = new Point3d(-0.5, -0.5, 0.2);
            Assert.IsTrue(cp.DistanceTo(p) == 0);
        }

        [TestMethod]
        public void PointDistanceTest_04()
        {
            // Point in vertex
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(new Box3d());
            Point3d p = new Point3d(0.5, -0.5, 0.5);
            Assert.IsTrue(cp.DistanceTo(p) == 0);
        }

        [TestMethod]
        public void PointDistanceTest_05()
        {
            // Point outside, projection to face
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(new Box3d());
            Point3d p = new Point3d(0.6, -0.1, 0.1);
            Assert.IsTrue(Abs(cp.DistanceTo(p) - 0.1) < GeometRi3D.DefaultTolerance);

            p = new Point3d(0.1, -0.1, 0.6);
            Assert.IsTrue(Abs(cp.DistanceTo(p) - 0.1) < GeometRi3D.DefaultTolerance);
        }

        [TestMethod]
        public void PointDistanceTest_06()
        {
            // Point outside
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(new Box3d());
            Point3d p = new Point3d(1.5, 1.5, 0.0);
            Assert.IsTrue(Abs(cp.DistanceTo(p) - Sqrt(2.0)) < GeometRi3D.DefaultTolerance);

            p = new Point3d(0.5, 1.5, -1.5);
            Assert.IsTrue(Abs(cp.DistanceTo(p) - Sqrt(2.0)) < GeometRi3D.DefaultTolerance);
        }

        #region "Segment intersection"
        [TestMethod]
        public void SegmentPolyhedronIntersectionCheckTest_01()
        {
            // Segment is inside
            Box3d box = new Box3d(new Point3d(0, 0, 0), new Point3d(1, 1, 1));
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(box);
            Segment3d s = new Segment3d(new Point3d(0.1, 0.1, 0.1), new Point3d(0.5, 0.5, 0.5));
            Assert.IsTrue(cp.Intersects(s));
        }

        [TestMethod]
        public void SegmentPolyhedronIntersectionCheckTest_02()
        {
            // Segment intersects face
            Box3d box = new Box3d(new Point3d(0, 0, 0), new Point3d(1, 1, 1));
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(box);
            Segment3d s = new Segment3d(new Point3d(0.5, 0.5, 0.5), new Point3d(1.5, 0.5, 0.5));
            Assert.IsTrue(cp.Intersects(s));
        }

        [TestMethod]
        public void SegmentPolyhedronIntersectionCheckTest_03()
        {
            // Segment intersects edge
            Box3d box = new Box3d(new Point3d(0, 0, 0), new Point3d(1, 1, 1));
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(box);
            Segment3d s = new Segment3d(new Point3d(0.5, 0.5, 0.5), new Point3d(1.5, 1.5, 0.5));
            Assert.IsTrue(cp.Intersects(s));
        }

        [TestMethod]
        public void SegmentPolyhedronIntersectionCheckTest_04()
        {
            // Segment intersects vertex
            Box3d box = new Box3d(new Point3d(0, 0, 0), new Point3d(1, 1, 1));
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(box);
            Segment3d s = new Segment3d(new Point3d(0.5, 0.5, 0.5), new Point3d(1.5, 1.5, 1.5));
            Assert.IsTrue(cp.Intersects(s));
        }

        [TestMethod]
        public void SegmentPolyhedronIntersectionCheckTest_05()
        {
            // Segment end touch face
            Box3d box = new Box3d(new Point3d(0, 0, 0), new Point3d(1, 1, 1));
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(box);
            Segment3d s = new Segment3d(new Point3d(1.0 - 1e-12, 0.5, 0.5), new Point3d(1.5, 0.5, 0.5));
            Assert.IsTrue(cp.Intersects(s));
        }

        [TestMethod]
        public void SegmentPolyhedronIntersectionCheckTest_06()
        {
            // Segment end touch edge
            Box3d box = new Box3d(new Point3d(0, 0, 0), new Point3d(1, 1, 1));
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(box);
            Segment3d s = new Segment3d(new Point3d(1.0 - 1e-13, 1.0 - 1e-13, 0.5), new Point3d(1.5, 1.5, 0.5));
            Assert.IsTrue(cp.Intersects(s));
        }

        [TestMethod]
        public void SegmentPolyhedronIntersectionCheckTest_07()
        {
            // Segment end touch vertex
            Box3d box = new Box3d(new Point3d(0, 0, 0), new Point3d(1, 1, 1));
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(box);
            Segment3d s = new Segment3d(new Point3d(1.0 - 1e-13, 1.0 - 1e-13, 1.0 - 1e-13), new Point3d(1.5, 1.5, 1.5));
            Assert.IsTrue(cp.Intersects(s));
        }

        [TestMethod]
        public void SegmentPolyhedronIntersectionCheckTest_08()
        {
            // Segment lies on face
            Box3d box = new Box3d(new Point3d(0, 0, 0), new Point3d(1, 1, 1));
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(box);
            Segment3d s = new Segment3d(new Point3d(1.0 - 1e-16, 0.5, 0.5), new Point3d(1.0, 0.2, 0.2));
            Assert.IsTrue(cp.Intersects(s));
        }

        [TestMethod]
        public void SegmentPolyhedronIntersectionCheckTest_09()
        {
            // Segment lies on edge
            Box3d box = new Box3d(new Point3d(0, 0, 0), new Point3d(1, 1, 1));
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(box);
            
            // Undefined behaviour
            Segment3d s = new Segment3d(new Point3d(1.0, 1.0, 0.5), new Point3d(1.0, 1.0, 0.8));
            Assert.IsTrue(cp.Intersects(s));

            s = new Segment3d(new Point3d(1.0, 1.0, 0.5), new Point3d(1.0, 1.0, 0.1));
            Assert.IsFalse(cp.Intersects(s));
        }

        [TestMethod]
        public void SegmentPolyhedronIntersectionCheckTest_10()
        {
            // Segment touch edge
            Box3d box = new Box3d(new Point3d(0, 0, 0), new Point3d(1, 1, 1));
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(box);
            Segment3d s = new Segment3d(new Point3d(2.0 - 1e-15, 0.0, 0.5), new Point3d(0.0, 2.0, 0.5));
            Assert.IsTrue(cp.Intersects(s));
        }

        [TestMethod]
        public void SegmentPolyhedronIntersectionCheckTest_11()
        {
            // Segment outside
            Box3d box = new Box3d(new Point3d(0, 0, 0), new Point3d(1, 1, 1));
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(box);
            Segment3d s = new Segment3d(new Point3d(2.0 , 0.0, 0.5), new Point3d(3.0, 2.0, 0.99));
            Assert.IsFalse(cp.Intersects(s));
        }

        [TestMethod]
        public void SegmentPolyhedronIntersectionCheckTest_12()
        {
            // Segment outside
            Box3d box = new Box3d(new Point3d(0, 0, 0), new Point3d(1, 1, 1));
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(box);
            Segment3d s = new Segment3d(new Point3d(2.0, 2.0, 2.0), new Point3d(3.0, 2.0, 3.0));
            Assert.IsFalse(cp.Intersects(s));
        }

        #endregion

        #region "Triangle intersection"
        [TestMethod]
        public void TrianglePolyhedronIntersectionCheckTest_01()
        {
            // Triangle is inside
            Box3d box = new Box3d(new Point3d(0, 0, 0), new Point3d(1, 1, 1));
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(box);
            Triangle t = new Triangle(new Point3d(0.1, 0.1, 0.1), new Point3d(0.5, 0.5, 0.5), new Point3d(0.1, 0.2, 0.3));
            Assert.IsTrue(cp.Intersects(t));
        }

        [TestMethod]
        public void TrianglePolyhedronIntersectionCheckTest_02()
        {
            // Triangle intersects face
            Box3d box = new Box3d(new Point3d(0, 0, 0), new Point3d(1, 1, 1));
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(box);
            Triangle t = new Triangle(new Point3d(0.1, 0.1, 0.1), new Point3d(0.5, 0.5, 0.5), new Point3d(1.1, 0.2, 0.3));
            Assert.IsTrue(cp.Intersects(t));
        }

        [TestMethod]
        public void TrianglePolyhedronIntersectionCheckTest_03()
        {
            // Triangle intersects edge
            Box3d box = new Box3d(new Point3d(0, 0, 0), new Point3d(1, 1, 1));
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(box);
            Triangle t = new Triangle(new Point3d(0.5, 0.5, 0.5), new Point3d(1.5, 1.5, 0.1), new Point3d(1.5, 1.5, 0.7));
            Assert.IsTrue(cp.Intersects(t));
        }

        [TestMethod]
        public void TrianglePolyhedronIntersectionCheckTest_04()
        {
            // Triangle touch edge
            Box3d box = new Box3d(new Point3d(0, 0, 0), new Point3d(1, 1, 1));
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(box);
            Triangle t = new Triangle(new Point3d(1.0 - 1e-13, 1.0 - 1e-13, 0.5), new Point3d(1.5, 1.5, 0.7), new Point3d(1.5, 1.5, 0.1));
            Assert.IsTrue(cp.Intersects(t));
        }

        [TestMethod]
        public void TrianglePolyhedronIntersectionCheckTest_05()
        {
            // Triangle outside
            Box3d box = new Box3d(new Point3d(0, 0, 0), new Point3d(1, 1, 1));
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(box);
            Triangle t = new Triangle(new Point3d(1.1, 1.1, 0.5), new Point3d(1.5, 1.5, 0.7), new Point3d(1.5, 1.5, 0.1));
            Assert.IsFalse(cp.Intersects(t));
        }

        #endregion

    }
}
