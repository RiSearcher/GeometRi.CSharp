using System;
using static System.Math;

namespace GeometRi
{
    /// <summary>
    /// Triangle in 3D space defined by three points.
    /// </summary>
    public class Triangle : FiniteObject, IPlanarObject
    {

        private Point3d _a;
        private Point3d _b;
        private Point3d _c;

        /// <summary>
        /// Initializes triangle object using three points.
        /// </summary>
        public Triangle(Point3d A, Point3d B, Point3d C)
        {
            if (Point3d.CollinearPoints(A, B, C))
            {
                throw new Exception("Collinear points");
            }
            _a = A.Copy();
            _b = B.Copy();
            _c = C.Copy();
        }

        /// <summary>
        /// Creates copy of the object
        /// </summary>
        public Triangle Copy()
        {
            return new Triangle(_a,_b,_c);
        }

        #region "Properties"
        /// <summary>
        /// First point of triangle
        /// </summary>
        public Point3d A
        {
            get { return _a.Copy(); }
            set
            {
                if (Point3d.CollinearPoints(value, _b, _c))
                {
                    throw new Exception("Collinear points");
                }
                _a = value.Copy();
            }
        }

        /// <summary>
        /// Second point of triangle
        /// </summary>
        public Point3d B
        {
            get { return _b.Copy(); }
            set
            {
                if (Point3d.CollinearPoints(_a, value, _c))
                {
                    throw new Exception("Collinear points");
                }
                _b = value.Copy();
            }
        }

        /// <summary>
        /// Third point of triangle
        /// </summary>
        public Point3d C
        {
            get { return _c.Copy(); }
            set
            {
                if (Point3d.CollinearPoints(_a, _b, value))
                {
                    throw new Exception("Collinear points");
                }
                _c = value.Copy();
            }
        }

        /// <summary>
        /// Length of AB side
        /// </summary>
        public double AB
        {
            get { return _a.DistanceTo(_b); }
        }

        /// <summary>
        /// Length of AC side
        /// </summary>
        public double AC
        {
            get { return _a.DistanceTo(_c); }
        }

        /// <summary>
        /// Length of BC side
        /// </summary>
        public double BC
        {
            get { return _b.DistanceTo(_c); }
        }

        /// <summary>
        /// Perimeter of the triangle
        /// </summary>
        public double Perimeter
        {
            get { return AB + BC + AC; }
        }

        /// <summary>
        /// Area of the triangle
        /// </summary>
        public double Area
        {
            get
            {
                Vector3d v1 = new Vector3d(_a, _b);
                Vector3d v2 = new Vector3d(_a, _c);
                return 0.5 * v1.Cross(v2).Norm;
            }
        }

        public Vector3d Normal
        {
            get { return new Vector3d(A, B).Cross(new Vector3d(A, C)); }
        }

        public bool IsOriented
        {
            get { return false; }
        }

        /// <summary>
        /// Circumcircle of the triangle
        /// </summary>
        public Circle3d Circumcircle
        {
            get { return new Circle3d(_a, _b, _c); }
        }

        /// <summary>
        /// Angle at the vertex A
        /// </summary>
        public double Angle_A
        {
            get { return new Vector3d(_a, _b).AngleTo(new Vector3d(_a, _c)); }
        }

        /// <summary>
        /// Angle at the vertex B
        /// </summary>
        public double Angle_B
        {
            get { return new Vector3d(_b, _a).AngleTo(new Vector3d(_b, _c)); }
        }

        /// <summary>
        /// Angle at the vertex C
        /// </summary>
        public double Angle_C
        {
            get { return new Vector3d(_c, _a).AngleTo(new Vector3d(_c, _b)); }
        }

        /// <summary>
        /// Angle bisector at the vertex A
        /// </summary>
        public Segment3d Bisector_A
        {
            get
            {
                Point3d p = _b + (_c - _b) / (1 + AC / AB);
                return new Segment3d(_a, p);
            }
        }

        /// <summary>
        /// Angle bisector at the vertex B
        /// </summary>
        public Segment3d Bisector_B
        {
            get
            {
                Point3d p = _c + (_a - _c) / (1 + AB / BC);
                return new Segment3d(_b, p);
            }
        }

