using System;
using static System.Math;

namespace GeometRi
{
    /// <summary>
    /// Point in 3D space defined in global or local reference frame.
    /// </summary>
    public class Point3d
    {

        private double _x;
        private double _y;
        private double _z;
        private Coord3d _coord;

        #region "Constructors"
        public Point3d(Coord3d coord = null)
        {
            _x = 0.0;
            _y = 0.0;
            _z = 0.0;
            if (coord != null)
            {
                _coord = coord;
            }
            else
            {
                _coord = Coord3d.GlobalCS;
            }
        }
        public Point3d(double x, double y, double z, Coord3d coord = null)
        {
            _x = x;
            _y = y;
            _z = z;
            if (coord != null)
            {
                _coord = coord;
            }
            else
            {
                _coord = Coord3d.GlobalCS;
            }
        }

        public Point3d(double[] a, Coord3d coord = null)
        {
            if (a.GetUpperBound(0) < 2)
                throw new Exception("Point3d: Array size mismatch");
            _x = a[0];
            _y = a[1];
            _z = a[2];
            if (coord != null)
            {
                _coord = coord;
            }
            else
            {
                _coord = Coord3d.GlobalCS;
            }
        }
        #endregion

        /// <summary>
        /// Creates copy of the object
        /// </summary>
        public Point3d Copy()
        {
            return new Point3d(_x,_y,_z,_coord);
        }

        /// <summary>
        /// X coordinate in reference coordinate system
        /// </summary>
        public double X
        {
            get { return _x; }
            set { _x = value; }
        }
        /// <summary>
        /// Y coordinate in reference coordinate system
        /// </summary>
        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }
        /// <summary>
        /// Z coordinate in reference coordinate system
        /// </summary>
        public double Z
        {
            get { return _z; }
            set { _z = value; }
        }

        /// <summary>
        ///  Reference coordinate system
        /// </summary>
        public Coord3d Coord
        {
            get { return _coord; }
        }

        /// <summary>
        /// Radius vector of point
        /// </summary>
        public Vector3d ToVector
        {
            get { return new Vector3d(this); }
        }

        /// <summary>
        /// Convert point to reference coordinate system
        /// </summary>
        public Point3d ConvertTo(Coord3d coord)
        {
            Point3d p = this.Copy();

            p = p.ConvertToGlobal();
            if (coord == null)
                return p;
            if (object.ReferenceEquals(coord, Coord3d.GlobalCS))
                return p;



            Vector3d v = default(Vector3d);
            // If coord is cloned from GlobalCS, its Origin does not have a reference to coord.sys.
            if (coord.Origin == new Point3d(0, 0, 0))
            {
                v = new Vector3d(p);
            }
            else
            {
                v = new Vector3d(coord.Origin, p);
            }
            p.X = v.Dot(coord.Xaxis);
            p.Y = v.Dot(coord.Yaxis);
            p.Z = v.Dot(coord.Zaxis);
            p._coord = coord;


            return p;
        }
        /// <summary>
        /// Convert point to global coordinate system
        /// </summary>
        /// <returns></returns>
        public Point3d ConvertToGlobal()
        {
            if (_coord == null || object.ReferenceEquals(_coord, Coord3d.GlobalCS))
            {
                return this;
            }
            else
            {
                Vector3d v = new Vector3d(this.X, this.Y, this.Z);
                v = _coord.Axes.Inverse() * v;

                return v.ToPoint + _coord.Origin;

            }

        }

        public Point3d Add(Point3d p)
        {
            if ((this._coord != p._coord))
                p = p.ConvertTo(this._coord);
            Point3d tmp = this.Copy();
            tmp.X += p.X;
            tmp.Y += p.Y;
            tmp.Z += p.Z;
            return tmp;
        }
        public Point3d Subtract(Point3d p)
        {
            if ((this._coord != p._coord))
                p = p.ConvertTo(this._coord);
            Point3d tmp = this.Copy();
            tmp.X -= p.X;
            tmp.Y -= p.Y;
            tmp.Z -= p.Z;
            return tmp;
        }
        public Point3d Scale(double a)
        {
            Point3d tmp = this.Copy();
            tmp.X *= a;
            tmp.Y *= a;
            tmp.Z *= a;
            return tmp;
        }

