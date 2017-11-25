using System;
using static System.Math;

namespace GeometRi
{
    /// <summary>
    /// Cartesian coordinate system defined by origin and transformation matrix (in row format).
    /// </summary>
    public class Coord3d
    {

        private Point3d _origin;
        private Matrix3d _axes;
        private string _name;
        private static int count = 0;

        public static readonly Coord3d GlobalCS = new Coord3d("Global_CS");

#region "Constructors"
        /// <summary>
        /// Initializes default coordinate system.
        /// </summary>
        /// <param name="name">Name of the coordinate system.</param>
        public Coord3d(string name = "")
        {
            _origin = new Point3d(0, 0, 0);
            _axes = Matrix3d.Identity();
            if ((!string.IsNullOrEmpty(name)))
            {
                _name = name;
            }
            else
            {
                _name = "Coord " + count.ToString();
            }
            count += 1;
        }

        /// <summary>
        /// Initializes coordinate system using origin point  and transformation matrix.
        /// </summary>
        /// <param name="p">Origin of the coordinate system.</param>
        /// <param name="m">Transformation matrix (in row format).</param>
        /// <param name="name">Name of the coordinate system.</param>
        public Coord3d(Point3d p, Matrix3d m, string name = "")
        {
            if (!m.IsOrthogonal)
            {
                throw new ArgumentException("The matrix is not orthogonal");
            }

            _origin = p.ConvertToGlobal();
            _axes = m.Copy();
            if ((!string.IsNullOrEmpty(name)))
            {
                _name = name;
            }
            else
            {
                _name = "Coord " + count.ToString();
            }
            count += 1;
        }

        /// <summary>
        /// Initializes coordinate system using origin point and two vectors.
        /// </summary>
        /// <param name="p">Origin of the coordinate system.</param>
        /// <param name="v1">Vector oriented along the X axis.</param>
        /// <param name="v2">Vector in the XY plane.</param>
        /// <param name="name">Name of the coordinate system.</param>
        public Coord3d(Point3d p, Vector3d v1, Vector3d v2, string name = "")
        {

            if (v1.IsParallelTo(v2))
            {
                throw new Exception("Vectors are parallel");
            }

            v1 = v1.ConvertToGlobal().Normalized;
            Vector3d v3 = v1.Cross(v2).Normalized;
            v2 = v3.Cross(v1).Normalized;

            _origin = p.ConvertToGlobal();
            _axes = new Matrix3d(v1, v2, v3);
            if ((!string.IsNullOrEmpty(name)))
            {
                _name = name;
            }
            else
            {
                _name = "Coord " + count.ToString();
            }
            count += 1;
        }

        /// <summary>
        /// Initializes coordinate system using three points.
        /// </summary>
        /// <param name="p1">Origin of the coordinate system.</param>
        /// <param name="p2">Point on the X axis.</param>
        /// <param name="p3">Point on the XY plane.</param>
        /// <param name="name">Name of the coordinate system.</param>
        public Coord3d(Point3d p1, Point3d p2, Point3d p3, string name = "")
        {
            Vector3d v1 = new Vector3d(p1, p2);
            Vector3d v2 = new Vector3d(p1, p3);

            if (v1.IsParallelTo(v2))
            {
                throw new Exception("Points are collinear");
            }

            v1 = v1.ConvertToGlobal().Normalized;
            Vector3d v3 = v1.Cross(v2).Normalized;
            v2 = v3.Cross(v1).Normalized;

            _origin = p1.ConvertToGlobal();
            _axes = new Matrix3d(v1, v2, v3);
            if ((!string.IsNullOrEmpty(name)))
            {
                _name = name;
            }
            else
            {
                _name = "Coord " + count.ToString();
            }
            count += 1;
        }

        /// <summary>
        /// Initializes coordinate system using origin point and two double arrays.
        /// </summary>
        /// <param name="p">Origin of the coordinate system.</param>
        /// <param name="d1">Vector oriented along the X axis.</param>
        /// <param name="d2">Vector in the XY plane.</param>
        /// <param name="name">Name of the coordinate system.</param>
        public Coord3d(Point3d p, double[] d1, double[] d2, string name = "")
        {
            Vector3d v1 = new Vector3d(d1);
            Vector3d v2 = new Vector3d(d2);
            if (v1.IsParallelTo(v2))
            {
                throw new Exception("Vectors are parallel");
            }

            v1 = v1.Normalized;
            Vector3d v3 = v1.Cross(v2).Normalized;
            v2 = v3.Cross(v1).Normalized;

            _origin = p.ConvertToGlobal();
            _axes = new Matrix3d(v1, v2, v3);
            if ((!string.IsNullOrEmpty(name)))
            {
                _name = name;
            }
            else
            {
                _name = "Coord " + count.ToString();
            }
            count += 1;
        }
        #endregion

