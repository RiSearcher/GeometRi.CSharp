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

    }
}
