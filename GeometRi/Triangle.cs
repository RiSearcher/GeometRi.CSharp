using System;
using static System.Math;

namespace GeometRi
{
    public class Triangle
    {

        private Point3d _a;
        private Point3d _b;
        private Point3d _c;

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
        public Triangle Rotate(Matrix3d m)
        {
            return new Triangle(_a.Rotate(m), _b.Rotate(m), _c.Rotate(m));
        }

        /// <summary>
        /// Rotate triangle by a given rotation matrix around point 'p' as a rotation center
        /// </summary>
        public Triangle Rotate(Matrix3d m, Point3d p)
        {
            return new Triangle(_a.Rotate(m, p), _b.Rotate(m, p), _c.Rotate(m, p));
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

            if ((this.A == t.A || this.A == t.B || this.A == t.C) && (this.B == t.A || this.B == t.B || this.B == t.C) && (this.C == t.A || this.C == t.B || this.C == t.C))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the hascode for the object.
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

