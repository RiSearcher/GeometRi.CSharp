using System;
using System.Security;
using static System.Math;

namespace GeometRi
{
    /// <summary>
    /// Triangle in 3D space defined by three points.
    /// </summary>
#if NET20
    [Serializable]
#endif
    public class Triangle : PlanarFiniteObject, IPlanarObject
    {
        private Point3d _a, _b, _c;
        private double? _ab, _ac, _bc, _perimeter, _area;
        private Vector3d _normal;
        private Circle3d _circumcircle;
        private double? _angleA, _angleB, _angleC;
        private Plane3d _plane;

        private bool HasChanged => _a.HasChanged || _b.HasChanged || _c.HasChanged;
        private void CheckFields()
        {
            if (HasChanged)
            {
                _a = _a.Copy();
                _b = _b.Copy();
                _c = _c.Copy();
                ClearCache();
            }
        }
        private void ClearCache()
        {
            _ab = null;
            _ac = null;
            _bc = null;
            _perimeter = null;
            _area = null;
            _normal = null;
            _circumcircle = null;
            _angleA = null;
            _angleB = null;
            _angleC = null;
            _plane = null;
        }

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
            return new Triangle(_a, _b, _c);
        }

        #region "Properties"
        /// <summary>
        /// First point of triangle
        /// </summary>
        public Point3d A
        {
            get { return _a; }
            set
            {
                if (Point3d.CollinearPoints(value, _b, _c))
                {
                    throw new Exception("Collinear points");
                }
                _a = value.Copy();
                ClearCache();
            }
        }

        /// <summary>
        /// Second point of triangle
        /// </summary>
        public Point3d B
        {
            get { return _b; }
            set
            {
                if (Point3d.CollinearPoints(_a, value, _c))
                {
                    throw new Exception("Collinear points");
                }
                _b = value.Copy();
                ClearCache();
            }
        }

        /// <summary>
        /// Third point of triangle
        /// </summary>
        public Point3d C
        {
            get { return _c; }
            set
            {
                if (Point3d.CollinearPoints(_a, _b, value))
                {
                    throw new Exception("Collinear points");
                }
                _c = value.Copy();
                ClearCache();
            }
        }

        /// <summary>
        /// Length of AB side
        /// </summary>
        public double AB
        {
            get
            {
                CheckFields();
                if (_ab == null)
                {
                    _ab = _a.DistanceTo(_b);
                }
                return _ab.Value;
            }
        }

        /// <summary>
        /// Length of AC side
        /// </summary>
        public double AC
        {
            get
            {
                CheckFields();
                if (_ac == null)
                {
                    _ac = _a.DistanceTo(_c);
                }
                return _ac.Value;
            }
        }

        /// <summary>
        /// Length of BC side
        /// </summary>
        public double BC
        {
            get
            {
                CheckFields();
                if (_bc == null)
                {
                    _bc = _b.DistanceTo(_c);
                }
                return _bc.Value;
            }
        }

        /// <summary>
        /// Perimeter of the triangle
        /// </summary>
        public double Perimeter
        {
            get
            {
                CheckFields();
                if (_perimeter == null)
                {
                    _perimeter = AB + BC + AC;
                }
                return _perimeter.Value;
            }
        }

        /// <summary>
        /// Area of the triangle
        /// </summary>
        public double Area
        {
            get
            {
                CheckFields();
                if (_area == null)
                {
                    Vector3d v1 = new Vector3d(_a, _b);
                    Vector3d v2 = new Vector3d(_a, _c);
                    _area = 0.5 * v1.Cross(v2).Norm;
                }
                return _area.Value;
            }
        }

        public Vector3d Normal
        {
            get
            {
                CheckFields();
                if (_normal == null)
                {
                    _normal = new Vector3d(_a, _b).Cross(new Vector3d(_a, _c)).Normalized;
                }
                return _normal;
            }
        }

        public bool IsOriented
        {
            get { return false; }
        }

        /// <summary>
        /// Plane the triangle belongs to
        /// </summary>
        internal override Plane3d Plane
        {
            get
            {
                CheckFields();
                if (_plane is null)
                {
                    _plane = new Plane3d(_a, Normal);
                }
                return _plane;
            }
        }

        /// <summary>
        /// Convert triangle to a new plane object.
        /// </summary>
        public Plane3d ToPlane
        {
            get
            {
                return Plane.Copy();
            }
        }

        /// <summary>
        /// Circumcircle of the triangle
        /// </summary>
        public Circle3d Circumcircle
        {
            get
            {
                CheckFields();
                if (_circumcircle == null)
                {
                    _circumcircle = new Circle3d(_a, _b, _c);
                }
                return _circumcircle;
            }
        }

        /// <summary>
        /// Angle at the vertex A
        /// </summary>
        public double Angle_A
        {
            get
            {
                CheckFields();
                if (_angleA == null)
                {
                    _angleA = new Vector3d(_a, _b).AngleTo(new Vector3d(_a, _c));
                }
                return _angleA.Value;
            }
        }

