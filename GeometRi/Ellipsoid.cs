using System;
using static System.Math;

namespace GeometRi
{
    /// <summary>
    /// Ellipsoid object defined by center point and three mutually orthogonal vectors.
    /// </summary>
    public class Ellipsoid
    {

        private Point3d _point;
        private Vector3d _v1;
        private Vector3d _v2;
        private Vector3d _v3;

        /// <summary>
        /// Initializes ellipsoid instance using center point and three orthogonal vectors.
        /// </summary>
        /// <param name="Center">Center point.</param>
        /// <param name="v1">First semiaxis.</param>
        /// <param name="v2">Second semiaxis.</param>
        /// <param name="v3">Third semiaxis.</param>
        public Ellipsoid(Point3d Center, Vector3d v1, Vector3d v2, Vector3d v3)
        {
            if ( !(v1.IsOrthogonalTo(v2) && v1.IsOrthogonalTo(v3) && v3.IsOrthogonalTo(v2)) )
            {
                throw new Exception("Semiaxes are not orthogonal");
            }
            _point = Center.Copy();
            if (v1.Norm >= v2.Norm && v1.Norm >= v3.Norm)
            {
                _v1 = v1.Copy();
                if (v2.Norm >= v3.Norm)
                {
                    _v2 = v2.Copy();
                    _v3 = v3.Copy();
                }
                else
                {
                    _v2 = v3.Copy();
                    _v3 = v2.Copy();
                }
            }
            else if (v2.Norm >= v1.Norm && v2.Norm >= v3.Norm)
            {
                _v1 = v2.Copy();
                if (v1.Norm >= v3.Norm)
                {
                    _v2 = v1.Copy();
                    _v3 = v3.Copy();
                }
                else
                {
                    _v2 = v3.Copy();
                    _v3 = v1.Copy();
                }
            }
            else
            {
                _v1 = v3.Copy();
                if (v1.Norm >= v2.Norm)
                {
                    _v2 = v1.Copy();
                    _v3 = v2.Copy();
                }
                else
                {
                    _v2 = v2.Copy();
                    _v3 = v1.Copy();
                }
            }
        }

        /// <summary>
        /// Creates copy of the object
        /// </summary>
        public Ellipsoid Copy()
        {
            return new Ellipsoid(_point.Copy(), _v1.Copy(), _v2.Copy(), _v3.Copy());
        }

        #region "Properties"
        public Point3d Center
        {
            get { return _point.Copy(); }
        }

        /// <summary>
        /// Major semiaxis
        /// </summary>
        public Vector3d SemiaxisA
        {
            get { return _v1.Copy(); }
        }

        /// <summary>
        /// Intermediate semiaxis
        /// </summary>
        public Vector3d SemiaxisB
        {
            get { return _v2.Copy(); }
        }

        /// <summary>
        /// Minor semiaxis
        /// </summary>
        public Vector3d SemiaxisC
        {
            get { return _v3.Copy(); }
        }

        /// <summary>
        /// Length of the major semiaxis
        /// </summary>
        public double A
        {
            get { return _v1.Norm; }
        }

        /// <summary>
        /// Length of the intermediate semiaxis
        /// </summary>
        public double B
        {
            get { return _v2.Norm; }
        }

        /// <summary>
        /// Length of the minor semiaxis
        /// </summary>
        public double C
        {
            get { return _v3.Norm; }
        }

        /// <summary>
        /// Volume of the ellipsoid
        /// </summary>
        public double Volume
        {
            get { return 4.0 / 3.0 * PI * A * B * C; }
        }

        /// <summary>
        /// Approximate surface area of the ellipsoid (accurate up to 1.061%).
        /// </summary>
        public double Area
        {
            get {
                double p = 1.6075;
                double tmp = Pow(A * B, p) + Pow(A * C, p) + Pow(C * B, p);
                return 4.0 * PI * Pow(tmp, 1/p);
            }
        }
        #endregion

        /// <summary>
        /// Orthogonal projection of ellipsoid to line.
        /// </summary>
        public Segment3d ProjectionTo(Line3d l)
        {
            //Stephen B. Pope "Algorithms for Ellipsoids"
            // https://tcg.mae.cornell.edu/pubs/Pope_FDA_08.pdf

            Coord3d lc = new Coord3d(_point, _v1, _v2);
            Point3d x0 = l.Point.ConvertTo(lc);
            Vector3d v = l.Direction.ConvertTo(lc);

            Matrix3d L_T = Matrix3d.DiagonalMatrix(this.A, this.B, this.C);
            Vector3d c = new Vector3d(0.0, 0.0, 0.0, lc);
            double s0 = v * (c - x0.ToVector) / (v * v);
            Vector3d w = L_T * v / (v * v);
            Point3d P1 = x0.Translate((s0 + w.Norm) * v);
            Point3d P2 = x0.Translate((s0 - w.Norm) * v);
            return new Segment3d(P1, P2);
        }

