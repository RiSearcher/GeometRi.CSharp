using System;
using static System.Math;
using System.Collections.Generic;

namespace GeometRi
{
    /// <summary>
    /// Axis aligned 3D box, can be degenerated with one or more dimensions equal 0. Defined only in Global CS.
    /// </summary>
#if NET20_OR_GREATER
    [Serializable]
#endif
    public class AABB : FiniteObject, IFiniteObject
    {

        private Point3d _center;
        private double _lx, _ly, _lz;

        private List<Point3d> _list_p = null;
        private List<Triangle> _list_t = null;
        private List<Segment3d> _list_e = null;
        private List<Plane3d> _list_plane = null;
        private static object _lock = new object();

        #region "Constructors"

        /// <summary>
        /// Default constructor, initializes box in the origin of the global coordinate system aligned with coordinate axes.
        /// </summary>
        public AABB(Point3d center, double lx, double ly, double lz)
        {
            _center = center.ConvertToGlobal().Copy();
            _lx = lx;
            _ly = ly;
            _lz = lz;
        }

        /// <summary>
        /// Initializes unit box in the origin of the global coordinate system aligned with coordinate axes.
        /// </summary>
        public AABB()
        {
            _center = new Point3d();
            _lx = _ly = _lz = 1.0;
        }

        /// <summary>
        /// Initializes axis aligned box by two points.
        /// </summary>
        public AABB(Point3d Pmin, Point3d Pmax)
        {
            Point3d p1 = Pmin.ConvertToGlobal();
            Point3d p2 = Pmax.ConvertToGlobal();
            _center = (p1 + p2) / 2;
            _lx = Abs(p2.X - p1.X);
            _ly = Abs(p2.Y - p1.Y);
            _lz = Abs(p2.Z - p1.Z);
        }

        #endregion

        /// <summary>
        /// Creates copy of the object
        /// </summary>
        public AABB Copy()
        {
            return new AABB(_center, _lx, _ly, _lz);
        }

        #region "Properties"
        /// <summary>
        /// Center point of the box.
        /// </summary>
        public Point3d Center
        {
            get { return _center; }
            set
            {
                _center = value.Copy();
                _list_p = null;
                _list_t = null;
                _list_e = null;
                _list_plane = null;
            }
        }

        /// <summary>
        /// First dimension.
        /// </summary>
        public double L1
        {
            get { return _lx; }
            set
            {
                _lx = value;
                _list_p = null;
                _list_t = null;
                _list_e = null;
                _list_plane = null;
            }
        }

        /// <summary>
        /// Second dimension.
        /// </summary>
        public double L2
        {
            get { return _ly; }
            set
            {
                _ly = value;
                _list_p = null;
                _list_t = null;
                _list_e = null;
                _list_plane = null;
            }
        }

        /// <summary>
        /// Third dimension.
        /// </summary>
        public double L3
        {
            get { return _lz; }
            set
            {
                _lz = value;
                _list_p = null;
                _list_t = null;
                _list_e = null;
                _list_plane = null;
            }
        }

        /// <summary>
        /// Orientation of the first dimension of the box.
        /// </summary>
        public Vector3d V1
        {
            get { return Coord3d.GlobalCS.Xaxis; }
        }

        /// <summary>
        /// Orientation of the second dimension of the box.
        /// </summary>
        public Vector3d V2
        {
            get { return Coord3d.GlobalCS.Yaxis; }
        }

        /// <summary>
        /// Orientation of the third dimension of the box.
        /// </summary>
        public Vector3d V3
        {
            get { return Coord3d.GlobalCS.Zaxis; }
        }


        /// <summary>
        /// Corner point of the box.
        /// </summary>
        public Point3d P1
        {
            get
            {
                return new Point3d(_center.X - 0.5 * _lx, _center.Y - 0.5 * _ly, _center.Z - 0.5 * _lz);
            }
        }