        /// <summary>
        /// Angle at the vertex B
        /// </summary>
        public double Angle_B
        {
            get
            {
                CheckFields();
                if (_angleB == null)
                {
                    _angleB = new Vector3d(_b, _a).AngleTo(new Vector3d(_b, _c));
                }
                return _angleB.Value;
            }
        }

        /// <summary>
        /// Angle at the vertex C
        /// </summary>
        public double Angle_C
        {
            get
            {
                CheckFields();
                if (_angleC == null)
                {
                    _angleC = new Vector3d(_c, _a).AngleTo(new Vector3d(_c, _b));
                }
                return _angleC.Value;
            }
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
        /// External angle bisector at the vertex A
        /// </summary>
        public Line3d ExternalBisector_A
        {
            get
            {
                Point3d p = _a + (_a - _b);
                return new Triangle(_a, p, _c).Bisector_A.ToLine;
            }
        }

        /// <summary>
        /// External angle bisector at the vertex B
        /// </summary>
        public Line3d ExternalBisector_B
        {
            get
            {
                Point3d p = _b + (_b - _a);
                return new Triangle(_b, p, _c).Bisector_A.ToLine;
            }
        }

        /// <summary>
        /// External angle bisector at the vertex C
        /// </summary>
        public Line3d ExternalBisector_C
        {
            get
            {
                Point3d p = _c + (_c - _a);
                return new Triangle(_c, p, _b).Bisector_A.ToLine;
            }
        }

        /// <summary>
        /// Incenter of the triangle
        /// </summary>
        public Point3d Incenter
        {
            get { return Bisector_A.Line.PerpendicularTo(Bisector_B.Line); }
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
            get { return Altitude_A.Line.PerpendicularTo(Altitude_B.Line); }
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
                Point3d p = Bisector_A.Line.PerpendicularTo(Bisector_B.Line);
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

        /// <summary>
        /// Check if two objects are coplanar
        /// </summary>
        public bool IsCoplanarTo(IPlanarObject obj)
        {
            return GeometRi3D._coplanar(this, obj);
        }

        /// <summary>
        /// Check if two objects are coplanar
        /// </summary>
        public bool IsCoplanarTo(ILinearObject obj)
        {
            return GeometRi3D._coplanar(this, obj);
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
        /// Return Bounding Box in given coordinate system.
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
        /// Return Axis Aligned Bounding Box (AABB).
        /// </summary>
        public AABB AABB()
        {
            return GeometRi.AABB.BoundingBox(new Point3d[] { A, B, C });
        }

        /// <summary>
        /// Return bounding sphere.
        /// </summary>
        public Sphere BoundingSphere
        {
            get { return new Sphere(Circumcenter, Circumcenter.DistanceTo(_a)); }
        }
        #endregion

        #region "Distance"
        /// <summary>
        /// Shortest distance between triangle and point
        /// </summary>
        public double DistanceTo(Point3d p)
        {
            Point3d projection_point = p.ProjectionTo(this.Plane);

            int code = _PointLocation(projection_point);

            if (code >= 0)
            {
                return p.DistanceTo(projection_point);
            }

            Segment3d AB = new Segment3d(this._a, this._b);
            Segment3d BC = new Segment3d(this._b, this._c);
            Segment3d AC = new Segment3d(this._a, this._c);

            double dist = p.DistanceTo(AB);
            double dist2 = p.DistanceTo(BC);
            double dist3 = p.DistanceTo(AC);

            return Min(dist, Min(dist2, dist3));
        }

        /// <summary>
        /// Shortest distance between triangle and point
        /// </summary>
        public double DistanceTo(Point3d p, out Point3d closest_point)
        {
            Point3d projection_point = p.ProjectionTo(this.Plane);

            int code = _PointLocation(projection_point);

            if (code >= 0)
            {
                closest_point = projection_point;
                return p.DistanceTo(projection_point);
            }

            Segment3d AB = new Segment3d(this._a, this._b);
            Segment3d BC = new Segment3d(this._b, this._c);
            Segment3d AC = new Segment3d(this._a, this._c);

            double dist = p.DistanceTo(AB);
            Point3d tmp_closest_point = AB.ClosestPoint(p);
            double dist2 = p.DistanceTo(BC);
            if (dist2 < dist)
            {
                dist = dist2;
                tmp_closest_point = BC.ClosestPoint(p);
            }
            dist2 = p.DistanceTo(AC);
            if (dist2 < dist)
            {
                dist = dist2;
                tmp_closest_point = AC.ClosestPoint(p);
            }

            closest_point = tmp_closest_point;
            return dist;
        }

        public double DistanceSquared(Point3d p)
        {
            Point3d projection_point = p.ProjectionTo(this.Plane);

            int code = _PointLocation(projection_point);

            if (code >= 0)
            {
                return p.DistanceSquared(projection_point);
            }

            Segment3d AB = new Segment3d(this._a, this._b);
            Segment3d BC = new Segment3d(this._b, this._c);
            Segment3d AC = new Segment3d(this._a, this._c);

            double dist = p.DistanceSquared(AB);
            double dist2 = p.DistanceSquared(BC);
            double dist3 = p.DistanceSquared(AC);

            return Min(dist, Min(dist2, dist3));
        }

        /// <summary>
        /// Calculates the point on the triangle closest to given point.
        /// </summary>
        public Point3d ClosestPoint(Point3d p)
        {
            Point3d projection_point = p.ProjectionTo(this.Plane);

            int code = _PointLocation(projection_point);

            if (code >= 0)
            {
                return projection_point;
            }

            Segment3d AB = new Segment3d(this._a, this._b);
            Segment3d BC = new Segment3d(this._b, this._c);
            Segment3d AC = new Segment3d(this._a, this._c);

            double dist = p.DistanceTo(AB);
            Point3d closest_point = AB.ClosestPoint(p);
            double dist2 = p.DistanceTo(BC);
            if (dist2 < dist)
            {
                dist = dist2;
                closest_point = BC.ClosestPoint(p);
            }
            dist2 = p.DistanceTo(AC);
            if (dist2 < dist)
            {
                dist = dist2;
                closest_point = AC.ClosestPoint(p);
            }

            return closest_point;
        }

        /// <summary>
        /// Shortest distance between triangle and circle (including interior points)
        /// </summary>
        public double DistanceTo(Circle3d c)
        {
            Point3d point_on_circle, point_on_triangle;
            return c.DistanceTo(this, out point_on_circle, out point_on_triangle);
        }

        /// <summary>
        /// Shortest distance between triangle and circle (including interior points)
        /// </summary>
        /// <param name="c">Target circle</param>
        /// <param name="point_on_triangle">Closest point on triangle</param>
        /// <param name="point_on_circle">Closest point on circle</param>
        public double DistanceTo(Circle3d c, out Point3d point_on_triangle, out Point3d point_on_circle)
        {
            return c.DistanceTo(this, out point_on_circle, out point_on_triangle);
        }

        /// <summary>
        /// Shortest distance between triangle and segment
        /// </summary>
        public double DistanceTo(Segment3d s)
        {

            double dist = _DistanceToSegment(s);

            return dist;
        }

        /// <summary>
        /// Shortest distance between triangle and segment (with closest points)
        /// </summary>
        public double DistanceTo(Segment3d s, out Point3d point_on_triangle, out Point3d point_on_segment)
        {
            double dist = _DistanceToSegment_with_points(s, out point_on_triangle, out point_on_segment);

            return dist;
        }

        /// <summary>
        /// Shortest distance between triangle and segment
        /// </summary>
        internal double _DistanceToSegment(Segment3d s)
        {
            if (s.P1.BelongsTo(this))
            {
                return 0;
            }
            if (s.P2.BelongsTo(this))
            {
                return 0;
            }

            Plane3d p = Plane;
            if (s.IsCoplanarTo(p))
            {
                double dist1 = s.DistanceTo(new Segment3d(this._a, this._b));
                double dist2 = s.DistanceTo(new Segment3d(this._a, this._c));
                double dist3 = s.DistanceTo(new Segment3d(this._c, this._b));
                return dist1 < dist2 ? Min(dist1, dist3) : Min(dist2, dist3);
            }

            //Segment is not coplanar with triangle's plane, therefore possible intersection is point
            object obj = s.IntersectionWith(p);
            if (obj != null && object.ReferenceEquals(obj.GetType(), typeof(Point3d)))
            {
                if (((Point3d)obj).BelongsTo(this))
                {
                    return 0;
                }
            }


            Point3d projection_1 = s.P1.ProjectionTo(p);
            Point3d projection_2 = s.P2.ProjectionTo(p);

            double dist = double.PositiveInfinity;
            if (projection_1.BelongsTo(this))
            {
                dist = s.P1.DistanceTo(projection_1);
            }
            if (projection_2.BelongsTo(this))
            {
                double tmp_dist = s.P2.DistanceTo(projection_2);
                if (tmp_dist < dist)
                {
                    dist = tmp_dist;
                }
            }

            double tmp = s.DistanceTo(new Segment3d(this._a, this._b));
            if (tmp < dist)
            {
                dist = tmp;
            }
            tmp = s.DistanceTo(new Segment3d(this._a, this._c));
            if (tmp < dist)
            {
                dist = tmp;
            }
            tmp = s.DistanceTo(new Segment3d(this._b, this._c));
            if (tmp < dist)
            {
                dist = tmp;
            }

            return dist;
        }


        /// <summary>
        /// Shortest distance between triangle and segment
        /// </summary>
        /// <param name="s">Target segment</param>
        /// <param name="point_on_triangle">Closest point on triangle</param>
        /// <param name="point_on_segment">Closest point on segment</param>
        internal double _DistanceToSegment_with_points(Segment3d s, out Point3d point_on_triangle, out Point3d point_on_segment)
        {
            if (s.P1.BelongsTo(this))
            {
                point_on_triangle = s.P1;
                point_on_segment = s.P1;
                return 0;
            }
            if (s.P2.BelongsTo(this))
            {
                point_on_triangle = s.P2;
                point_on_segment = s.P2;
                return 0;
            }
            Plane3d p = Plane;

            if (s.IsCoplanarTo(p))
            {
                double dist1 = s.DistanceTo(new Segment3d(this._a, this._b), out Point3d p1s, out Point3d p1t);
                double dist2 = s.DistanceTo(new Segment3d(this._a, this._c), out Point3d p2s, out Point3d p2t);
                double dist3 = s.DistanceTo(new Segment3d(this._c, this._b), out Point3d p3s, out Point3d p3t);
                if (dist1 < dist2)
                {
                    if (dist1 < dist3)
                    {
                        point_on_triangle = p1t;
                        point_on_segment = p1s;
                        return dist1;
                    }
                    else if (dist2 < dist3)
                    {
                        point_on_triangle = p2t;
                        point_on_segment = p2s;
                        return dist2;
                    }
                    else
                    {
                        point_on_triangle = p3t;
                        point_on_segment = p3s;
                        return dist3;
                    }
                }
            }

            //Segment is not coplanar with triangle's plane, therefore possible intersection is point
            object obj = s.IntersectionWith(p);
            if (obj != null && object.ReferenceEquals(obj.GetType(), typeof(Point3d)))
            {
                if (((Point3d)obj).BelongsTo(this))
                {
                    point_on_triangle = (Point3d)obj;
                    point_on_segment = (Point3d)obj;
                    return 0;
                }
            }

            double dist = double.PositiveInfinity;
            point_on_triangle = this.A; // Arbitrary point on triangle
            point_on_segment = s.P1; // Arbitrary point on segment

            Point3d projection_1 = s.P1.ProjectionTo(p);
            Point3d projection_2 = s.P2.ProjectionTo(p);

            if (projection_1.BelongsTo(this))
            {
                point_on_triangle = projection_1;
                point_on_segment = s.P1;
                dist = s.P1.DistanceTo(projection_1);
            }
            if (projection_2.BelongsTo(this))
            {
                double tmp_dist = s.P2.DistanceTo(projection_2);
                if (tmp_dist < dist)
                {
                    point_on_triangle = projection_2;
                    point_on_segment = s.P2;
                    dist = tmp_dist;
                }
            }

            double tmp = s.DistanceTo(new Segment3d(this._a, this._b), out Point3d ps, out Point3d pt);
            if (tmp < dist)
            {
                point_on_triangle = pt;
                point_on_segment = ps;
                dist = tmp;
            }
            tmp = s.DistanceTo(new Segment3d(this._a, this._c), out ps, out pt);
            if (tmp < dist)
            {
                point_on_triangle = pt;
                point_on_segment = ps;
                dist = tmp;
            }
            tmp = s.DistanceTo(new Segment3d(this._b, this._c), out ps, out pt);
            if (tmp < dist)
            {
                point_on_triangle = pt;
                point_on_segment = ps;
                dist = tmp;
            }

            return dist;
        }

        /// <summary>
        /// Shortest distance between two triangles
        /// </summary>
        public double DistanceTo(Triangle t)
        {

            double dist = _DistanceToTriangle(t);

            return dist;
        }

        /// <summary>
        /// Shortest distance between two triangles (with closest points)
        /// </summary>
        public double DistanceTo(Triangle t, out Point3d point_on_this_triangle, out Point3d point_on_target_triangle)
        {

            double dist = _DistanceToTriangle(t, out point_on_this_triangle, out point_on_target_triangle);

            return dist;
        }

        /// <summary>
        /// Shortest distance between two triangles
        /// </summary>
        internal double _DistanceToTriangle(Triangle t)
        {
            double dist = double.PositiveInfinity;
            double tmp;

            tmp = t.DistanceTo(new Segment3d(this._a, this._b));
            if (tmp <= GeometRi3D.Tolerance)
            {
                return tmp;
            }
            else if (tmp < dist)
            {
                dist = tmp;
            }
            tmp = t.DistanceTo(new Segment3d(this._a, this._c));
            if (tmp <= GeometRi3D.Tolerance)
            {
                return tmp;
            }
            else if (tmp < dist)
            {
                dist = tmp;
            }
            tmp = t.DistanceTo(new Segment3d(this._b, this._c));
            if (tmp <= GeometRi3D.Tolerance)
            {
                return tmp;
            }
            else if (tmp < dist)
            {
                dist = tmp;
            }
            tmp = this.DistanceTo(new Segment3d(t._a, t._b));
            if (tmp <= GeometRi3D.Tolerance)
            {
                return tmp;
            }
            else if (tmp < dist)
            {
                dist = tmp;
            }
            tmp = this.DistanceTo(new Segment3d(t._a, t._c));
            if (tmp <= GeometRi3D.Tolerance)
            {
                return tmp;
            }
            else if (tmp < dist)
            {
                dist = tmp;
            }
            tmp = this.DistanceTo(new Segment3d(t._b, t._c));
            if (tmp <= GeometRi3D.Tolerance)
            {
                return tmp;
            }
            else if (tmp < dist)
            {
                dist = tmp;
            }

            return dist;
        }

        /// <summary>
        /// Shortest distance between two triangles (with closest points)
        /// </summary>
        internal double _DistanceToTriangle(Triangle t, out Point3d point_on_this_triangle, out Point3d point_on_target_triangle)
        {
            double dist = double.PositiveInfinity;
            double tmp;
            Point3d point_on_triangle, point_on_segment;

            tmp = t.DistanceTo(new Segment3d(this._a, this._b), out point_on_triangle, out point_on_segment);
            point_on_this_triangle = point_on_segment;
            point_on_target_triangle = point_on_triangle;
            if (tmp <= GeometRi3D.Tolerance)
            {
                return tmp;
            }
            else if (tmp < dist)
            {
                dist = tmp;
            }
            tmp = t.DistanceTo(new Segment3d(this._a, this._c), out point_on_triangle, out point_on_segment);
            if (tmp <= GeometRi3D.Tolerance)
            {
                point_on_this_triangle = point_on_segment;
                point_on_target_triangle = point_on_triangle;
                return tmp;
            }
            else if (tmp < dist)
            {
                point_on_this_triangle = point_on_segment;
                point_on_target_triangle = point_on_triangle;
                dist = tmp;
            }
            tmp = t.DistanceTo(new Segment3d(this._b, this._c), out point_on_triangle, out point_on_segment);
            if (tmp <= GeometRi3D.Tolerance)
            {
                point_on_this_triangle = point_on_segment;
                point_on_target_triangle = point_on_triangle;
                return tmp;
            }
            else if (tmp < dist)
            {
                point_on_this_triangle = point_on_segment;
                point_on_target_triangle = point_on_triangle;
                dist = tmp;
            }
            tmp = this.DistanceTo(new Segment3d(t._a, t._b), out point_on_triangle, out point_on_segment);
            if (tmp <= GeometRi3D.Tolerance)
            {
                point_on_this_triangle = point_on_triangle;
                point_on_target_triangle = point_on_segment;
                return tmp;
            }
            else if (tmp < dist)
            {
                point_on_this_triangle = point_on_triangle;
                point_on_target_triangle = point_on_segment;
                dist = tmp;
            }
            tmp = this.DistanceTo(new Segment3d(t._a, t._c), out point_on_triangle, out point_on_segment);
            if (tmp <= GeometRi3D.Tolerance)
            {
                point_on_this_triangle = point_on_triangle;
                point_on_target_triangle = point_on_segment;
                return tmp;
            }
            else if (tmp < dist)
            {
                point_on_this_triangle = point_on_triangle;
                point_on_target_triangle = point_on_segment;
                dist = tmp;
            }
            tmp = this.DistanceTo(new Segment3d(t._b, t._c), out point_on_triangle, out point_on_segment);
            if (tmp <= GeometRi3D.Tolerance)
            {
                point_on_this_triangle = point_on_triangle;
                point_on_target_triangle = point_on_segment;
                return tmp;
            }
            else if (tmp < dist)
            {
                point_on_this_triangle = point_on_triangle;
                point_on_target_triangle = point_on_segment;
                dist = tmp;
            }

            return dist;
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

        /// <summary>
        /// Get intersection of line with triangle.
        /// Returns 'null' (no intersection) or object of type 'Point3d' or 'Segment3d'.
        /// </summary>
        public object IntersectionWith(Line3d l)
        {

            // Relative tolerance ================================
            if (!GeometRi3D.UseAbsoluteTolerance)
            {
                double tol = GeometRi3D.Tolerance;
                GeometRi3D.Tolerance = tol * Max(AB, Max(BC, AC));
                GeometRi3D.UseAbsoluteTolerance = true;
                object result = this.IntersectionWith(l);
                GeometRi3D.UseAbsoluteTolerance = false;
                GeometRi3D.Tolerance = tol;
                return result;
            }
            //====================================================

            Plane3d s = new Plane3d(this.A, this.Normal);

            object obj = l.IntersectionWith(s);
            if (obj == null)
            {
                return null;
            }
            else
            {
                if (obj.GetType() == typeof(Line3d))
                {
                    // Coplanar line and triangle
                    return _coplanar_IntersectionWith(l);
                }
                else
                {
                    // result of intersection is point
                    Point3d p = (Point3d)obj;
                    if (p.BelongsTo(this))
                    {
                        return p;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

        }

        /// <summary>
        /// Get intersection of plane with triangle.
        /// Returns 'null' (no intersection) or object of type 'Triangle', 'Point3d' or 'Segment3d'.
        /// </summary>
        public object IntersectionWith(Plane3d s)
        {

            // Relative tolerance ================================
            if (!GeometRi3D.UseAbsoluteTolerance)
            {
                double tol = GeometRi3D.Tolerance;
                GeometRi3D.Tolerance = tol * Max(AB, Max(BC, AC));
                GeometRi3D.UseAbsoluteTolerance = true;
                object result = this.IntersectionWith(s);
                GeometRi3D.UseAbsoluteTolerance = false;
                GeometRi3D.Tolerance = tol;
                return result;
            }
            //====================================================

            Plane3d st = new Plane3d(this.A, this.Normal);

            if (s.IsParallelTo(st))
            {
                if (A.BelongsTo(s))
                {
                    return this.Copy();
                }
                else
                {
                    return null;
                }
            }

            Line3d l = (Line3d)s.IntersectionWith(st);

            // Line "l" is coplanar with triangle by construction

            return _coplanar_IntersectionWith(l);
        }

        /// <summary>
        /// Get intersection of line with triangle (coplanar).
        /// Returns 'null' (no intersection) or object of type 'Point3d' or 'Segment3d'.
        /// </summary>
        internal object _coplanar_IntersectionWith(Line3d l)
        {
            // Check intersection with first two sides
            Point3d onAB = l.PerpendicularTo(new Segment3d(_a, _b).Line);
            Point3d onBC = l.PerpendicularTo(new Segment3d(_b, _c).Line);
            if (onAB != null && onBC != null)
            {
                double pos_onAB = new Vector3d(_a, onAB).Dot(new Vector3d(_a, _b)) / (AB * AB);
                double pos_onBC = new Vector3d(_b, onBC).Dot(new Vector3d(_b, _c)) / (BC * BC);
                if (pos_onAB >= 1 - GeometRi3D.Tolerance && pos_onAB <= 1 + GeometRi3D.Tolerance &&
                    pos_onBC >= -GeometRi3D.Tolerance && pos_onBC <= GeometRi3D.Tolerance)
                {
                    // intersection in one point "B"
                    return B;
                }
                else if (pos_onAB >= -GeometRi3D.Tolerance && pos_onAB < 1 - GeometRi3D.Tolerance &&
                         pos_onBC > GeometRi3D.Tolerance && pos_onBC <= 1 + GeometRi3D.Tolerance)
                {
                    return new Segment3d(onAB, onBC);
                }
            }

            //Check intersection with third side
            Point3d onAC = l.PerpendicularTo(new Segment3d(_a, _c).Line);
            if (onAB != null && onAC != null)
            {
                double pos_onAB = new Vector3d(_a, onAB).Dot(new Vector3d(_a, _b)) / (AB * AB);
                double pos_onAC = new Vector3d(_a, onAC).Dot(new Vector3d(_a, _c)) / (AC * AC);
                if (pos_onAB >= -GeometRi3D.Tolerance && pos_onAB <= GeometRi3D.Tolerance &&
                    pos_onAC >= -GeometRi3D.Tolerance && pos_onAC <= GeometRi3D.Tolerance)
                {
                    // intersection in one point "A"
                    return A;
                }
                else if (pos_onAB > GeometRi3D.Tolerance && pos_onAB <= 1 + GeometRi3D.Tolerance &&
                         pos_onAC > GeometRi3D.Tolerance && pos_onAC <= 1 + GeometRi3D.Tolerance)
                {
                    return new Segment3d(onAB, onAC);
                }
            }

            if (onBC != null && onAC != null)
            {
                double pos_onBC = new Vector3d(_b, onBC).Dot(new Vector3d(_b, _c)) / (BC * BC);
                double pos_onAC = new Vector3d(_a, onAC).Dot(new Vector3d(_a, _c)) / (AC * AC);
                if (pos_onBC >= 1 - GeometRi3D.Tolerance && pos_onBC <= 1 + GeometRi3D.Tolerance &&
                    pos_onAC >= 1 - GeometRi3D.Tolerance && pos_onAC <= 1 + GeometRi3D.Tolerance)
                {
                    // intersection in one point "C"
                    return C;
                }
                else if (pos_onBC >= -GeometRi3D.Tolerance && pos_onBC < 1 - GeometRi3D.Tolerance &&
                         pos_onAC >= -GeometRi3D.Tolerance && pos_onAC < 1 - GeometRi3D.Tolerance)
                {
                    return new Segment3d(onBC, onAC);
                }
            }

            return null;
        }


        /// <summary>
        /// Get intersection of segment with triangle.
        /// Returns 'null' (no intersection) or object of type 'Point3d' or 'Segment3d'.
        /// </summary>
        public object IntersectionWith(Segment3d s)
        {

            // Relative tolerance ================================
            if (!GeometRi3D.UseAbsoluteTolerance)
            {
                double tol = GeometRi3D.Tolerance;
                GeometRi3D.Tolerance = tol * Max(s.Length, Max(AB, Max(BC, AC)));
                GeometRi3D.UseAbsoluteTolerance = true;
                object result = this.IntersectionWith(s);
                GeometRi3D.UseAbsoluteTolerance = false;
                GeometRi3D.Tolerance = tol;
                return result;
            }
            //====================================================

            object obj = this.IntersectionWith(s.Line);

            if (obj == null)
            {
                return null;
            }
            else if (obj.GetType() == typeof(Point3d))
            {
                Point3d p = (Point3d)obj;
                if (p.BelongsTo(s))
                {
                    return p;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return s.IntersectionWith((Segment3d)obj);
            }
        }

        /// <summary>
        /// Get intersection of ray with triangle.
        /// Returns 'null' (no intersection) or object of type 'Point3d' or 'Segment3d'.
        /// </summary>
        public object IntersectionWith(Ray3d r)
        {
            // Relative tolerance ================================
            if (!GeometRi3D.UseAbsoluteTolerance)
            {
                double tol = GeometRi3D.Tolerance;
                GeometRi3D.Tolerance = tol * Max(AB, Max(BC, AC));
                GeometRi3D.UseAbsoluteTolerance = true;
                object result = this.IntersectionWith_Moller(r);
                GeometRi3D.UseAbsoluteTolerance = false;
                GeometRi3D.Tolerance = tol;
                return result;
            }
            //====================================================
            else
            {
                return IntersectionWith_Moller(r);
            }
        }

        [Obsolete("this method is not optimzed. For better performance use IntersectionWith_Moller() method instead.")]
        /// <summary>
        /// Get intersection of ray with triangle.
        /// Returns 'null' (no intersection) or object of type 'Point3d' or 'Segment3d'.
        /// </summary>
        private object IntersectionWith_original(Ray3d r)
        {
            object obj = this.IntersectionWith(r.ToLine);

            if (obj == null)
            {
                return null;
            }
            else if (obj.GetType() == typeof(Point3d))
            {
                Point3d p = (Point3d)obj;
                if (p.BelongsTo(r))
                {
                    return p;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return r.IntersectionWith((Segment3d)obj);
            }
        }


        /// <summary>
        /// acc. to https://en.wikipedia.org/wiki/M%C3%B6ller%E2%80%93Trumbore_intersection_algorithm
        /// </summary>
        /// <param name="ray"></param>
        /// <returns></returns>
        private object IntersectionWith_Moller(Ray3d ray)
        {
            double tolerance = GeometRi3D.Tolerance;

            Point3d vertex0 = A;
            Point3d vertex1 = B;
            Point3d vertex2 = C;

            Vector3d edge1 = new Vector3d(vertex0, vertex1);
            Vector3d edge2 = new Vector3d(vertex0, vertex2);
            Vector3d h = ray.Direction.Cross(edge2);
            double a = edge1.Dot(h);

            if (a > -tolerance && a < tolerance)
            {
                // The ray is parallel to the triangle
                // Check if the ray is in the plane of the triangle
                Plane3d plane = ToPlane;
                if (Math.Abs(plane.Normal.Dot(ray.Direction)) < tolerance && ray.Point.BelongsTo(plane))
                {
                    // Ray in the plane of the triangle: intersection = segment (portion of the ray inside the triangle)
                    // We look for the intersections of the ray with the 3 edges of the triangle
                    Point3d[] intersections = new Point3d[2];
                    int count = 0;
                    Segment3d[] edges = new Segment3d[] {
                        new Segment3d(vertex0, vertex1),
                        new Segment3d(vertex1, vertex2),
                        new Segment3d(vertex2, vertex0)
                    };
                    foreach (var edge in edges)
                    {
                        object inter = ray.IntersectionWith(edge);
                        if (inter is Point3d p && count < 2)
                        {
                            // Check that the point is actually inside the triangle
                            if (p.BelongsTo(this))
                                intersections[count++] = p;
                        }
                    }
                    if (count == 2 && intersections[0] != intersections[1])
                    {
                        return new Segment3d(intersections[0], intersections[1]);
                    }
                    else if (count == 1)
                    {
                        if (intersections[0] == ray.Point)
                            return intersections[0];
                        else
                            return new Segment3d(ray.Point, intersections[0]);
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

            double f = 1.0 / a;
            Vector3d s = new Vector3d(vertex0, ray.Point);
            double u = f * s.Dot(h);
            if (u < 0.0 - tolerance || u > 1.0 + tolerance)
            {
                return null;
            }

            Vector3d q = s.Cross(edge1);
            double v = f * ray.Direction.Dot(q);
            if (v < 0.0 && Math.Abs(v) > tolerance || u + v > 1.0 && Math.Abs(u + v - 1) > tolerance)
            {
                return null;
            }

            double t = f * edge2.Dot(q);
            if (t > tolerance)
            {
                // intersectionPoint = rayOrigin + t * rayVector
                Point3d intersectionPoint = ray.Point + ray.Direction.Mult(t);
                return intersectionPoint;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Intersection check between circle and triangle
        /// </summary>
        public bool Intersects(Circle3d c)
        {
            return c.Intersects(this);
        }

        /// <summary>
        /// Intersection check between tetrahedron and triangle
        /// </summary>
        public bool Intersects(Tetrahedron t)
        {
            return t.Intersects(this);
        }

        /// <summary>
        /// Intersection check between triangle and sphere
        /// </summary>
        public bool Intersects(Sphere s)
        {
            Point3d projection_point = s.Center.ProjectionTo(Plane);

            if (s.Center.DistanceTo(projection_point) > s.R)
            {
                return false;
            }
            else
            {
                if (projection_point.BelongsTo(this)) return true;

                if (s.IntersectionWith(new Segment3d(A, B)) != null) return true;
                if (s.IntersectionWith(new Segment3d(A, C)) != null) return true;
                if (s.IntersectionWith(new Segment3d(C, B)) != null) return true;
            }
            return false;
        }

        /// <summary>
        /// Intersection check between two triangles
        /// </summary>
        public bool Intersects(Triangle t)
        {
            if (this.IsCoplanarTo(t))
            {
                if (t._a.BelongsTo(this)) return true;
                if (t._b.BelongsTo(this)) return true;
                if (t._c.BelongsTo(this)) return true;

                if (this._a.BelongsTo(t)) return true;
                if (this._b.BelongsTo(t)) return true;
                if (this._c.BelongsTo(t)) return true;

                if (this.IntersectionWith(new Segment3d(t._a, t._b)) != null) return true;
                if (this.IntersectionWith(new Segment3d(t._b, t._c)) != null) return true;
                if (this.IntersectionWith(new Segment3d(t._c, t._a)) != null) return true;
            }

            object obj = this.IntersectionWith(t.Plane);
            if (obj != null && obj.GetType() == typeof(Point3d))
            {
                return ((Point3d)obj).BelongsTo(t);
            }
            else if (obj != null && obj.GetType() == typeof(Segment3d))
            {
                return ((Segment3d)obj).IntersectionWith(t) != null;
            }

            return false;
        }

        /// <summary>
        /// Check intersection of triangle with box
        /// </summary>
        public bool Intersects(Box3d box)
        {
            return box.Intersects(this);
        }

        internal override int _PointLocation(Point3d p)
        {

            if (GeometRi3D.UseAbsoluteTolerance)
            {
                Plane3d s = new Plane3d(this._a, this.Normal);
                Point3d proj = p.ProjectionTo(s);
                if (GeometRi3D.AlmostEqual(p.DistanceTo(proj), 0))
                {
                    if (p.BelongsTo(new Segment3d(_a, _b)) || p.BelongsTo(new Segment3d(_a, _c)) || p.BelongsTo(new Segment3d(_c, _b)))
                    {
                        return 0; // Point is on boundary
                    }
                    else
                    {

                        double area = this.Area;

                        double alpha = new Vector3d(proj, _b).Cross(new Vector3d(proj, _c)).Norm / (2 * area);
                        double beta = new Vector3d(proj, _c).Cross(new Vector3d(proj, _a)).Norm / (2 * area);
                        double gamma = new Vector3d(proj, _a).Cross(new Vector3d(proj, _b)).Norm / (2 * area);

                        if (GeometRi3D.AlmostEqual(((alpha + beta + gamma) - 1.0) * (AB + BC + AC) / 3, 0.0))
                        {
                            return 1; // Point is strictly inside
                        }
                        else
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
                GeometRi3D.Tolerance = tol * (AB + BC + AC) / 3;
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

        /// <summary>
        /// Scale triangle relative to given point
        /// </summary>
        public virtual Triangle Scale(double scale, Point3d scaling_center)
        {
            Point3d p1 = scaling_center + scale * (_a - scaling_center);
            Point3d p2 = scaling_center + scale * (_b - scaling_center);
            Point3d p3 = scaling_center + scale * (_c - scaling_center);
            return new Triangle(p1, p2, p3);
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
            if (object.ReferenceEquals(t1, null))
                return object.ReferenceEquals(t2, null);
            return t1.Equals(t2);
        }
        public static bool operator !=(Triangle t1, Triangle t2)
        {
            if (object.ReferenceEquals(t1, null))
                return !object.ReferenceEquals(t2, null);
            return !t1.Equals(t2);
        }
   
    }
}