        /// <summary>
        /// Intersection of ellipsoid with line.
        /// Returns 'null' (no intersection) or object of type 'Point3d' or 'Segment3d'.
        /// </summary>
        public object IntersectionWith(Line3d s)
        {
            // Analytical solution from:
            // https://johannesbuchner.github.io/intersection/intersection_line_ellipsoid.html

            // Define local cordinate system for ellipsoid
            // and present line in parametric form in local coordinate system
            // x: t + x0
            // y: k * t + y0
            // z: l * t + z0
            // For numerical stability choose local X axis such that k<=1 and l<=1 !!!

            Coord3d lc = new Coord3d(_point, _v1, _v2);
            Vector3d v0 = s.Direction.ConvertTo(lc);
            if (Abs(v0.Y) > Abs(v0.X) || Abs(v0.Z) > Abs(v0.X))
            {
                // Bad choice of X axis, try again
                lc = new Coord3d(_point, _v2, _v3);
                v0 = s.Direction.ConvertTo(lc);
                if (Abs(v0.Y) > Abs(v0.X) || Abs(v0.Z) > Abs(v0.X))
                {
                    lc = new Coord3d(_point, _v3, _v1);
                    v0 = s.Direction.ConvertTo(lc);
                }
            }
            // Normalize direction vector
            double k = v0.Y / v0.X;
            double l = v0.Z / v0.X;

            Point3d p0 = s.Point.ConvertTo(lc);
            double x0 = p0.X;
            double y0 = p0.Y;
            double z0 = p0.Z;

            double a2b2 = A * A * B * B;
            double a2c2 = A * A * C * C;
            double b2c2 = B * B * C * C;

            double det = a2b2 * C * C * (a2b2*l*l + a2c2*k*k - A*A*k*k*z0*z0 +
                                         2*A*A*k*l*y0*z0 - A*A*l*l*y0*y0 + b2c2 - 
                                         B*B*l*l*x0*x0 + 2*B*B*l*x0*z0 - B*B*z0*z0 -
                                         C*C*k*k*x0*x0 + 2*C*C*k*x0*y0 - C*C*y0*y0);

            if (det < -GeometRi3D.Tolerance)
            {
                return null;
            }

            double sum1 = a2b2 * l * z0 + a2c2 * k * y0 + b2c2 * x0;
            double sum2 = a2b2 * l * l + a2c2 * k * k + b2c2;

            if (Abs(det) <= GeometRi3D.Tolerance)
            {
                // Intersection is point
                double t = -sum1 / sum2;
                return new Point3d(t + x0, k * t + y0, l * t + z0, lc);
            }
            else
            {
                double t = -(sum1 + Sqrt(det)) / sum2;
                Point3d p1 = new Point3d(t + x0, k * t + y0, l * t + z0, lc);
                t = -(sum1 - Sqrt(det)) / sum2;
                Point3d p2 = new Point3d(t + x0, k * t + y0, l * t + z0, lc);
                return new Segment3d(p1, p2);
            }
        }

        /// <summary>
        /// Intersection of ellipsoid with plane.
        /// Returns 'null' (no intersection) or object of type 'Point3d' or 'Ellipse'.
        /// </summary>
        public object IntersectionWith(Plane3d plane)
        {
            // Solution 1:
            // Peter Paul Klein 
            // On the Ellipsoid and Plane Intersection Equation
            // Applied Mathematics, 2012, 3, 1634-1640 (DOI:10.4236/am.2012.311226)

            // Solution 2:
            // Sebahattin Bektas
            // Intersection of an Ellipsoid and a Plane
            // International Journal of Research in Engineering and Applied Sciences, VOLUME 6, ISSUE 6 (June, 2016)

            Coord3d lc = new Coord3d(_point, _v1, _v2, "LC1");
            plane.SetCoord(lc);
            double Ax, Ay, Az, Ad;
            double a, b, c;
            if (Abs(plane.C) >= Abs(plane.A) && Abs(plane.C) >= Abs(plane.B))
            {
                a = this.A; b = this.B; c = this.C;
            }
            else
            {
                lc = new Coord3d(_point, _v2, _v3, "LC2");
                plane.SetCoord(lc);
                if (Abs(plane.C) >= Abs(plane.A) && Abs(plane.C) >= Abs(plane.B))
                {
                    a = this.B; b = this.C; c = this.A;
                }
                else
                {
                    lc = new Coord3d(_point, _v3, _v1, "LC3");
                    plane.SetCoord(lc);
                    a = this.C; b = this.A; c = this.B;
                }
            }

