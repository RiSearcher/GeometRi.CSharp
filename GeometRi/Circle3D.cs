using System;
using static System.Math;
using System.Collections.Generic;

namespace GeometRi
{
    /// <summary>
    /// Circle in 3D space defined by center point, radius and normal vector.
    /// </summary>
    public class Circle3d : FiniteObject, IPlanarObject, IFiniteObject
    {

        private Point3d _point;
        private double _r;
        private Vector3d _normal;

        /// <summary>
        /// Initializes circle instance using center point, radius and normal vector.
        /// </summary>
        public Circle3d(Point3d Center, double Radius, Vector3d Normal)
        {
            _point = Center.Copy();
            _r = Radius;
            _normal = Normal.Copy();
        }

        /// <summary>
        /// Initializes circle passing through three points.
        /// </summary>
        public Circle3d(Point3d p1, Point3d p2, Point3d p3)
        {
            Vector3d v1 = new Vector3d(p1, p2);
            Vector3d v2 = new Vector3d(p1, p3);
            if (v1.Cross(v2).Norm < GeometRi3D.Tolerance)
            {
                throw new Exception("Collinear points");
            }

            Coord3d CS = new Coord3d(p1, v1, v2);
            Point3d a1 = p1.ConvertTo(CS);
            Point3d a2 = p2.ConvertTo(CS);
            Point3d a3 = p3.ConvertTo(CS);

            double d1 = Math.Pow(a1.X, 2) + Math.Pow(a1.Y, 2);
            double d2 = Math.Pow(a2.X, 2) + Math.Pow(a2.Y, 2);
            double d3 = Math.Pow(a3.X, 2) + Math.Pow(a3.Y, 2);
            double f = 2.0 * (a1.X * (a2.Y - a3.Y) - a1.Y * (a2.X - a3.X) + a2.X * a3.Y - a3.X * a2.Y);

            double X = (d1 * (a2.Y - a3.Y) + d2 * (a3.Y - a1.Y) + d3 * (a1.Y - a2.Y)) / f;
            double Y = (d1 * (a3.X - a2.X) + d2 * (a1.X - a3.X) + d3 * (a2.X - a1.X)) / f;
            //_point = (new Point3d(X, Y, 0, CS)).ConvertTo(p1.Coord);
            _point = new Point3d(X, Y, 0, CS);
            _point = _point.ConvertTo(p1.Coord);
            _r = Sqrt((X - a1.X) * (X - a1.X) + (Y - a1.Y) * (Y - a1.Y));
            _normal = v1.Cross(v2);

        }

        /// <summary>
        /// Creates copy of the object
        /// </summary>
        public Circle3d Copy()
        {
            return new Circle3d(_point, _r, _normal);
        }

        /// <summary>
        /// Center of the circle
        /// </summary>
        public Point3d Center
        {
            get { return _point.Copy(); }
            set { _point = value.Copy(); }
        }

        /// <summary>
        /// Radius of the circle
        /// </summary>
        public double R
        {
            get { return _r; }
            set { _r = value; }
        }

        /// <summary>
        /// Normal of the circle
        /// </summary>
        public Vector3d Normal
        {
            get { return _normal.Copy(); }
            set { _normal = value.Copy(); }
        }

        public bool IsOriented
        {
            get { return false; }
        }

        public double Perimeter
        {
            get { return 2 * PI * _r; }
        }

        /// <summary>
        /// Area of the circle.
        /// </summary>
        public double Area
        {
            get { return PI * Math.Pow(_r, 2); }
        }

        /// <summary>
        /// Convert circle to ellipse object.
        /// </summary>
        public Ellipse ToEllipse
        {
            get
            {
                Vector3d v1 = _r * _normal.OrthogonalVector.Normalized;
                Vector3d v2 = _r * (_normal.Cross(v1)).Normalized;
                return new Ellipse(_point, v1, v2);
            }
        }

        /// <summary>
        /// Convert circle to plane object.
        /// </summary>
        public Plane3d ToPlane
        {
            get
            {
                return new Plane3d(_point, _normal);
            }
        }

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
                Vector3d v1 = this.Normal.OrthogonalVector.Normalized;
                Vector3d v2 = this.Normal.Cross(v1).Normalized;
                Vector3d v3 = this.Normal.Normalized;
                Matrix3d m = new Matrix3d(v1, v2, v3);
                Rotation r = new Rotation(m.Transpose());
                return new Box3d(_point, 2.0 * _r, 2.0 * _r, 0, r);
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
            Segment3d s1 = this.ProjectionTo(l1);
            Segment3d s2 = this.ProjectionTo(l2);
            Segment3d s3 = this.ProjectionTo(l3);
            return new Box3d(_point, s1.Length, s2.Length, s3.Length, coord);
        }

        /// <summary>
        /// Return bounding sphere.
        /// </summary>
        public Sphere BoundingSphere
        {
            get { return new Sphere(_point, _r); }

        }

