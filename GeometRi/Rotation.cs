using System;
using static System.Math;


namespace GeometRi
{
    /// <summary>
    /// Rotation action in 3D space defined in global or local reference frame (internally represented by rotation matrix).
    /// </summary>
    public class Rotation
    {
        private Matrix3d _r;
        private Coord3d _coord;

        #region "Constructors"
        public Rotation(Coord3d coord = null)
        {
            _r = Matrix3d.Identity();
            _coord = (coord == null) ? Coord3d.GlobalCS : coord;
        }

        public Rotation(Matrix3d m, Coord3d coord = null)
        {
            _r = m.Copy();
            _coord = (coord == null) ? Coord3d.GlobalCS : coord;
        }

        public Rotation(Quaternion q)
        {
            _r = q.RotationMatrix();
            _coord = q.Coord;
        }

        /// <summary>
        /// Defines counterclockwise rotation around axis
        /// </summary>
        /// <param name="axis">Rotation axis</param>
        /// <param name="alpha">Angle of rotation (radians)</param>
        public Rotation(Vector3d axis, double alpha)
        {
            Vector3d v = axis.Normalized;
            double c = Cos(alpha);
            double s = Sin(alpha);

            _r = new Matrix3d();
            _r[0, 0] = c + v.X * v.X * (1 - c);
            _r[0, 1] = v.X * v.Y * (1 - c) - v.Z * s;
            _r[0, 2] = v.X * v.Z * (1 - c) + v.Y * s;

            _r[1, 0] = v.Y * v.X * (1 - c) + v.Z * s;
            _r[1, 1] = c + v.Y * v.Y * (1 - c);
            _r[1, 2] = v.Y * v.Z * (1 - c) - v.X * s;

            _r[2, 0] = v.Z * v.X * (1 - c) - v.Y * s;
            _r[2, 1] = v.Z * v.Y * (1 - c) + v.X * s;
            _r[2, 2] = c + v.Z * v.Z * (1 - c);

            _coord = axis.Coord;
        }

        //public Rotation(Vector3d axis, double alpha)
        //{
        //    Vector3d v = axis.Normalized;
        //    Matrix3d S = new Matrix3d();
        //    S[0, 1] = -v.Z;
        //    S[0, 2] = v.Y;
        //    S[1, 0] = v.Z;
        //    S[1, 2] = -v.X;
        //    S[2, 0] = -v.Y;
        //    S[2, 1] = v.X;

        //    _rot = Matrix3d.Identity() + Sin(alpha) * S + (1.0 - Cos(alpha)) * S * S;
        //    _coord = axis.Coord;
        //}
        #endregion

        /// <summary>
        /// Creates copy of the object
        /// </summary>
        public Rotation Copy()
        {
            return new Rotation(_r, _coord);
        }

        #region "Properties"
        public Matrix3d RotationMatrix
        {
            get { return this._r; }
        }

        public Quaternion Quaternion
        {
            get { return new Quaternion(_r, _coord); }
        }

        public Vector3d Axis
        {
            get
            {
                double angle = this.Angle;
                if (GeometRi3D.AlmostEqual(angle,0.0))
                {
                    return new Vector3d(1, 0, 0, _coord);
                } else if (GeometRi3D.Smaller(angle,PI))
                {
                    Vector3d v = new Vector3d(_r[2, 1] - _r[1, 2], _r[0, 2] - _r[2, 0], _r[1, 0] - _r[0, 1], _coord);
                    return v.Normalized;
                } else if (_r[0,0] >= _r[1,1] && _r[0,0] >= _r[2,2])
                {
                    double x = Sqrt(_r[0, 0] - _r[1, 1] - _r[2, 2] + 1) / 2;
                    double y = _r[0, 1] / (2 * x);
                    double z = _r[0, 2] / (2 * x);
                    return new Vector3d(x, y, z, _coord);
                } else if (_r [1,1] >= _r [0,0] && _r [1,1] >= _r [2,2])
                {
                    double y = Sqrt(_r[1, 1] - _r[0, 0] - _r[2, 2] + 1) / 2;
                    double x = _r[0, 1] / (2 * y);
                    double z = _r[1, 2] / (2 * y);
                    return new Vector3d(x, y, z, _coord);
                } else
                {
                    double z = Sqrt(_r[2, 2] - _r[0, 0] - _r[1, 1] + 1) / 2;
                    double x = _r[0, 2] / (2 * z);
                    double y = _r[1, 2] / (2 * z);
                    return new Vector3d(x, y, z, _coord);
                }
            }
        }

