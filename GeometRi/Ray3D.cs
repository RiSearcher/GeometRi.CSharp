using System;
using static System.Math;

namespace GeometRi
{
    /// <summary>
    /// Ray in 3D space defined by point and direction vector.
    /// </summary>
    public class Ray3d : ILinearObject
    {

        private Point3d _point;
        private Vector3d _dir;

        /// <summary>
        /// Default constructor, initializes ray starting from origin and aligned with X-axis (in global coordinate system).
        /// </summary>
        public Ray3d()
        {
            _point = new Point3d();
            _dir = new Vector3d(1, 0, 0);
        }

        /// <summary>
        /// Initializes ray using starting point and direction.
        /// </summary>
        public Ray3d(Point3d p, Vector3d v)
        {
            _point = p.Copy();
            _dir = v.ConvertTo(p.Coord).Normalized;
        }

        /// <summary>
        /// Creates copy of the object
        /// </summary>
        public Ray3d Copy()
        {
            return new Ray3d(_point,_dir);
        }

        /// <summary>
        /// Base point of the ray
        /// </summary>
        /// <returns></returns>
        public Point3d Point
        {
            get { return _point.Copy(); }
            set { _point = value.Copy(); }
        }

        /// <summary>
        /// Direction vector of the ray
        /// </summary>
        /// <returns></returns>
        public Vector3d Direction
        {
            get { return _dir.Copy(); }
            set { _dir = value.Copy(); }
        }

        public bool IsOriented
        {
            get { return false; }
        }

        #region "ParallelMethods"
        /// <summary>
        /// Check if two objects are parallel
        /// </summary>
        public bool IsParallelTo(ILinearObject obj)
        {
            return this.Direction.IsParallelTo(obj.Direction);
        }

        /// <summary>
        /// Check if two objects are NOT parallel
        /// </summary>
        public bool IsNotParallelTo(ILinearObject obj)
        {
            return this.Direction.IsNotParallelTo(obj.Direction);
        }

        /// <summary>
        /// Check if two objects are orthogonal
        /// </summary>
        public bool IsOrthogonalTo(ILinearObject obj)
        {
            return this.Direction.IsOrthogonalTo(obj.Direction);
        }

        /// <summary>
        /// Check if two objects are parallel
        /// </summary>
        public bool IsParallelTo(IPlanarObject obj)
        {
            return this.Direction.IsOrthogonalTo(obj.Normal);
        }

        /// <summary>
        /// Check if two objects are NOT parallel
        /// </summary>
        public bool IsNotParallelTo(IPlanarObject obj)
        {
            return !this.Direction.IsOrthogonalTo(obj.Normal);
        }