        /// <summary>
        /// Angle bisector at the vertex C
        /// </summary>
        public Segment3d Bisector_C
        {
            get
            {
                Point3d p = _a + (_b - _a) / (1 + BC / AC);
                return new Segment3d(_c, p);
            }
        }

        /// <summary>
        /// Incenter of the triangle
        /// </summary>
        public Point3d Incenter
        {
            get { return Bisector_A.ToLine.PerpendicularTo(Bisector_B.ToLine); }
        }

        /// <summary>
        /// Centroid of the triangle
        /// </summary>
        public Point3d Centroid
        {
            get { return new Point3d((A.X + B.X + C.X) / 3, (A.Y + B.Y + C.Y) / 3, (A.Z + B.Z + C.Z) / 3); }
        }

        /// <summary>
        /// Orthocenter of the triangle
        /// </summary>
        public Point3d Orthocenter
        {
            get { return Altitude_A.ToLine.PerpendicularTo(Altitude_B.ToLine); }
        }

        /// <summary>
        /// Circumcenter of the triangle
        /// </summary>
        public Point3d Circumcenter
        {
            get { return new Circle3d(_a, _b, _c).Center; }
        }

        /// <summary>
        /// Incircle of the triangle
        /// </summary>
        public Circle3d Incircle
        {
            get
            {
                Point3d p = Bisector_A.ToLine.PerpendicularTo(Bisector_B.ToLine);
                double r = 2 * Area / Perimeter;
                Vector3d v = new Vector3d(_a, _b).Cross(new Vector3d(_a, _c));
                return new Circle3d(p, r, v);
            }
        }

        /// <summary>
        /// Altitude at the vertex A
        /// </summary>
        public Segment3d Altitude_A
        {
            get
            {
                Point3d p = _a.ProjectionTo(new Line3d(_b, _c));
                return new Segment3d(_a, p);
            }
        }

        /// <summary>
        /// Altitude at the vertex B
        /// </summary>
        public Segment3d Altitude_B
        {
            get
            {
                Point3d p = _b.ProjectionTo(new Line3d(_a, _c));
                return new Segment3d(_b, p);
            }
        }

        /// <summary>
        /// Altitude at the vertex C
        /// </summary>
        public Segment3d Altitude_C
        {
            get
            {
                Point3d p = _c.ProjectionTo(new Line3d(_a, _b));
                return new Segment3d(_c, p);
            }
        }

        /// <summary>
        /// Median at the vertex A
        /// </summary>
        public Segment3d Median_A
        {
            get { return new Segment3d(_a, (_b + _c) / 2); }
        }

        /// <summary>
        /// Median at the vertex B
        /// </summary>
        public Segment3d Median_B
        {
            get { return new Segment3d(_b, (_a + _c) / 2); }
        }

        /// <summary>
        /// Median at the vertex C
        /// </summary>
        public Segment3d Median_C
        {
            get { return new Segment3d(_c, (_a + _b) / 2); }
        }
        #endregion

        #region "TriangleProperties"
        /// <summary>
        /// True if all sides of the triangle are the same length
        /// </summary>
        public bool IsEquilateral
        {
            get { return GeometRi3D.AlmostEqual(AB, AC) && GeometRi3D.AlmostEqual(AB, BC); }
        }

        /// <summary>
        /// True if two sides of the triangle are the same length
        /// </summary>
        public bool IsIsosceles
        {
            get { return GeometRi3D.AlmostEqual(AB, AC) || GeometRi3D.AlmostEqual(AB, BC) || GeometRi3D.AlmostEqual(AC, BC); }
        }

        /// <summary>
        /// True if all sides are unequal
        /// </summary>
        public bool IsScalene
        {
            get { return GeometRi3D.NotEqual(AB, AC) && GeometRi3D.NotEqual(AB, BC) && GeometRi3D.NotEqual(AC, BC); }
        }

