using System;
using static System.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometRi;

namespace GeometRi_Tests
{
    [TestClass]
    public class EllipseTest
    {
        //===============================================================
        // Ellipse tests
        //===============================================================

        [TestMethod()]
        public void PointBelongsToEllipseTest()
        {
            Vector3d v1 = new Vector3d(3, 0, 1);
            Vector3d v2 = 3 * v1.OrthogonalVector;
            Ellipse e = new Ellipse(new Point3d(5, 6, 1), v1, v2);
            Assert.IsTrue(e.ParametricForm(0.5).BelongsTo(e));
        }

        [TestMethod()]
        public void EllipseProjectionToPlaneTest()
        {
            Vector3d v1 = new Vector3d(3, 0, 1);
            Vector3d v2 = 3 * v1.OrthogonalVector;
            Ellipse e = new Ellipse(new Point3d(5, 6, 1), v1, v2);
            Plane3d s = new Plane3d(5, 2, 3, -3);

            Point3d p = e.ParametricForm(0.5).ProjectionTo(s);
            Assert.IsTrue(p.BelongsTo(e.ProjectionTo(s)));
            p = e.ParametricForm(0.725).ProjectionTo(s);
            Assert.IsTrue(p.BelongsTo(e.ProjectionTo(s)));
            p = e.ParametricForm(2.7122).ProjectionTo(s);
            Assert.IsTrue(p.BelongsTo(e.ProjectionTo(s)));
        }

        [TestMethod()]
        public void EllipseIntersectionWithPlaneTest()
        {
            Point3d p = new Point3d(0, 0, 0);
            Vector3d v1 = new Vector3d(4, 0, 0);
            Vector3d v2 = new Vector3d(0, 6, 0);
            Ellipse e = new Ellipse(p, v1, v2);

            p = new Point3d(0, 50, 0);
            v1 = new Vector3d(-1000, 1, 0);
            Plane3d s = new Plane3d(p, v1);

            Segment3d res = new Segment3d(new Point3d(-0.0440003630169716, 5.99963698302842, 0), new Point3d(-0.0559994119835347, -5.99941198353467, 0));
            Assert.AreEqual(e.IntersectionWith(s), res);
        }
    }
}
