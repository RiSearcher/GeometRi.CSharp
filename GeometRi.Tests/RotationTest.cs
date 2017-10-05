using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

namespace GeometRi.Tests
{
    [TestClass]
    public class RotationTest
    {
        [TestMethod]
        public void RotationMatrixTest()
        {
            Vector3d v = new Vector3d(4, 111, 6);
            double angle = 0.0515;

            Rotation r = new Rotation(v, angle);

            Assert.AreEqual(Matrix3d.RotationMatrix(v, angle), r.ToRotationMatrix);
        }

        [TestMethod]
        public void InverseRotationTest()
        {
            // Inverse axis and negative angle should produce the same rotation
            Vector3d v = new Vector3d(4, 111, 6);
            double angle = 0.0515;

            Rotation r1 = new Rotation(v, angle);
            Rotation r2 = new Rotation(-v, -angle);
            Assert.AreEqual(r1, r2);

            Quaternion q1 = new Quaternion(v, angle);
            Quaternion q2 = new Quaternion(-v, -angle);
            Assert.AreEqual(q1, q2);
        }

        [TestMethod]
        public void PI_RotationTest()
        {
            // +PI and -PI should produce the same rotation
            Vector3d v = new Vector3d(4, 111, 6);

            Rotation r1 = new Rotation(v, PI);
            Rotation r2 = new Rotation(v, -PI);
            Assert.AreEqual(r1, r2);

            Quaternion q1 = new Quaternion(v, PI);
            Quaternion q2 = new Quaternion(v, -PI);
            // For such rotation quaternion representation is not unique!!!
            Assert.IsTrue(q1 == q2 || q1 == q2.Conjugate);
        }

        [TestMethod]
        public void AxisToFromQuaternionTest()
        {
            Vector3d v = new Vector3d(4, 111, 6);
            double angle = 0.0515;

            Quaternion q = new Quaternion(v, angle);

            Assert.IsTrue(q.ToAxis.IsParallelTo(v));
            Assert.IsTrue(GeometRi3D.AlmostEqual(q.ToAngle, angle));
        }

        [TestMethod]
        public void RotationFromCoordTest()
        {
            Coord3d coord = new Coord3d(new Point3d(2, 3, 1), new Vector3d(2, 1, 5), new Vector3d(1, 1, 1));
            Rotation r = new Rotation(coord);

            Assert.AreEqual(r * Coord3d.GlobalCS.Xaxis, coord.Xaxis);
            Assert.AreEqual(r * Coord3d.GlobalCS.Yaxis, coord.Yaxis);
            Assert.AreEqual(r * Coord3d.GlobalCS.Zaxis, coord.Zaxis);
        }

        [TestMethod]
        public void AxisToFromMatrixTest()
        {
            Vector3d v = new Vector3d(4, 111, 6);
            double angle = 0.0515;

            Rotation r = new Rotation(v, angle);

            Assert.IsTrue(r.ToAxis.IsParallelTo(v));
            Assert.IsTrue(GeometRi3D.AlmostEqual(r.ToAngle, angle));
        }

        [TestMethod]
        public void QuaternionToFromMatrixTest()
        {
            Quaternion q = new Quaternion(0.5, 0.5, 100.5, 0.5);
            Rotation r = new Rotation(q);
            Assert.AreEqual(r.ToQuaternion, q);

            q = new Quaternion(1.0 / Sqrt(3), 1.0 / Sqrt(3), 1.0 / Sqrt(3), 0.0);
            r = new Rotation(q);
            Assert.AreEqual(r.ToQuaternion, q);

            q = new Quaternion(0.5, 0.5, 0.5, 0.5);
            r = new Rotation(q);
            Assert.AreEqual(r.ToQuaternion, q);

            q = new Quaternion(0.0, 1.0 / Sqrt(3), 1.0 / Sqrt(3), 1.0 / Sqrt(3));
            r = new Rotation(q);
            Assert.AreEqual(r.ToQuaternion, q);

            q = new Quaternion(1.0, 0.0, 0.0, 0.0);
            r = new Rotation(q);
            Assert.AreEqual(r.ToQuaternion, q);
        }

