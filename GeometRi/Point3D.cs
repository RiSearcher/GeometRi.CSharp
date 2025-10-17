using System;
using static System.Math;

namespace GeometRi
{
    /// <summary>
    /// Point in 3D space defined in global or local reference frame.
    /// </summary>
#if NET20_OR_GREATER
    [Serializable]
#endif
    public class Point3d
    {

        private double _x;
        private double _y;
        private double _z;
        internal Coord3d _coord;

        #region "Constructors"
        /// <summary>
        /// Default constructor, initializes zero point.
        /// </summary>
        /// <param name="coord">Reference coordinate system (default - Coord3d.GlobalCS).</param>
        public Point3d(Coord3d coord = null)
        {
            _x = 0.0;
            _y = 0.0;
            _z = 0.0;
            _coord = (coord == null) ? Coord3d.GlobalCS : coord;
        }

        /// <summary>
        /// Initiaizes point object using coordinates.
        /// </summary>
        /// <param name="coord">Reference coordinate system (default - Coord3d.GlobalCS).</param>
        public Point3d(double x, double y, double z, Coord3d coord = null)
        {
            _x = x;
            _y = y;
            _z = z;
            _coord = (coord == null) ? Coord3d.GlobalCS : coord;
        }

        /// <summary>
        /// Initiaizes point object using double array.
        /// </summary>
        /// <param name="coord">Reference coordinate system (default - Coord3d.GlobalCS).</param>
        public Point3d(double[] a, Coord3d coord = null)
        {
            if (a.GetUpperBound(0) < 2)
                throw new Exception("Point3d: Array size mismatch");
            _x = a[0];
            _y = a[1];
            _z = a[2];
            _coord = (coord == null) ? Coord3d.GlobalCS : coord;
        }
        #endregion

        /// <summary>
        /// Creates copy of the object
        /// </summary>
        public Point3d Copy()
        {
            return new Point3d(_x,_y,_z,_coord);
        }

        internal bool HasChanged { get; private set; }

        /// <summary>
        /// X coordinate in reference coordinate system
        /// </summary>
        public double X
        {
            get { return _x; }
            set { _x = value; HasChanged = true; }
        }
        /// <summary>
        /// Y coordinate in reference coordinate system
        /// </summary>
        public double Y
        {
            get { return _y; }
            set { _y = value; HasChanged = true;}
        }
        /// <summary>
        /// Z coordinate in reference coordinate system
        /// </summary>
        public double Z
        {
            get { return _z; }
            set { _z = value; HasChanged = true;}
        }

        /// <summary>
        ///  Reference coordinate system
        /// </summary>
        public Coord3d Coord
        {
            get { return _coord; }
        }

        /// <summary>
        /// Radius vector of point (in global coordinate system)
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
            Point3d p = this.ConvertToGlobal();
            if (coord == null || object.ReferenceEquals(coord, Coord3d.GlobalCS))
                return p;

            p = coord.Axes * (p - coord.Origin);
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
                Point3d p = _coord.Axes.TransposeMult(this);
                p._coord = Coord3d.GlobalCS;