        #region "DistanceTo"
        /// <summary>
        /// Returns distance between two points
        /// </summary>
        public double DistanceTo(Point3d p)
        {
            if ((this._coord != p._coord))
                p = p.ConvertTo(this._coord);
            return new Vector3d(this, p).Norm;
        }

        /// <summary>
        /// Returns shortest distance to the line
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public double DistanceTo(Line3d l)
        {
            Vector3d v = new Vector3d(this, l.Point);
            return v.Cross(l.Direction).Norm / l.Direction.Norm;
        }

        /// <summary>
        /// Returns shortest distance from point to the plane
        /// </summary>
        public double DistanceTo(Plane3d s)
        {
            s.SetCoord(this.Coord);
            return Abs(X * s.A + Y * s.B + Z * s.C + s.D) / Sqrt(Math.Pow(s.A, 2) + Math.Pow(s.B, 2) + Math.Pow(s.C, 2));
        }

        /// <summary>
        /// Returns shortest distance from point to the ray
        /// </summary>
        public double DistanceTo(Ray3d r)
        {
            if (this.ProjectionTo(r.ToLine).BelongsTo(r))
            {
                return this.DistanceTo(r.ToLine);
            }
            else
            {
                return this.DistanceTo(r.Point);
            }
        }

        /// <summary>
        /// Returns shortest distance from point to the segment
        /// </summary>
        public double DistanceTo(Segment3d s)
        {
            if (this.ProjectionTo(s.ToLine).BelongsTo(s))
            {
                return this.DistanceTo(s.ToLine);
            }
            else
            {
                return Min(this.DistanceTo(s.P1), this.DistanceTo(s.P2));
            }
        }

        #endregion


        /// <summary>
        /// Returns orthogonal projection of the point to the plane
        /// </summary>
        public Point3d ProjectionTo(Plane3d s)
        {
            Vector3d r0 = new Vector3d(this);
            s.SetCoord(this.Coord);
            Vector3d n = new Vector3d(s.A, s.B, s.C, _coord);
            r0 = r0 - (r0 * n + s.D) / (n * n) * n;
            return r0.ToPoint;
        }
        /// <summary>
        /// Returns orthogonal projection of the point to the line
        /// </summary>
        public Point3d ProjectionTo(Line3d l)
        {
            Vector3d r0 = new Vector3d(this);
            Vector3d r1 = l.Point.ToVector;
            Vector3d s = l.Direction;
            r0 = r1 - ((r1 - r0) * s) / (s * s) * s;
            return r0.ToPoint;
        }

        /// <summary>
        /// Returns orthogonal projection of the point to the surface of the sphere
        /// </summary>
        public Point3d ProjectionTo(Sphere s)
        {
            Vector3d v = new Vector3d(s.Center, this);
            return s.Center + s.R * v.Normalized.ToPoint;
        }

        /// <summary>
        /// Check if point belongs to the line
        /// </summary>
        /// <param name="l"></param>
        /// <returns>True, if the point belongs to the line</returns>
        public bool BelongsTo(Line3d l)
        {
            if (this == l.Point)
            {
                return true;
            }
            else
            {
                return l.Direction.IsParallelTo(new Vector3d(this, l.Point));
            }
        }

