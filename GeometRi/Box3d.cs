using System;
using static System.Math;
using System.Collections.Generic;

namespace GeometRi
{
    /// <summary>
    /// Arbitrary oriented 3D box, can be degenerated with one or more dimensions equal 0.
    /// </summary>
    public class Box3d : FiniteObject, IFiniteObject
    {

        private Point3d _center;
        private double _lx, _ly, _lz;
        private Rotation _r;

        #region "Constructors"

        /// <summary>
        /// Default constructor, initializes unit box in the origin of the global coordinate system aligned with coordinate axes.
        /// </summary>
        public Box3d(Point3d center, double lx, double ly, double lz)
        {
            _center = center.Copy();
            _lx = lx;
            _ly = ly;
            _lz = lz;
            _r = new Rotation();
        }

        /// <summary>
        /// Initializes unit box in the origin of the reference coordinate system aligned with coordinate axes.
        /// </summary>
        /// <param name="coord">Reference coordinate system.</param>
        public Box3d(Coord3d coord = null)
        {
            coord = (coord == null) ? Coord3d.GlobalCS : coord;
            _center = new Point3d(coord);
            _lx = _ly = _lz = 1.0;
            _r = new Rotation(coord);
        }

        /// <summary>
        /// Initializes box with specified dimensions and orientation defined by rotation object.
        /// </summary>
        /// <param name="center">Center point of the box.</param>
        /// <param name="lx">First dimension.</param>
        /// <param name="ly">Second dimension.</param>
        /// <param name="lz">Third dimension.</param>
        /// <param name="r">Orientation of the box, defined as rotation from axis aligned position (in global CS) to final position.</param>
        public Box3d(Point3d center, double lx, double ly, double lz, Rotation r)
        {
            _center = center.Copy();
            _lx = lx;
            _ly = ly;
            _lz = lz;
            _r = r.Copy();
        }

        /// <summary>
        /// Initializes axis aligned box in local coordinate system.
        /// </summary>
        /// <param name="center">Center point of the box.</param>
        /// <param name="lx">First dimension.</param>
        /// <param name="ly">Second dimension.</param>
        /// <param name="lz">Third dimension.</param>
        /// <param name="coord">Local coordinate system.</param>
        public Box3d(Point3d center, double lx, double ly, double lz, Coord3d coord)
        {
            _center = center.Copy();
            _lx = lx;
            _ly = ly;
            _lz = lz;
            _r = new Rotation(coord);
        }
        #endregion

        /// <summary>
        /// Creates copy of the object
        /// </summary>
        public Box3d Copy()
        {
            return new Box3d(_center, _lx, _ly, _lz, _r);
        }

        #region "Properties"
        /// <summary>
        /// Center point of the box.
        /// </summary>
        public Point3d Center
        {
            get { return _center.Copy(); }
            set { _center = value.Copy(); }
        }

        /// <summary>
        /// First dimension.
        /// </summary>
        public double L1
        {
            get { return _lx; }
            set { _lx = value; }
        }

        /// <summary>
        /// Second dimension.
        /// </summary>
        public double L2
        {
            get { return _ly; }
            set { _ly = value; }
        }

        /// <summary>
        /// Third dimension.
        /// </summary>
        public double L3
        {
            get { return _lz; }
            set { _lz = value; }
        }

        /// <summary>
        /// Orientation of the first dimension of the box.
        /// </summary>
        public Vector3d V1
        {
            get { return _r.ConvertToGlobal().ToRotationMatrix.Column1; }
        }

        /// <summary>
        /// Orientation of the second dimension of the box.
        /// </summary>
        public Vector3d V2
        {
            get { return _r.ConvertToGlobal().ToRotationMatrix.Column2; }
        }

        /// <summary>
        /// Orientation of the third dimension of the box.
        /// </summary>
        public Vector3d V3
        {
            get { return _r.ConvertToGlobal().ToRotationMatrix.Column3; }
        }

