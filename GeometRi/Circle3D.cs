using System;
using System.Collections.Generic;
using System.Drawing;
using static System.Math;

namespace GeometRi
{
    /// <summary>
    /// Circle in 3D space defined by center point, radius and normal vector.
    /// </summary>
#if NET20_OR_GREATER
    [Serializable]
#endif
    public class Circle3d : FiniteObject, IPlanarObject, IFiniteObject
    {

        internal Point3d _point;
        private double _r;
        internal Vector3d _normal;

        /// <summary>
        /// Initializes circle instance using center point, radius and normal vector.
        /// </summary>
        public Circle3d(Point3d Center, double Radius, Vector3d Normal)
        {
            _point = Center.Copy();
            _r = Radius;
            _normal = Normal.Normalized;
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
            _normal = v1.Cross(v2).Normalized;

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
            get { return _point; }
            set { _point = value.Copy(); }
        }

        /// <summary>
        /// X component of the circles' center
        /// </summary>
        public double X
        {
            get { return _point.X; }
            //set { _point.X = value; }
        }

        /// <summary>
        /// Y component of the circles' center
        /// </summary>
        public double Y
        {
            get { return _point.Y; }
            //set { _point.Y = value; }
        }

        /// <summary>
        /// Z component of the circles' center
        /// </summary>
        public double Z
        {
            get { return _point.Z; }
            //set { _point.Z = value; }
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
            get { return _normal; }
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
            get { return PI * _r * _r; }
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
                Vector3d v3 = this.Normal;
                Matrix3d m = new Matrix3d(v1, v2, v3);
                Rotation r = new Rotation(m.Transpose());
                return new Box3d(_point, 2.0 * _r, 2.0 * _r, 0, r);
            }
        }

        /// <summary>
        /// Return Bounding Box in given coordinate system.
        /// </summary>
        public Box3d BoundingBox(Coord3d coord = null)
        {
            coord = (coord == null) ? Coord3d.GlobalCS : coord;
            Vector3d v = _normal.ConvertTo(coord);

            double s1 = _r * Sqrt(1 - v.X * v.X);
            double s2 = _r * Sqrt(1 - v.Y * v.Y);
            double s3 = _r * Sqrt(1 - v.Z * v.Z);

            return new Box3d(_point, 2 * s1, 2 * s2, 2 * s3, coord);
        }

        /// <summary>
        /// Return Axis Aligned Bounding Box (AABB).
        /// </summary>
        public AABB AABB()
        {
            Vector3d v = _normal.ConvertToGlobal();

            double s1 = _r * Sqrt(1 - v.X * v.X);
            double s2 = _r * Sqrt(1 - v.Y * v.Y);
            double s3 = _r * Sqrt(1 - v.Z * v.Z);

            return new AABB(_point, 2 * s1, 2 * s2, 2 * s3);
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

            if (!this._point.IsInside(box)) return false;

            Coord3d local_coord = box.LocalCoord();
            Box3d circle_box = this.BoundingBox(local_coord);
            Point3d p = _point.ConvertTo(local_coord);

            if (box.L1 / 2 - (Abs(p.X) + circle_box.L1 / 2) < GeometRi3D.Tolerance) return false;
            if (box.L2 / 2 - (Abs(p.Y) + circle_box.L2 / 2) < GeometRi3D.Tolerance) return false;
            if (box.L3 / 2 - (Abs(p.Z) + circle_box.L3 / 2) < GeometRi3D.Tolerance) return false;

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
            return _point + v1 * Cos(t) + v2 * Sin(t);
        }

        #region "DistanceMethods"
        /// <summary>
        /// Distance from circle to plane
        /// </summary>
        public double DistanceTo(Plane3d s)
        {
            double center_distance = Math.Abs((this._point - s._point).Dot(s._normal));
            double sin_angle = this._normal.Cross(s._normal).Norm;
            double delta = Abs(this.R * sin_angle);

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

            Vector3d v1 = this._normal.Cross(p.Normal);
            Vector3d v2 = this._normal.Cross(v1);

            double d = (p._point - this._point).Dot(p._normal) / (p._normal * v2);
            Point3d intersection_point = this._point.Translate(d * v2);

            if (intersection_point.DistanceTo(this.Center) <= this.R)
            {
                point_on_circle = intersection_point;
                point_on_plane = intersection_point;
                return 0;
            }
            else
            {
                point_on_circle = this._point.Translate(this.R * v2.Normalized);
                point_on_plane = point_on_circle.ProjectionTo(p);
                return point_on_circle.DistanceTo(point_on_plane);

                //Point3d delta = point_on_circle - p._point;
                //double dist = delta.Dot(p._normal);
                //point_on_plane = point_on_circle - dist * p._normal;
                //return dist;

            }
        }