            Ax = plane.A; Ay = plane.B; Az = plane.C; Ad = plane.D;
            double tmp = (Az * Az * c * c);
            double AA = 1.0 / (a * a) + Ax * Ax / tmp;
            double BB = 2.0 * Ax * Ay / tmp;
            double CC = 1.0 / (b * b) + Ay * Ay / tmp;
            double DD = 2.0 * Ax * Ad / tmp;
            double EE = 2.0 * Ay * Ad / tmp;
            double FF = Ad * Ad / tmp - 1.0;

            double det = 4.0 * AA * CC - BB * BB;
            if (GeometRi3D.AlmostEqual(det, 0))
            {
                return null;
            }
            double X0 = (BB * EE - 2 * CC * DD) / det;
            double Y0 = (BB * DD - 2 * AA * EE) / det;
            double Z0 = -(Ax * X0 + Ay * Y0 + Ad) / Az;

            Point3d P0 = new Point3d(X0, Y0, Z0, lc);
            if (P0.BelongsTo(this))
            {
                // the plane is tangent to ellipsoid
                return P0;
            }
            else if (P0.IsInside(this))
            {
                Vector3d q = P0.ToVector.ConvertTo(lc);
                Matrix3d D1 = Matrix3d.DiagonalMatrix(1 / a, 1 / b, 1 / c);
                Vector3d r = plane.Normal.ConvertTo(lc).OrthogonalVector.Normalized;
                Vector3d s = plane.Normal.ConvertTo(lc).Cross(r).Normalized;

                double omega = 0;
                double qq, qr, qs, rr, ss, rs;
                if (!GeometRi3D.AlmostEqual((D1*r)*(D1*s),0))
                {
                    rr = (D1 * r) * (D1 * r);
                    rs = (D1 * r) * (D1 * s);
                    ss = (D1 * s) * (D1 * s);
                    if (GeometRi3D.AlmostEqual(rr-ss, 0))
                    {
                        omega = PI / 4;
                    }
                    else
                    {
                        omega = 0.5 * Atan(2.0 * rs / (rr - ss));
                    }
                    Vector3d rprim = Cos(omega) * r + Sin(omega) * s;
                    Vector3d sprim = -Sin(omega) * r + Cos(omega) * s;
                    r = rprim;
                    s = sprim;
                }

                qq = (D1 * q) * (D1 * q);
                qr = (D1 * q) * (D1 * r);
                qs = (D1 * q) * (D1 * s);
                rr = (D1 * r) * (D1 * r);
                ss = (D1 * s) * (D1 * s);

                double d = qq - qr * qr / rr - qs * qs / ss;
                AA = Sqrt((1 - d) / rr);
                BB = Sqrt((1 - d) / ss);

                return new Ellipse(P0, AA * r, BB * s);

            }
            else
            {
                return null;
            }

        }

        #region "TranslateRotateReflect"
        /// <summary>
        /// Translate ellipsoid by a vector
        /// </summary>
        public Ellipsoid Translate(Vector3d v)
        {
            return new Ellipsoid(this.Center.Translate(v), _v1, _v2, _v3);
        }

        /// <summary>
        /// Rotate ellipsoid by a given rotation matrix
        /// </summary>
        [System.Obsolete("use Rotation object and specify rotation center: this.Rotate(Rotation r, Point3d p)")]
        public Ellipsoid Rotate(Matrix3d m)
        {
            return new Ellipsoid(this.Center.Rotate(m), _v1.Rotate(m), _v2.Rotate(m), _v3.Rotate(m));
        }

        /// <summary>
        /// Rotate ellipsoid by a given rotation matrix around point 'p' as a rotation center
        /// </summary>
        [System.Obsolete("use Rotation object: this.Rotate(Rotation r, Point3d p)")]
        public Ellipsoid Rotate(Matrix3d m, Point3d p)
        {
            return new Ellipsoid(this.Center.Rotate(m, p), _v1.Rotate(m), _v2.Rotate(m), _v3.Rotate(m));
        }

        /// <summary>
        /// Rotate ellipsoid around point 'p' as a rotation center
        /// </summary>
        public Ellipsoid Rotate(Rotation r, Point3d p)
        {
            return new Ellipsoid(this.Center.Rotate(r, p), _v1.Rotate(r), _v2.Rotate(r), _v3.Rotate(r));
        }

        /// <summary>
        /// Reflect ellipsoid in given point
        /// </summary>
        public Ellipsoid ReflectIn(Point3d p)
        {
            return new Ellipsoid(this.Center.ReflectIn(p), _v1.ReflectIn(p), _v2.ReflectIn(p), _v3.ReflectIn(p));
        }