        /// <summary>
        /// True if one angle is equal 90 degrees
        /// </summary>
        public bool IsRight
        {
            get { return GeometRi3D.AlmostEqual(Angle_A, PI / 2) || GeometRi3D.AlmostEqual(Angle_B, PI / 2) || GeometRi3D.AlmostEqual(Angle_C, PI / 2); }
        }

        /// <summary>
        /// True if one angle is greater than 90 degrees
        /// </summary>
        public bool IsObtuse
        {
            get { return GeometRi3D.Greater(Angle_A, PI / 2) || GeometRi3D.Greater(Angle_B, PI / 2) || GeometRi3D.Greater(Angle_C, PI / 2); }
        }

        /// <summary>
        /// True if all angles are less than 90 degrees
        /// </summary>
        public bool IsAcute
        {
            get { return GeometRi3D.Smaller(Angle_A, PI / 2) && GeometRi3D.Smaller(Angle_B, PI / 2) && GeometRi3D.Smaller(Angle_C, PI / 2); }
        }
        #endregion

        #region "ParallelMethods"
        /// <summary>
        /// Check if two objects are parallel
        /// </summary>
        public bool IsParallelTo(ILinearObject obj)
        {
            return this.Normal.IsOrthogonalTo(obj.Direction);
        }

        /// <summary>
        /// Check if two objects are NOT parallel
        /// </summary>
        public bool IsNotParallelTo(ILinearObject obj)
        {
            return !this.Normal.IsOrthogonalTo(obj.Direction);
        }

        /// <summary>
        /// Check if two objects are orthogonal
        /// </summary>
        public bool IsOrthogonalTo(ILinearObject obj)
        {
            return this.Normal.IsParallelTo(obj.Direction);
        }

        /// <summary>
        /// Check if two objects are parallel
        /// </summary>
        public bool IsParallelTo(IPlanarObject obj)
        {
            return this.Normal.IsParallelTo(obj.Normal);
        }

        /// <summary>
        /// Check if two objects are NOT parallel
        /// </summary>
        public bool IsNotParallelTo(IPlanarObject obj)
        {
            return this.Normal.IsNotParallelTo(obj.Normal);
        }

        /// <summary>
        /// Check if two objects are orthogonal
        /// </summary>
        public bool IsOrthogonalTo(IPlanarObject obj)
        {
            return this.Normal.IsOrthogonalTo(obj.Normal);
        }
        #endregion

        #region "BoundingBox"
        /// <summary>
        /// Return minimum bounding box.
        /// </summary>
        public Box3d MinimumBoundingBox
        {
            get
            {
                double s1, s2, s3;
                s1 = AB * Altitude_C.Length;
                s2 = BC * Altitude_A.Length;
                s3 = AC * Altitude_B.Length;

                if (s1 <= s2 && s1 <= s3)
                {
                    Vector3d v1 = new Vector3d(_a, _b);
                    Vector3d v2 = new Vector3d(_a, _c);
                    Coord3d coord = new Coord3d(_a, v1, v2);
                    return this.BoundingBox(coord);
                }
                else if (s2 <= s3)
                {
                    Vector3d v1 = new Vector3d(_b, _c);
                    Vector3d v2 = new Vector3d(_b, _a);
                    Coord3d coord = new Coord3d(_b, v1, v2);
                    return this.BoundingBox(coord);
                }
                else
                {
                    Vector3d v1 = new Vector3d(_a, _c);
                    Vector3d v2 = new Vector3d(_a, _b);
                    Coord3d coord = new Coord3d(_a, v1, v2);
                    return this.BoundingBox(coord);
                }
            }
        }

        /// <summary>
        /// Return Axis Aligned Bounding Box (AABB) in given coordinate system.
        /// </summary>
        public Box3d BoundingBox(Coord3d coord = null)
        {
            coord = (coord == null) ? Coord3d.GlobalCS : coord;
            Line3d l1 = new Line3d(coord.Origin, coord.Xaxis);
            Line3d l2 = new Line3d(coord.Origin, coord.Yaxis);
            Line3d l3 = new Line3d(coord.Origin, coord.Zaxis);

            double px, py, pz, lx, ly, lz;
            Segment3d s = this.ProjectionTo(l1);
            px = (0.5 * (s.P1 + s.P2)).ConvertTo(coord).X;
            lx = s.Length;
            s = this.ProjectionTo(l2);
            py = (0.5 * (s.P1 + s.P2)).ConvertTo(coord).Y;
            ly = s.Length;
            s = this.ProjectionTo(l3);
            pz = (0.5 * (s.P1 + s.P2)).ConvertTo(coord).Z;
            lz = s.Length;

            return new Box3d(new Point3d(px, py, pz, coord), lx, ly, lz, coord);
        }

