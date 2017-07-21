using System;
using static System.Math;

namespace GeometRi
{
    public class Ray3d
    {

        private Point3d _point;
        private Vector3d _dir;

        public Ray3d()
        {
            _point = new Point3d();
            _dir = new Vector3d(1, 0, 0);
        }
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
        /// Returns object of type 'Nothing', 'Point3d' or 'Ray3d'.
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

        /// <summary>
        /// Angle between ray and plane in radians (0 &lt; angle &lt; Pi/2)
        /// </summary>
        public double AngleTo(Plane3d s)
        {
            double ang = Asin(this.Direction.Dot(s.Normal) / this.Direction.Norm / s.Normal.Norm);
            return Abs(ang);
        }
        /// <summary>
        /// Angle between ray and plane in degrees (0 &lt; angle &lt; 90)
        /// </summary>
        public double AngleToDeg(Plane3d s)
        {
            return AngleTo(s) * 180 / PI;
        }

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
        public Ray3d Rotate(Matrix3d m)
        {
            Ray3d l = this.Copy();
            l.Point = l.Point.Rotate(m);
            l.Direction = l.Direction.Rotate(m);
            return l;
        }

        /// <summary>
        /// Rotate ray by a given rotation matrix around point 'p' as a rotation center
        /// </summary>
        public Ray3d Rotate(Matrix3d m, Point3d p)
        {
            Ray3d l = this.Copy();
            l.Point = l.Point.Rotate(m, p);
            l.Direction = l.Direction.Rotate(m);
            return l;
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
            return this.Point == r.Point && Abs(this.Direction.Normalized * r.Direction.Normalized - 1) < GeometRi3D.Tolerance;
        }

        /// <summary>
        /// Returns the hascode for the object.
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
            return l1.Equals(l2);
        }
        public static bool operator !=(Ray3d l1, Ray3d l2)
        {
            return !l1.Equals(l2);
        }


    }
}


