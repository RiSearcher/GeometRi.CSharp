using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

namespace GeometRi_Tests
{
    [TestClass]
    public class Matrix3dTest
    {
        //===============================================================
        // Matrix3d tests
        //===============================================================

        [TestMethod()]
        public void MatrixDeterminantTest()
        {
            Matrix3d m = new Matrix3d(new[] { 1.0, 2.0, 3.0 }, new[] { 0.0, -4.0, 1.0 }, new[] { 0.0, 3.0, -1.0 });
            Assert.IsTrue(Abs(m.Det - 1) < GeometRi3D.Tolerance);
        }

        [TestMethod()]
        public void MatrixInverseTest()
        {
            Matrix3d m = new Matrix3d(new[] { 1.0, 2.0, 3.0 }, new[] { 0.0, -4.0, 1.0 }, new[] { 0.0, 3.0, -1.0 });
            Assert.IsTrue(m.Inverse() * m == Matrix3d.Identity());
        }

        [TestMethod()]
        public void RotationMatrixOrthogonalityTest()
        {
            Matrix3d r = Matrix3d.RotationMatrix(new Vector3d(1, 2, 3), PI / 2);
            Assert.IsTrue(r.Transpose() * r == Matrix3d.Identity());
        }

        [TestMethod()]
        public void MatrixMaxNormTest()
        {
            Matrix3d m = new Matrix3d(new[] { 1.0, 2.0, 3.0 }, new[] { 0.0, -4.0, 1.0 }, new[] { 0.0, 3.0, -1.0 });
            Assert.IsTrue(Abs(m.MaxNorm - 4) < GeometRi3D.Tolerance);
        }

    }
}