        /// <summary>
        /// Orientation of the box, defined as rotation from axis aligned position (in global CS) to final position.
        /// </summary>
        public Rotation Orientation
        {
            get { return _r.Copy(); }
            set { _r = value.Copy(); }
        }

        /// <summary>
        /// Corner point of the box.
        /// </summary>
        public Point3d P1
        {
            get
            {
                return _center.ConvertToGlobal() + 0.5 * (
                    - _lx * _r.ConvertToGlobal().ToRotationMatrix.Column1.ToPoint
                    - _ly * _r.ConvertToGlobal().ToRotationMatrix.Column2.ToPoint
                    - _lz * _r.ConvertToGlobal().ToRotationMatrix.Column3.ToPoint);
            }
        }

        /// <summary>
        /// Corner point of the box.
        /// </summary>
        public Point3d P2
        {
            get
            {
                return _center.ConvertToGlobal() + 0.5 * (
                    + _lx * _r.ConvertToGlobal().ToRotationMatrix.Column1.ToPoint
                    - _ly * _r.ConvertToGlobal().ToRotationMatrix.Column2.ToPoint
                    - _lz * _r.ConvertToGlobal().ToRotationMatrix.Column3.ToPoint);
            }
        }

        /// <summary>
        /// Corner point of the box.
        /// </summary>
        public Point3d P3
        {
            get
            {
                return _center.ConvertToGlobal() + 0.5 * (
                    + _lx * _r.ConvertToGlobal().ToRotationMatrix.Column1.ToPoint
                    + _ly * _r.ConvertToGlobal().ToRotationMatrix.Column2.ToPoint
                    - _lz * _r.ConvertToGlobal().ToRotationMatrix.Column3.ToPoint);
            }
        }

        /// <summary>
        /// Corner point of the box.
        /// </summary>
        public Point3d P4
        {
            get
            {
                return _center.ConvertToGlobal() + 0.5 * (
                    - _lx * _r.ConvertToGlobal().ToRotationMatrix.Column1.ToPoint
                    + _ly * _r.ConvertToGlobal().ToRotationMatrix.Column2.ToPoint
                    - _lz * _r.ConvertToGlobal().ToRotationMatrix.Column3.ToPoint);
            }
        }

        /// <summary>
        /// Corner point of the box.
        /// </summary>
        public Point3d P5
        {
            get
            {
                return _center.ConvertToGlobal() + 0.5 * (
                    - _lx * _r.ConvertToGlobal().ToRotationMatrix.Column1.ToPoint
                    - _ly * _r.ConvertToGlobal().ToRotationMatrix.Column2.ToPoint
                    + _lz * _r.ConvertToGlobal().ToRotationMatrix.Column3.ToPoint);
            }
        }

        /// <summary>
        /// Corner point of the box.
        /// </summary>
        public Point3d P6
        {
            get
            {
                return _center.ConvertToGlobal() + 0.5 * (
                    + _lx * _r.ConvertToGlobal().ToRotationMatrix.Column1.ToPoint
                    - _ly * _r.ConvertToGlobal().ToRotationMatrix.Column2.ToPoint
                    + _lz * _r.ConvertToGlobal().ToRotationMatrix.Column3.ToPoint);
            }
        }

        /// <summary>
        /// Corner point of the box.
        /// </summary>
        public Point3d P7
        {
            get
            {
                return _center.ConvertToGlobal() + 0.5 * (
                    + _lx * _r.ConvertToGlobal().ToRotationMatrix.Column1.ToPoint
                    + _ly * _r.ConvertToGlobal().ToRotationMatrix.Column2.ToPoint
                    + _lz * _r.ConvertToGlobal().ToRotationMatrix.Column3.ToPoint);
            }
        }

        /// <summary>
        /// Corner point of the box.
        /// </summary>
        public Point3d P8
        {
            get
            {
                return _center.ConvertToGlobal() + 0.5 * (
                    - _lx * _r.ConvertToGlobal().ToRotationMatrix.Column1.ToPoint
                    + _ly * _r.ConvertToGlobal().ToRotationMatrix.Column2.ToPoint
                    + _lz * _r.ConvertToGlobal().ToRotationMatrix.Column3.ToPoint);
            }
        }