        /// <summary>
        /// Check if circle is located inside box with tolerance defined by global tolerance property (GeometRi3D.Tolerance).
        /// </summary>
        public bool IsInside(Box3d box)
        {
            // Relative tolerance ================================
            if (!GeometRi3D.UseAbsoluteTolerance)
            {
                double tol = GeometRi3D.Tolerance;
                GeometRi3D.Tolerance = tol * this.R;
                GeometRi3D.UseAbsoluteTolerance = true;
                bool result = this.IsInside(box);
                GeometRi3D.UseAbsoluteTolerance = false;
                GeometRi3D.Tolerance = tol;
                return result;
            }
            //====================================================

            if (!this.Center.IsInside(box)) return false;

            // quick check
            Point3d center = this.Center.ConvertToGlobal();
            if (!box.IsAxisAligned)
            {
                Coord3d coord = new Coord3d(box.Center, box.V1, box.V2);
                center = center.ConvertTo(coord);
            }
            if (Abs(center.X - box.Center.X) > 0.5 * box.L1 - this.R) return false;
            if (Abs(center.Y - box.Center.Y) > 0.5 * box.L2 - this.R) return false;
            if (Abs(center.Z - box.Center.Z) > 0.5 * box.L3 - this.R) return false;


            Plane3d p = new Plane3d(box.P1, box.P2, box.P3);
            if (this.DistanceTo(p) < GeometRi3D.Tolerance) return false;

            p = new Plane3d(box.P1, box.P2, box.P6);
            if (this.DistanceTo(p) < GeometRi3D.Tolerance) return false;

            p = new Plane3d(box.P2, box.P3, box.P7);
            if (this.DistanceTo(p) < GeometRi3D.Tolerance) return false;

            p = new Plane3d(box.P3, box.P4, box.P8);
            if (this.DistanceTo(p) < GeometRi3D.Tolerance) return false;

            p = new Plane3d(box.P4, box.P1, box.P5);
            if (this.DistanceTo(p) < GeometRi3D.Tolerance) return false;

            p = new Plane3d(box.P5, box.P6, box.P7);
            if (this.DistanceTo(p) < GeometRi3D.Tolerance) return false;

            return true;
        }
        #endregion

        /// <summary>
        /// Returns point on circle for given parameter 't' (0 &lt;= t &lt; 2Pi)
        /// </summary>
        public Point3d ParametricForm(double t)
        {

            // Get two orthogonal coplanar vectors
            Vector3d v1 = _r * _normal.OrthogonalVector.Normalized;
            Vector3d v2 = _r * (_normal.Cross(v1)).Normalized;
            return _point + v1.ToPoint * Cos(t) + v2.ToPoint * Sin(t);

        }

