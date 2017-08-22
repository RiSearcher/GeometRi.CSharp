using System;
using static System.Math;

namespace GeometRi
{
    /// <summary>
    /// Unit quaternion (W + X*i + Y*j + Z*k).
    /// </summary>
    public class Quaternion
    {

        private double _w, _x, _y, _z;
        private Coord3d _coord;

        #region "Constructors"
        public Quaternion(Coord3d coord = null)
        {
            _w = 0; _x = 0; _y = 0; _z = 0;
            _coord = (coord == null) ? Coord3d.GlobalCS : coord;
        }

        public Quaternion(double w, double x, double y, double z, Coord3d coord = null)
        {
            _w = w; _x = x; _y = y; _z = z;
            _coord = (coord == null) ? Coord3d.GlobalCS : coord;
        }

        public Quaternion(double[] q, Coord3d coord = null)
        {
            if (q.GetUpperBound(0) < 3)
                throw new Exception("Quaternion: Array size mismatch");
            _w = q[0];
            _x = q[1];
            _y = q[2];
            _z = q[3];
            _coord = (coord == null) ? Coord3d.GlobalCS : coord;
        }

        public Quaternion(Vector3d axis, double angle)
        {
            Vector3d v = axis.Normalized;
            _w = Cos(angle / 2);
            _x = v.X * Sin(angle / 2);
            _y = v.Y * Sin(angle / 2);
            _z = v.Z * Sin(angle / 2);

            _coord = axis.Coord;
        }

        public Quaternion(Matrix3d m, Coord3d coord)
        {
            if (!m.IsOrthogonal)
            {
                throw new Exception("Matrix is not orthogonal");
            }
            double tr = m.Trace;
            if (tr > 0)
            {
                _w = 0.5 * Sqrt(tr + 1);
                _x = (m[1, 2] - m[2, 1]) / (4 * _w);
                _y = (m[2, 0] - m[0, 2]) / (4 * _w);
                _y = (m[0, 1] - m[1, 0]) / (4 * _w);
            } else if (m[0, 0] >= m[1, 1] && m[0, 0] >= m[2, 2])
            {
                _x = Sqrt(m[0, 0] - m[1, 1] - m[2, 2] + 1) / 2;
                _w = (m[1, 2] - m[2, 1]) / (4 * _x);
                _y = (m[0, 1] + m[1, 0]) / (4 * _x);
                _z = (m[0, 2] + m[2, 0]) / (4 * _x);
            } else if (m[1, 1] >= m[0, 0] && m[1, 1] >= m[2, 2])
            {
                _y = Sqrt(m[1, 1] - m[0, 0] - m[2, 2] + 1) / 2;
                _w = (m[2, 0] - m[0, 2]) / (4 * _y);
                _x = (m[0, 1] + m[1, 0]) / (4 * _y);
                _z = (m[1, 2] + m[2, 1]) / (4 * _y);
            } else
            {
                _z = Sqrt(m[2, 2] - m[0, 0] - m[1, 1] + 1) / 2;
                _w = (m[0, 1] - m[1, 0]) / (4 * _z);
                _x = (m[0, 2] + m[2, 0]) / (4 * _z);
                _y = (m[1, 2] + m[2, 1]) / (4 * _z);
            }

            _coord = coord;
        }
        #endregion


        /// <summary>
        /// Creates copy of the object
        /// </summary>
        public Quaternion Copy()
        {
            return new Quaternion(this.W, this.X, this.Y, this.Z, _coord);
        }

        #region "Properties"
        /// <summary>
        /// X component in reference coordinate system
        /// </summary>
        public double X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Y component in reference coordinate system
        /// </summary>
        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// Z component in reference coordinate system
        /// </summary>
        public double Z
        {
            get { return _z; }
            set { _z = value; }
        }

        /// <summary>
        /// W component in reference coordinate system
        /// </summary>
        public double W
        {
            get { return _w; }
            set { _w = value; }
        }

        /// <summary>
        ///  Reference coordinate system
        /// </summary>
        public Coord3d Coord
        {
            get { return _coord; }
        }

        /// <summary>
        /// Norm of a quaternion
        /// </summary>
        public double Norm
        {
            get { return Sqrt(_w * _w + _x * _x + _y * _y + _z * _z); }
        }

        /// <summary>
        /// Square of the norm of a quaternion
        /// </summary>
        public double SquareNorm
        {
            get { return (_w * _w + _x * _x + _y * _y + _z * _z); }
        }

        /// <summary>
        /// Conjugate of a quaternion
        /// </summary>
        public Quaternion Conjugate
        {
            get {
                Quaternion qq = this.Copy();
                qq.X = -this.X;
                qq.Y = -this.Y;
                qq.Z = -this.Z;
                return qq;
            }
        }

        /// <summary>
        /// Get axis of rotation in current reference coordinate system.
        /// </summary>
        public Vector3d Axis
        {
            get
            {
                if (GeometRi3D.AlmostEqual(Abs(_w), 1.0))
                {
                    return new Vector3d(1, 0 ,0, _coord);
                }
                else
                {
                    double tmp =  1.0 / Sqrt(1 - 2 * Acos(_w));
                    return new Vector3d(_x, _y, _z, _coord).Mult(tmp);
                }
            }
        }

        /// <summary>
        /// Get rotation angle in current reference coordinate system.
        /// </summary>
        public double Angle
        {
            get
            {
                if (GeometRi3D.AlmostEqual(Abs(_w),1.0))
                {
                    return 0.0;
                } else
                {
                    return 2 * Acos(_w);
                }
            }
        }
        #endregion