        /// <summary>
        /// List of corner points.
        /// </summary>
        public List<Point3d> ListOfPoints
        {
            get { return new List<Point3d> { P1, P2, P3, P4, P5, P6, P7, P8 }; }
        }

        /// <summary>
        /// List of triangles forming the box's surface
        /// </summary>
        public List<Triangle> ListOfTriangles
        {
            get
            {
                List<Triangle> list = new List<Triangle> { };
                list.Add(new Triangle(P1, P3, P2));
                list.Add(new Triangle(P1, P4, P3));

                list.Add(new Triangle(P1, P2, P6));
                list.Add(new Triangle(P1, P6, P5));

                list.Add(new Triangle(P2, P3, P7));
                list.Add(new Triangle(P2, P7, P6));

                list.Add(new Triangle(P3, P4, P8));
                list.Add(new Triangle(P3, P8, P7));

                list.Add(new Triangle(P4, P1, P5));
                list.Add(new Triangle(P4, P5, P8));

                list.Add(new Triangle(P5, P6, P7));
                list.Add(new Triangle(P5, P7, P8));

                return list;
            }
        }

        /// <summary>
        /// Volume of the box.
        /// </summary>
        public double Volume
        {
            get { return _lx * _ly * _lz; }
        }

        /// <summary>
        /// Surface area of the box.
        /// </summary>
        public double Area
        {
            get { return 2.0 * (_lx*_ly + _lx*_lz + _ly*_lz); }
        }

        /// <summary>
        /// Length of the box diagonal.
        /// </summary>
        public double Diagonal
        {
            get { return Sqrt(_lx * _lx + _ly * _ly + _lz * _lz); }
        }

        /// <summary>
        /// True if box is axis aligned
        /// </summary>
        public bool IsAxisAligned
        {
            get { return _r.ToRotationMatrix.IsIdentity; }
        }

        #endregion

        #region "BoundingBox"
        /// <summary>
        /// Return minimum bounding box.
        /// </summary>
        public Box3d MinimumBoundingBox
        {
            get { return this.Copy(); }
        }

        /// <summary>
        /// Return Axis Aligned Bounding Box (AABB) in given coordinate system.
        /// </summary>
        public Box3d BoundingBox(Coord3d coord = null)
        {
            coord = (coord == null) ? Coord3d.GlobalCS : coord;
            Point3d c = _center.ConvertTo(coord);
            double mx = c.X;
            double my = c.Y;
            double mz = c.Z;

            foreach (Point3d p in this.ListOfPoints)
            {
                Point3d t = p.ConvertTo(coord);
                if (t.X < mx) mx = t.X;
                if (t.Y < my) my = t.Y;
                if (t.Z < mz) mz = t.Z;
            }

            return new Box3d(c, 2.0 * (c.X - mx), 2.0 * (c.Y - my), 2.0 * (c.Z - mz), coord);
        }

        /// <summary>
        /// Return bounding sphere.
        /// </summary>
        public Sphere BoundingSphere
        {
            get
            {
                double r = 0.5 * Sqrt(_lx * _lx + _ly * _ly + _lz * _lz);
                return new Sphere(this.Center, r);
            }
        }
        #endregion

        #region "Intersection"
        /// <summary>
        /// Get intersection of line with box.
        /// Returns 'null' (no intersection) or object of type 'Point3d' or 'Segment3d'.
        /// </summary>
        public object IntersectionWith(Line3d l)
        {
            return _line_intersection(l, double.NegativeInfinity, double.PositiveInfinity);
        }

        /// <summary>
        /// Get intersection of ray with box.
        /// Returns 'null' (no intersection) or object of type 'Point3d' or 'Segment3d'.
        /// </summary>
        public object IntersectionWith(Ray3d r)
        {
            return _line_intersection(r.ToLine, 0.0, double.PositiveInfinity);
        }

