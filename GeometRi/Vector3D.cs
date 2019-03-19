using System;
using static System.Math;

namespace GeometRi
{
    /// <summary>
    /// Vector in 3D space defined in global or local reference frame.
    /// </summary>
    public class Vector3d : ILinearObject
    {

        private double[] val;
        private Coord3d _coord;

        #region "Constructors"
        /// <summary>
        /// Default constructor, initializes zero vector.
        /// </summary>
        /// <param name="coord">Reference coordinate system (default - Coord3d.GlobalCS).</param>
        public Vector3d(Coord3d coord = null)
        {
            this.val = new double[3];
            this.val[0] = 0.0;
            this.val[1] = 0.0;
            this.val[2] = 0.0;
            _coord = (coord == null) ? Coord3d.GlobalCS : coord;
        }

        /// <summary>
        /// Initializes vector object using components in reference coordinate system.
        /// </summary>
        /// <param name="coord">Reference coordinate system (default - Coord3d.GlobalCS).</param>
        public Vector3d(double X, double Y, double Z, Coord3d coord = null)
        {
            this.val = new double[3];
            this.val[0] = X;
            this.val[1] = Y;
            this.val[2] = Z;
            _coord = (coord == null) ? Coord3d.GlobalCS : coord;
        }

        /// <summary>
        /// Initializes vector object as radius vector of a point in reference coordinate system.
        /// </summary>
        public Vector3d(Point3d p, Coord3d coord = null)
        {
            p = p.ConvertTo(coord);
            this.val = new double[3];
            this.val[0] = p.X;
            this.val[1] = p.Y;
            this.val[2] = p.Z;
            _coord = (coord == null) ? Coord3d.GlobalCS : coord;
        }

        /// <summary>
        /// Initializes vector object using two points in reference coordinate system of the first point.
        /// </summary>
        /// <param name="p1">Start point.</param>
        /// <param name="p2">End point.</param>
        public Vector3d(Point3d p1, Point3d p2)
        {
            if (p1.Coord != p2.Coord)
                p2 = p2.ConvertTo(p1.Coord);
            this.val = new double[3];
            this.val[0] = p2.X - p1.X;
            this.val[1] = p2.Y - p1.Y;
            this.val[2] = p2.Z - p1.Z;
            _coord = p1.Coord;
        }

        /// <summary>
        /// Initializes vector using double array.
        /// </summary>
        /// <param name="coord">Reference coordinate system (default - Coord3d.GlobalCS).</param>
        public Vector3d(double[] a, Coord3d coord = null)
        {
            if (a.GetUpperBound(0) < 2)
                throw new Exception("Vector3d: Array size mismatch");
            this.val = new double[3];
            this.val[0] = a[0];
            this.val[1] = a[1];
            this.val[2] = a[2];
            _coord = (coord == null) ? Coord3d.GlobalCS : coord;
        }
        #endregion

        /// <summary>
        /// Creates copy of the object
        /// </summary>
        public Vector3d Copy()
        {
            return new Vector3d(val, _coord);
        }

        public double this[int i]
        {
            get { return val[i]; }
            set { val[i] = value; }
        }

        /// <summary>
        /// X component in reference coordinate system
        /// </summary>
        public double X
        {
            get { return val[0]; }
            set { val[0] = value; }
        }

        /// <summary>
        /// Y component in reference coordinate system
        /// </summary>
        public double Y
        {
            get { return val[1]; }
            set { val[1] = value; }
        }

        /// <summary>
        /// Z component in reference coordinate system
        /// </summary>
        public double Z
        {
            get { return val[2]; }
            set { val[2] = value; }
        }

        /// <summary>
        /// Norm of a vector
        /// </summary>
        public double Norm
        {
            get { return Sqrt(Math.Pow(val[0], 2) + Math.Pow(val[1], 2) + Math.Pow(val[2], 2)); }
        }

        /// <summary>
        ///  Reference coordinate system
        /// </summary>
        public Coord3d Coord
        {
            get { return _coord; }
        }

        // ILinearObject interface implementation
        public Vector3d Direction
        {
            get { return this.Copy();  }
        }

        public bool IsOriented
        {
            get {  return true;  }
        }

        /// <summary>
        /// Returns line passing through origin in the direction of vector
        /// </summary>
        public Line3d ToLine
        {
            get { return new Line3d(new Point3d(), this.Direction); }
        }
        //////////////////////////////////////////

        #region "ParallelMethods"
        /// <summary>
        /// Check if two objects are parallel
        /// </summary>
        public bool IsParallelTo(ILinearObject obj)
        {
            Vector3d v = obj.Direction;
            if ((this._coord != v._coord))
                v = v.ConvertTo(this._coord);

            return GeometRi3D.AlmostEqual(this.Normalized.Cross(v.Normalized).Norm, 0.0);        
        }