        /// <summary>
        /// Return normalized quaternion
        /// </summary>
        public Quaternion Normalized
        {
            get
            {
                Quaternion tmp = this.Copy();
                double tmp_norm = this.Norm;
                tmp.W = _w / tmp_norm;
                tmp.X = _x / tmp_norm;
                tmp.Y = _y / tmp_norm;
                tmp.Z = _z / tmp_norm;
                return tmp;
            }
        }

        /// <summary>
        /// Normalize the current quaternion
        /// </summary>
        public void Normalize()
        {
            double tmp = 1.0 / this.Norm;
            _w = _w * tmp;
            _x = _x * tmp;
            _y = _y * tmp;
            _z = _z * tmp;
        }

        public Quaternion Add(Quaternion q)
        {
            if ((this._coord != q._coord))
                q = q.ConvertTo(this._coord);

            Quaternion m = new Quaternion(this.Coord);
            m.W = _w + q.W;
            m.X = _x + q.X;
            m.Y = _y + q.Y;
            m.Z = _z + q.Z;
            return m;
        }

        public Quaternion Subtract(Quaternion q)
        {
            if ((this._coord != q._coord))
                q = q.ConvertTo(this._coord);

            Quaternion m = new Quaternion(this.Coord);
            m.W = _w - q.W;
            m.X = _x - q.X;
            m.Y = _y - q.Y;
            m.Z = _z - q.Z;
            return m;
        }

        public Quaternion Mult (Quaternion q)
        {
            if ((this._coord != q._coord))
                q = q.ConvertTo(this._coord);

            Quaternion m = new Quaternion(this.Coord);
            m.W = W * q.W - X * q.X - Y * q.Y - Z * q.Z;
            m.X = W * q.X + X * q.W + Y * q.Z - Z * q.Y;
            m.Y = W * q.Y - X * q.Z + Y * q.W + Z * q.X;
            m.Z = W * q.Z + X * q.Y - Y * q.X + Z * q.W;

            return m;

        }

        public Quaternion Scale(double a)
        {
            return new Quaternion(_w*a, _x*a, _y*a, _z*a, this.Coord);
        }

        public Quaternion Inverse()
        {
            return this.Conjugate.Scale(1.0 / this.SquareNorm);
        }

        /// <summary>
        /// Convert quaternion to global coordinate system.
        /// </summary>
        public Quaternion ConvertToGlobal()
        {
            if (_coord == null || object.ReferenceEquals(_coord, Coord3d.GlobalCS))
            {
                return this.Copy();
            }
            else
            {
                Vector3d axis = this.Axis;
                double angle = this.Angle;
                axis = axis.ConvertToGlobal();
                return new Quaternion(axis, angle);
            }
        }

        /// <summary>
        /// Convert quaternion to reference coordinate system.
        /// </summary>
        public Quaternion ConvertTo(Coord3d coord)
        {
            Vector3d axis = this.Axis;
            double angle = this.Angle;
            axis = axis.ConvertTo(coord);
            return new Quaternion(axis, angle);
        }

        /// <summary>
        /// Returns rotation matrix (in current reference coordinate system).
        /// </summary>
        public Matrix3d RotationMatrix()
        {
            Matrix3d m = new Matrix3d();
            this.Normalize();

            m[0, 0] = 1 - 2 * (_y * _y - _z * _z);
            m[0, 1] = 2 * (_x * _y - _w * _z);
            m[0, 2] = 2 * (_x * _z + _w * _y);

            m[1, 0] = 2 * (_x * _y + _w * _z);
            m[1, 1] = 1 - 2 * (_x * _x - _z * _z);
            m[1, 2] = 2 * (_y * _z - _w * _x);

            m[2, 0] = 2 * (_x * _z - _w * _y);
            m[2, 1] = 2 * (_y * _z + _w * _x);
            m[2, 2] = 1 - 2 * (_x * _x - _y * _y);
            return m;
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
            Quaternion q = (Quaternion)obj;
            return (this- q).Norm <= GeometRi3D.Tolerance;
        }

        /// <summary>
        /// Returns the hashcode for the object.
        /// </summary>
        public override int GetHashCode()
        {
            return GeometRi3D.HashFunction(_w.GetHashCode(), _x.GetHashCode(), _y.GetHashCode(), _z.GetHashCode());
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
        public string ToString(Coord3d coord)
        {
            if (coord == null) { coord = Coord3d.GlobalCS; }
            Quaternion q = this.ConvertTo(coord);
            return string.Format("Quaternion -> ({0,10:g5}, {1,10:g5}, {2,10:g5}, {2,10:g5})", q.W, q.X, q.Y, q.Z);
        }

        // Operators overloads
        //-----------------------------------------------------------------
        public static Quaternion operator +(Quaternion q1, Quaternion q2)
        {
            return q1.Add(q2);
        }

        public static Quaternion operator -(Quaternion q1, Quaternion q2)
        {
            return q1.Subtract(q2);
        }

        public static Quaternion operator *(Quaternion q1, Quaternion q2)
        {
            return q1.Mult(q2);
        }

        public static Quaternion operator *(Quaternion q1, double a)
        {
            return q1.Scale(a);
        }

        public static Quaternion operator /(Quaternion q1, double a)
        {
            return q1.Scale(1.0/a);
        }

        public static Quaternion operator *(double a, Quaternion q1)
        {
            return q1.Scale(a);
        }

        public static bool operator ==(Quaternion q1, Quaternion q2)
        {
            return q1.Equals(q2);
        }
        public static bool operator !=(Quaternion q1, Quaternion q2)
        {
            return !q1.Equals(q2);
        }

    }
}