        /// <summary>
        /// Shortest distance from point to circle (including interior points)
        /// </summary>
        public double DistanceTo(Point3d p)
        {
            Point3d delta = p - this._point;
            Point3d Q = p.Subtract(this._normal, delta.Dot(this._normal));
            double dist = this._point.DistanceTo(Q);
            if (dist < this._r)
            {
                return Q.DistanceTo(p); ;
            }
            else
            {
                Point3d K = this._point + this._r / dist * (Q - this._point);
                return K.DistanceTo(p); ;
            }
        }

        /// <summary>
        /// Point on circle (including interior points) closest to target point "p".
        /// </summary>
        public Point3d ClosestPoint(Point3d p)
        {
            Point3d delta = p - this._point;
            Point3d Q = p.Subtract(this._normal, delta.Dot(this._normal));
            double dist = this._point.DistanceTo(Q);
            if (dist < this._r)
            {
                return Q;
            }
            else
            {
                return this._point + this._r / dist * (Q - this._point);
            }
        }

        /// <summary>
        /// Point on circle (excluding interior points) closest to target point "p".
        /// </summary>
        public Point3d ClosestPointOnBoundary(Point3d p)
        {
            Point3d delta = p - this._point;
            Point3d Q = p.Subtract(this._normal, delta.Dot(this._normal));
            double dist = this._point.DistanceTo(Q);
            return this._point + this._r / dist * (Q - this._point);
        }

        /// <summary>
        /// Shortest distance between two circles (including interior points) (approximate solution)
        /// <para> Default tolerance for numerical solution: GeometRi3D.DefaultTolerance.</para>
        /// </summary>
        /// <param name="c">Target circle</param>
        public double DistanceTo(Circle3d c)
        {
            return DistanceTo(c, GeometRi3D.DefaultTolerance);
        }

