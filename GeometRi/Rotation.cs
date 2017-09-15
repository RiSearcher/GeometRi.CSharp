using System;
using static System.Math;


namespace GeometRi
{
    /// <summary>
    /// Rotation in 3D space defined in global or local reference frame (internally represented by rotation matrix).
    /// </summary>
    public class Rotation
    {
        private Matrix3d _r;
        private Coord3d _coord;

        #region "Constructors"
        /// <summary>
        /// Default constructor, initializes identity matrix.
        /// </summary>
        /// <param name="coord">Reference coordinate system (default - Coord3d.GlobalCS).</param>
        public Rotation(Coord3d coord = null)
        {
            _r = Matrix3d.Identity();
            _coord = (coord == null) ? Coord3d.GlobalCS : coord;
        }

        /// <summary>
        /// Initializes rotation using rotation matrix.
        /// </summary>
        /// <param name="m">Rotation matrix.</param>
        /// <param name="coord">Reference coordinate system (default - Coord3d.GlobalCS).</param>
        public Rotation(Matrix3d m, Coord3d coord = null)
        {
            _r = m.Copy();
            _coord = (coord == null) ? Coord3d.GlobalCS : coord;
        }

        /// <summary>
        /// Initializes rotation using quaternion.
        /// </summary>
        /// <param name="q"></param>
        public Rotation(Quaternion q)
        {
            _r = q.ToRotationMatrix();
            _coord = q.Coord;
        }

        /// <summary>
        /// Initializes rotation using axis and angle of rotation.
        /// </summary>
        /// <param name="axis">Rotation axis</param>
        /// <param name="alpha">Angle of rotation (counterclockwise, radians)</param>
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

        private double this[int i, int j]
        {
            get { return this._r[i, j]; }
            set { this._r[i, j] = value; }
        }

        /// <summary>
        /// Creates copy of the object
        /// </summary>
        public Rotation Copy()
        {
            return new Rotation(_r, _coord);
        }

        #region "Properties"
        public Matrix3d ToRotationMatrix
        {
            get { return this._r; }
        }

        public Quaternion ToQuaternion
        {
            get { return new Quaternion(_r, _coord); }
        }

        public Vector3d ToAxis
        {
            get
            {
                double angle = this.ToAngle;
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

        public double ToAngle
        {
            // To avoid singularities convert to quaternion first.
            // Another way:
            // http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToAngle/index.htm
            get { return this.ToQuaternion.ToAngle; }
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
        /// Creates rotation object by composing three elemental rotations, i.e. rotations about the axes of a coordinate system.
        /// <para>Both proper Euler angles ("xyx", "zxz", etc.) or Tait–Bryan angles ("xyz", "yzx") are allowed.</para>
        /// Extrinsic rotations (rotations in fixed frame) should be written in lower case ("xyz", zxz", etc.).
        /// <para>Intrinsic rotations (rotations in moving frame) should be written in upper case ("XYZ", "ZXZ", etc.).</para>
        /// </summary>
        /// <param name="alpha">First rotation angle.</param>
        /// <param name="beta">Second rotation angle.</param>
        /// <param name="gamma">Third rotation angle.</param>
        /// <param name="RotationOrder">String, representing rotation axes in the form "xyz" (extrinsic rotations, fixed frame) or "XYZ" (intrinsic rotations, moving frame).</param>
        /// <param name="coord">Reference coordinate system, default - Coord3d.GlobalCS.</param>
        /// <returns></returns>
        public static Rotation FromEulerAngles(double alpha, double beta, double gamma, string RotationOrder, Coord3d coord = null)
        {
            if (string.IsNullOrEmpty(RotationOrder) || RotationOrder.Length < 3)
            {
                throw new ArgumentException("Invalid parameter: RotationOrder");
            }

            coord = (coord == null) ? Coord3d.GlobalCS : coord;
            Vector3d v1 = CharToVector(RotationOrder[0], coord);
            Vector3d v2 = CharToVector(RotationOrder[1], coord);
            Vector3d v3 = CharToVector(RotationOrder[2], coord);

            Rotation r1 = new Rotation(v1, alpha);
            Rotation r2 = new Rotation(v2, beta);
            Rotation r3 = new Rotation(v3, gamma);

            if (RotationOrder[0] == 'x' || RotationOrder[0] == 'y' || RotationOrder[0] == 'z')
            {
                // Rotation in fixed frame
                return r3 * r2 * r1;
            }
            else
            {
                // Rotation in moving frame
                return r1 * r2 * r3;
            }
        }
        private static Vector3d CharToVector(char c, Coord3d coord)
        {
            if (c == 'x' || c == 'X') return new Vector3d(1, 0, 0, coord);
            if (c == 'y' || c == 'Y') return new Vector3d(0, 1, 0, coord);
            if (c == 'z' || c == 'Z') return new Vector3d(0, 0, 1, coord);

            throw new ArgumentException("Invalid parameter: RotationOrder");
        }

        /// <summary>
        /// Spherical linear interpolation of two rotations.
        /// </summary>
        /// <param name="r1">Initial rotation</param>
        /// <param name="r2">Final rotation</param>
        /// <param name="t">Interpolation parameter within range [0, 1]</param>
        public static Rotation SLERP(Rotation r1, Rotation r2, double t)
        {
            return new Rotation(Quaternion.SLERP(r1.ToQuaternion, r2.ToQuaternion, t));
        }

        /// <summary>
        /// Combine two rotations.
        /// </summary>
        public Rotation Mult(Rotation r)
        {
            Matrix3d m = this.ToRotationMatrix * r.ConvertTo(this.Coord).ToRotationMatrix;
            return new Rotation(m, this.Coord);
        }

        /// <summary>
        /// Multiply rotation matrix by vector.
        /// <para>The rotation matrix is first transformed into reference coordinate system of vector.</para>
        /// </summary>
        public Vector3d Mult(Vector3d v)
        {
            return this.ConvertTo(v.Coord).ToRotationMatrix * v;
        }

        /// <summary>
        /// Multiply rotation matrix by point.
        /// <para>The rotation matrix is first transformed into reference coordinate system of point.</para>
        /// </summary>
        public Point3d Mult(Point3d p)
        {
            return this.ConvertTo(p.Coord).ToRotationMatrix * p;
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
                Vector3d axis = this.ToAxis;
                double angle = this.ToAngle;
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
                Vector3d axis = this.ToAxis;
                double angle = this.ToAngle;
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
            Rotation r = (Rotation)obj;
            return (this.ToRotationMatrix - r.ConvertTo(this.Coord).ToRotationMatrix).MaxNorm < GeometRi3D.Tolerance;
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
            Rotation r = this.ConvertTo(coord);
            string str = "Rotation (reference coord.sys. " + coord.Name + "):" + nl;
            str += string.Format("Row1 -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", r[0, 0], r[0, 1], r[0, 2]) + nl;
            str += string.Format("Row2 -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", r[1, 0], r[1, 1], r[1, 2]) + nl;
            str += string.Format("Row3 -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", r[2, 0], r[2, 1], r[2, 2]);
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
        /// Combine two rotations.
        /// </summary>
        public static Rotation operator *(Rotation r1, Rotation r2)
        {
            return r1.Mult(r2);
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