        /// <summary>
        /// Return bounding sphere.
        /// </summary>
        public Sphere BoundingSphere
        {
            get { return new Sphere(Circumcenter, Circumcenter.DistanceTo(_a)); }
        }
        #endregion

        /// <summary>
        /// Orthogonal projection of the triangle to line
        /// </summary>
        public Segment3d ProjectionTo(Line3d l)
        {
            double d12, d23, d13;
            Point3d p1, p2, p3;
            p1 = _a.ProjectionTo(l);
            p2 = _b.ProjectionTo(l);
            p3 = _c.ProjectionTo(l);
            d12 = p1.DistanceTo(p2);
            d13 = p1.DistanceTo(p3);
            d23 = p2.DistanceTo(p3);
            if (d12 >= d13 && d12 >= d23)
            {
                return new Segment3d(p1, p2);
            }
            else if (d13 >= d23)
            {
                return new Segment3d(p1, p3);
            }
            else
            {
                return new Segment3d(p2, p3);
            }
        }

        internal override int _PointLocation(Point3d p)
        {
            if (GeometRi3D.UseAbsoluteTolerance)
            {
                Plane3d s = new Plane3d(this.A, this.Normal);
                Point3d proj = p.ProjectionTo(s);
                if (GeometRi3D.AlmostEqual(p.DistanceTo(proj), 0))
                {
                    if (p.BelongsTo(new Segment3d(_a,_b)) || p.BelongsTo(new Segment3d(_a, _b)) || p.BelongsTo(new Segment3d(_a, _b)))
                    {
                        return 0; // Point is on boundary
                    }
                    else
                    {
                        double area = this.Area;
                        double alpha = new Vector3d(proj, _b).Cross(new Vector3d(proj, _c)).Norm / (2 * area);
                        double beta = new Vector3d(proj, _c).Cross(new Vector3d(proj, _a)).Norm / (2 * area);
                        double gamma = 1 - alpha - beta;
                        if ( 0 < alpha && alpha < 1 && 0 < beta && beta < 1 && 0 < gamma && gamma < 1 )
                        {
                            return 1; // Point is strictly inside
                        } else
                        {
                            return -1;
                        }
                    }
                }
                else
                {
                    return -1; // Point is outside
                }
            }
            else
            {
                double tol = GeometRi3D.Tolerance;
                GeometRi3D.Tolerance = tol * (AB+BC+AC) / 3;
                GeometRi3D.UseAbsoluteTolerance = true;
                int result = this._PointLocation(p);
                GeometRi3D.UseAbsoluteTolerance = false;
                GeometRi3D.Tolerance = tol;
                return result;
            }
        }

        #region "AngleTo"
        /// <summary>
        /// Angle between two objects in radians (0 &lt; angle &lt; Pi)
        /// </summary>
        public double AngleTo(ILinearObject obj)
        {
            return GeometRi3D.GetAngle(this, obj);
        }
        /// <summary>
        /// Angle between two objects in degrees (0 &lt; angle &lt; 180)
        /// </summary>
        public double AngleToDeg(ILinearObject obj)
        {
            return AngleTo(obj) * 180 / PI;
        }

        /// <summary>
        /// Angle between two objects in radians (0 &lt; angle &lt; Pi)
        /// </summary>
        public double AngleTo(IPlanarObject obj)
        {
            return GeometRi3D.GetAngle(this, obj);
        }
        /// <summary>
        /// Angle between two objects in degrees (0 &lt; angle &lt; 180)
        /// </summary>
        public double AngleToDeg(IPlanarObject obj)
        {
            return AngleTo(obj) * 180 / PI;
        }
        #endregion