        /// <summary>
        /// Reflect ellipsoid in given line
        /// </summary>
        public Ellipsoid ReflectIn(Line3d l)
        {
            return new Ellipsoid(this.Center.ReflectIn(l), _v1.ReflectIn(l), _v2.ReflectIn(l), _v3.ReflectIn(l));
        }

        /// <summary>
        /// Reflect ellipsoid in given plane
        /// </summary>
        public Ellipsoid ReflectIn(Plane3d s)
        {
            return new Ellipsoid(this.Center.ReflectIn(s), _v1.ReflectIn(s), _v2.ReflectIn(s), _v3.ReflectIn(s));
        }
        #endregion

        /// <summary>
        /// Determines whether two objects are equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null || (!object.ReferenceEquals(this.GetType(), obj.GetType())))
            {
                return false;
            }
            Ellipsoid e = (Ellipsoid)obj;

            if (this.Center != e.Center)
            {
                return false;
            }

            if (GeometRi3D.AlmostEqual(this.A, this.B) && GeometRi3D.AlmostEqual(this.A, this.C))
            {
                // Ellipsoid is sphere
                if (GeometRi3D.AlmostEqual(e.A, e.B) && GeometRi3D.AlmostEqual(e.A, e.C))
                {
                    // Second ellipsoid also sphere
                    return GeometRi3D.AlmostEqual(this.A, e.A);
                }
                else
                {
                    return false;
                }
            }
            else if (GeometRi3D.AlmostEqual(this.A, this.B) && GeometRi3D.AlmostEqual(e.A, e.B))
            {
                return GeometRi3D.AlmostEqual(this.A, e.A) && GeometRi3D.AlmostEqual(this.C, e.C) &&
                       e.SemiaxisC.IsParallelTo(this.SemiaxisC);
            }
            else if (GeometRi3D.AlmostEqual(this.A, this.C) && GeometRi3D.AlmostEqual(e.A, e.C))
            {
                return GeometRi3D.AlmostEqual(this.A, e.A) && GeometRi3D.AlmostEqual(this.B, e.B) &&
                       e.SemiaxisB.IsParallelTo(this.SemiaxisB);
            }
            else if (GeometRi3D.AlmostEqual(this.C, this.B) && GeometRi3D.AlmostEqual(e.C, e.B))
            {
                return GeometRi3D.AlmostEqual(this.A, e.A) && GeometRi3D.AlmostEqual(this.C, e.C) &&
                       e.SemiaxisA.IsParallelTo(this.SemiaxisA);
            }
            else
            {
                return GeometRi3D.AlmostEqual(this.A, e.A) && e.SemiaxisA.IsParallelTo(this.SemiaxisA) &&
                       GeometRi3D.AlmostEqual(this.B, e.B) && e.SemiaxisB.IsParallelTo(this.SemiaxisB) &&
                       GeometRi3D.AlmostEqual(this.C, e.C) && e.SemiaxisC.IsParallelTo(this.SemiaxisC);
            }
        }

        /// <summary>
        /// Returns the hashcode for the object.
        /// </summary>
        public override int GetHashCode()
        {
            return GeometRi3D.HashFunction(_point.GetHashCode(), _v1.GetHashCode(), _v2.GetHashCode(), _v3.GetHashCode());
        }

        /// <summary>
        /// String representation of an object in global coordinate system.
        /// </summary>
        public override String ToString()
        {
            return ToString(Coord3d.GlobalCS);
        }

        /// <summary>
        /// String representation of an object in reference coordinate system.
        /// </summary>
        public String ToString(Coord3d coord)
        {
            string nl = System.Environment.NewLine;

            if (coord == null) { coord = Coord3d.GlobalCS; }
            Point3d P = _point.ConvertTo(coord);
            Vector3d v1 = _v1.ConvertTo(coord);
            Vector3d v2 = _v2.ConvertTo(coord);
            Vector3d v3 = _v3.ConvertTo(coord);

            string str = string.Format("Ellipsoid: ") + nl;
            str += string.Format("  Center -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", P.X, P.Y, P.Z) + nl;
            str += string.Format("  Semiaxis A -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", v1.X, v1.Y, v1.Z) + nl;
            str += string.Format("  Semiaxis B -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", v2.X, v2.Y, v2.Z) + nl;
            str += string.Format("  Semiaxis C -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", v3.X, v3.Y, v3.Z) + nl;
            return str;
        }

        // Operators overloads
        //-----------------------------------------------------------------

        public static bool operator ==(Ellipsoid c1, Ellipsoid c2)
        {
            return c1.Equals(c2);
        }
        public static bool operator !=(Ellipsoid c1, Ellipsoid c2)
        {
            return !c1.Equals(c2);
        }
    }
}
