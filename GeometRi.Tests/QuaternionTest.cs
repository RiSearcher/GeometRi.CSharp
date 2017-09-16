using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

namespace GeometRi_Tests
{
    [TestClass]
    public class QuaternionTest
    {
        [TestMethod]
        public void IdentityMatrixTest()
        {
            Matrix3d m = Matrix3d.Identity();
            Quaternion q = new Quaternion(m);
            Assert.AreEqual(q, new Quaternion(1, 0, 0, 0));
        }

        [TestMethod]
        public void ZeroRotationTest()
        {
            Rotation r = new Rotation();
            Assert.AreEqual(r.ToQuaternion, new Quaternion(1, 0, 0, 0));
        }
    }
}
