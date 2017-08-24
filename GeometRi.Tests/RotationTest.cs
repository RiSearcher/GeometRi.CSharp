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

            Assert.AreEqual(Matrix3d.RotationMatrix(v, angle), r.RotationMatrix);
        }

        [TestMethod]
        public void AxisToFromQuaternionTest()
        {
            Vector3d v = new Vector3d(4, 111, 6);
            double angle = 0.0515;

            Quaternion q = new Quaternion(v, angle);

            Assert.IsTrue(q.Axis.IsParallelTo(v));
            Assert.IsTrue(GeometRi3D.AlmostEqual(q.Angle, angle));
        }

        [TestMethod]
        public void AxisToFromMatrixTest()
        {
            Vector3d v = new Vector3d(4, 111, 6);
            double angle = 0.0515;

            Rotation r = new Rotation(v, angle);

            Assert.IsTrue(r.Axis.IsParallelTo(v));
            Assert.IsTrue(GeometRi3D.AlmostEqual(r.Angle, angle));
        }

        [TestMethod]
        public void QuaternionToFromMatrixTest()
        {
            Quaternion q = new Quaternion(0.5, 0.5, 100.5, 0.5);
            Rotation r = new Rotation(q);
            Assert.AreEqual(r.Quaternion, q);

            q = new Quaternion(1.0 / Sqrt(3), 1.0 / Sqrt(3), 1.0 / Sqrt(3), 0.0);
            r = new Rotation(q);
            Assert.AreEqual(r.Quaternion, q);

            q = new Quaternion(0.5, 0.5, 0.5, 0.5);
            r = new Rotation(q);
            Assert.AreEqual(r.Quaternion, q);

            q = new Quaternion(0.0, 1.0 / Sqrt(3), 1.0 / Sqrt(3), 1.0 / Sqrt(3));
            r = new Rotation(q);
            Assert.AreEqual(r.Quaternion, q);

            q = new Quaternion(1.0, 0.0, 0.0, 0.0);
            r = new Rotation(q);
            Assert.AreEqual(r.Quaternion, q);
        }

    }
}