        /// <summary>
        /// Shortest distance between two circles (including interior points) (approximate solution)
        /// </summary>
        /// <param name="c">Target circle</param>
        /// <param name="tolerance">Tolerance for numerical solution, default GeometRi3D.DefaultTolerance</param>
        public double DistanceTo(Circle3d c, double tolerance)
        {
            double dist;
            if (GeometRi3D.AlmostEqual(this._normal * c._normal, 1.0))
            {
                Point3d projection = c._point.ProjectionTo(this.ToPlane);
                dist = projection.DistanceTo(this._point);
                double vdist = projection.DistanceTo(c._point);
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

            //object obj = this.IntersectionWith(c);
            if (_circle_circle_intersection_check_2(c))
            {
                // Restore initial state
                GeometRi3D.UseAbsoluteTolerance = mode;
                GeometRi3D.Tolerance = tol;

                return 0;
            }

            Point3d p_on_circle, p_on_plane;
            dist = this.DistanceTo(c.ToPlane, out p_on_circle, out p_on_plane);
            if (p_on_plane.DistanceTo(c._point) <= c.R)
            {
                // Restore initial state
                GeometRi3D.UseAbsoluteTolerance = mode;
                GeometRi3D.Tolerance = tol;
                return dist;
            }

            dist = c.DistanceTo(this.ToPlane, out p_on_circle, out p_on_plane);
            if (p_on_plane.DistanceTo(this._point) <= this.R)
            {
                // Restore initial state
                GeometRi3D.UseAbsoluteTolerance = mode;
                GeometRi3D.Tolerance = tol;
                return dist;
            }

            dist = _distance_circle_to_circle(this, c, out Point3d p1, out Point3d p2, tolerance);
            // Restore initial state
            GeometRi3D.UseAbsoluteTolerance = mode;
            GeometRi3D.Tolerance = tol;
            return dist;

        }

        /// <summary>
        /// Shortest distance between two circles (including interior points) (approximate solution)
        /// <para> The output points may be not unique in case of parallel or intersecting circles.</para>
        /// <para> Default tolerance for numerical solution: GeometRi3D.DefaultTolerance.</para>
        /// </summary>
        /// <param name="c">Target circle</param>
        /// <param name="p1">Closest point on source circle</param>
        /// <param name="p2">Closest point on target circle</param>
        public double DistanceTo(Circle3d c, out Point3d p1, out Point3d p2)
        {
            return DistanceTo(c, out p1, out p2, GeometRi3D.DefaultTolerance);
        }

        /// <summary>
        /// Shortest distance between two circles (including interior points) (approximate solution)
        /// <para> The output points may be not unique in case of parallel or intersecting circles.</para>
        /// </summary>
        /// <param name="c">Target circle</param>
        /// <param name="p1">Closest point on source circle</param>
        /// <param name="p2">Closest point on target circle</param>
        /// <param name="tolerance">Tolerance for numerical solution, default GeometRi3D.DefaultTolerance</param>
        public double DistanceTo(Circle3d c, out Point3d p1, out Point3d p2, double tolerance)
        {
            double dist;
            if (GeometRi3D.AlmostEqual(this._normal * c._normal, 1.0))
            {
                Point3d projection = c._point.ProjectionTo(this.ToPlane);
                dist = projection.DistanceTo(this._point);
                double vdist = projection.DistanceTo(c._point);
                if (dist < this.R + c.R)
                {
                    if (projection.BelongsTo(this))
                    {
                        p1 = projection;
                        p2 = c.Center;
                    }
                    else
                    {
                        p1 = this._point.Translate(this.R * new Vector3d(this._point, projection).Normalized);
                        p2 = p1.ProjectionTo(c.ToPlane);
                    }
                    return vdist;
                }
                else
                {
                    Vector3d v = new Vector3d(this._point, projection).Normalized;
                    p1 = this._point.Translate(this.R * v);
                    p2 = c._point.Translate(-c.R * v);
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
            if (p2.DistanceTo(c._point) <= c.R)
            {
                // Restore initial state
                GeometRi3D.UseAbsoluteTolerance = mode;
                GeometRi3D.Tolerance = tol;
                return dist;
            }

            dist = c.DistanceTo(this.ToPlane, out p2, out p1);
            if (p1.DistanceTo(this._point) <= this.R)
            {
                // Restore initial state
                GeometRi3D.UseAbsoluteTolerance = mode;
                GeometRi3D.Tolerance = tol;
                return dist;
            }

            dist = _distance_circle_to_circle(this, c, out p1, out p2, tolerance);

            // Restore initial state
            GeometRi3D.UseAbsoluteTolerance = mode;
            GeometRi3D.Tolerance = tol;

            return dist;

        }


        /// <summary>
        /// Check if distance between two circles is greater than threshold
        /// </summary>
        public bool DistanceGreater(Circle3d c, double threshold, double tolerance)
        {

            // Relative tolerance ================================
            if (!GeometRi3D.UseAbsoluteTolerance)
            {
                double tol = GeometRi3D.Tolerance;
                GeometRi3D.Tolerance = tol * Max(this.R, c.R);
                GeometRi3D.UseAbsoluteTolerance = true;
                bool result = this.DistanceGreater(c, threshold, tolerance);
                GeometRi3D.UseAbsoluteTolerance = false;
                GeometRi3D.Tolerance = tol;
                return result;
            }
            //====================================================

            // Early exit (separated circles)
            double d = this._point.DistanceTo(c._point);
            if (d > this.R + c.R + GeometRi3D.Tolerance + threshold)
                return true;

            if (GeometRi3D.AlmostEqual(this._normal * c._normal, 1.0))
            {
                if (this._point.BelongsTo(new Plane3d(c._point, c._normal)))
                {
                    // Coplanar objects
                    if (d <= this.R + c.R + GeometRi3D.Tolerance + threshold)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    // parallel objects
                    return this.DistanceTo(c, out Point3d p1, out Point3d p2, tolerance) > threshold;
                }
            }
            else
            {
                // Check 3D intersection
                //Vector3d v = new Vector3d(this._point, c._point);
                Point3d v = c._point - this._point;
                //double this_norm = this._normal.Norm;
                //double c_norm = c._normal.Norm;
                double this_norm = 1;
                double c_norm = 1;

                double cos_angle1 = v.Dot(this._normal) / this_norm / d;
                double delta1 = Abs(d * cos_angle1);

                double sin_angle2 = this._normal.Cross(c._normal).Norm / this_norm / c_norm;
                double delta2 = Abs(this.R * sin_angle2);

                if (delta1 > delta2 + threshold) return true;

                cos_angle1 = v.Dot(c._normal) / c_norm / d;
                delta1 = Abs(d * cos_angle1);
                delta2 = Abs(c.R * sin_angle2);

                if (delta1 > delta2 + threshold) return true;

                return this.DistanceTo(c, tolerance) > threshold;

            }
        }

        private double _distance_circle_to_circle(Circle3d c1, Circle3d c2, out Point3d p1, out Point3d p2, double tol)
        {
            double dist_prev = double.PositiveInfinity;
            int max_iter = 100;
            p1 = c1.ClosestPointOnBoundary(c2.Center);
            p2 = c2.ClosestPointOnBoundary(p1);
            double dist = p1.DistanceTo(p2);

            int iter = 0;
            while (iter < max_iter)
            {
                dist_prev = dist;
                p1 = c1.ClosestPointOnBoundary(p2);
                p2 = c2.ClosestPointOnBoundary(p1);
                dist = p1.DistanceTo(p2);

                if (Math.Abs(dist - dist_prev) < tol)
                {
                    return dist;
                }

                iter++;
            }
            return dist;

        }

        private double _distance_circle_to_circle_old(Circle3d c1, Circle3d c2, out Point3d p1, out Point3d p2, double tol)
        // Use quadratic interpolation to find closest point on one circle to other
        // p1 and p2 - closest points on both circles
        {
            //double tol = GeometRi3D.DefaultTolerance;
            double d1 = 1e20;
            double t1 = 0;
            Point3d p;

            // Prepare data for parametric form for circle "c1".
            // _point + v1.ToPoint * Cos(t) + v2.ToPoint * Sin(t);
            // Get two orthogonal vectors coplanar "c1"
            Vector3d v1 = c1._r * c1._normal.OrthogonalVector.Normalized;
            Vector3d v2 = c1._r * (c1._normal.Cross(v1)).Normalized;
            Point3d pf1 = v1.ToPoint.ConvertTo(c1._point.Coord);
            Point3d pf2 = v2.ToPoint.ConvertTo(c1._point.Coord);

            for (int i = 0; i < 16; i++)
            {
                double t = i * Math.PI / 8;
                p = c1._point + pf1 * Cos(t) + pf2 * Sin(t);
                double dist = p.DistanceTo(c2);
                if (dist < d1)
                {
                    d1 = dist;
                    t1 = t;
                }
            }
            double t2 = t1 - Math.PI / 8;
            p = c1._point + pf1 * Cos(t2) + pf2 * Sin(t2);
            double d2 = p.DistanceTo(c2);
            double t3 = t1 + Math.PI / 8;
            p = c1._point + pf1 * Cos(t3) + pf2 * Sin(t3);
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
                p = c1._point + pf1 * Cos(t) + pf2 * Sin(t);
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
                    p = c1._point + pf1 * Cos(t) + pf2 * Sin(t);
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
                    p = c1._point + pf1 * Cos(t) + pf2 * Sin(t);
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

            p1 = c1._point + pf1 * Cos(t1) + pf2 * Sin(t1);
            p2 = c2.ClosestPoint(p1);
            return d1;
        }

        /// <summary>
        /// Shortest distance between circle and sphere (including interior points) (approximate solution)
        /// </summary>
        public double DistanceTo(Sphere s)
        {
            return DistanceTo(s, GeometRi3D.DefaultTolerance);
        }

        /// <summary>
        /// Shortest distance between circle and sphere (including interior points) (approximate solution)
        /// </summary>
        /// <param name="s">Target sphere</param>
        /// <param name="tolerance">Tolerance for numerical solution, default GeometRi3D.DefaultTolerance</param>
        public double DistanceTo(Sphere s, double tolerance)
        {
            Plane3d p = this.ToPlane;
            if (s.Center.ProjectionTo(p).BelongsTo(this))
            {
                return s.DistanceTo(p);
            }

            if (this.Intersects(s))
                return 0;

            Point3d p1, p2;
            double dist = _distance_circle_to_sphere(this, s, out p1, out p2, tolerance);

            return dist;
        }

        /// <summary>
        /// Shortest distance between circle and sphere (including interior points) (approximate solution)
        /// <para> The output points may be not unique in case of intersecting objects.</para>
        /// <para> Default tolerance for numerical solution: GeometRi3D.DefaultTolerance.</para>
        /// </summary>
        /// <param name="s">Target sphere</param>
        /// <param name="p1">Closest point on circle</param>
        /// <param name="p2">Closest point on sphere</param>
        public double DistanceTo(Sphere s, out Point3d p1, out Point3d p2)
        {
            return DistanceTo(s, out p1, out p2, GeometRi3D.DefaultTolerance);
        }

        /// <summary>
        /// Shortest distance between circle and sphere (including interior points) (approximate solution)
        /// <para> The output points may be not unique in case of intersecting objects.</para>
        /// </summary>
        /// <param name="s">Target sphere</param>
        /// <param name="p1">Closest point on circle</param>
        /// <param name="p2">Closest point on sphere</param>
        /// <param name="tolerance">Tolerance for numerical solution, default GeometRi3D.DefaultTolerance</param>
        public double DistanceTo(Sphere s, out Point3d p1, out Point3d p2, double tolerance)
        {
            Plane3d p = this.ToPlane;
            if (s.Center.ProjectionTo(p).BelongsTo(this))
            {
                p1 = s.Center.ProjectionTo(p);
                if (s.Center == p1)
                {
                    p2 = s.Center.Translate(s.R * this.Normal);
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
                    p1 = this._point.Translate(this.R * new Vector3d(this._point, c._point).Normalized);
                    p2 = c._point.Translate(c.R * new Vector3d(c._point, this._point).Normalized);
                }
                return 0;
            }

            double dist = _distance_circle_to_sphere(this, s, out p1, out p2, tolerance);

            return dist;
        }

        private double _distance_circle_to_sphere(Circle3d c1, Sphere c2, out Point3d p1, out Point3d p2, double tol)
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
            while (d2 - d1 > 0.2 * tol && d1 > tol)
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
        /// Shortest distance between line and circle (excluding interior points)
        /// </summary>
        public double DistanceToBoundary(Line3d l)
        {
            Point3d point_on_circle, point_on_line;
            double dist = _distance_circle_boundary_to_line(l, out point_on_circle, out point_on_line);
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
        /// Shortest distance between line and circle (excluding interior points)
        /// </summary>
        /// <param name="l">Target line</param>
        /// <param name="point_on_circle">Closest point on circle</param>
        /// <param name="point_on_line">Closest point on line</param>
        public double DistanceToBoundary(Line3d l, out Point3d point_on_circle, out Point3d point_on_line)
        {
            double dist = _distance_circle_boundary_to_line(l, out point_on_circle, out point_on_line);
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
            Line3d l = s.Line;
            Plane3d plane = this.ToPlane;
            if (l.IsNotParallelTo(plane._normal))
            {
                Line3d l_projection = (Line3d)l.ProjectionTo(plane);
                Object obj = l_projection.IntersectionWith(this);
                if (obj != null && obj.GetType() == typeof(Segment3d))
                {
                    Segment3d segm = (Segment3d)obj;
                    return s.DistanceTo(segm, out point_on_segment, out point_on_circle);
                }
                if (obj != null && obj.GetType() == typeof(Point3d))
                {
                    point_on_circle = (Point3d)obj;
                    point_on_segment = s.ClosestPoint(point_on_circle);
                    return point_on_segment.DistanceTo(point_on_circle);
                }
            }

            // Line is parallel
            if (l.IsParallelTo(this))
            {
                point_on_segment = this.Center.ProjectionTo(l);
                if (s._AxialPointLocation(point_on_segment) >= 0)
                {
                    point_on_circle = this.ClosestPoint(point_on_segment);
                    return point_on_circle.DistanceTo(point_on_segment);
                }
            }
            
            
            double dist = _distance_circle_boundary_to_line(l, out point_on_circle, out point_on_segment);

            int code = s._AxialPointLocation(point_on_segment);
            if (code >= 0)
            {
                return dist;
            }
            else if (code == -1)
            {
                point_on_circle = this.ClosestPoint(s.P1);
                point_on_segment = s.P1;
            }
            else
            {
                point_on_circle = this.ClosestPoint(s.P2);
                point_on_segment = s.P2;
            }

            return point_on_circle.DistanceTo(point_on_segment);

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

            return _distance_circle_boundary_to_line(l, out p1, out p2);
        }

        /// <summary>
        /// Shortest distance between line and circle's boundary (excluding interior points)
        /// (only one point will be returned for symmetrical case)
        /// </summary>
        /// <param name="l">Target line</param>
        /// <param name="point_on_circle">Closest point on circle</param>
        /// <param name="point_on_line">Closest point on line</param>
        private double _distance_circle_boundary_to_line(Line3d l, out Point3d point_on_circle, out Point3d point_on_line)
        {

            // Line is parallel
            if (l.IsParallelTo(this))
            {
                Plane3d plane = this.ToPlane;
                Line3d line_proj = (Line3d)l.ProjectionTo(plane);

                object obj = line_proj.IntersectionWith(this);
                if (obj == null)
                {
                    // Non-intersecting objects
                    point_on_line = this.Center.ProjectionTo(l);
                    point_on_circle = this.ClosestPoint(point_on_line);
                    return point_on_line.DistanceTo(point_on_circle);
                }
                else if (obj.GetType() == typeof(Point3d))
                {
                    // Touching objects
                    point_on_circle = (Point3d)obj;
                    point_on_line = point_on_circle.ProjectionTo(l);
                    return point_on_line.DistanceTo(point_on_circle);
                }
                else
                {
                    // Intrsecting objects, only one point will be used
                    Segment3d segm = (Segment3d)obj;
                    point_on_circle = segm.P1;
                    point_on_line = point_on_circle.ProjectionTo(l);
                    return point_on_line.DistanceTo(point_on_circle);
                }
            }

            // Orthogonal line
            if (l.IsOrthogonalTo(this))
            {
                Plane3d plane = this.ToPlane;
                Point3d projection_point = (Point3d)l.IntersectionWith(plane);

                if (projection_point == this.Center)
                {
                    point_on_line = projection_point;
                    point_on_circle = this.ParametricForm(0);
                    return point_on_line.DistanceTo(point_on_circle);
                }
                else
                {
                    Vector3d v = new Vector3d(this.Center, projection_point).Normalized;
                    point_on_line = projection_point;
                    point_on_circle = this.Center.Translate(this.R * v);
                    return point_on_line.DistanceTo(point_on_circle);
                }
            }


            // General case

            double d1 = 1e20;
            double t1 = 0;
            Point3d p;

            for (int i = 0; i < 16; i++)
            {
                double t = i * Math.PI / 8;
                p = this.ParametricForm(t);
                double dist = p.DistanceTo(l);
                if (dist < d1)
                {
                    d1 = dist;
                    t1 = t;
                }
            }
            double t2 = t1 - Math.PI / 8;
            p = this.ParametricForm(t2);
            double d2 = p.DistanceTo(l);
            double t3 = t1 + Math.PI / 8;
            p = this.ParametricForm(t3);
            double d3 = p.DistanceTo(l);

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
                p = this.ParametricForm(t);
                double d = p.DistanceTo(l);

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

            point_on_circle = this.ParametricForm(t1);
            point_on_line = point_on_circle.ProjectionTo(l);
            return point_on_line.DistanceTo(point_on_circle);

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
            double dist = this.DistanceTo(t.Plane, out point_on_circle, out point_on_triangle);
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
        /// Shortest distance from circle to convex polyhedron
        /// </summary>
        public double DistanceTo(ConvexPolyhedron cp)
        {
            return cp.DistanceTo(this);
        }
        #endregion

        /// <summary>
        /// Intersection check between circle and sphere
        /// </summary>
        public bool Intersects(Sphere s)
        {
            if (this._point.DistanceTo(s.Center) <= this.R + s.R)
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

            // Relative tolerance ================================
            if (!GeometRi3D.UseAbsoluteTolerance)
            {
                double tol = GeometRi3D.Tolerance;
                GeometRi3D.Tolerance = tol * Max(this.R, c.R);
                GeometRi3D.UseAbsoluteTolerance = true;
                bool result = this.Intersects(c);
                GeometRi3D.UseAbsoluteTolerance = false;
                GeometRi3D.Tolerance = tol;
                return result;
            }
            //====================================================

            // Early exit (separated circles)
            double d = this._point.DistanceTo(c._point);
            if (d > this.R + c.R + GeometRi3D.Tolerance)
                return false;

            //this._normal.IsParallelTo(c._normal)
            if (GeometRi3D.AlmostEqual(this._normal * c._normal, 1.0))
            {
                if (this._point.BelongsTo(new Plane3d(c._point, c._normal)))
                {
                    // Coplanar objects
                    if (d <= this.R + c.R + GeometRi3D.Tolerance)
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
                    // parallel objects
                    return false;
                }
            }
            else
            {
                // Check 3D intersection
                Vector3d v = new Vector3d(this._point, c._point);
                double this_norm = this._normal.Norm;
                double c_norm = c._normal.Norm;

                double cos_angle1 = this._normal * v / this_norm / d;
                double delta1 = Abs(d * cos_angle1);

                double sin_angle2 = this._normal.Cross(c._normal).Norm / this_norm / c_norm;
                double delta2 = Abs(c.R * sin_angle2);

                if (delta1 > delta2) return false;

                cos_angle1 = c._normal * v / c_norm / d;
                delta1 = Abs(d * cos_angle1);
                delta2 = Abs(this.R * sin_angle2);

                if (delta1 > delta2) return false;



                return _circle_circle_intersection_check_2(c);

            }

        }

        private bool _circle_circle_intersection_check_2(Circle3d c)
        {
            Segment3d segm;
            double dist;

            Object obj = this.IntersectionWith(c.ToPlane);
            if (obj == null) return false;
            if (obj != null && obj.GetType() == typeof(Point3d))
            {
                Point3d point = (Point3d)obj;
                dist = c.Center.DistanceTo(point);
            }
            else
            {
                segm = (Segment3d)obj;
                dist = c.Center.DistanceTo(segm);
            }

            if (dist <= c.R + GeometRi3D.Tolerance)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private bool _circle_circle_intersection_check(Circle3d c)
        {
            Plane3d plane_this = new Plane3d(this._point, this._normal);

            Line3d l = (Line3d)plane_this.IntersectionWith(new Plane3d(c._point, c._normal));
            Coord3d local_coord = new Coord3d(this._point, l.Direction, this._normal.Cross(l.Direction));
            Point3d p = l.Point.ConvertTo(local_coord);

            if (GeometRi3D.Greater(Abs(p.Y), this.R))
            {
                // No intersection
                return false;
            }
            else if (GeometRi3D.AlmostEqual(Abs(p.Y), this.R))
            {
                // Intersection in one point
                Point3d pp = new Point3d(0, p.Y, 0, local_coord);
                if (pp.DistanceTo(c._point) <= c.R + GeometRi3D.Tolerance)
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
                double dd = Sqrt(this.R * this.R - p.Y * p.Y);
                Point3d p1 = new Point3d(-dd, p.Y, 0, local_coord);
                Point3d p2 = new Point3d(dd, p.Y, 0, local_coord);

                // check if at least one point is outside circle "c"
                if (p1.DistanceTo(c._point) <= c.R + GeometRi3D.Tolerance) return true;

                // Now check if segment (p1,p2) intrsects circle "c"
                // Use local coord with center in c.Point and X-axis aligned with segment
                local_coord = new Coord3d(c._point, l.Direction, c._normal.Cross(l.Direction));
                p1 = p1.ConvertTo(local_coord);
                p2 = p2.ConvertTo(local_coord);

                // use parametric form
                // x=t*x1+(1-t)x2
                // y=t*y1+(1-t)y2
                // and take into account that y1=y2, x0=y0=0
                double aa = (p1.X - p2.X) * (p1.X - p2.X);
                double bb = 2 * p2.X * (p1.X - p2.X);
                double cc = p2.X * p2.X + p2.Y * p2.Y - c.R * c.R;
                double discr = bb * bb - 4 * aa * cc;

                if (discr < 0)
                {
                    return false;
                }
                else
                {
                    discr = Sqrt(discr);
                    double t1 = (-bb + discr) / (2 * aa);
                    double t2 = (-bb - discr) / (2 * aa);
                    if ((t1 >= 0 && t1 <= 1) || (t2 >= 0 && t2 <= 1))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }



        /// <summary>
        /// Intersection check between circle and triangle
        /// </summary>
        public bool Intersects(Triangle t)
        {
            Plane3d t_plane = t.Plane;
            if (this.DistanceTo(t_plane) > 0) return false;

            if (this.IsCoplanarTo(t))
            {
                if (t.A.DistanceTo(this._point) <= this._r) return true;
                if (t.B.DistanceTo(this._point) <= this._r) return true;
                if (t.C.DistanceTo(this._point) <= this._r) return true;

                if (this._point.BelongsTo(t)) return true;
                if (this.IntersectionWith(new Segment3d(t.A, t.B)) != null) return true;
                if (this.IntersectionWith(new Segment3d(t.B, t.C)) != null) return true;
                if (this.IntersectionWith(new Segment3d(t.C, t.A)) != null) return true;
            }
            object obj = this.IntersectionWith(t_plane);
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
        /// Intersection check between circle and box
        /// </summary>
        public bool Intersects(Box3d box)
        {
            return box.Intersects(this);
        }

        /// <summary>
        /// Intersection check between circle and segment
        /// </summary>
        public bool Intersects(Segment3d s)
        {
            return this.IntersectionWith(s) != null;
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

            // Relative tolerance ================================
            if (!GeometRi3D.UseAbsoluteTolerance)
            {
                double tol = GeometRi3D.Tolerance;
                GeometRi3D.Tolerance = tol * this.R;
                GeometRi3D.UseAbsoluteTolerance = true;
                object result = this.IntersectionWith(l);
                GeometRi3D.UseAbsoluteTolerance = false;
                GeometRi3D.Tolerance = tol;
                return result;
            }
            //====================================================


            if (l.Direction.IsOrthogonalTo(this._normal))
            {
                if (l.Point.BelongsTo(new Plane3d(this._point, this._normal)))
                {
                    // coplanar objects
                    // Find intersection of line and circle (2D)

                    // Local coord: X - line direction, Z - circle normal
                    Coord3d local_coord = new Coord3d(this._point, l.Direction, this._normal.Cross(l.Direction));
                    Point3d p = l.Point.ConvertTo(local_coord);

                    double c = p.Y;

                    if (Abs(c) > this.R + GeometRi3D.Tolerance)
                    {
                        return null;
                    }
                    else if (Abs(c) < this.R)
                    {
                        double x1 = Sqrt(this.R * this.R - Abs(c) * Abs(c));
                        double x2 = -x1;
                        return new Segment3d(new Point3d(x1, c, 0, local_coord), new Point3d(x2, c, 0, local_coord));
                    }
                    else if (c > 0)
                    {
                        return new Point3d(0, this.R, 0, local_coord);
                    }
                    else
                    {
                        return new Point3d(0, -this.R, 0, local_coord);
                    }

                }
                else
                {
                    // parallel objects
                    return null;
                }
            }
            else
            {
                // Line intersects circle' plane
                Point3d p = (Point3d)l.IntersectionWith(new Plane3d(this._point, this._normal));
                if (p.DistanceTo(this._point) < this.R + GeometRi3D.Tolerance)
                {
                    return p;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Intersection of circle with segment.
        /// Returns 'null' (no intersection) or object of type 'Point3d' or 'Segment3d'.
        /// </summary>
        public object IntersectionWith(Segment3d s)
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
        /// Intersection of circle with ray.
        /// Returns 'null' (no intersection) or object of type 'Point3d' or 'Segment3d'.
        /// </summary>
        public object IntersectionWith(Ray3d r)
        {

            // Relative tolerance ================================
            if (!GeometRi3D.UseAbsoluteTolerance)
            {
                double tol = GeometRi3D.Tolerance;
                GeometRi3D.Tolerance = tol * this.R;
                GeometRi3D.UseAbsoluteTolerance = true;
                object result = this.IntersectionWith(r);
                GeometRi3D.UseAbsoluteTolerance = false;
                GeometRi3D.Tolerance = tol;
                return result;
            }
            //====================================================

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

            if (this._normal.IsParallelTo(s.Normal))
            {
                if (this._point.BelongsTo(s))
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

                Vector3d v1 = this._normal.Cross(s.Normal);
                Vector3d v2 = this._normal.Cross(v1);
                Line3d l = new Line3d(this._point, v2);
                Point3d intersection_point = (Point3d)l.IntersectionWith(s);

                double dist = intersection_point.DistanceTo(this.Center);

                if (Abs(R - dist) <= GeometRi3D.DefaultTolerance)
                {
                    // Point
                    return intersection_point;
                }
                else if (R - dist > 0)
                {
                    // Segment
                    double half_length = Sqrt(R * R - dist * dist);
                    v1 = v1.Normalized;
                    Point3d p1 = intersection_point.Translate(half_length * v1);
                    Point3d p2 = intersection_point.Translate(-half_length * v1);
                    return new Segment3d(p1, p2);
                }
                else
                {
                    return null;
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

            // Early exit (separated circles)
            double d = this._point.DistanceTo(c._point);
            if (d > this.R  + c.R  +  GeometRi3D.Tolerance)
                return null;

            if (this._normal.IsParallelTo(c._normal))
            {
                if (this._point.BelongsTo(new Plane3d(c._point, c._normal)))
                {
                    // Coplanar objects
                    // Search 2D intersection of two circles

                    // Equal circles
                    if (GeometRi3D.AlmostEqual(d, 0) && GeometRi3D.AlmostEqual(this.R, c.R))
                    {
                        return this.Copy();
                    }

                    

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
                        Vector3d vec = new Vector3d(this._point, c._point);
                        return this._point.Translate(this.R * vec.Normalized);
                    }

                    // Inner tangency
                    if (Abs(Abs(this.R - c.R) - d) < GeometRi3D.Tolerance)
                    {
                        Vector3d vec = new Vector3d(this._point, c._point);
                        if (this.R > c.R)
                        {
                            return this._point.Translate(this.R * vec.Normalized);
                        }
                        else
                        {
                            return this._point.Translate(-this.R * vec.Normalized);
                        }
                        
                    }

                    // intersecting circles
                    // Create local CS with origin in circle's center
                    Vector3d vec1 = new Vector3d(this._point, c._point);
                    Vector3d vec2 = vec1.Cross(this._normal);
                    Coord3d local_cs = new Coord3d(this._point, vec1, vec2);

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

                Vector3d v = new Vector3d(this._point, c._point);
                double this_norm = this._normal.Norm;
                double c_norm = c._normal.Norm;

                double cos_angle1 = this._normal * v / this_norm / d;
                double delta1 = Abs(d * cos_angle1);

                double sin_angle2 = this._normal.Cross(c._normal).Norm / this_norm / c_norm;
                double delta2 = Abs(c.R * sin_angle2);

                if (delta1 > delta2) return null;

                cos_angle1 = c._normal * v / c_norm / d;
                delta1 = Abs(d * cos_angle1);
                delta2 = Abs(this.R * sin_angle2);

                if (delta1 > delta2) return null;


                Plane3d plane_this = new Plane3d(this._point, this._normal);
                object obj = c.IntersectionWith(plane_this);

                if (obj == null)
                {
                    return null;
                }
                else if (obj.GetType() == typeof(Point3d))
                {
                    Point3d p = (Point3d)obj;
                    if (p.DistanceTo(this._point) < this.R + GeometRi3D.Tolerance)
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

        /// <summary>
        /// Scale circle relative to given point
        /// </summary>
        public virtual Circle3d Scale(double scale, Point3d scaling_center)
        {
            Point3d new_center = scaling_center + scale * (this.Center - scaling_center);
            return new Circle3d(new_center, this._r * scale, this._normal);
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
            return ToString(Coord3d.GlobalCS, false);
        }

        /// <summary>
        /// String representation of an object in global coordinate system.
        /// </summary>
        public String ToString(bool full_precision = false)
        {
            return ToString(Coord3d.GlobalCS, full_precision);
        }

        /// <summary>
        /// String representation of an object in reference coordinate system.
        /// </summary>
        public String ToString(Coord3d coord, bool full_precision = false)
        {
            string nl = System.Environment.NewLine;

            if (coord == null) { coord = Coord3d.GlobalCS; }
            Point3d P = _point.ConvertTo(coord);
            Vector3d normal = _normal.ConvertTo(coord);

            string str = string.Format("Circle: ") + nl;
            if (full_precision)
            {
                str += string.Format("  Center -> ({0}, {1}, {2})", P.X, P.Y, P.Z) + nl;
                str += string.Format("  Radius -> {0}", _r) + nl;
                str += string.Format("  Normal -> ({0}, {1}, {2})", normal.X, normal.Y, normal.Z);
            }
            else
            {
                str += string.Format("  Center -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", P.X, P.Y, P.Z) + nl;
                str += string.Format("  Radius -> {0,10:g5}", _r) + nl;
                str += string.Format("  Normal -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", normal.X, normal.Y, normal.Z);
            }

            return str;
        }

        public String ToString(Coord3d coord, bool full_precision = false, bool code = true)
        {
            string nl = System.Environment.NewLine;

            if (coord == null) { coord = Coord3d.GlobalCS; }
            Point3d P = _point.ConvertTo(coord);
            Vector3d normal = _normal.ConvertTo(coord);

            string str = string.Format("Circle3d circle = new Circle3d(new Point3d({0}, {1}, {2}), {3}, new Vector3d({4}, {5}, {6}));",
                   P.X, P.Y, P.Z, this.R, normal.X, normal.Y, normal.Z) + nl;

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