        [TestMethod]
        public void FromEulerAnglesTest()
        {
            Rotation r = Rotation.FromEulerAngles(PI / 2, PI / 2, PI / 2, "zxy");
            Rotation res = new Rotation(new Vector3d(1, 0, 0), PI / 2);
            Assert.AreEqual(r, res);

            r = Rotation.FromEulerAngles(PI / 2, -PI / 2, -PI / 2, "ZYZ");
            res = new Rotation(new Vector3d(1, 0, 0), PI / 2);
            Assert.AreEqual(r, res);

            r = Rotation.FromEulerAngles(PI / 4, -PI / 2, PI / 4, "zyx");
            res = new Rotation(new Vector3d(0, 1, 0), -PI / 2);
            Assert.AreEqual(r, res);

            r = Rotation.FromEulerAngles(PI / 4, -PI / 2, -PI / 4, "ZYX");
            res = new Rotation(new Vector3d(0, 1, 0), -PI / 2);
            Assert.AreEqual(r, res);
        }

        [TestMethod]
        public void SLERPTest()
        {
            // Zero rotation test
            Quaternion q0 = new Quaternion(1, 0, 0, 0);
            Quaternion q1 = q0;
            Quaternion q2 = q0;
            Assert.AreEqual(Quaternion.SLERP(q1, q2, 0.5), q0);

            // Interpolate from zero to 90 degrees
            q1 = new Quaternion(new Vector3d(1, 2, 3), PI / 2);
            q2 = new Quaternion(new Vector3d(1, 2, 3), PI / 4);
            Assert.AreEqual(Quaternion.SLERP(q0, q1, 0.5), q2);

            // Interpolate from zero to 180 degrees
            q1 = new Quaternion(new Vector3d(1, -2, -3), PI);
            q2 = new Quaternion(new Vector3d(1, -2, -3), PI / 2);
            Assert.AreEqual(Quaternion.SLERP(q0, q1, 0.5), q2);

            q1 = new Quaternion(new Vector3d(1, 2, -3), 0.9832);
            q2 = new Quaternion(new Vector3d(10, -2, 0.5), 1.8945);
            Quaternion q_0_5 = Quaternion.SLERP(q1, q2, 0.5);
            Quaternion q_0_25 = Quaternion.SLERP(q1, q2, 0.25);
            Quaternion q_0_75 = Quaternion.SLERP(q1, q2, 0.75);
            Assert.AreEqual(Quaternion.SLERP(q1, q_0_5, 0.5), q_0_25);
            Assert.AreEqual(Quaternion.SLERP(q_0_5, q2, 0.5), q_0_75);

        }

        [TestMethod]
        public void ToEulerAnglesXYZTest()
        {
            double a1, a2, a3;
            a1 = 0.43553;
            a2 = 0.1233;
            a3 = 1.2342354;
            Rotation r = Rotation.FromEulerAngles(a1, a1, a3, "XYZ");
            var res = r.ToEulerAngles("XYZ");
            Rotation r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "XYZ");
            Assert.AreEqual(r, r2);

            r = Rotation.FromEulerAngles(a1, a2, a3, "zyx");
            res = r.ToEulerAngles("zyx");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "zyx");
            Assert.AreEqual(r, r2);