        #region "TranslateRotateReflect"
        /// <summary>
        /// Translate triangle by a vector
        /// </summary>
        public Triangle Translate(Vector3d v)
        {
            return new Triangle(_a.Translate(v), _b.Translate(v), _c.Translate(v));
        }

        /// <summary>
        /// Rotate triangle by a given rotation matrix
        /// </summary>
        [System.Obsolete("use Rotation object and specify rotation center: this.Rotate(Rotation r, Point3d p)")]
        public Triangle Rotate(Matrix3d m)
        {
            return new Triangle(_a.Rotate(m), _b.Rotate(m), _c.Rotate(m));
        }

        /// <summary>
        /// Rotate triangle by a given rotation matrix around point 'p' as a rotation center
        /// </summary>
        [System.Obsolete("use Rotation object: this.Rotate(Rotation r, Point3d p)")]
        public Triangle Rotate(Matrix3d m, Point3d p)
        {
            return new Triangle(_a.Rotate(m, p), _b.Rotate(m, p), _c.Rotate(m, p));
        }

        /// <summary>
        /// Rotate triangle around point 'p' as a rotation center
        /// </summary>
        public Triangle Rotate(Rotation r, Point3d p)
        {
            return new Triangle(_a.Rotate(r, p), _b.Rotate(r, p), _c.Rotate(r, p));
        }

        /// <summary>
        /// Reflect triangle in given point
        /// </summary>
        public Triangle ReflectIn(Point3d p)
        {
            return new Triangle(_a.ReflectIn(p), _b.ReflectIn(p), _c.ReflectIn(p));
        }

        /// <summary>
        /// Reflect triangle in given line
        /// </summary>
        public Triangle ReflectIn(Line3d l)
        {
            return new Triangle(_a.ReflectIn(l), _b.ReflectIn(l), _c.ReflectIn(l));
        }

        /// <summary>
        /// Reflect triangle in given plane
        /// </summary>
        public Triangle ReflectIn(Plane3d s)
        {
            return new Triangle(_a.ReflectIn(s), _b.ReflectIn(s), _c.ReflectIn(s));
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
            Triangle t = (Triangle)obj;
            if (GeometRi3D.UseAbsoluteTolerance)
            {
                return ((this.A == t.A || this.A == t.B || this.A == t.C) &&
                        (this.B == t.A || this.B == t.B || this.B == t.C) &&
                        (this.C == t.A || this.C == t.B || this.C == t.C));
            }
            else
            {
                double tol = GeometRi3D.Tolerance;
                GeometRi3D.Tolerance = tol * this.Perimeter;
                GeometRi3D.UseAbsoluteTolerance = true;
                bool result = ((this.A == t.A || this.A == t.B || this.A == t.C) &&
                               (this.B == t.A || this.B == t.B || this.B == t.C) &&
                               (this.C == t.A || this.C == t.B || this.C == t.C));
                GeometRi3D.UseAbsoluteTolerance = false;
                GeometRi3D.Tolerance = tol;
                return result;
            }

        }

        /// <summary>
        /// Returns the hashcode for the object.
        /// </summary>
        public override int GetHashCode()
        {
            return GeometRi3D.HashFunction(_a.GetHashCode(), _b.GetHashCode(), _c.GetHashCode());
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
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            string nl = System.Environment.NewLine;

            if (coord == null) { coord = Coord3d.GlobalCS; }
            Point3d A = _a.ConvertTo(coord);
            Point3d B = _b.ConvertTo(coord);
            Point3d C = _c.ConvertTo(coord);

            str.Append("Triangle:" + nl);
            str.Append(string.Format("A -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", A.X, A.Y, A.Z) + nl);
            str.Append(string.Format("B -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", B.X, B.Y, B.Z) + nl);
            str.Append(string.Format("C -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", C.X, C.Y, C.Z));
            return str.ToString();
        }

        // Operators overloads
        //-----------------------------------------------------------------

        public static bool operator ==(Triangle t1, Triangle t2)
        {
            return t1.Equals(t2);
        }
        public static bool operator !=(Triangle t1, Triangle t2)
        {
            return !t1.Equals(t2);
        }
    }
}