        /// <summary>
        /// Get intersection of segment with box.
        /// Returns 'null' (no intersection) or object of type 'Point3d' or 'Segment3d'.
        /// </summary>
        public object IntersectionWith(Segment3d s)
        {
            return _line_intersection(s.ToLine, 0.0, s.Length);
        }

        private object _line_intersection(Line3d l, double t0, double t1)
        {
            // Smith's algorithm:
            // "An Efficient and Robust Ray–Box Intersection Algorithm"
            // Amy Williams, Steve Barrus, R. Keith Morley, Peter Shirley
            // http://www.cs.utah.edu/~awilliam/box/box.pdf

            // Modified to allow tolerance based checks
            
            // Relative tolerance ================================
            if (!GeometRi3D.UseAbsoluteTolerance)
            {
                double tol = GeometRi3D.Tolerance;
                GeometRi3D.Tolerance = tol * this.Diagonal;
                GeometRi3D.UseAbsoluteTolerance = true;
                object result = _line_intersection(l, t0, t1);
                GeometRi3D.UseAbsoluteTolerance = false;
                GeometRi3D.Tolerance = tol;
                return result;
            }
            //====================================================

            // Define local CS aligned with box
            Coord3d local_CS = new Coord3d(this.Center, this.Orientation.ConvertToGlobal().ToRotationMatrix, "local_CS");

            Point3d Pmin = this.P1.ConvertTo(local_CS);
            Point3d Pmax = this.P7.ConvertTo(local_CS);

            l = new Line3d(l.Point.ConvertTo(local_CS), l.Direction.ConvertTo(local_CS).Normalized);

            double tmin, tmax, tymin, tymax, tzmin, tzmax;
            double divx = 1 / l.Direction.X;
            if (divx >= 0)
            {
                tmin = (Pmin.X - l.Point.X) * divx;
                tmax = (Pmax.X - l.Point.X) * divx;
            }
            else
            {
                tmin = (Pmax.X - l.Point.X) * divx;
                tmax = (Pmin.X - l.Point.X) * divx;
            }

            double divy = 1 / l.Direction.Y;
            if (divy >= 0)
            {
                tymin = (Pmin.Y - l.Point.Y) * divy;
                tymax = (Pmax.Y - l.Point.Y) * divy;
            }
            else
            {
                tymin = (Pmax.Y - l.Point.Y) * divy;
                tymax = (Pmin.Y - l.Point.Y) * divy;
            }

            if (GeometRi3D.Greater(tmin, tymax) || GeometRi3D.Greater(tymin, tmax))
                return null;
            if (GeometRi3D.Greater(tymin, tmin))
                tmin = tymin;
            if (GeometRi3D.Smaller(tymax, tmax))
                tmax = tymax;

            double divz = 1 / l.Direction.Z;
            if (divz >= 0)
            {
                tzmin = (Pmin.Z - l.Point.Z) * divz;
                tzmax = (Pmax.Z - l.Point.Z) * divz;
            }
            else
            {
                tzmin = (Pmax.Z - l.Point.Z) * divz;
                tzmax = (Pmin.Z - l.Point.Z) * divz;
            }

            if (GeometRi3D.Greater(tmin, tzmax) || GeometRi3D.Greater(tzmin, tmax))
                return null;
            if (GeometRi3D.Greater(tzmin, tmin))
                tmin = tzmin;
            if (GeometRi3D.Smaller(tzmax, tmax))
                tmax = tzmax;

            // Now check the overlapping portion of the segments
            // This part is missing in the original algorithm
            if (GeometRi3D.Greater(tmin, t1))
                return null;
            if (GeometRi3D.Smaller(tmax, t0))
                return null;

            if (GeometRi3D.Smaller(tmin, t0))
                tmin = t0;
            if (GeometRi3D.Greater(tmax, t1))
                tmax = t1;