                return p + _coord.Origin;
            }

        }

        public Point3d Add(Point3d p)
        {
            if ((this._coord != p._coord))
                p = p.ConvertTo(this._coord);
            return new Point3d(_x + p._x, _y + p._y, _z + p._z, _coord);
        }
        internal Point3d Add(Point3d p, double a, double b)
        {
            if ((this._coord != p._coord))
                p = p.ConvertTo(this._coord);
            return new Point3d(a * this.X + b * p.X, a * this.Y + b * p.Y, a * this.Z + b * p.Z, _coord);
        }

        public Point3d Add(Vector3d p)
        {
            if ((this._coord != p._coord))
                p = p.ConvertTo(this._coord);
            return new Point3d(_x + p.X, _y + p.Y, _z + p.Z, _coord);
        }
        public Point3d Subtract(Point3d p)
        {
            if ((this._coord != p._coord))
                p = p.ConvertTo(this._coord);
            return new Point3d(_x - p._x, _y - p._y, _z - p._z, _coord);
        }
        internal Point3d Subtract(Point3d p, double a, double b)
        {
            if ((this._coord != p._coord))
                p = p.ConvertTo(this._coord);
            return new Point3d(a * this.X - b * p.X, a * this.Y - b * p.Y, a * this.Z - b * p.Z, _coord);
        }
        public Point3d Subtract(Vector3d p)
        {
            if ((this._coord != p._coord))
                p = p.ConvertTo(this._coord);
            return new Point3d(_x - p.X, _y - p.Y, _z - p.Z, _coord);
        }
        internal Point3d Subtract(Vector3d v, double a)
        {
            if ((this._coord != v._coord))
                v = v.ConvertTo(this._coord);
            return new Point3d(_x - a * v.X, _y - a * v.Y, _z - a * v.Z, _coord);
        }
        public Point3d Scale(double a)
        {
            return new Point3d(a * _x, a * _y, a * _z, _coord);
        }
        internal double Dot(Point3d p)
        {
            if ((this._coord != p._coord))
                p = p.ConvertTo(this._coord);
            return this.X * p.X + this.Y * p.Y + this.Z * p.Z;
        }
        internal double Dot(Vector3d p)
        {
            if ((this._coord != p._coord))
                p = p.ConvertTo(this._coord);
            return this.X * p.X + this.Y * p.Y + this.Z * p.Z;
        }
        internal Point3d Cross(Point3d v)
        {
            if ((this._coord != v._coord))
                v = v.ConvertTo(this._coord);
            double x = this.Y * v.Z - this.Z * v.Y;
            double y = this.Z * v.X - this.X * v.Z;
            double z = this.X * v.Y - this.Y * v.X;
            return new Point3d(x, y, z, _coord); ;
        }

        //internal (double x, double y, double z) _TSubtract(Point3d p)
        //{
        //    if ((this._coord != p._coord))
        //        p = p.ConvertTo(this._coord);
        //    return (_x - p._x, _y - p._y, _z - p._z);
        //}

        //internal double Dot(double x, double y, double z)
        //{
        //    return this.X * x + this.Y * y + this.Z * z;
        //}

        //internal Point3d _TAdd(ValueTuple<double, double, double> p)
        //{
        //    return new Point3d(_x + p.Item1, _y + p.Item2, _z + p.Item3, _coord);
        //}

        #region "DistanceTo"
        /// <summary>
        /// Returns distance between two points
        /// </summary>
        public double DistanceTo(Point3d p)
        {
            if ((this._coord != p._coord))
                p = p.ConvertTo(this._coord);
            return Sqrt((this._x - p._x) * (this._x - p._x) + (this._y - p._y) * (this._y - p._y) + (this._z - p._z) * (this._z - p._z));
        }

        /// <summary>
        /// Returns squared distance between two points
        /// </summary>
        public double DistanceSquared(Point3d p)
        {
            if ((this._coord != p._coord))
                p = p.ConvertTo(this._coord);
            return (this._x - p._x) * (this._x - p._x) + (this._y - p._y) * (this._y - p._y) + (this._z - p._z) * (this._z - p._z);
        }

        /// <summary>
        /// Returns shortest distance to the line
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public double DistanceTo(Line3d l)
        {
            Vector3d v = new Vector3d(this, l.Point);
            return v.Cross(l.Direction).Norm;
        }

        public double DistanceSquared(Line3d l)
        {
            Vector3d v = new Vector3d(this, l.Point);
            return v.Cross(l.Direction).NormSquared;
        }

        /// <summary>
        /// Returns shortest distance from point to the plane
        /// </summary>
        public double DistanceTo(Plane3d s)
        {
            return Math.Abs(s._normal * new Vector3d(s._point, this));
        }

        public double DistanceSquared(Plane3d s)
        {
            double dist = Math.Abs(s._normal * new Vector3d(s._point, this));
            return dist * dist;
        }

        /// <summary>
        /// Returns shortest distance from point to the ray
        /// </summary>
        public double DistanceTo(Ray3d r)
        {
            double t = r.Direction * new Vector3d(r.Point, this);
            if (t > 0)
            {
                return this.DistanceTo(r.Point + t * r.Direction);
            }
            else
            {
                return this.DistanceTo(r.Point);
            }
        }

        public double DistanceSquared(Ray3d r)
        {
            double t = r.Direction * new Vector3d(r.Point, this);
            if (t > 0)
            {
                return this.DistanceSquared(r.Point + t * r.Direction);
            }
            else
            {
                return this.DistanceSquared(r.Point);
            }
        }

        /// <summary>
        /// Shortest distance from point to the segment
        /// </summary>
        public double DistanceTo(Segment3d s)
        {

            Point3d dir = s.P2 - s.P1;
            double t0 = dir.Dot(this - s.P1);

            //(double x1, double y1, double z1) = s.P2._TSubtract(s.P1);
            //(double x2, double y2, double z2) = this._TSubtract(s.P1);
            //double t0 = x1 * x2 + y1 * y2 + z1 * z2;

            if (t0 <= 0)
            {
                return this.DistanceTo(s.P1);
            }

            double dir_sqr = dir.Dot(dir);
            //double dir_sqr = x1 * x1 + y1 * y1 + z1 * z1;
            t0 = t0 / dir_sqr;
            if (t0 >= 1)
            {
                return this.DistanceTo(s.P2);
            }
            else
            {
                //Point3d p = s.P1._TAdd((t0 * x1, t0  *y1, t0 * z1));
                //return this.DistanceTo(p);
                return this.DistanceTo(s.P1 + t0 * dir);
            }
        }

        /// <summary>
        /// Shortest distance from point to the segment
        /// </summary>
        public double DistanceTo(Segment3d s, out Point3d closest_point)
        {
            // Segment S = P1 + t * (P2-P1)
            //Vector3d dir = new Vector3d(s.P1, s.P2);
            //double t0 = dir * new Vector3d(s.P1, this);
            Point3d dir = s.P2 - s.P1;
            double t0 = dir.Dot(this - s.P1);

            if (t0 <= 0)
            {
                closest_point = s.P1;
                return this.DistanceTo(s.P1);
            }

            //double dir_sqr = dir * dir;
            double dir_sqr = dir.Dot(dir);
            t0 = t0 / dir_sqr;
            if (t0 >= 1)
            {
                closest_point = s.P2;
                return this.DistanceTo(s.P2);
            }
            else
            {
                closest_point = s.P1 + t0 * dir;
                return this.DistanceTo(closest_point);
            }

        }

        /// <summary>
        /// Squared distance from point to the segment
        /// </summary>
        public double DistanceSquared(Segment3d s)
        {
            // Segment S = P1 + t * (P2-P1)
            Vector3d dir = new Vector3d(s.P1, s.P2);
            double t0 = dir * new Vector3d(s.P1, this);

            if (t0 <= 0)
            {
                return this.DistanceSquared(s.P1);
            }

            double dir_sqr = dir * dir;
            if (dir_sqr > 0)
            {
                t0 = t0 / dir_sqr;
                if (t0 >= 1)
                {
                    return this.DistanceSquared(s.P2);
                }
                else
                {
                    return this.DistanceSquared(s.P1 + t0 * dir);
                }
            }
            else
            {
                return this.DistanceSquared(s.P1);
            }
        }

        /// <summary>
        /// Shortest distance between point and sphere (including interior points).
        /// </summary>
        public double DistanceTo(Sphere s)
        {
            return s.DistanceTo(this);
        }

        /// <summary>
        /// Shortest distance from point to circle (including interior points)
        /// </summary>
        public double DistanceTo(Circle3d c)
        {
            return c.DistanceTo(this);
        }

        /// <summary>
        /// Distance from box to point (zero will be returned for point located inside box)
        /// </summary>
        public double DistanceTo(Box3d box)
        {
            return box.ClosestPoint(this).DistanceTo(this);
        }

        /// <summary>
        /// Distance from AABB to point (zero will be returned for point located inside box)
        /// </summary>
        public double DistanceTo(AABB box)
        {
            return box.ClosestPoint(this).DistanceTo(this);
        }

        /// <summary>
        /// Shortest distance from point to triangle (including interior points)
        /// </summary>
        public double DistanceTo(Triangle t)
        {
            return t.DistanceTo(this);
        }

        public double DistanceSquared(Triangle t)
        {
            return t.DistanceSquared(this);
        }

        /// <summary>
        /// Shortest distance from point to triangle (including interior points)
        /// </summary>
        public double DistanceTo(Triangle t, out Point3d closest_point)
        {
            double dist = t.DistanceTo(this, out closest_point);
            return dist;
        }

        #endregion

        /// <summary>
        /// Closest point on circle (including interior points).
        /// </summary>
        public Point3d ClosestPoint(Circle3d c)
        {
            return c.ClosestPoint(this);
        }

        /// <summary>
        /// Closest point on box (including interior points).
        /// </summary>
        public Point3d ClosestPoint(Box3d box)
        {
            return box.ClosestPoint(this);
        }

        /// <summary>
        /// Closest point on triangle.
        /// </summary>
        public Point3d ClosestPoint(Triangle t)
        {
            return t.ClosestPoint(this);
        }

        /// <summary>
        /// Point on sphere's surface closest to target point "p".
        /// </summary>
        public Point3d ClosestPoint(Sphere s)
        {
            return s.ClosestPoint(this);
        }


        /// <summary>
        /// Returns orthogonal projection of the point to the plane
        /// </summary>
        public Point3d ProjectionTo(Plane3d s)
        {
            Vector3d delta = new Vector3d(s._point, this);
            return this - (s._normal * delta) * s._normal;
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
        /// <para>Test if point is located in the epsilon neighborhood of the line.</para>
        /// <para>Epsilon neighborhood is defined by a GeometRi3D.Tolerance property.</para>
        /// </summary>
        public bool BelongsTo(Line3d l)
        {
            return this.DistanceTo(l) <= GeometRi3D.Tolerance;
        }

        /// <summary>
        /// <para>Test if point is located in the epsilon neighborhood of the ray.</para>
        /// <para>Epsilon neighborhood is defined by a GeometRi3D.Tolerance property.</para>
        /// </summary>
        public bool BelongsTo(Ray3d l)
        {
            if (this == l.Point)
            {
                return true;
            }

            if (this.DistanceTo(l.ToLine) <= GeometRi3D.Tolerance)
            {
                Point3d proj = this.ProjectionTo(l.ToLine);
                if (new Vector3d(l.Point, proj).AngleTo(l.Direction) < PI / 4)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// <para>Test if point is located in the epsilon neighborhood of the plane.</para>
        /// <para>Epsilon neighborhood is defined by a GeometRi3D.Tolerance property.</para>
        /// </summary>
        public bool BelongsTo(Plane3d s)
        {
            s.SetCoord(this.Coord);
            if (GeometRi3D.UseAbsoluteTolerance)
            {
                return Abs(s.A * X + s.B * Y + s.C * Z + s.D) / Sqrt(s.A*s.A + s.B * s.B + s.C * s.C) < GeometRi3D.Tolerance;
            }
            else
            {
                double d = this.DistanceTo(this._coord.Origin);
                if (d > 0.0)
                {
                    return Abs(s.A * X + s.B * Y + s.C * Z + s.D) / Sqrt(s.A * s.A + s.B * s.B + s.C * s.C) / d < GeometRi3D.Tolerance;
                }
                else
                {
                    return Abs(s.A * X + s.B * Y + s.C * Z + s.D) / Sqrt(s.A * s.A + s.B * s.B + s.C * s.C) < GeometRi3D.Tolerance;
                }
            }
        }




        // =========================================================================
        #region "Point location"

        /// <summary>
        /// <para>Test if point is located in the epsilon neighborhood of the object.</para>
        /// <para>Epsilon neighborhood is defined by a GeometRi3D.Tolerance property.</para>
        /// <para>For relative tolerance tests a fraction of the typical object's dimension is used to define epsilon neighborhood.</para>
        /// </summary>
        public bool BelongsTo(FiniteObject obj)
        {
            if (obj._PointLocation(this) >= 0) { return true; }
            return false;
        }

        /// <summary>
        /// <para>Test if point is located strictly inside (not in the epsilon neighborhood of the boundary) of the object.</para>
        /// <para>Epsilon neighborhood is defined by a GeometRi3D.Tolerance property.</para>
        /// <para>For relative tolerance tests a fraction of the typical object's dimension is used to define epsilon neighborhood.</para>
        /// </summary>
        public bool IsInside(FiniteObject obj)
        {
            if (obj._PointLocation(this) == 1) { return true; }
            return false;
        }

        /// <summary>
        /// <para>Test if point is located outside of the epsilon neighborhood of the object.</para>
        /// <para>Epsilon neighborhood is defined by a GeometRi3D.Tolerance property.</para>
        /// <para>For relative tolerance tests a fraction of the typical object's dimension is used to define epsilon neighborhood.</para>
        /// </summary>        
        public bool IsOutside(FiniteObject obj)
        {
            if (obj._PointLocation(this) < 0) { return true; }
            return false;
        }

        /// <summary>
        /// <para>Test if point is located in the epsilon neighborhood of the object's boundary.</para>
        /// <para>Epsilon neighborhood is defined by a GeometRi3D.Tolerance property.</para>
        /// <para>For relative tolerance tests a fraction of the typical object's dimension is used to define epsilon neighborhood.</para>
        /// </summary>
        public bool IsOnBoundary(FiniteObject obj)
        {
            if (obj._PointLocation(this) == 0) { return true; }
            return false;
        }

        #endregion
        // =========================================================================



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


            if (GeometRi3D.UseAbsoluteTolerance)
            {
                return this.DistanceSquared(p) < GeometRi3D.Tolerance * GeometRi3D.Tolerance;
            }
            else
            {
                if (this.DistanceSquared(_coord.Origin) < GeometRi3D.Tolerance * GeometRi3D.Tolerance)
                {
                    return this.DistanceSquared(p) < GeometRi3D.Tolerance * GeometRi3D.Tolerance;
                }
                else
                {
                    return this.DistanceSquared(p) < Math.Pow(GeometRi3D.Tolerance * this.DistanceTo(_coord.Origin), 2);
                }
            }
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
        public static Point3d operator +(Point3d v, Vector3d a)
        {
            return v.Add(a);
        }
        public static Point3d operator -(Point3d v, Point3d a)
        {
            return v.Subtract(a);
        }
        public static Point3d operator -(Point3d v, Vector3d a)
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
            if (object.ReferenceEquals(p1, null))
                return object.ReferenceEquals(p2, null);
            return p1.Equals(p2);
        }
        public static bool operator !=(Point3d p1, Point3d p2)
        {
            if (object.ReferenceEquals(p1, null))
                return !object.ReferenceEquals(p2, null);
            return !p1.Equals(p2);
        }

    }
}