        /// <summary>
        /// Distance from circle to plane
        /// </summary>
        public double DistanceTo(Plane3d s)
        {
            double center_distance = this.Center.DistanceTo(s);
            double angle = this.Normal.AngleTo(s.Normal);
            double delta = Abs(this.R * Sin(angle));

            if (delta < center_distance)
            {
                return center_distance - delta;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Shortest distance between plane and circle
        /// <para> The output points may be not unique in case of parallel or intersecting objects.</para>
        /// </summary>
        /// <param name="p">Target plane</param>
        /// <param name="point_on_circle">Closest point on circle</param>
        /// <param name="point_on_plane">Closest point on plane</param>
        public double DistanceTo(Plane3d p, out Point3d point_on_circle, out Point3d point_on_plane)
        {
            if (this.IsParallelTo(p))
            {
                point_on_circle = this.Center;
                point_on_plane = point_on_circle.ProjectionTo(p);
                return point_on_circle.DistanceTo(point_on_plane);
            }

            Vector3d v1 = this.Normal.Cross(p.Normal);
            Vector3d v2 = this.Normal.Cross(v1);
            Line3d l = new Line3d(this.Center, v2);
            Point3d intersection_point = (Point3d)l.IntersectionWith(p);

            if (intersection_point.DistanceTo(this) <= GeometRi3D.DefaultTolerance)
            {
                point_on_circle = intersection_point;
                point_on_plane = intersection_point;
                return 0;
            }
            else
            {
                v1 = new Vector3d(this.Center, intersection_point).Normalized;
                point_on_circle = this.Center.Translate(this.R * v1);
                point_on_plane = point_on_circle.ProjectionTo(p);
                return point_on_circle.DistanceTo(point_on_plane);
            }
        }

        /// <summary>
        /// Shortest distance from point to circle (including interior points)
        /// </summary>
        public double DistanceTo(Point3d p)
        {
            Point3d projection = p.ProjectionTo(this.ToPlane);
            if (projection.Coord != this.Center.Coord)
            {
                projection = projection.ConvertTo(this.Center.Coord);
            }
            double proj_dist_cent = projection.DistanceTo(this.Center);

            if (proj_dist_cent <= this.R)
            {
                return projection.DistanceTo(p);
            }
            else
            {
                // find closest point on circle's boundary
                double x = this._point.X + this.R / proj_dist_cent * (projection.X - this._point.X);
                double y = this._point.Y + this.R / proj_dist_cent * (projection.Y - this._point.Y);
                double z = this._point.Z + this.R / proj_dist_cent * (projection.Z - this._point.Z);

                return Sqrt((x - p.X) * (x - p.X) + (y - p.Y) * (y - p.Y) + (z - p.Z) * (z - p.Z));
            }
        }

        /// <summary>
        /// Point on circle (including interior points) closest to target point "p".
        /// </summary>
        public Point3d ClosestPoint(Point3d p)
        {
            Point3d projection = p.ProjectionTo(this.ToPlane);
            if (projection.Coord != this.Center.Coord)
            {
                projection = projection.ConvertTo(this.Center.Coord);
            }
            double proj_dist_cent = projection.DistanceTo(this.Center);

            if (proj_dist_cent <= this.R)
            {
                return projection;
            }
            else
            {
                // find closest point on circle's boundary
                double x = this._point.X + this.R / proj_dist_cent * (projection.X - this._point.X);
                double y = this._point.Y + this.R / proj_dist_cent * (projection.Y - this._point.Y);
                double z = this._point.Z + this.R / proj_dist_cent * (projection.Z - this._point.Z);

                return new Point3d(x, y, z, this.Center.Coord);
            }
        }

        /// <summary>
        /// Shortest distance between two circles (including interior points) (approximate solution)
        /// </summary>
        public double DistanceTo(Circle3d c)
        {
            double dist;
            if (this.Normal.IsParallelTo(c.Normal))
            {
                Point3d projection = c.Center.ProjectionTo(this.ToPlane);
                dist = projection.DistanceTo(this.Center);
                double vdist = projection.DistanceTo(c.Center);
                if (dist < this.R + c.R)
                {
                    return vdist;
                }
                else
                {
                    return Sqrt((dist - this.R - c.R) * (dist - this.R - c.R) + vdist * vdist);
                }
            }

            double tol = GeometRi3D.Tolerance;
            bool mode = GeometRi3D.UseAbsoluteTolerance;
            GeometRi3D.Tolerance = GeometRi3D.DefaultTolerance;
            GeometRi3D.UseAbsoluteTolerance = true;

            object obj = this.IntersectionWith(c);
            if (obj != null)
            {
                // Restore initial state
                GeometRi3D.UseAbsoluteTolerance = mode;
                GeometRi3D.Tolerance = tol;

                return 0;
            }

            Point3d p_on_circle, p_on_plane;
            dist = this.DistanceTo(c.ToPlane, out p_on_circle, out p_on_plane);
            if (p_on_plane.DistanceTo(c.Center) <= c.R)
            {
                // Restore initial state
                GeometRi3D.UseAbsoluteTolerance = mode;
                GeometRi3D.Tolerance = tol;
                return dist;
            }

            dist = c.DistanceTo(this.ToPlane, out p_on_circle, out p_on_plane);
            if (p_on_plane.DistanceTo(this.Center) <= this.R)
            {
                // Restore initial state
                GeometRi3D.UseAbsoluteTolerance = mode;
                GeometRi3D.Tolerance = tol;
                return dist;
            }

            dist = _distance_circle_to_circle(this, c, out Point3d p1, out Point3d p2);
            // Restore initial state
            GeometRi3D.UseAbsoluteTolerance = mode;
            GeometRi3D.Tolerance = tol;
            return dist;

        }

        /// <summary>
        /// Shortest distance between two circles (including interior points) (approximate solution)
        /// <para> The output points may be not unique in case of parallel or intersecting circles.</para>
        /// </summary>
        /// <param name="c">Target circle</param>
        /// <param name="p1">Closest point on source circle</param>
        /// <param name="p2">Closest point on target circle</param>
        public double DistanceTo(Circle3d c, out Point3d p1, out Point3d p2)
        {
            double dist;
            if (this.Normal.IsParallelTo(c.Normal))
            {
                Point3d projection = c.Center.ProjectionTo(this.ToPlane);
                dist = projection.DistanceTo(this.Center);
                double vdist = projection.DistanceTo(c.Center);
                if (dist < this.R + c.R)
                {
                    if (projection.BelongsTo(this))
                    {
                        p1 = projection;
                        p2 = c.Center;
                    }
                    else
                    {
                        p1 = this.Center.Translate(this.R * new Vector3d(this.Center, projection).Normalized);
                        p2 = p1.ProjectionTo(c.ToPlane);
                    }
                    return vdist;
                }
                else
                {
                    Vector3d v = new Vector3d(this.Center, projection).Normalized;
                    p1 = this.Center.Translate(this.R * v);
                    p2 = c.Center.Translate(-c.R * v);
                    return Sqrt((dist - this.R - c.R) * (dist - this.R - c.R) + vdist * vdist);
                }
            }

            double tol = GeometRi3D.Tolerance;
            bool mode = GeometRi3D.UseAbsoluteTolerance;
            GeometRi3D.Tolerance = GeometRi3D.DefaultTolerance;
            GeometRi3D.UseAbsoluteTolerance = true;

            object obj = this.IntersectionWith(c);
            if (obj != null)
            {
                // Restore initial state
                GeometRi3D.UseAbsoluteTolerance = mode;
                GeometRi3D.Tolerance = tol;

                if (obj.GetType() == typeof(Point3d))
                {
                    p1 = (Point3d)obj;
                    p2 = (Point3d)obj;
                }
                else if (obj.GetType() == typeof(Segment3d))
                {
                    p1 = ((Segment3d)obj).P1;
                    p2 = ((Segment3d)obj).P1;
                }
                else
                {
                    p1 = ((Circle3d)obj).Center;
                    p2 = ((Circle3d)obj).Center;
                }

                return 0;
            }

            dist = this.DistanceTo(c.ToPlane, out p1, out p2);
            if (p2.DistanceTo(c.Center) <= c.R)
            {
                // Restore initial state
                GeometRi3D.UseAbsoluteTolerance = mode;
                GeometRi3D.Tolerance = tol;
                return dist;
            }

            dist = c.DistanceTo(this.ToPlane, out p2, out p1);
            if (p1.DistanceTo(this.Center) <= this.R)
            {
                // Restore initial state
                GeometRi3D.UseAbsoluteTolerance = mode;
                GeometRi3D.Tolerance = tol;
                return dist;
            }

            dist = _distance_circle_to_circle(this, c, out p1, out p2);

            // Restore initial state
            GeometRi3D.UseAbsoluteTolerance = mode;
            GeometRi3D.Tolerance = tol;

            return dist;

        }

        private double _distance_circle_to_circle(Circle3d c1, Circle3d c2, out Point3d p1, out Point3d p2)
        // Use quadratic interpolation to find closest point on one circle to other
        // p1 and p2 - closest points on both circles
        {
            double tol = GeometRi3D.DefaultTolerance;
            double d1 = 1e20;
            double t1 = 0;
            Point3d p;

            for (int i = 0; i < 16; i++)
            {
                double t = i * Math.PI / 8;
                p = c1.ParametricForm(t);
                double dist = p.DistanceTo(c2);
                if (dist < d1)
                {
                    d1 = dist;
                    t1 = t;
                }
            }
            double t2 = t1 - Math.PI / 8;
            p = c1.ParametricForm(t2);
            double d2 = p.DistanceTo(c2);
            double t3 = t1 + Math.PI / 8;
            p = c1.ParametricForm(t3);
            double d3 = p.DistanceTo(c2);

            int iter = 0;
            bool flag = false;
            while ((d2 - d1 > tol || d3 - d1 > tol)  && d1 > tol)
            {
                if (++iter > 100) break;

                double ax = 2.0 * d1 / (t1 - t2) / (t1 - t3);
                double aa = 0.5 * ax * (t2 + t3);
                double bx = 2.0 * d2 / (t2 - t1) / (t2 - t3);
                double bb = 0.5 * bx * (t1 + t3);
                double cx = 2.0 * d3 / (t3 - t1) / (t3 - t2);
                double cc = 0.5 * cx * (t1 + t2);

                double t = (aa + bb + cc) / (ax + bx + cx);
                p = c1.ParametricForm(t);
                double d = p.DistanceTo(c2);

                if (d > d1)
                // Possible special case, non-smooth function ( f(t)=|t| )
                {
                    flag = true;
                    break;
                }

                if (t>t2 & t<t1)
                {
                    t3 = t1; d3 = d1;
                }
                else
                {
                    t2 = t1; d2 = d1;
                }
                t1 = t; d1 = d;
            }

            if (flag)
            // Possible special case, non-smooth function ( f(t)=|t| )
            {
                while ((d2 - d1 > tol || d3 - d1 > tol) && d1 > tol)
                {
                    if (++iter > 100) break;

                    double t = (t2+t1) / 2;
                    p = c1.ParametricForm(t);
                    double d = p.DistanceTo(c2);
                    if (d < d1)
                    {
                        t3 = t1; d3 = d1;
                        t1 = t;  d1 = d;
                    }
                    else
                    {
                        t2 = t; d2 = d;
                    }

                    t = (t3 + t1) / 2;
                    p = c1.ParametricForm(t);
                    d = p.DistanceTo(c2);
                    if (d < d1)
                    {
                        t2 = t1; d2 = d1;
                        t1 = t; d1 = d;
                    }
                    else
                    {
                        t3 = t; d3 = d;
                    }
                }
            }

            p1 = c1.ParametricForm(t1);
            p2 = c2.ClosestPoint(p1);
            return d1;
        }

        /// <summary>
        /// Intersection check between circle and sphere
        /// </summary>
        public bool Intersects(Sphere s)
        {
            if (this.Center.DistanceTo(s.Center) <= this.R + s.R)
            {
                Object obj = s.IntersectionWith(this.ToPlane);
                if (obj != null && obj.GetType() == typeof(Circle3d))
                {
                    // Check for circle-circle intersection
                    if (this.IntersectionWith((Circle3d)obj) != null)
                        return true;
                }
                else if (obj != null && obj.GetType() == typeof(Point3d))
                {
                    return ((Point3d)obj).BelongsTo(this);
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Intersection check between two circles
        /// </summary>
        public bool Intersects(Circle3d c)
        {
            if (this.IsCoplanarTo(c))
            {
                if (this.Center.DistanceTo(c.Center) <= this.R + c.R) return true;
            }

            if (this.DistanceTo(c.ToPlane) > 0) return false;

            object obj = this.IntersectionWith(c);
            if (obj != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Intersection check between circle and triangle
        /// </summary>
        public bool Intersects(Triangle t)
        {
            if (this.IsCoplanarTo(t))
            {
                if (t.A.BelongsTo(this)) return true;
                if (t.B.BelongsTo(this)) return true;
                if (t.C.BelongsTo(this)) return true;

                if (this.Center.BelongsTo(t)) return true;
                if (this.IntersectionWith(new Segment3d(t.A, t.B)) != null) return true;
                if (this.IntersectionWith(new Segment3d(t.B, t.C)) != null) return true;
                if (this.IntersectionWith(new Segment3d(t.C, t.A)) != null) return true;
            }

            if (this.DistanceTo(t.ToPlane) > 0) return false;

            object obj = this.IntersectionWith(t.ToPlane);
            if (obj != null && obj.GetType() == typeof(Point3d))
            {
                return ((Point3d)obj).BelongsTo(this);
            }
            else if (obj != null && obj.GetType() == typeof(Segment3d))
            {
                return ((Segment3d)obj).IntersectionWith(t) != null;
            }

            return false;
        }

        /// <summary>
        /// Intersection check between circle and box
        /// </summary>
        public bool Intersects(Box3d box)
        {
            return box.Intersects(this);
        }

        /// <summary>
        /// Shortest distance between circle and sphere (including interior points) (approximate solution)
        /// </summary>
        public double DistanceTo(Sphere s)
        {
            Plane3d p = this.ToPlane;
            if (s.Center.ProjectionTo(p).BelongsTo(this))
            {
                return s.DistanceTo(p);
            }

            if (this.Intersects(s))
                return 0;

            Point3d p1, p2;
            double dist = _distance_circle_to_sphere(this, s, out p1, out p2);

            return dist;
        }

        /// <summary>
        /// Shortest distance between two circles (including interior points) (approximate solution)
        /// <para> The output points may be not unique in case of intersecting objects.</para>
        /// </summary>
        /// <param name="s">Target sphere</param>
        /// <param name="p1">Closest point on circle</param>
        /// <param name="p2">Closest point on sphere</param>
        public double DistanceTo(Sphere s, out Point3d p1, out Point3d p2)
        {
            Plane3d p = this.ToPlane;
            if (s.Center.ProjectionTo(p).BelongsTo(this))
            {
                p1 = s.Center.ProjectionTo(p);
                if (s.Center == p1)
                {
                    p2 = s.Center.Translate(s.R * this.Normal.Normalized);
                }
                else
                {
                    p2 = s.Center.Translate(s.R * new Vector3d(s.Center, p1).Normalized);
                }
                return s.DistanceTo(p);
            }

            if (this.Intersects(s))
            {
                Object obj = s.IntersectionWith(p);
                if (obj.GetType() == typeof(Point3d))
                {
                    p1 = (Point3d)obj;
                    p2 = p1;
                }
                else
                {
                    Circle3d c = (Circle3d)obj;
                    p1 = this.Center.Translate(this.R * new Vector3d(this.Center, c.Center).Normalized);
                    p2 = c.Center.Translate(c.R * new Vector3d(c.Center, this.Center).Normalized);
                }
                return 0;
            }

            double dist = _distance_circle_to_sphere(this, s, out p1, out p2);

            return dist;
        }

        private double _distance_circle_to_sphere(Circle3d c1, Sphere c2, out Point3d p1, out Point3d p2)
        // Use quadratic interpolation to find closest point on circle
        // p1 and p2 - closest points on circle and sphere respectively
        {
            double d1 = 1e20;
            double t1 = 0;
            Point3d p;

            for (int i = 0; i < 16; i++)
            {
                double t = i * Math.PI / 8;
                p = c1.ParametricForm(t);
                double dist = p.DistanceTo(c2);
                if (dist < d1)
                {
                    d1 = dist;
                    t1 = t;
                }
            }
            double t2 = t1 - Math.PI / 8;
            p = c1.ParametricForm(t2);
            double d2 = p.DistanceTo(c2);
            double t3 = t1 + Math.PI / 8;
            p = c1.ParametricForm(t3);
            double d3 = p.DistanceTo(c2);

            int iter = 0;
            while (d2 - d1 > 0.2 * GeometRi3D.DefaultTolerance && d1 > GeometRi3D.DefaultTolerance)
            {
                if (++iter > 100) break;

                double ax = 2.0 * d1 / (t1 - t2) / (t1 - t3);
                double aa = 0.5 * ax * (t2 + t3);
                double bx = 2.0 * d2 / (t2 - t1) / (t2 - t3);
                double bb = 0.5 * bx * (t1 + t3);
                double cx = 2.0 * d3 / (t3 - t1) / (t3 - t2);
                double cc = 0.5 * cx * (t1 + t2);

                double t = (aa + bb + cc) / (ax + bx + cx);
                p = c1.ParametricForm(t);
                double d = p.DistanceTo(c2);

                if (d < d1)
                {
                    if (t > t2 & t < t1)
                    {
                        t3 = t1; d3 = d1;
                    }
                    else
                    {
                        t2 = t1; d2 = d1;
                    }
                    t1 = t; d1 = d;
                }
                else
                {
                    if (t < t1)
                    {
                        t2 = t; d2 = d;
                    }
                    else
                    {
                        t3 = t; d3 = d;
                    }
                }


            }

            p1 = c1.ParametricForm(t1);
            p2 = c2.ClosestPoint(p1);
            return d1;
        }

        /// <summary>
        /// Shortest distance between line and circle (including interior points)
        /// </summary>
        public double DistanceTo(Line3d l)
        {
            Point3d point_on_circle, point_on_line;
            double dist = _distance_circle_to_line(l, out point_on_circle, out point_on_line);
            return dist;
        }

        /// <summary>
        /// Shortest distance between line and circle (including interior points)
        /// </summary>
        /// <param name="l">Target line</param>
        /// <param name="point_on_circle">Closest point on circle</param>
        /// <param name="point_on_line">Closest point on line</param>
        public double DistanceTo(Line3d l, out Point3d point_on_circle, out Point3d point_on_line)
        {
            double dist = _distance_circle_to_line(l, out point_on_circle, out point_on_line);
            return dist;
        }

        /// <summary>
        /// Shortest distance between ray and circle (including interior points)
        /// </summary>
        public double DistanceTo(Ray3d r)
        {
            Point3d point_on_circle, point_on_ray;
            return DistanceTo(r, out point_on_circle, out point_on_ray);
        }

        /// <summary>
        /// Shortest distance between ray and circle (including interior points)
        /// </summary>
        /// <param name="r">Target ray</param>
        /// <param name="point_on_circle">Closest point on circle</param>
        /// <param name="point_on_ray">Closest point on ray</param>
        public double DistanceTo(Ray3d r, out Point3d point_on_circle, out Point3d point_on_ray)
        {
            double dist = _distance_circle_to_line(r.ToLine, out point_on_circle, out point_on_ray);

            if (point_on_ray.BelongsTo(r)) return dist;

            point_on_ray = r.Point;
            point_on_circle = this.ClosestPoint(point_on_ray);
            return point_on_ray.DistanceTo(point_on_circle);
        }

        /// <summary>
        /// Shortest distance between segment and circle (including interior points)
        /// </summary>
        public double DistanceTo(Segment3d s)
        {
            Point3d point_on_circle, point_on_ray;
            return DistanceTo(s, out point_on_circle, out point_on_ray);
        }

        /// <summary>
        /// Shortest distance between segment and circle (including interior points)
        /// </summary>
        /// <param name="s">Target segment</param>
        /// <param name="point_on_circle">Closest point on circle</param>
        /// <param name="point_on_segment">Closest point on segment</param>
        public double DistanceTo(Segment3d s, out Point3d point_on_circle, out Point3d point_on_segment)
        {
            double dist = _distance_circle_to_line(s.ToLine, out point_on_circle, out point_on_segment);

            if (point_on_segment.BelongsTo(s)) return dist;

            Point3d point_on_circle1 = this.ClosestPoint(s.P1);
            Point3d point_on_circle2 = this.ClosestPoint(s.P2);
            double dist1 = point_on_circle1.DistanceTo(s.P1);
            double dist2 = point_on_circle2.DistanceTo(s.P2);

            if (dist1 < dist2)
            {
                point_on_circle = point_on_circle1;
                point_on_segment = s.P1;
                return dist1;
            }
            else
            {
                point_on_circle = point_on_circle2;
                point_on_segment = s.P2;
                return dist2;
            }
        }


        /// <summary>
        /// Shortest distance between line and circle (including interior points)
        /// </summary>
        /// <param name="l">Target line</param>
        /// <param name="p1">Closest point on circle</param>
        /// <param name="p2">Closest point on line</param>
        private double _distance_circle_to_line(Line3d l, out Point3d p1, out Point3d p2)
        {

            // Line is parallel
            if (l.IsParallelTo(this))
            {
                p2 = this.Center.ProjectionTo(l);
                p1 = this.ClosestPoint(p2);
                return p1.DistanceTo(p2);
            }

            // Intrsecting line
            object obj = l.IntersectionWith(this);
            if (obj != null)
            {
                p1 = (Point3d)obj;
                p2 = p1;
                return 0;
            }

            Point3d p = (Point3d)l.IntersectionWith(this.ToPlane);
            Vector3d v = new Vector3d(this.Center, p).Normalized;
            p1 = this.Center.Translate(this.R * v);
            p2 = p1.ProjectionTo(l);
            return p1.DistanceTo(p2);
        }


        /// <summary>
        /// Shortest distance between triangle and circle (including interior points)
        /// </summary>
        public double DistanceTo(Triangle t)
        {
            Point3d point_on_circle, point_on_triangle;
            return DistanceTo(t, out point_on_circle, out point_on_triangle);
        }

        /// <summary>
        /// Shortest distance between triangle and circle (including interior points)
        /// </summary>
        /// <param name="t">Target triangle</param>
        /// <param name="point_on_circle">Closest point on circle</param>
        /// <param name="point_on_triangle">Closest point on triangle</param>
        public double DistanceTo(Triangle t, out Point3d point_on_circle, out Point3d point_on_triangle)
        {
            double dist = this.DistanceTo(t.ToPlane, out point_on_circle, out point_on_triangle);
            if (t.DistanceTo(point_on_triangle) <= GeometRi3D.DefaultTolerance)
            {
                return dist;
            }

            Segment3d AB = new Segment3d(t.A, t.B);
            Segment3d BC = new Segment3d(t.B, t.C);
            Segment3d AC = new Segment3d(t.A, t.C);

            dist = this.DistanceTo(AB, out point_on_circle, out point_on_triangle);
            Point3d point_on_circle2, point_on_triangle2;
            double dist2 = this.DistanceTo(BC, out point_on_circle2, out point_on_triangle2);
            if (dist2 < dist)
            {
                dist = dist2;
                point_on_circle = point_on_circle2;
                point_on_triangle = point_on_triangle2;
            }
            dist2 = this.DistanceTo(AC, out point_on_circle2, out point_on_triangle2);
            if (dist2 < dist)
            {
                dist = dist2;
                point_on_circle = point_on_circle2;
                point_on_triangle = point_on_triangle2;
            }
            return dist;
        }

        /// <summary>
        /// Shortest distance from circle to box
        /// </summary>
        public double DistanceTo(Box3d box)
        {
            return box.DistanceTo(this);
        }



        /// <summary>
        /// Orthogonal projection of the circle to plane
        /// </summary>
        public Ellipse ProjectionTo(Plane3d s)
        {
            return this.ToEllipse.ProjectionTo(s);
        }

        /// <summary>
        /// Orthogonal projection of the circle to line
        /// </summary>
        public Segment3d ProjectionTo(Line3d l)
        {
            double s = _r * Cos(l.AngleTo(this));
            Vector3d v = l.Direction.Normalized;
            Point3d p = _point.ProjectionTo(l);
            return new Segment3d(p.Translate(-s * v), p.Translate(s * v));
        }

        /// <summary>
        /// Intersection of circle with line.
        /// Returns 'null' (no intersection) or object of type 'Point3d' or 'Segment3d'.
        /// </summary>
        public object IntersectionWith(Line3d l)
        {
            Ellipse e = this.ToEllipse;
            return e.IntersectionWith(l);
        }

        /// <summary>
        /// Intersection of circle with segment.
        /// Returns 'null' (no intersection) or object of type 'Point3d' or 'Segment3d'.
        /// </summary>
        public object IntersectionWith(Segment3d s)
        {
            Ellipse e = this.ToEllipse;
            return e.IntersectionWith(s);
        }

        /// <summary>
        /// Intersection of circle with ray.
        /// Returns 'null' (no intersection) or object of type 'Point3d' or 'Segment3d'.
        /// </summary>
        public object IntersectionWith(Ray3d r)
        {
            Ellipse e = this.ToEllipse;
            return e.IntersectionWith(r);
        }

        /// <summary>
        /// Intersection of circle with plane.
        /// Returns 'null' (no intersection) or object of type 'Circle3d', 'Point3d' or 'Segment3d'.
        /// </summary>
        public object IntersectionWith(Plane3d s)
        {

            // Relative tolerance ================================
            if (!GeometRi3D.UseAbsoluteTolerance)
            {
                double tol = GeometRi3D.Tolerance;
                GeometRi3D.Tolerance = tol * this.R;
                GeometRi3D.UseAbsoluteTolerance = true;
                object result = this.IntersectionWith(s);
                GeometRi3D.UseAbsoluteTolerance = false;
                GeometRi3D.Tolerance = tol;
                return result;
            }
            //====================================================

            if (this.Normal.IsParallelTo(s.Normal))
            {
                if (this.Center.BelongsTo(s))
                {
                    // coplanar objects
                    return this.Copy();
                }
                else
                {
                    // parallel objects
                    return null;
                }
            }
            else
            {
                Line3d l = (Line3d)s.IntersectionWith(new Plane3d(this.Center, this.Normal));
                Coord3d local_coord = new Coord3d(this.Center, l.Direction, this.Normal.Cross(l.Direction));
                Point3d p = l.Point.ConvertTo(local_coord);

                if (GeometRi3D.Greater(Abs(p.Y), this.R))
                {
                    return null;
                }
                else if (GeometRi3D.AlmostEqual(p.Y, this.R))
                {
                    return new Point3d(0, this.R, 0, local_coord);
                }
                else if (GeometRi3D.AlmostEqual(p.Y, -this.R))
                {
                    return new Point3d(0, -this.R, 0, local_coord);
                }
                else
                {
                    double d = Sqrt(Math.Pow(this.R, 2) - Math.Pow(p.Y, 2));
                    Point3d p1 = new Point3d(-d, p.Y, 0, local_coord);
                    Point3d p2 = new Point3d(d, p.Y, 0, local_coord);
                    return new Segment3d(p1, p2);
                }
            }

        }

        /// <summary>
        /// Intersection of two circles.
        /// Returns 'null' (no intersection) or object of type 'Circle3d', 'Point3d' or 'Segment3d'.
        /// In 2D (coplanar circles) the segment will define two intersection points.
        /// </summary>
        public object IntersectionWith(Circle3d c)
        {

            // Relative tolerance ================================
            if (!GeometRi3D.UseAbsoluteTolerance)
            {
                double tol = GeometRi3D.Tolerance;
                GeometRi3D.Tolerance = tol * Max(this.R, c.R);
                GeometRi3D.UseAbsoluteTolerance = true;
                object result = this.IntersectionWith(c);
                GeometRi3D.UseAbsoluteTolerance = false;
                GeometRi3D.Tolerance = tol;
                return result;
            }
            //====================================================

            if (this.Normal.IsParallelTo(c.Normal))
            {
                if (this.Center.BelongsTo(new Plane3d(c.Center, c.Normal)))
                {
                    // Coplanar objects
                    // Search 2D intersection of two circles

                    // Equal circles
                    if (this.Center == c.Center && GeometRi3D.AlmostEqual(this.R, c.R))
                    {
                        return this.Copy();
                    }

                    double d = this.Center.DistanceTo(c.Center);

                    // Separated circles
                    if (GeometRi3D.Greater(d, this.R + c.R))
                        return null;

                    // One circle inside the other
                    if (d < Abs(this.R - c.R) - GeometRi3D.Tolerance)
                    {
                        if (this.R > c.R)
                        {
                            return c.Copy();
                        }
                        else
                        {
                            return this.Copy();
                        }
                    }

                    // Outer tangency
                    if (GeometRi3D.AlmostEqual(d, this.R + c.R))
                    {
                        Vector3d vec = new Vector3d(this.Center, c.Center);
                        return this.Center.Translate(this.R * vec.Normalized);
                    }

                    // Inner tangency
                    if (Abs(Abs(this.R - c.R) - d) < GeometRi3D.Tolerance)
                    {
                        Vector3d vec = new Vector3d(this.Center, c.Center);
                        if (this.R > c.R)
                        {
                            return this.Center.Translate(this.R * vec.Normalized);
                        }
                        else
                        {
                            return this.Center.Translate(-this.R * vec.Normalized);
                        }
                        
                    }

                    // intersecting circles
                    // Create local CS with origin in circle's center
                    Vector3d vec1 = new Vector3d(this.Center, c.Center);
                    Vector3d vec2 = vec1.Cross(this.Normal);
                    Coord3d local_cs = new Coord3d(this.Center, vec1, vec2);

                    double x = 0.5 * (d * d - c.R * c.R + this.R * this.R) / d;
                    double y = 0.5 * Sqrt((-d + c.R - this.R) * (-d - c.R + this.R) * (-d + c.R + this.R) * (d + c.R + this.R)) / d;
                    Point3d p1 = new Point3d(x, y, 0, local_cs);
                    Point3d p2 = new Point3d(x, -y, 0, local_cs);
                    return new Segment3d(p1, p2);
                }
                else
                {
                    // parallel objects
                    return null;
                }
            }
            else
            {
                // Check 3D intersection
                Plane3d plane = new Plane3d(this.Center, this.Normal);
                object obj = plane.IntersectionWith(c);

                if (obj == null)
                {
                    return null;
                }
                else if (obj.GetType() == typeof(Point3d))
                {
                    Point3d p = (Point3d)obj;
                    if (p.BelongsTo(c))
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
                    Segment3d s = (Segment3d)obj;
                    return s.IntersectionWith(this);
                }

            }

        }

        internal override int _PointLocation(Point3d p)
        {
            if (GeometRi3D.UseAbsoluteTolerance)
            {
                Plane3d s = new Plane3d(this.Center, this.Normal);
                Point3d proj = p.ProjectionTo(s);
                if (GeometRi3D.AlmostEqual(p.DistanceTo(proj), 0))
                {
                    if ( GeometRi3D.Smaller(p.DistanceTo(this.Center), this.R))
                    {
                        return 1; // Point is strictly inside
                    }
                    else if (GeometRi3D.AlmostEqual(p.DistanceTo(this.Center), this.R) )
                    {
                        return 0; // Point is on boundary
                    }
                    else
                    {
                        return -1; // Point is outside
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
                GeometRi3D.Tolerance = tol * this.R;
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
        /// Translate circle by a vector
        /// </summary>
        public Circle3d Translate(Vector3d v)
        {
            return new Circle3d(this.Center.Translate(v), this.R, this.Normal);
        }

        /// <summary>
        /// Rotate circle by a given rotation matrix
        /// </summary>
        [System.Obsolete("use Rotation object and specify rotation center: this.Rotate(Rotation r, Point3d p)")]
        public Circle3d Rotate(Matrix3d m)
        {
            return new Circle3d(this.Center.Rotate(m), this.R, this.Normal.Rotate(m));
        }

        /// <summary>
        /// Rotate circle by a given rotation matrix around point 'p' as a rotation center
        /// </summary>
        [System.Obsolete("use Rotation object: this.Rotate(Rotation r, Point3d p)")]
        public Circle3d Rotate(Matrix3d m, Point3d p)
        {
            return new Circle3d(this.Center.Rotate(m, p), this.R, this.Normal.Rotate(m));
        }

        /// <summary>
        /// Rotate circle around point 'p' as a rotation center
        /// </summary>
        public Circle3d Rotate(Rotation r, Point3d p)
        {
            return new Circle3d(this.Center.Rotate(r, p), this.R, this.Normal.Rotate(r));
        }

        /// <summary>
        /// Reflect circle in given point
        /// </summary>
        public Circle3d ReflectIn(Point3d p)
        {
            return new Circle3d(this.Center.ReflectIn(p), this.R, this.Normal.ReflectIn(p));
        }

        /// <summary>
        /// Reflect circle in given line
        /// </summary>
        public Circle3d ReflectIn(Line3d l)
        {
            return new Circle3d(this.Center.ReflectIn(l), this.R, this.Normal.ReflectIn(l));
        }

        /// <summary>
        /// Reflect circle in given plane
        /// </summary>
        public Circle3d ReflectIn(Plane3d s)
        {
            return new Circle3d(this.Center.ReflectIn(s), this.R, this.Normal.ReflectIn(s));
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
            Circle3d c = (Circle3d)obj;

            if (GeometRi3D.UseAbsoluteTolerance)
            {
                return c.Center == this.Center && Abs(c.R - this.R) <= GeometRi3D.Tolerance && c.Normal.IsParallelTo(this.Normal);
            }
            else
            {
                return Abs(c.Center.DistanceTo(this.Center)) / this.R <= GeometRi3D.Tolerance && 
                       Abs(c.R - this.R) / this.R  <= GeometRi3D.Tolerance && 
                       c.Normal.IsParallelTo(this.Normal);
            }
        }

        /// <summary>
        /// Returns the hashcode for the object.
        /// </summary>
        public override int GetHashCode()
        {
            return GeometRi3D.HashFunction(_point.GetHashCode(), _r.GetHashCode(), _normal.GetHashCode());
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
            Vector3d normal = _normal.ConvertTo(coord);

            string str = string.Format("Circle: ") + nl;
            str += string.Format("  Center -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", P.X, P.Y, P.Z) + nl;
            str += string.Format("  Radius -> {0,10:g5}", _r) + nl;
            str += string.Format("  Normal -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", normal.X, normal.Y, normal.Z);
            return str;
        }

        // Operators overloads
        //-----------------------------------------------------------------

        public static bool operator ==(Circle3d c1, Circle3d c2)
        {
            if (object.ReferenceEquals(c1, null))
                return object.ReferenceEquals(c2, null);
            return c1.Equals(c2);
        }
        public static bool operator !=(Circle3d c1, Circle3d c2)
        {
            if (object.ReferenceEquals(c1, null))
                return !object.ReferenceEquals(c2, null);
            return !c1.Equals(c2);
        }

    }
}