        /// <summary>
        /// Corner point of the box.
        /// </summary>
        public Point3d P2
        {
            get
            {
                return new Point3d(_center.X + 0.5 * _lx, _center.Y - 0.5 * _ly, _center.Z - 0.5 * _lz);
            }
        }

        /// <summary>
        /// Corner point of the box.
        /// </summary>
        public Point3d P3
        {
            get
            {
                return new Point3d(_center.X + 0.5 * _lx, _center.Y + 0.5 * _ly, _center.Z - 0.5 * _lz);
            }
        }

        /// <summary>
        /// Corner point of the box.
        /// </summary>
        public Point3d P4
        {
            get
            {
                return new Point3d(_center.X - 0.5 * _lx, _center.Y + 0.5 * _ly, _center.Z - 0.5 * _lz);
            }
        }

        /// <summary>
        /// Corner point of the box.
        /// </summary>
        public Point3d P5
        {
            get
            {
                return new Point3d(_center.X - 0.5 * _lx, _center.Y - 0.5 * _ly, _center.Z + 0.5 * _lz);
            }
        }

        /// <summary>
        /// Corner point of the box.
        /// </summary>
        public Point3d P6
        {
            get
            {
                return new Point3d(_center.X + 0.5 * _lx, _center.Y - 0.5 * _ly, _center.Z + 0.5 * _lz);
            }
        }

        /// <summary>
        /// Corner point of the box.
        /// </summary>
        public Point3d P7
        {
            get
            {
                return new Point3d(_center.X + 0.5 * _lx, _center.Y + 0.5 * _ly, _center.Z + 0.5 * _lz);
            }
        }

        /// <summary>
        /// Corner point of the box.
        /// </summary>
        public Point3d P8
        {
            get
            {
                return new Point3d(_center.X - 0.5 * _lx, _center.Y + 0.5 * _ly, _center.Z + 0.5 * _lz);
            }
        }

        /// <summary>
        /// List of corner points.
        /// </summary>
        public List<Point3d> ListOfPoints
        {
            get
            {
                lock (_lock)
                {
                    if (_list_p == null)
                    {
                        _list_p = new List<Point3d> { P1, P2, P3, P4, P5, P6, P7, P8 };
                        return _list_p;
                    }
                    else
                    {
                        return _list_p;
                    }
                }
            }
        }

        /// <summary>
        /// List of triangles forming the box's surface
        /// </summary>
        public List<Triangle> ListOfTriangles
        {
            get
            {
                lock (_lock)
                {
                    if (_list_t == null)
                    {
                        Point3d P1 = this.P1;
                        Point3d P2 = this.P2;
                        Point3d P3 = this.P3;
                        Point3d P4 = this.P4;
                        Point3d P5 = this.P5;
                        Point3d P6 = this.P6;
                        Point3d P7 = this.P7;
                        Point3d P8 = this.P8;
                        _list_t = new List<Triangle> { };
                        _list_t.Add(new Triangle(P1, P3, P2));
                        _list_t.Add(new Triangle(P1, P4, P3));

                        _list_t.Add(new Triangle(P1, P2, P6));
                        _list_t.Add(new Triangle(P1, P6, P5));

                        _list_t.Add(new Triangle(P2, P3, P7));
                        _list_t.Add(new Triangle(P2, P7, P6));

                        _list_t.Add(new Triangle(P3, P4, P8));
                        _list_t.Add(new Triangle(P3, P8, P7));

                        _list_t.Add(new Triangle(P4, P1, P5));
                        _list_t.Add(new Triangle(P4, P5, P8));

                        _list_t.Add(new Triangle(P5, P6, P7));
                        _list_t.Add(new Triangle(P5, P7, P8));

                        return _list_t;
                    }
                    else
                    {
                        return _list_t;
                    }
                }
            }
        }