        /// <summary>
        /// Creates copy of the object
        /// </summary>
        public Coord3d Copy()
        {
            return new Coord3d(_origin, _axes);
        }


        /// <summary>
        /// Get or Set the origin of the coordinate system
        /// </summary>
        /// <returns></returns>
        public Point3d Origin
        {
            get { return new Point3d(_origin.X, _origin.Y, _origin.Z); }
            set { _origin = value.ConvertToGlobal(); }
        }
        /// <summary>
        /// Get or Set unit vectors of the axes, stored as row-matrix(3x3)
        /// </summary>
        /// <returns></returns>
        public Matrix3d Axes
        {
            get { return _axes.Copy(); }
            set
            {
                if (value.IsOrthogonal)
                {
                    _axes = value.Copy();
                }
                else
                {
                    throw new ArgumentException("The matrix is not orthogonal");
                }
            }
        }

        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Get total number of defined coordinate systems
        /// </summary>
        public static int Counts
        {
            get { return count; }
        }


        /// <summary>
        /// Get X-axis
        /// </summary>
        public Vector3d Xaxis
        {
            get { return _axes.Row1; }
        }
        /// <summary>
        /// Get Y-axis
        /// </summary>
        public Vector3d Yaxis
        {
            get { return _axes.Row2; }
        }
        /// <summary>
        /// Get Z-axis
        /// </summary>
        public Vector3d Zaxis
        {
            get { return _axes.Row3; }
        }

        /// <summary>
        /// XY plane in the current coordinate system
        /// </summary>
        public Plane3d XY_plane
        {
            get { return new Plane3d(0, 0, 1, 0, this); }
        }

        /// <summary>
        /// XZ plane in the current coordinate system
        /// </summary>
        public Plane3d XZ_plane
        {
            get { return new Plane3d(0, 1, 0, 0, this); }
        }

        /// <summary>
        /// YZ plane in the current coordinate system
        /// </summary>
        public Plane3d YZ_plane
        {
            get { return new Plane3d(1, 0, 0, 0, this); }
        }

        /// <summary>
        /// Rotate coordinate system
        /// </summary>
        public void Rotate(Rotation r)
        {
            _axes = _axes * r.ConvertToGlobal().ToRotationMatrix.Transpose();
        }

        /// <summary>
        /// Rotate coordinate system around rotation axis
        /// </summary>
        /// <param name="axis">Rotation axis</param>
        /// <param name="angle">Rotation angle (radians, counterclockwise)</param>
        public void Rotate(Vector3d axis, double angle)
        {
            _axes = _axes * Matrix3d.RotationMatrix(axis.ConvertToGlobal(), angle).Transpose();
        }

        /// <summary>
        /// Rotate coordinate system around rotation axis
        /// </summary>
        /// <param name="axis">Rotation axis</param>
        /// <param name="angle">Rotation angle (degrees, counterclockwise)</param>
        public void RotateDeg(Vector3d axis, double angle)
        {
            _axes = _axes * Matrix3d.RotationMatrix(axis.ConvertToGlobal(), angle * PI / 180).Transpose();
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
            Coord3d cs = (Coord3d)obj;
            return this._name == cs._name;
        }

        /// <summary>
        /// Returns the hashcode for the object.
        /// </summary>
        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }

        /// <summary>
        /// String representation of an object in global coordinate system.
        /// </summary>
        public override String ToString()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            string nl = System.Environment.NewLine;
            str.Append("Coord3d: " + _name + nl);
            str.Append(string.Format("Origin -> X: {0,10:g5}, Y: {1,10:g5}, Z: {2,10:g5}", _origin.X, _origin.Y, _origin.Z) + nl);
            str.Append(string.Format("Xaxis  -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", Xaxis.X, Xaxis.Y, Xaxis.Z) + nl);
            str.Append(string.Format("Yaxis  -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", Yaxis.X, Yaxis.Y, Yaxis.Z) + nl);
            str.Append(string.Format("Zaxis  -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", Zaxis.X, Zaxis.Y, Zaxis.Z));
            return str.ToString();
        }

        // Operators overloads
        //-----------------------------------------------------------------
        public static bool operator ==(Coord3d c1, Coord3d c2)
        {

            if ((object)c1 != null)
            {
                return c1.Equals(c2);
            }
            else if ((object)c1 == null && (object)c2 == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool operator !=(Coord3d c1, Coord3d c2)
        {
            if ((object)c1 != null)
            {
                return !c1.Equals(c2);
            }
            else if ((object)c1 == null && (object)c2 == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}