            r = Rotation.FromEulerAngles(a1, a2, a3, "XZY");
            res = r.ToEulerAngles("XZY");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "XZY");
            Assert.AreEqual(r, r2);

            r = Rotation.FromEulerAngles(a1, a2, a3, "yzx");
            res = r.ToEulerAngles("yzx");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "yzx");
            Assert.AreEqual(r, r2);

            r = Rotation.FromEulerAngles(a1, a2, a3, "YXZ");
            res = r.ToEulerAngles("YXZ");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "YXZ");
            Assert.AreEqual(r, r2);

            r = Rotation.FromEulerAngles(a1, a2, a3, "zxy");
            res = r.ToEulerAngles("zxy");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "zxy");
            Assert.AreEqual(r, r2);

            r = Rotation.FromEulerAngles(a1, a2, a3, "YZX");
            res = r.ToEulerAngles("YZX");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "YZX");
            Assert.AreEqual(r, r2);

            r = Rotation.FromEulerAngles(a1, a2, a3, "xzy");
            res = r.ToEulerAngles("xzy");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "xzy");
            Assert.AreEqual(r, r2);

            r = Rotation.FromEulerAngles(a1, a2, a3, "ZXY");
            res = r.ToEulerAngles("ZXY");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "ZXY");
            Assert.AreEqual(r, r2);

            r = Rotation.FromEulerAngles(a1, a2, a3, "yxz");
            res = r.ToEulerAngles("yxz");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "yxz");
            Assert.AreEqual(r, r2);

            r = Rotation.FromEulerAngles(a1, a2, a3, "ZYX");
            res = r.ToEulerAngles("ZYX");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "ZYX");
            Assert.AreEqual(r, r2);

            r = Rotation.FromEulerAngles(a1, a2, a3, "xyz");
            res = r.ToEulerAngles("xyz");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "xyz");
            Assert.AreEqual(r, r2);

        }

        [TestMethod]
        public void ToEulerAnglesXYXTest()
        {
            double a1, a2, a3;
            a1 = 0.43553;
            a2 = 0.1233;
            a3 = 1.2342354;
            Rotation r = Rotation.FromEulerAngles(a1, a1, a3, "XYX");
            var res = r.ToEulerAngles("XYX");
            Rotation r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "XYX");
            Assert.AreEqual(r, r2);

            r = Rotation.FromEulerAngles(a1, a2, a3, "xyx");
            res = r.ToEulerAngles("xyx");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "xyx");
            Assert.AreEqual(r, r2);

            r = Rotation.FromEulerAngles(a1, a2, a3, "XZX");
            res = r.ToEulerAngles("XZX");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "XZX");
            Assert.AreEqual(r, r2);

            r = Rotation.FromEulerAngles(a1, a2, a3, "xzx");
            res = r.ToEulerAngles("xzx");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "xzx");
            Assert.AreEqual(r, r2);

            r = Rotation.FromEulerAngles(a1, a2, a3, "YXY");
            res = r.ToEulerAngles("YXY");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "YXY");
            Assert.AreEqual(r, r2);

            r = Rotation.FromEulerAngles(a1, a2, a3, "yxy");
            res = r.ToEulerAngles("yxy");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "yxy");
            Assert.AreEqual(r, r2);

            r = Rotation.FromEulerAngles(a1, a2, a3, "YZY");
            res = r.ToEulerAngles("YZY");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "YZY");
            Assert.AreEqual(r, r2);

            r = Rotation.FromEulerAngles(a1, a2, a3, "yzy");
            res = r.ToEulerAngles("yzy");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "yzy");
            Assert.AreEqual(r, r2);

            r = Rotation.FromEulerAngles(a1, a2, a3, "ZXZ");
            res = r.ToEulerAngles("ZXZ");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "ZXZ");
            Assert.AreEqual(r, r2);

            r = Rotation.FromEulerAngles(a1, a2, a3, "zxz");
            res = r.ToEulerAngles("zxz");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "zxz");
            Assert.AreEqual(r, r2);

            r = Rotation.FromEulerAngles(a1, a2, a3, "ZYZ");
            res = r.ToEulerAngles("ZYZ");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "ZYZ");
            Assert.AreEqual(r, r2);

            r = Rotation.FromEulerAngles(a1, a2, a3, "zyz");
            res = r.ToEulerAngles("zyz");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "zyz");
            Assert.AreEqual(r, r2);
        }

        [TestMethod]
        public void ToEulerAnglesDegenerateTest()
        {
            // Degenerate test cases

            double a1, a2, a3;
            a1 = 0.0;
            a2 = 0.0;
            a3 = 0.0;
            Rotation r = Rotation.FromEulerAngles(a1, a1, a3, "XYZ");
            var res = r.ToEulerAngles("XYZ");
            Rotation r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "XYZ");
            Assert.AreEqual(r, r2);

            r = Rotation.FromEulerAngles(a1, a2, a3, "xyx");
            res = r.ToEulerAngles("xyx");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "xyx");
            Assert.AreEqual(r, r2);

            a1 = PI;
            a2 = PI;
            a3 = PI;
            r = Rotation.FromEulerAngles(a1, a1, a3, "XYZ");
            res = r.ToEulerAngles("XYZ");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "XYZ");
            Assert.AreEqual(r, r2);

            r = Rotation.FromEulerAngles(a1, a2, a3, "xyx");
            res = r.ToEulerAngles("xyx");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "xyx");
            Assert.AreEqual(r, r2);

            a1 = -PI;
            a2 = -PI;
            a3 = -PI;
            r = Rotation.FromEulerAngles(a1, a1, a3, "XYZ");
            res = r.ToEulerAngles("XYZ");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "XYZ");
            Assert.AreEqual(r, r2);

            r = Rotation.FromEulerAngles(a1, a2, a3, "xyx");
            res = r.ToEulerAngles("xyx");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "xyx");
            Assert.AreEqual(r, r2);

            a1 = -PI;
            a2 = -PI / 2;
            a3 = PI / 2;
            r = Rotation.FromEulerAngles(a1, a1, a3, "XYZ");
            res = r.ToEulerAngles("XYZ");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "XYZ");
            Assert.AreEqual(r, r2);

            r = Rotation.FromEulerAngles(a1, a2, a3, "xyx");
            res = r.ToEulerAngles("xyx");
            r2 = Rotation.FromEulerAngles(res[0], res[1], res[2], "xyx");
            Assert.AreEqual(r, r2);
        }

    }
}