        /// <summary>
        /// List of planes forming the box's surface
        /// </summary>
        public List<Plane3d> ListOfPlanes
        {
            get
            {
                lock (_lock)
                {
                    if (_list_plane == null)
                    {
                        _list_plane = new List<Plane3d> { };
                        _list_plane.Add(new Plane3d(P1, -V1));
                        _list_plane.Add(new Plane3d(P2, V1));
                        _list_plane.Add(new Plane3d(P1, -V2));
                        _list_plane.Add(new Plane3d(P4, V2));
                        _list_plane.Add(new Plane3d(P1, -V3));
                        _list_plane.Add(new Plane3d(P5, V3));

                        return _list_plane;
                    }
                    else
                    {
                        return _list_plane;
                    }
                }
            }
        }

        /// <summary>
        /// List of edges forming the box
        /// </summary>
        public List<Segment3d> ListOfEdges
        {
            get
            {
                lock (_lock)
                {
                    if (_list_e == null)
                    {
                        _list_e = new List<Segment3d> { };
                        _list_e.Add(new Segment3d(P1, P2));
                        _list_e.Add(new Segment3d(P2, P3));
                        _list_e.Add(new Segment3d(P3, P4));
                        _list_e.Add(new Segment3d(P4, P1));
                        _list_e.Add(new Segment3d(P5, P6));
                        _list_e.Add(new Segment3d(P6, P7));
                        _list_e.Add(new Segment3d(P7, P8));
                        _list_e.Add(new Segment3d(P8, P5));
                        _list_e.Add(new Segment3d(P1, P5));
                        _list_e.Add(new Segment3d(P2, P6));
                        _list_e.Add(new Segment3d(P3, P7));
                        _list_e.Add(new Segment3d(P4, P8));

                        return _list_e;
                    }
                    else
                    {
                        return _list_e;
                    }
                }
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
            get { return 2.0 * (_lx * _ly + _lx * _lz + _ly * _lz); }
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
            get { return true; }
        }

        #endregion

        #region "BoundingBox"
        /// <summary>
        /// Return minimum bounding box.
        /// </summary>
        public Box3d MinimumBoundingBox
        {
            get { return new Box3d(_center, _lx, _ly, _lz); }
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

        /// <summary>
        /// Return Axis Aligned Bounding Box (AABB) for a cloud of points.
        /// </summary>
        public static AABB BoundingBox(IEnumerable<Point3d> points)
        {
            double maxx = double.NegativeInfinity;
            double maxy = double.NegativeInfinity;
            double maxz = double.NegativeInfinity;
            double minx = double.PositiveInfinity;
            double miny = double.PositiveInfinity;
            double minz = double.PositiveInfinity;

            foreach (Point3d p in points)
            {
                Point3d t = p.ConvertToGlobal();
                if (t.X > maxx) { maxx = t.X; }
                if (t.Y > maxy) { maxy = t.Y; }
                if (t.Z > maxz) { maxz = t.Z; }
                if (t.X < minx) { minx = t.X; }
                if (t.Y < miny) { miny = t.Y; }
                if (t.Z < minz) { minz = t.Z; }
            }

            return new AABB(new Point3d(0.5 * (maxx + minx), 0.5 * (maxy + miny), 0.5 * (maxz + minz)), maxx - minx, maxy - miny, maxz - minz);
        }


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
            return _line_intersection(s.Line, 0.0, s.Length);
        }

        /// <summary>
        /// Check intersection of box with triangle
        /// </summary>
        public bool Intersects(Triangle t)
        {
            if (t.A.BelongsTo(this) || t.B.BelongsTo(this) || t.C.BelongsTo(this))
            {
                return true;
            }

            foreach (Triangle bt in this.ListOfTriangles)
            {
                if (bt.Intersects(t)) return true;
            }

            return false;
        }

        /// <summary>
        /// Check intersection of two boxes (only for AABB boxes, no check is performed for speed)
        /// </summary>
        public bool Intersects(Box3d box)
        {
            bool x = Abs(this._center.X - box._center.X) <= 0.5 * (this.L1 + box.L1) ? true : false;
            bool y = Abs(this._center.Y - box._center.Y) <= 0.5 * (this.L2 + box.L2) ? true : false;
            bool z = Abs(this._center.Z - box._center.Z) <= 0.5 * (this.L3 + box.L3) ? true : false;

            return x && y && z;
        }

        /// <summary>
        /// Check intersection of two AABB
        /// </summary>
        public bool Intersects(AABB box)
        {
            bool x = Abs(this._center.X - box._center.X) <= 0.5 * (this.L1 + box.L1) ? true : false;
            bool y = Abs(this._center.Y - box._center.Y) <= 0.5 * (this.L2 + box.L2) ? true : false;
            bool z = Abs(this._center.Z - box._center.Z) <= 0.5 * (this.L3 + box.L3) ? true : false;

            return x && y && z;
        }

        /// <summary>
        /// Check if AABB is located inside box.
        /// </summary>
        public bool IsInside(Box3d box)
        {
            foreach (Point3d p in ListOfPoints)
            {
                if (!p.IsInside(box)) return false;
            }
            return true;
        }

        /// <summary>
        /// Check if AABB is located inside box.
        /// </summary>
        public bool IsInside(AABB box)
        {
            foreach (Point3d p in ListOfPoints)
            {
                if (!p.IsInside(box)) return false;
            }
            return true;
        }

        /// <summary>
        /// Intersection of two AABB (null for non-intersecting AABB)
        /// </summary>
        public AABB IntersectionWith(AABB box)
        {
            double x1min = this._center.X - 0.5 * this.L1;
            double x1max = this._center.X + 0.5 * this.L1;
            double y1min = this._center.Y - 0.5 * this.L2;
            double y1max = this._center.Y + 0.5 * this.L2;
            double z1min = this._center.Z - 0.5 * this.L3;
            double z1max = this._center.Z + 0.5 * this.L3;

            double x2min = box._center.X - 0.5 * box.L1;
            double x2max = box._center.X + 0.5 * box.L1;
            double y2min = box._center.Y - 0.5 * box.L2;
            double y2max = box._center.Y + 0.5 * box.L2;
            double z2min = box._center.Z - 0.5 * box.L3;
            double z2max = box._center.Z + 0.5 * box.L3;

            double xmin, xmax, ymin, ymax, zmin, zmax;

            xmin = Max(x1min, x2min);
            xmax = Min(x1max, x2max);
            if (xmax >= xmin)
            {
                ymin = Max(y1min, y2min);
                ymax = Min(y1max, y2max);
                if (ymax >= ymin)
                {
                    zmin = Max(z1min, z2min);
                    zmax = Min(z1max, z2max);
                    if (zmax >= zmin)
                    {
                        return new AABB(new Point3d(xmin, ymin, zmin), new Point3d(xmax, ymax, zmax));
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

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
            Coord3d local_CS = this.LocalCoord();

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
            return new Coord3d(this._center, V1, V2);
        }

        /// <summary>
        /// Point on box (including interior points) closest to target point "p".
        /// </summary>
        public Point3d ClosestPoint(Point3d p)
        {
            p = p.ConvertToGlobal();
            double x = GeometRi3D.Clamp(p.X, this._center.X - _lx / 2, this._center.X + _lx / 2);
            double y = GeometRi3D.Clamp(p.Y, this._center.Y - _ly / 2, this._center.Y + _ly / 2);
            double z = GeometRi3D.Clamp(p.Z, this._center.Z - _lz / 2, this._center.Z + _lz / 2);

            return new Point3d(x, y, z);
        }

        /// <summary>
        /// Distance from box to point (zero will be returned for point located inside box)
        /// </summary>
        public double DistanceTo(Point3d p)
        {
            return ClosestPoint(p).DistanceTo(p);
        }

        /// <summary>
        /// Distance between two AABB
        /// </summary>
        public double DistanceTo(AABB box)
        {
            double dx = Abs(_center.X - box._center.X) - 0.5 * (_lx + box._lx);
            double dy = Abs(_center.Y - box._center.Y) - 0.5 * (_ly + box._ly);
            double dz = Abs(_center.Z - box._center.Z) - 0.5 * (_lz + box._lz);
            if (dx < 0) { dx = 0; }
            if (dy < 0) { dy = 0; }
            if (dz < 0) { dz = 0; }
            return Sqrt(dx * dx + dy * dy + dz * dz);
        }

        /// <summary>
        /// Shortest distance from box to sphere
        /// </summary>
        public double DistanceTo(Sphere s)
        {
            Point3d p = this.ClosestPoint(s._point);
            double dist = p.DistanceTo(s._point);
            return dist <= s.R ? 0.0 : dist - s.R;
        }

        /// <summary>
        /// Shortest distance from box to circle
        /// </summary>
        public double DistanceTo(Circle3d c)
        {
            if (c._point.IsInside(this))
            {
                return 0;
            }
            double min_dist = Double.PositiveInfinity;
            foreach (Triangle triangle in ListOfTriangles)
            {
                double dist = c.DistanceTo(triangle);
                if (dist <= GeometRi3D.Tolerance) return 0;
                if (dist < min_dist) min_dist = dist;
            }
            return min_dist;
        }

        /// <summary>
        /// Intersection check between circle and box
        /// </summary>
        public bool Intersects(Circle3d c)
        {
            //if (c.Center.IsInside(this)) return true;
            double dist = c._point.DistanceTo(this);
            if (dist > c.R) return false;
            if (dist < GeometRi3D.Tolerance) return true;

            foreach (Triangle triangle in ListOfTriangles)
            {
                if (c.Intersects(triangle)) return true;
            }
            return false;
        }

        internal override int _PointLocation(Point3d p)
        {
            Coord3d coord = this.LocalCoord();
            p = p.ConvertTo(coord);
            if (GeometRi3D.UseAbsoluteTolerance)
            {
                if ((Abs(p.X) - L1 / 2) <= GeometRi3D.Tolerance && (Abs(p.Y) - L2 / 2) <= GeometRi3D.Tolerance && (Abs(p.Z) - L3 / 2) <= GeometRi3D.Tolerance)
                {
                    if ((Abs(p.X) - L1 / 2) < -GeometRi3D.Tolerance && (Abs(p.Y) - L2 / 2) < -GeometRi3D.Tolerance && (Abs(p.Z) - L3 / 2) < -GeometRi3D.Tolerance)
                    {
                        return 1; // Point is strictly inside box
                    }
                    else
                    {
                        return 0; // Point is on boundary
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
        public AABB Translate(Vector3d v)
        {
            return new AABB(_center.Translate(v), _lx, _ly, _lz);
        }


        /// <summary>
        /// Reflect box in given point
        /// <para>The order of corner points will be changed during reflection operation.</para>
        /// </summary>
        public virtual AABB ReflectIn(Point3d p)
        {
            Point3d new_center = this.Center.ReflectIn(p);
            return new AABB(new_center, _lx, _ly, _lz);
        }

        /// <summary>
        /// Scale box relative to given point
        /// </summary>
        public virtual AABB Scale(double scale, Point3d scaling_center)
        {
            Point3d new_center = scaling_center + scale * (this._center - scaling_center);
            return new AABB(new_center, scale * _lx, scale * _ly, scale * _lz);
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
            AABB b = (AABB)obj;

            if (GeometRi3D.UseAbsoluteTolerance)
            {
                return this._center == b._center &&
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
            return GeometRi3D.HashFunction(_center.GetHashCode(), hash_code);
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
            return str;
        }

        // Operators overloads
        //-----------------------------------------------------------------
        public static bool operator ==(AABB b1, AABB b2)
        {
            if (object.ReferenceEquals(b1, null))
                return object.ReferenceEquals(b2, null);
            return b1.Equals(b2);
        }
        public static bool operator !=(AABB b1, AABB b2)
        {
            if (object.ReferenceEquals(b1, null))
                return !object.ReferenceEquals(b2, null);
            return !b1.Equals(b2);
        }
    }
}