        /// <summary>
        /// Check if two objects are orthogonal
        /// </summary>
        public bool IsOrthogonalTo(IPlanarObject obj)
        {
            return this.Direction.IsParallelTo(obj.Normal);
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

        /// <summary>
        /// Convert ray to line
        /// </summary>
        public Line3d ToLine
        {
            get { return new Line3d(this.Point, this.Direction); }
        }

        #region "DistanceTo"
        /// <summary>
        /// Distance from ray to point
        /// </summary>
        public double DistanceTo(Point3d p)
        {
            return p.DistanceTo(this);
        }

        /// <summary>
        /// Shortest distance to a line
        /// </summary>
        public double DistanceTo(Line3d l)
        {
            Vector3d r1 = this.Point.ToVector;
            Vector3d r2 = l.Point.ToVector;
            Vector3d s1 = this.Direction;
            Vector3d s2 = l.Direction;
            if (s1.Cross(s2).Norm > GeometRi3D.Tolerance)
            {
                // Crossing lines
                Point3d p = l.PerpendicularTo(new Line3d(this.Point, this.Direction));
                if (p.BelongsTo(this))
                {
                    return Abs((r2 - r1) * s1.Cross(s2)) / s1.Cross(s2).Norm;
                }
                else
                {
                    return this.Point.DistanceTo(l);
                }
            }
            else
            {
                // Parallel lines
                return (r2 - r1).Cross(s1).Norm / s1.Norm;
            }
        }

        /// <summary>
        /// Distance to a segment
        /// </summary>
        public double DistanceTo(Segment3d s)
        {
            return s.DistanceTo(this);
        }

        /// <summary>
        /// Distance between two rays
        /// </summary>
        public double DistanceTo(Ray3d r)
        {

            if (this.Direction.IsParallelTo(r.Direction))
                return this.ToLine.DistanceTo(r.ToLine);

            if (this.ToLine.PerpendicularTo(r.ToLine).BelongsTo(r) && r.ToLine.PerpendicularTo(this.ToLine).BelongsTo(this))
            {
                return this.ToLine.DistanceTo(r.ToLine);
            }

            double d1 = double.PositiveInfinity;
            double d2 = double.PositiveInfinity;
            bool flag = false;

            if (r.Point.ProjectionTo(this.ToLine).BelongsTo(this))
            {
                d1 = r.Point.DistanceTo(this.ToLine);
                flag = true;
            }
            if (this.Point.ProjectionTo(r.ToLine).BelongsTo(r))
            {
                d2 = this.Point.DistanceTo(r.ToLine);
                flag = true;
            }

            if (flag)
            {
                return Min(d1, d2);
            }
            else
            {
                return this.Point.DistanceTo(r.Point);
            }

        }

        /// <summary>
        /// Shortest distance between ray and circle (including interior points)
        /// </summary>
        public double DistanceTo(Circle3d c)
        {
            return c.DistanceTo(this);
        }

        /// <summary>
        /// Shortest distance between ray and circle (including interior points)
        /// </summary>
        /// <param name="c">Target circle</param>
        /// <param name="point_on_ray">Closest point on ray</param>
        /// <param name="point_on_circle">Closest point on circle</param>
        public double DistanceTo(Circle3d c, out Point3d point_on_ray, out Point3d point_on_circle)
        {
            return c.DistanceTo(this, out point_on_circle, out point_on_ray);
        }
        #endregion


        /// <summary>
        /// Point on the perpendicular to the line
        /// </summary>
        public Point3d PerpendicularTo(Line3d l)
        {
            Vector3d r1 = this.Point.ToVector;
            Vector3d r2 = l.Point.ToVector;
            Vector3d s1 = this.Direction;
            Vector3d s2 = l.Direction;
            if (s1.Cross(s2).Norm > GeometRi3D.Tolerance)
            {
                Point3d p = l.PerpendicularTo(new Line3d(this.Point, this.Direction));
                if (p.BelongsTo(this))
                {
                    r1 = r2 + (r2 - r1) * s1.Cross(s1.Cross(s2)) / (s1 * s2.Cross(s1.Cross(s2))) * s2;
                    return r1.ToPoint;
                }
                else
                {
                    return this.Point.ProjectionTo(l);
                }
            }
            else
            {
                throw new Exception("Lines are parallel");
            }
        }

        /// <summary>
        /// Get intersection of ray with plane.
        /// Returns 'null' (no intersection) or object of type 'Point3d' or 'Ray3d'.
        /// </summary>
        public object IntersectionWith(Plane3d s)
        {

            Vector3d r1 = this.Point.ToVector;
            Vector3d s1 = this.Direction;
            Vector3d n2 = s.Normal;
            if (Abs(s1 * n2) < GeometRi3D.Tolerance)
            {
                // Ray and plane are parallel
                if (this.Point.BelongsTo(s))
                {
                    // Ray lies in the plane
                    return this;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                // Intersection point
                s.SetCoord(r1.Coord);
                r1 = r1 - ((r1 * n2) + s.D) / (s1 * n2) * s1;
                if (r1.ToPoint.BelongsTo(this))
                {
                    return r1.ToPoint;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Get intersection of ray with line.
        /// Returns 'null' (no intersection) or object of type 'Point3d' or 'Ray3d'.
        /// </summary>
        public object IntersectionWith(Line3d l)
        {

            if (this.Point.BelongsTo(l) && this.IsParallelTo(l))
            {
                return this;
            }

            object obj = this.ToLine.IntersectionWith(l);
            if (obj == null)
            {
                return null;
            }
            else
            {
                Point3d p = (Point3d)obj;
                if (p.BelongsTo(this)) {
                    return p;
                }
                else
                {
                    return null;
                }
            }

        }

        /// <summary>
        /// Get intersection of ray with other ray.
        /// Returns 'null' (no intersection) or object of type 'Point3d', 'Segment3d' or 'Ray3d'.
        /// </summary>
        public object IntersectionWith(Ray3d r)
        {

            if (this == r)
            {
                return this;
            }

            if (this.Point.BelongsTo(r) && this.IsParallelTo(r))
            {
                if (r.Point.BelongsTo(this)) {
                    // Two rays are collinear and opposite
                    if (this.Point == r. Point)
                    {
                        return this.Point;
                    }
                    else
                    {
                        return new Segment3d(this.Point, r.Point);
                    }
                }
                else
                {
                    return this;
                }
            }

            if (r.Point.BelongsTo(this) && r.IsParallelTo(this))
            {
                return r;
            }

            object obj = this.ToLine.IntersectionWith(r.ToLine);
            if (obj == null)
            {
                return null;
            }
            else
            {
                Point3d p = (Point3d)obj;
                if (p.BelongsTo(this) && p.BelongsTo(r))
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
        /// Get intersection of ray with segment.
        /// Returns 'null' (no intersection) or object of type 'Point3d' or 'Segment3d'.
        /// </summary>
        public object IntersectionWith(Segment3d s)
        {

            // Relative tolerance ================================
            if (!GeometRi3D.UseAbsoluteTolerance)
            {
                double tol = GeometRi3D.Tolerance;
                GeometRi3D.Tolerance = tol * s.Length;
                GeometRi3D.UseAbsoluteTolerance = true;
                object result = this.IntersectionWith(s);
                GeometRi3D.UseAbsoluteTolerance = false;
                GeometRi3D.Tolerance = tol;
                return result;
            }
            //====================================================

            object obj = this.ToLine.IntersectionWith(s);
            if (obj == null)
            {
                return null;
            }
            else if (obj.GetType() == typeof(Point3d))
            {
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
            else
            {
                Segment3d intersection = (Segment3d)obj;
                if (intersection.P1.BelongsTo(this) && intersection.P2.BelongsTo(this))
                {
                    return intersection;
                }
                else if (intersection.P1.BelongsTo(this))
                {
                    if (intersection.P1 == this.Point)
                    {
                        return this.Point;
                    }
                    else
                    {
                        return new Segment3d(this.Point, intersection.P1);
                    }
                }
                else if (intersection.P2.BelongsTo(this))
                {
                    if (intersection.P2 == this.Point)
                    {
                        return this.Point;
                    }
                    else
                    {
                        return new Segment3d(this.Point, intersection.P2);
                    }
                }
                else
                {
                    return null;
                }
            }

        }

        /// <summary>
        /// Get intersection of ray with sphere.
        /// Returns 'null' (no intersection) or object of type 'Point3d' or 'Segment3d'.
        /// </summary>
        public object IntersectionWith(Sphere s)
        {
            return s.IntersectionWith(this);
        }

        /// <summary>
        /// Get intersection of ray with ellipse.
        /// Returns 'null' (no intersection) or object of type 'Point3d' or 'Segment3d'.
        /// </summary>
        public object IntersectionWith(Ellipse e)
        {
            return e.IntersectionWith(this);
        }

        /// <summary>
        /// Get intersection of ray with ellipsoid.
        /// Returns 'null' (no intersection) or object of type 'Point3d' or 'Segment3d'.
        /// </summary>
        public object IntersectionWith(Ellipsoid e)
        {
            return e.IntersectionWith(this);
        }

        /// <summary>
        /// Get intersection of ray with circle.
        /// Returns 'null' (no intersection) or object of type 'Point3d' or 'Segment3d'.
        /// </summary>
        public object IntersectionWith(Circle3d c)
        {
            return c.IntersectionWith(this);
        }

        /// <summary>
        /// Get intersection of ray with triangle.
        /// Returns 'null' (no intersection) or object of type 'Point3d' or 'Segment3d'.
        /// </summary>
        public object IntersectionWith(Triangle t)
        {
            return t.IntersectionWith(this);
        }

        /// <summary>
        /// Get intersection of ray with box.
        /// Returns 'null' (no intersection) or object of type 'Point3d' or 'Segment3d'.
        /// </summary>
        public object IntersectionWith(Box3d b)
        {
            return b.IntersectionWith(this);
        }

        /// <summary>
        /// Get the orthogonal projection of a ray to the plane.
        /// Return object of type 'Ray3d' or 'Point3d'
        /// </summary>
        public object ProjectionTo(Plane3d s)
        {
            Vector3d n1 = s.Normal;
            Vector3d n2 = this.Direction.Cross(n1);
            if (n2.Norm < GeometRi3D.Tolerance)
            {
                // Ray is perpendicular to the plane
                return this.Point.ProjectionTo(s);
            }
            else
            {
                return new Ray3d(this.Point.ProjectionTo(s), n1.Cross(n2));
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
        /// Translate ray by a vector
        /// </summary>
        public Ray3d Translate(Vector3d v)
        {
            Ray3d l = this.Copy();
            l.Point = l.Point.Translate(v);
            return l;
        }

        /// <summary>
        /// Rotate ray by a given rotation matrix
        /// </summary>
        [System.Obsolete("use Rotation object and specify rotation center: this.Rotate(Rotation r, Point3d p)")]
        public Ray3d Rotate(Matrix3d m)
        {
            return new Ray3d(this.Point.Rotate(m), this.Direction.Rotate(m));
        }

        /// <summary>
        /// Rotate ray by a given rotation matrix around point 'p' as a rotation center
        /// </summary>
        [System.Obsolete("use Rotation object: this.Rotate(Rotation r, Point3d p)")]
        public Ray3d Rotate(Matrix3d m, Point3d p)
        {
            return new Ray3d(this.Point.Rotate(m, p), this.Direction.Rotate(m));
        }

        /// <summary>
        /// Rotate ray around point 'p' as a rotation center
        /// </summary>
        public Ray3d Rotate(Rotation r, Point3d p)
        {
            return new Ray3d(this.Point.Rotate(r, p), this.Direction.Rotate(r));
        }

        /// <summary>
        /// Reflect ray in given point
        /// </summary>
        public Ray3d ReflectIn(Point3d p)
        {
            return new Ray3d(this.Point.ReflectIn(p), this.Direction.ReflectIn(p));
        }

        /// <summary>
        /// Reflect ray in given line
        /// </summary>
        public Ray3d ReflectIn(Line3d l)
        {
            return new Ray3d(this.Point.ReflectIn(l), this.Direction.ReflectIn(l));
        }

        /// <summary>
        /// Reflect ray in given plane
        /// </summary>
        public Ray3d ReflectIn(Plane3d s)
        {
            return new Ray3d(this.Point.ReflectIn(s), this.Direction.ReflectIn(s));
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
            Ray3d r = (Ray3d)obj;

            if (GeometRi3D.UseAbsoluteTolerance)
            {
                return this.Point == r.Point && Abs(this.Direction.Normalized * r.Direction.Normalized - 1) < GeometRi3D.Tolerance;
            }
            else
            {
                double tol = GeometRi3D.Tolerance;
                GeometRi3D.Tolerance = tol * this.Point.DistanceTo(this.Point.Coord.Origin);
                GeometRi3D.UseAbsoluteTolerance = true;
                bool res1 = this.Point == r.Point;
                GeometRi3D.UseAbsoluteTolerance = false;
                GeometRi3D.Tolerance = tol;
                bool res2 = Abs(this.Direction.Normalized * r.Direction.Normalized - 1) < GeometRi3D.Tolerance;
                return res1 && res2;
            }
        }

        /// <summary>
        /// Returns the hashcode for the object.
        /// </summary>
        public override int GetHashCode()
        {
            return GeometRi3D.HashFunction(_point.GetHashCode(), _dir.GetHashCode());
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
            Point3d P = _point.ConvertTo(coord);
            Vector3d dir = _dir.ConvertTo(coord);

            str.Append("Ray:" + nl);
            str.Append(string.Format("Point  -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", P.X, P.Y, P.Z) + nl);
            str.Append(string.Format("Direction -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", dir.X, dir.Y, dir.Z));
            return str.ToString();
        }

        // Operators overloads
        //-----------------------------------------------------------------
        public static bool operator ==(Ray3d l1, Ray3d l2)
        {
            if (object.ReferenceEquals(l1, null))
                return object.ReferenceEquals(l2, null);
            return l1.Equals(l2);
        }
        public static bool operator !=(Ray3d l1, Ray3d l2)
        {
            if (object.ReferenceEquals(l1, null))
                return !object.ReferenceEquals(l2, null);
            return !l1.Equals(l2);
        }


    }
}