        /// <summary>
        /// Check if point belongs to the ray
        /// </summary>
        /// <param name="l"></param>
        /// <returns>True, if the point belongs to the ray</returns>
        public bool BelongsTo(Ray3d l)
        {
            if (this == l.Point)
            {
                return true;
            }
            else
            {
                Vector3d v = new Vector3d(l.Point, this);
                if (l.Direction.Normalized == v.Normalized)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Check if point belongs to the segment
        /// </summary>
        /// <returns>True, if the point belongs to the segment</returns>
        public bool BelongsTo(Segment3d s)
        {
            return this.BelongsTo(s.ToRay) && this.BelongsTo(new Ray3d(s.P2, new Vector3d(s.P2, s.P1)));
        }

        /// <summary>
        /// Check if point belongs to the plane
        /// </summary>
        /// <returns>True, if the point belongs to the plane</returns>
        public bool BelongsTo(Plane3d s)
        {
            s.SetCoord(this.Coord);
            return Abs(s.A * X + s.B * Y + s.C * Z + s.D) < GeometRi3D.Tolerance;
        }

        /// <summary>
        /// Check if point belongs to the circle
        /// </summary>
        /// <returns>True, if the point belongs to the circle</returns>
        public bool BelongsTo(Circle3d c)
        {
            return GeometRi3D.AlmostEqual(this.DistanceTo(c.Center), c.R) && c.Normal.IsOrthogonalTo(new Vector3d(c.Center, this));
        }

        /// <summary>
        /// Check if point belongs to the ellipse
        /// </summary>
        /// <returns>True, if the point belongs to the ellipse</returns>
        public bool BelongsTo(Ellipse e)
        {
            if (this.BelongsTo(new Plane3d(e.Center, e.MajorSemiaxis, e.MinorSemiaxis)))
            {
                if ((GeometRi3D.AlmostEqual(this.DistanceTo(e.F1) + this.DistanceTo(e.F2), 2 * e.A)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if point belongs to the sphere surface
        /// </summary>
        /// <returns>True, if the point belongs to the sphere surface</returns>
        public bool BelongsTo(Sphere s)
        {
            return GeometRi3D.AlmostEqual(this.DistanceTo(s.Center), s.R);
        }

        /// <summary>
        /// Check if point belongs to the ellipsoid surface
        /// </summary>
        public bool BelongsTo(Ellipsoid e)
        {
            Coord3d lc = new Coord3d(e.Center,e.SemiaxisA, e.SemiaxisB);
            Point3d p = this.ConvertTo(lc);
            return GeometRi3D.AlmostEqual(p.X * p.X / e.A / e.A + p.Y * p.Y / e.B / e.B + p.Z * p.Z / e.C / e.C, 1.0);
        }

        /// <summary>
        /// Check if point is inside ellipsoid
        /// </summary>
        public bool IsInside(Ellipsoid e)
        {
            Coord3d lc = new Coord3d(e.Center, e.SemiaxisA, e.SemiaxisB);
            Point3d p = this.ConvertTo(lc);
            return GeometRi3D.Smaller(p.X * p.X / e.A / e.A + p.Y * p.Y / e.B / e.B + p.Z * p.Z / e.C / e.C, 1.0);
        }


        /// <summary>
        /// Check if point is inside circle
        /// </summary>
        /// <returns>True, if the point is inside circle</returns>
        public bool IsInside(Circle3d c)
        {
            if (this.BelongsTo(new Plane3d(c.Center, c.Normal)))
            {
                return GeometRi3D.Smaller(this.DistanceTo(c.Center), c.R);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if point is inside ellipse
        /// </summary>
        /// <returns>True, if the point is inside ellipse</returns>
        public bool IsInside(Ellipse e)
        {
            if (this.BelongsTo(new Plane3d(e.Center, e.MajorSemiaxis, e.MinorSemiaxis)))
            {
                return GeometRi3D.Smaller(this.DistanceTo(e.F1) + this.DistanceTo(e.F2), 2 * e.A);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if point is inside sphere
        /// </summary>
        /// <returns>True, if the point is inside sphere</returns>
        public bool IsInside(Sphere s)
        {
            return this.DistanceTo(s.Center) < s.R - GeometRi3D.Tolerance;
        }

        #region "TranslateRotateReflect"
        /// <summary>
        /// Translate point by a vector
        /// </summary>
        public Point3d Translate(Vector3d v)
        {
            if ((this._coord != v.Coord))
                v = v.ConvertTo(this._coord);
            return this + v.ToPoint;
        }

        /// <summary>
        /// Rotate point by a given rotation matrix
        /// </summary>
        [System.Obsolete("use Rotation object and specify rotation center: this.Rotate(Rotation r, Point3d p)")]
        public Point3d Rotate(Matrix3d m)
        {
            return m * this;
        }

        /// <summary>
        /// Rotate point by a given rotation matrix around point 'p' as a rotation center
        /// </summary>
        [System.Obsolete("use Rotation object: this.Rotate(Rotation r, Point3d p)")]
        public Point3d Rotate(Matrix3d m, Point3d p)
        {
            if (this._coord != p.Coord)
                p = p.ConvertTo(this._coord);
            return m * (this - p) + p;
        }

        /// <summary>
        /// Rotate object around origin in object's reference coordinate system.
        /// </summary>
        public Point3d Rotate(Rotation r)
        {
            if (this._coord != r.Coord) r = r.ConvertTo(this._coord);
            return r.ToRotationMatrix * this;
        }

        /// <summary>
        /// Rotate point around point 'p' as a rotation center.
        /// </summary>
        public Point3d Rotate(Rotation r, Point3d p)
        {
            if (this._coord != r.Coord) r = r.ConvertTo(this._coord);
            if (this._coord != p.Coord) p = p.ConvertTo(this._coord);
            return r.ToRotationMatrix * (this - p) + p;
        }

        /// <summary>
        /// Reflect point in given point
        /// </summary>
        public Point3d ReflectIn(Point3d p)
        {
            if ((this._coord != p.Coord))
                p = p.ConvertTo(this._coord);
            Vector3d v = new Vector3d(this, p);
            return this.Translate(2 * v);
        }

        /// <summary>
        /// Reflect point in given line
        /// </summary>
        public Point3d ReflectIn(Line3d l)
        {
            Vector3d v = new Vector3d(this, this.ProjectionTo(l));
            return this.Translate(2 * v);
        }

        /// <summary>
        /// Reflect point in given plane
        /// </summary>
        public Point3d ReflectIn(Plane3d s)
        {
            Vector3d v = new Vector3d(this, this.ProjectionTo(s));
            return this.Translate(2 * v);
        }
        #endregion

        /// <summary>
        /// Check if three points are collinear
        /// </summary>
        public static bool CollinearPoints(Point3d A, Point3d B, Point3d C)
        {
            Vector3d v1 = new Vector3d(A, B);
            Vector3d v2 = new Vector3d(A, C);
            if (v1.Cross(v2).Norm < GeometRi3D.Tolerance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether two objects are equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null || (!object.ReferenceEquals(this.GetType(), obj.GetType())))
            {
                return false;
            }
            Point3d p = (Point3d)obj;
            if ((this._coord != p.Coord))
                p = p.ConvertTo(_coord);
            return this.DistanceTo(p) < GeometRi3D.Tolerance;
        }

        /// <summary>
        /// Returns the hashcode for the object.
        /// </summary>
        public override int GetHashCode()
        {
            return GeometRi3D.HashFunction(_x.GetHashCode(), _y.GetHashCode(), _z.GetHashCode(), _coord.GetHashCode());
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
            if (coord == null) { coord = Coord3d.GlobalCS; }
            Point3d p = this.ConvertTo(coord);

            string str = string.Format("Point3d -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", p.X, p.Y, p.Z) + System.Environment.NewLine;
            return str;
        }


        // Operators overloads
        //-----------------------------------------------------------------
        public static Point3d operator +(Point3d v, Point3d a)
        {
            return v.Add(a);
        }
        public static Point3d operator -(Point3d v, Point3d a)
        {
            return v.Subtract(a);
        }
        public static Point3d operator -(Point3d v)
        {
            return v.Scale(-1.0);
        }
        public static Point3d operator *(Point3d v, double a)
        {
            return v.Scale(a);
        }
        public static Point3d operator *(double a, Point3d v)
        {
            return v.Scale(a);
        }
        public static Point3d operator /(Point3d v, double a)
        {
            return v.Scale(1.0 / a);
        }

        public static bool operator ==(Point3d p1, Point3d p2)
        {
            return p1.Equals(p2);
        }
        public static bool operator !=(Point3d p1, Point3d p2)
        {
            return !p1.Equals(p2);
        }

    }
}