            if (GeometRi3D.AlmostEqual(tmin, tmax))
            {
                return l.Point.Translate(tmin * l.Direction);
            }
            else
            {
                return new Segment3d(l.Point.Translate(tmin * l.Direction), l.Point.Translate(tmax * l.Direction));
            }
        }
        #endregion


        /// <summary>
        /// Local coordinate system with origin in box's center and aligned with box
        /// </summary>
        public Coord3d LocalCoord()
        {
            return new Coord3d(_center, _r.ConvertToGlobal().ToRotationMatrix.Transpose());
        }

        /// <summary>
        /// Point on box (including interior points) closest to target point "p".
        /// </summary>
        public Point3d ClosestPoint(Point3d p)
        {
            Coord3d local_coord = this.LocalCoord();
            p = p.ConvertTo(local_coord);
            double x = GeometRi3D.Clamp(p.X, -_lx / 2, _lx / 2);
            double y = GeometRi3D.Clamp(p.Y, -_ly / 2, _ly / 2);
            double z = GeometRi3D.Clamp(p.Z, -_lz / 2, _lz / 2);

            return new Point3d(x, y, z, local_coord);
        }

        /// <summary>
        /// Distance from box to point (zero will be returned for point located inside box)
        /// </summary>
        public double DistanceTo(Point3d p)
        {
            return ClosestPoint(p).DistanceTo(p);
        }

        /// <summary>
        /// Shortest distance from box to sphere
        /// </summary>
        public double DistanceTo(Sphere s)
        {
            Point3d p = this.ClosestPoint(s.Center);
            double dist = p.DistanceTo(s.Center);
            return dist <= s.R ? 0.0 : dist - s.R;
        }

        /// <summary>
        /// Shortest distance from box to circle
        /// </summary>
        public double DistanceTo(Circle3d c)
        {
            if (c.Center.IsInside(this))
            {
                return 0;
            }
            double min_dist = Double.PositiveInfinity;
            foreach (Triangle triangle in ListOfTriangles)
            {
                double dist = c.DistanceTo(triangle);
                if (dist < min_dist) min_dist = dist;
            }
            return min_dist;
        }

        /// <summary>
        /// Intersection check between circle and box
        /// </summary>
        public bool Intersects(Circle3d c)
        {
            if (c.Center.IsInside(this)) return true;
            if (c.Center.DistanceTo(this) > c.R) return false;

            foreach (Triangle triangle in ListOfTriangles)
            {
                if (c.Intersects(triangle)) return true;
            }
            return false;
        }

        internal override int _PointLocation(Point3d p)
        {
            Coord3d coord = new Coord3d(this.Center, this.V1, this.V2);
            p = p.ConvertTo(coord);
            if (GeometRi3D.UseAbsoluteTolerance)
            {
                if ( (Abs(p.X)-L1/2) <= GeometRi3D.Tolerance && (Abs(p.Y) - L2 / 2) <= GeometRi3D.Tolerance && (Abs(p.Z) - L3 / 2) <= GeometRi3D.Tolerance )
                {
                    if ( (Abs(p.X) - L1 / 2) < -GeometRi3D.Tolerance && (Abs(p.Y) - L2 / 2) < -GeometRi3D.Tolerance && (Abs(p.Z) - L3 / 2) < -GeometRi3D.Tolerance)
                    {
                        return 1; // Point is strictly inside box
                    } else
                    {
                        return 0; // Point is on boundary
                    }
                } else
                {
                    return -1; // Point is outside
                }
            }
            else
            {
                double tol = GeometRi3D.Tolerance;
                GeometRi3D.Tolerance = tol * this.Diagonal;
                GeometRi3D.UseAbsoluteTolerance = true;
                int result = this._PointLocation(p);
                GeometRi3D.UseAbsoluteTolerance = false;
                GeometRi3D.Tolerance = tol;
                return result;
            }
        }

        #region "TranslateRotateReflect"
        /// <summary>
        /// Translate box by a vector
        /// </summary>
        public Box3d Translate(Vector3d v)
        {
            return new Box3d(_center.Translate(v), _lx, _ly, _lz);
        }