        public double Angle
        {
            // To avoid singularities convert to quaternion first.
            // Another way:
            // http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToAngle/index.htm
            get { return this.Quaternion.Angle; }
        }

        /// <summary>
        ///  Reference coordinate system
        /// </summary>
        public Coord3d Coord
        {
            get { return _coord; }
        }
        #endregion

        /// <summary>
        /// Multiply rotation matrix by vector.
        /// <para>The rotation matrix is first transformed into reference coordinate system of vector.</para>
        /// </summary>
        public Vector3d Mult(Vector3d v)
        {
            return this.ConvertTo(v.Coord).RotationMatrix * v;
        }

        /// <summary>
        /// Multiply rotation matrix by point.
        /// <para>The rotation matrix is first transformed into reference coordinate system of point.</para>
        /// </summary>
        public Point3d Mult(Point3d p)
        {
            return this.ConvertTo(p.Coord).RotationMatrix * p;
        }

        /// <summary>
        /// Convert rotation object to global coordinate system.
        /// </summary>
        public Rotation ConvertToGlobal()
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
                return new Rotation(axis, angle);
            }
        }

        /// <summary>
        /// Convert rotation object to reference coordinate system.
        /// </summary>
        public Rotation ConvertTo(Coord3d coord)
        {
            if (this._coord == coord)
            {
                return this.Copy();
            } else
            {
                Vector3d axis = this.Axis;
                double angle = this.Angle;
                axis = axis.ConvertTo(coord);
                return new Rotation(axis, angle);
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
            Matrix3d m = (Matrix3d)obj;
            return (this.RotationMatrix - m).MaxNorm < GeometRi3D.Tolerance;
        }

        /// <summary>
        /// Returns the hashcode for the object.
        /// </summary>
        public override int GetHashCode()
        {
            return GeometRi3D.HashFunction(_r.Row1.GetHashCode(), 
                                           _r.Row2.GetHashCode(), 
                                           _r.Row3.GetHashCode());
        }

        /// <summary>
        /// String representation of an object.
        /// </summary>
        public override string ToString()
        {
            string str = "Rotation: " + System.Environment.NewLine;
            str += string.Format("Row1 -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", _r[0, 0], _r[0, 1], _r[0, 2]) + System.Environment.NewLine;
            str += string.Format("Row2 -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", _r[1, 0], _r[1, 1], _r[1, 2]) + System.Environment.NewLine;
            str += string.Format("Row3 -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", _r[2, 0], _r[2, 1], _r[2, 2]);
            return str;
        }

        // Operators overloads
        //-----------------------------------------------------------------
        public static bool operator ==(Rotation m1, Rotation m2)
        {
            return m1.Equals(m2);
        }
        public static bool operator !=(Rotation m1, Rotation m2)
        {
            return !m1.Equals(m2);
        }

        /// <summary>
        /// Multiply rotation matrix by vector.
        /// <para>The rotation matrix is first transformed into reference coordinate system of vector.</para>
        /// </summary>
        public static Vector3d operator *(Rotation r, Vector3d v)
        {
            return r.Mult(v);
        }

        /// <summary>
        /// Multiply rotation matrix by point.
        /// <para>The rotation matrix is first transformed into reference coordinate system of point.</para>
        /// </summary>
        public static Point3d operator *(Rotation r, Point3d p)
        {
            return r.Mult(p);
        }
    }
}