        /// <summary>
        /// Check if two objects are NOT parallel
        /// </summary>
        public bool IsNotParallelTo(ILinearObject obj)
        {
            return ! this.IsParallelTo(obj);
        }

        /// <summary>
        /// Check if two objects are orthogonal
        /// </summary>
        public bool IsOrthogonalTo(ILinearObject obj)
        {
            Vector3d v = obj.Direction;
            if ((this._coord != v._coord))
                v = v.ConvertTo(this._coord);

            double this_norm = this.Norm;
            double v_norm = v.Norm;
            return GeometRi3D.AlmostEqual(Abs(this * v) / (this_norm * v_norm), 0.0);

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
            return ! this.Direction.IsOrthogonalTo(obj.Normal);
        }

        /// <summary>
        /// Check if two objects are orthogonal
        /// </summary>
        public bool IsOrthogonalTo(IPlanarObject obj)
        {
            return this.Direction.IsParallelTo(obj.Normal);
        }
        #endregion


        /// <summary>
        /// Point, represented by vector starting in origin
        /// </summary>
        public Point3d ToPoint
        {
            get { return new Point3d(val[0], val[1], val[2], _coord); }
        }

        /// <summary>
        /// Return normalized vector
        /// </summary>
        public Vector3d Normalized
        {
            get
            {
                Vector3d tmp = this.Copy();
                double tmp_norm = this.Norm;
                tmp[0] = val[0] / tmp_norm;
                tmp[1] = val[1] / tmp_norm;
                tmp[2] = val[2] / tmp_norm;
                return tmp;
            }
        }

        /// <summary>
        /// Normalize the current vector
        /// </summary>
        public void Normalize()
        {
            double tmp = 1.0 / this.Norm;
            val[0] = val[0] * tmp;
            val[1] = val[1] * tmp;
            val[2] = val[2] * tmp;
        }

        public Vector3d Add(double a)
        {
            Vector3d tmp = this.Copy();
            tmp[0] += a;
            tmp[1] += a;
            tmp[2] += a;
            return tmp;
        }
        public Vector3d Add(Vector3d v)
        {
            if ((this._coord != v._coord))
                v = v.ConvertTo(this._coord);
            Vector3d tmp = this.Copy();
            tmp[0] += v.X;
            tmp[1] += v.Y;
            tmp[2] += v.Z;
            return tmp;
        }
        public Vector3d Subtract(double a)
        {
            Vector3d tmp = this.Copy();
            tmp[0] -= a;
            tmp[1] -= a;
            tmp[2] -= a;
            return tmp;
        }
        public Vector3d Subtract(Vector3d v)
        {
            if ((this._coord != v._coord))
                v = v.ConvertTo(this._coord);
            Vector3d tmp = this.Copy();
            tmp[0] -= v.X;
            tmp[1] -= v.Y;
            tmp[2] -= v.Z;
            return tmp;
        }
        public Vector3d Mult(double a)
        {
            Vector3d tmp = this.Copy();
            tmp[0] *= a;
            tmp[1] *= a;
            tmp[2] *= a;
            return tmp;
        }

        /// <summary>
        /// Dot product of two vectors
        /// </summary>
        public double Dot(Vector3d v)
        {
            if ((this._coord != v._coord))
                v = v.ConvertTo(this._coord);
            return this.val[0] * v.val[0] + this.val[1] * v.val[1] + this.val[2] * v.val[2];
        }

        /// <summary>
        /// Cross product of two vectors
        /// </summary>
        public Vector3d Cross(Vector3d v)
        {
            if ((this._coord != v._coord))
                v = v.ConvertTo(this._coord);
            double x = this.Y * v.Z - this.Z * v.Y;
            double y = this.Z * v.X - this.X * v.Z;
            double z = this.X * v.Y - this.Y * v.X;
            return new Vector3d(x, y, z, _coord); ;
        }

        /// <summary>
        /// Convert vector to reference coordinate system.
        /// </summary>
        public Vector3d ConvertTo(Coord3d coord)
        {
            Vector3d v1 = this.Copy();
            v1 = v1.ConvertToGlobal();
            if (coord != null && !object.ReferenceEquals(coord, Coord3d.GlobalCS))
            {
                v1 = coord.Axes * v1;
                v1._coord = coord;
            }
            return v1;
        }