        /// <summary>
        /// Rotate box around point 'p' as a rotation center.
        /// </summary>
        public Box3d Rotate(Rotation r, Point3d p)
        {
            Point3d new_center = r.ToRotationMatrix * (this._center - p) + p;
            Rotation new_rotation = r * this.Orientation;
            return new Box3d(new_center, _lx, _ly, _lz, new_rotation);
        }

        /// <summary>
        /// Reflect box in given point
        /// <para>The order of corner points will be changed during reflection operation.</para>
        /// </summary>
        public virtual Box3d ReflectIn(Point3d p)
        {
            Point3d new_center = this.Center.ReflectIn(p);
            return new Box3d(new_center, _lx, _ly, _lz, this._r);
        }

        /// <summary>
        /// Reflect box in given line
        /// <para>The order of corner points will be changed during reflection operation.</para>
        /// </summary>
        public virtual Box3d ReflectIn(Line3d l)
        {
            Point3d new_center = this.Center.ReflectIn(l);
            Rotation r = new GeometRi.Rotation(l.Direction, PI);
            Rotation new_rotation = r * this.Orientation;
            return new Box3d(new_center, _lx, _ly, _lz, new_rotation);
        }

        /// <summary>
        /// Reflect box in given plane
        /// <para>The order of corner points will be changed during reflection operation.</para>
        /// </summary>
        public virtual Box3d ReflectIn(Plane3d s)
        {
            Point3d new_center = this.Center.ReflectIn(s);
            Vector3d nV1 = this.V1.ReflectIn(s);
            Vector3d nV2 = this.V2.ReflectIn(s);
            Coord3d coord = new Coord3d(new_center, nV1, nV2);
            Rotation new_rotation = new Rotation(coord);
            return new Box3d(new_center, _lx, _ly, _lz, new_rotation);
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
            Box3d b = (Box3d)obj;

            if (GeometRi3D.UseAbsoluteTolerance)
            {
                return this.Center == b.Center && _r == b.Orientation &&
                       GeometRi3D.AlmostEqual(L1, b.L1) &&
                       GeometRi3D.AlmostEqual(L2, b.L2) &&
                       GeometRi3D.AlmostEqual(L3, b.L3);
            }
            else
            {
                double tol = GeometRi3D.Tolerance;
                GeometRi3D.Tolerance = tol * this.Diagonal;
                GeometRi3D.UseAbsoluteTolerance = true;
                bool result = this.Equals(b);
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
            int hash_code = GeometRi3D.HashFunction(_lx.GetHashCode(), _ly.GetHashCode(), _lz.GetHashCode());
            return GeometRi3D.HashFunction(_center.GetHashCode(), _r.GetHashCode(), hash_code);
        }

        /// <summary>
        /// String representation of an object in global coordinate system.
        /// </summary>
        public override string ToString()
        {
            return ToString(Coord3d.GlobalCS);
        }

        /// <summary>
        /// String representation of an object in reference coordinate system.
        /// </summary>
        public string ToString(Coord3d coord)
        {
            string nl = System.Environment.NewLine;

            if (coord == null) { coord = Coord3d.GlobalCS; }
            Point3d p = _center.ConvertTo(coord);

            string str = string.Format("Box3d (reference coord.sys. ") + coord.Name + "):" + nl;
            str += string.Format("Center -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", p.X, p.Y, p.Z) + nl;
            str += string.Format("Lx, Ly, Lz -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", _lx, _ly, _lz) + nl;
            str += _r.ToString(coord);
            return str;
        }

        // Operators overloads
        //-----------------------------------------------------------------
        public static bool operator ==(Box3d b1, Box3d b2)
        {
            if (object.ReferenceEquals(b1, null))
                return object.ReferenceEquals(b2, null);
            return b1.Equals(b2);
        }
        public static bool operator !=(Box3d b1, Box3d b2)
        {
            if (object.ReferenceEquals(b1, null))
                return !object.ReferenceEquals(b2, null);
            return !b1.Equals(b2);
        }
    }
}