        /// <summary>
        /// Convert vector to global coordinate system
        /// </summary>
        public Vector3d ConvertToGlobal()
        {
            if (_coord == null || object.ReferenceEquals(_coord, Coord3d.GlobalCS))
            {
                return this.Copy();
            }
            else
            {
                Vector3d v = this.Copy();
                v = _coord.Axes.Transpose() * v;
                v._coord = Coord3d.GlobalCS;
                return v;
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

        /// <summary>
        /// Return projection of the current vector to the second vector
        /// </summary>
        public Vector3d ProjectionTo(Vector3d v)
        {
            if ((this._coord != v._coord))
                v = v.ConvertTo(this._coord);
            return (this * v) / (v * v) * v;
        }

        /// <summary>
        /// Return arbitrary vector, orthogonal to the current vector
        /// </summary>
        public Vector3d OrthogonalVector
        {
            get
            {
                if (Abs(this.X) <= Abs(this.Y) && Abs(this.X) <= Abs(this.Z))
                {
                    return new Vector3d(0, this.Z, -this.Y, this.Coord);
                }
                else if (Abs(this.Y) <= Abs(this.X) && Abs(this.Y) <= Abs(this.Z))
                {
                    return new Vector3d(this.Z, 0, -this.X, this.Coord);
                }
                else
                {
                    return new Vector3d(this.Y, -this.X, 0, this.Coord);
                }
            }
        }

        #region "RotateReflect"
        /// <summary>
        /// Rotate vector by a given rotation matrix
        /// </summary>
        [System.Obsolete("use Rotation object: this.Rotate(Rotation r)")]
        public Vector3d Rotate(Matrix3d m)
        {
            return m * this;
        }

        /// <summary>
        /// Rotate vector
        /// </summary>
        public Vector3d Rotate(Rotation r)
        {
            if (this._coord != r.Coord) r = r.ConvertTo(this._coord);
            return r.ToRotationMatrix * this;
        }

        /// <summary>
        /// Reflect vector in given point
        /// </summary>
        public Vector3d ReflectIn(Point3d p)
        {
            return -this;
        }

        /// <summary>
        /// Reflect vector in given line
        /// </summary>
        public Vector3d ReflectIn(Line3d l)
        {
            Point3d p1 = new Point3d(0, 0, 0, this._coord);
            Point3d p2 = p1.Translate(this);
            return new Vector3d(p1.ReflectIn(l), p2.ReflectIn(l));
        }

        /// <summary>
        /// Reflect vector in given plane
        /// </summary>
        public Vector3d ReflectIn(Plane3d s)
        {
            Point3d p1 = new Point3d(0, 0, 0, this._coord);
            Point3d p2 = p1.Translate(this);
            return new Vector3d(p1.ReflectIn(s), p2.ReflectIn(s));
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
            Vector3d v = (Vector3d)obj;
            if ((this._coord != v.Coord))
                v = v.ConvertTo(_coord);

            if (GeometRi3D.UseAbsoluteTolerance)
            {
                return Abs(this.X - v.X) + Abs(this.Y - v.Y) + Abs(this.Z - v.Z) < GeometRi3D.Tolerance;
            }
            else
            {
                return (Abs(this.X - v.X) + Abs(this.Y - v.Y) + Abs(this.Z - v.Z)) / this.Norm < GeometRi3D.Tolerance;
            }

        }

        /// <summary>
        /// Returns the hashcode for the object.
        /// </summary>
        public override int GetHashCode()
        {
            return GeometRi3D.HashFunction(val[0].GetHashCode(), val[1].GetHashCode(), val[2].GetHashCode(), _coord.GetHashCode());
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
            Vector3d v = this.ConvertTo(coord);
            return string.Format("Vector3d -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", v.X, v.Y, v.Z);
        }

        // Operators overloads
        //-----------------------------------------------------------------
        // "+"
        public static Vector3d operator +(Vector3d v, Vector3d a)
        {
            return v.Add(a);
        }
        // "-"
        public static Vector3d operator -(Vector3d v)
        {
            return v.Mult(-1.0);
        }
        public static Vector3d operator -(Vector3d v, Vector3d a)
        {
            return v.Subtract(a);
        }
        // "*"
        public static Vector3d operator *(Vector3d v, double a)
        {
            return v.Mult(a);
        }
        public static Vector3d operator /(Vector3d v, double a)
        {
            return v.Mult(1.0/a);
        }
        public static Vector3d operator *(double a, Vector3d v)
        {
            return v.Mult(a);
        }
        /// <summary>
        /// Dot product of two vectors
        /// </summary>
        public static double operator *(Vector3d v, Vector3d a)
        {
            return v.Dot(a);
        }

        public static bool operator ==(Vector3d v1, Vector3d v2)
        {
            if (object.ReferenceEquals(v1, null))
                return object.ReferenceEquals(v2, null);
            return v1.Equals(v2);
        }
        public static bool operator !=(Vector3d v1, Vector3d v2)
        {
            if (object.ReferenceEquals(v1, null))
                return !object.ReferenceEquals(v2, null);
            return !v1.Equals(v2);
        }

    }
}


