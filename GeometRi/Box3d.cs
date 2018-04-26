using System;
using static System.Math;
using System.Collections.Generic;

namespace GeometRi
{
    /// <summary>
    /// Arbitrary oriented 3D box, can be degenerated with one or more dimensions equal 0.
    /// </summary>
    public class Box3d : FiniteObject, IFiniteObject
    {

        private Point3d _center;
        private double _lx, _ly, _lz;
        private Rotation _r;

        #region "Constructors"

        /// <summary>
        /// Default constructor, initializes unit box in the origin of the global coordinate system aligned with coordinate axes.
        /// </summary>
        public Box3d(Point3d center, double lx, double ly, double lz)
        {
            _center = center.Copy();
            _lx = lx;
            _ly = ly;
            _lz = lz;
            _r = new Rotation();
        }

        /// <summary>
        /// Initializes unit box in the origin of the reference coordinate system aligned with coordinate axes.
        /// </summary>
        /// <param name="coord">Reference coordinate system.</param>
        public Box3d(Coord3d coord = null)
        {
            coord = (coord == null) ? Coord3d.GlobalCS : coord;
            _center = new Point3d(coord);
            _lx = _ly = _lz = 1.0;
            _r = new Rotation(coord);
        }

        /// <summary>
        /// Initializes box with specified dimensions and orientation defined by rotation object.
        /// </summary>
        /// <param name="center">Center point of the box.</param>
        /// <param name="lx">First dimension.</param>
        /// <param name="ly">Second dimension.</param>
        /// <param name="lz">Third dimension.</param>
        /// <param name="r">Orientation of the box, defined as rotation from axis aligned position (in global CS) to final position.</param>
        public Box3d(Point3d center, double lx, double ly, double lz, Rotation r)
        {
            _center = center.Copy();
            _lx = lx;
            _ly = ly;
            _lz = lz;
            _r = r.Copy();
        }

        /// <summary>
        /// Initializes axis aligned box in local coordinate system.
        /// </summary>
        /// <param name="center">Center point of the box.</param>
        /// <param name="lx">First dimension.</param>
        /// <param name="ly">Second dimension.</param>
        /// <param name="lz">Third dimension.</param>
        /// <param name="coord">Local coordinate system.</param>
        public Box3d(Point3d center, double lx, double ly, double lz, Coord3d coord)
        {
            _center = center.Copy();
            _lx = lx;
            _ly = ly;
            _lz = lz;
            _r = new Rotation(coord);
        }
        #endregion

        /// <summary>
        /// Creates copy of the object
        /// </summary>
        public Box3d Copy()
        {
            return new Box3d(_center, _lx, _ly, _lz, _r);
        }

        #region "Properties"
        /// <summary>
        /// Center point of the box.
        /// </summary>
        public Point3d Center
        {
            get { return _center.Copy(); }
            set { _center = value.Copy(); }
        }

        /// <summary>
        /// First dimension.
        /// </summary>
        public double L1
        {
            get { return _lx; }
            set { _lx = value; }
        }

        /// <summary>
        /// Second dimension.
        /// </summary>
        public double L2
        {
            get { return _ly; }
            set { _ly = value; }
        }

        /// <summary>
        /// Third dimension.
        /// </summary>
        public double L3
        {
            get { return _lz; }
            set { _lz = value; }
        }

        /// <summary>
        /// Orientation of the first dimension of the box.
        /// </summary>
        public Vector3d V1
        {
            get { return _r.ConvertToGlobal().ToRotationMatrix.Column1; }
        }

        /// <summary>
        /// Orientation of the second dimension of the box.
        /// </summary>
        public Vector3d V2
        {
            get { return _r.ConvertToGlobal().ToRotationMatrix.Column2; }
        }

        /// <summary>
        /// Orientation of the third dimension of the box.
        /// </summary>
        public Vector3d V3
        {
            get { return _r.ConvertToGlobal().ToRotationMatrix.Column3; }
        }

        /// <summary>
        /// Orientation of the box, defined as rotation from axis aligned position (in global CS) to final position.
        /// </summary>
        public Rotation Orientation
        {
            get { return _r.Copy(); }
            set { _r = value.Copy(); }
        }

        /// <summary>
        /// Corner point of the box.
        /// </summary>
        public Point3d P1
        {
            get
            {
                return _center.ConvertToGlobal() + 0.5 * (
                    - _lx * _r.ConvertToGlobal().ToRotationMatrix.Column1.ToPoint
                    - _ly * _r.ConvertToGlobal().ToRotationMatrix.Column2.ToPoint
                    - _lz * _r.ConvertToGlobal().ToRotationMatrix.Column3.ToPoint);
            }
        }

        /// <summary>
        /// Corner point of the box.
        /// </summary>
        public Point3d P2
        {
            get
            {
                return _center.ConvertToGlobal() + 0.5 * (
                    + _lx * _r.ConvertToGlobal().ToRotationMatrix.Column1.ToPoint
                    - _ly * _r.ConvertToGlobal().ToRotationMatrix.Column2.ToPoint
                    - _lz * _r.ConvertToGlobal().ToRotationMatrix.Column3.ToPoint);
            }
        }

        /// <summary>
        /// Corner point of the box.
        /// </summary>
        public Point3d P3
        {
            get
            {
                return _center.ConvertToGlobal() + 0.5 * (
                    + _lx * _r.ConvertToGlobal().ToRotationMatrix.Column1.ToPoint
                    + _ly * _r.ConvertToGlobal().ToRotationMatrix.Column2.ToPoint
                    - _lz * _r.ConvertToGlobal().ToRotationMatrix.Column3.ToPoint);
            }
        }

        /// <summary>
        /// Corner point of the box.
        /// </summary>
        public Point3d P4
        {
            get
            {
                return _center.ConvertToGlobal() + 0.5 * (
                    - _lx * _r.ConvertToGlobal().ToRotationMatrix.Column1.ToPoint
                    + _ly * _r.ConvertToGlobal().ToRotationMatrix.Column2.ToPoint
                    - _lz * _r.ConvertToGlobal().ToRotationMatrix.Column3.ToPoint);
            }
        }

        /// <summary>
        /// Corner point of the box.
        /// </summary>
        public Point3d P5
        {
            get
            {
                return _center.ConvertToGlobal() + 0.5 * (
                    - _lx * _r.ConvertToGlobal().ToRotationMatrix.Column1.ToPoint
                    - _ly * _r.ConvertToGlobal().ToRotationMatrix.Column2.ToPoint
                    + _lz * _r.ConvertToGlobal().ToRotationMatrix.Column3.ToPoint);
            }
        }

        /// <summary>
        /// Corner point of the box.
        /// </summary>
        public Point3d P6
        {
            get
            {
                return _center.ConvertToGlobal() + 0.5 * (
                    + _lx * _r.ConvertToGlobal().ToRotationMatrix.Column1.ToPoint
                    - _ly * _r.ConvertToGlobal().ToRotationMatrix.Column2.ToPoint
                    + _lz * _r.ConvertToGlobal().ToRotationMatrix.Column3.ToPoint);
            }
        }

        /// <summary>
        /// Corner point of the box.
        /// </summary>
        public Point3d P7
        {
            get
            {
                return _center.ConvertToGlobal() + 0.5 * (
                    + _lx * _r.ConvertToGlobal().ToRotationMatrix.Column1.ToPoint
                    + _ly * _r.ConvertToGlobal().ToRotationMatrix.Column2.ToPoint
                    + _lz * _r.ConvertToGlobal().ToRotationMatrix.Column3.ToPoint);
            }
        }

        /// <summary>
        /// Corner point of the box.
        /// </summary>
        public Point3d P8
        {
            get
            {
                return _center.ConvertToGlobal() + 0.5 * (
                    - _lx * _r.ConvertToGlobal().ToRotationMatrix.Column1.ToPoint
                    + _ly * _r.ConvertToGlobal().ToRotationMatrix.Column2.ToPoint
                    + _lz * _r.ConvertToGlobal().ToRotationMatrix.Column3.ToPoint);
            }
        }

        /// <summary>
        /// List of corner points.
        /// </summary>
        public List<Point3d> ListOfPoints
        {
            get { return new List<Point3d> { P1, P2, P3, P4, P5, P6, P7, P8 }; }
        }

        /// <summary>
        /// Volume of the box.
        /// </summary>
        public double Volume
        {
            get { return _lx * _ly * _lz; }
        }

        /// <summary>
        /// Surface area of the box.
        /// </summary>
        public double Area
        {
            get { return 2.0 * (_lx*_ly + _lx*_lz + _ly*_lz); }
        }

        /// <summary>
        /// Length of the box diagonal.
        /// </summary>
        public double Diagonal
        {
            get { return Sqrt(_lx * _lx + _ly * _ly + _lz * _lz); }
        }

        #endregion

        #region "BoundingBox"
        /// <summary>
        /// Return minimum bounding box.
        /// </summary>
        public Box3d MinimumBoundingBox
        {
            get { return this.Copy(); }
        }

        /// <summary>
        /// Return Axis Aligned Bounding Box (AABB) in given coordinate system.
        /// </summary>
        public Box3d BoundingBox(Coord3d coord = null)
        {
            coord = (coord == null) ? Coord3d.GlobalCS : coord;
            Point3d c = _center.ConvertTo(coord);
            double mx = c.X;
            double my = c.Y;
            double mz = c.Z;

            foreach (Point3d p in this.ListOfPoints)
            {
                Point3d t = p.ConvertTo(coord);
                if (t.X < mx) mx = t.X;
                if (t.Y < my) my = t.Y;
                if (t.Z < mz) mz = t.Z;
            }

            return new Box3d(c, 2.0 * (c.X - mx), 2.0 * (c.Y - my), 2.0 * (c.Z - mz), coord);
        }

        /// <summary>
        /// Return bounding sphere.
        /// </summary>
        public Sphere BoundingSphere
        {
            get
            {
                double r = 0.5 * Sqrt(_lx * _lx + _ly * _ly + _lz * _lz);
                return new Sphere(this.Center, r);
            }
        }
        #endregion

        internal override int _PointLocation(Point3d p)
        {
            Coord3d coord = new Coord3d(this.Center, this.V1, this.V2);
            p = p.ConvertTo(coord);
            if (GeometRi3D.UseAbsoluteTolerance)
            {
                if ( (Abs(p.X)-L1/2) <= GeometRi3D.Tolerance && (Abs(p.Y) - L2 / 2) <= GeometRi3D.Tolerance && (Abs(p.Z) - L3 / 2) <= GeometRi3D.Tolerance )
                {
                    if ( (Abs(p.X) - L1 / 2) < -GeometRi3D.Tolerance && (Abs(p.Y) - L2 / 2) < -GeometRi3D.Tolerance && (Abs(p.Z) - L3 / 2) < -GeometRi3D.Tolerance)
                    {
                        return 1; // Point is strictly inside box
                    } else
                    {
                        return 0; // Point is on boundary
                    }
                } else
                {
                    return -1; // Point is outside
                }
            }
            else
            {
                double tol = GeometRi3D.Tolerance;
                GeometRi3D.Tolerance = tol * this.Diagonal;
                GeometRi3D.UseAbsoluteTolerance = true;
                int result = this._PointLocation(p);
                GeometRi3D.UseAbsoluteTolerance = false;
                GeometRi3D.Tolerance = tol;
                return result;
            }
        }

        #region "TranslateRotateReflect"
        /// <summary>
        /// Translate box by a vector
        /// </summary>
        public Box3d Translate(Vector3d v)
        {
            return new Box3d(_center.Translate(v), _lx, _ly, _lz);
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
            Box3d b = (Box3d)obj;

            if (GeometRi3D.UseAbsoluteTolerance)
            {
                return this.Center == b.Center && _r == b.Orientation &&
                       GeometRi3D.AlmostEqual(L1, b.L1) &&
                       GeometRi3D.AlmostEqual(L2, b.L2) &&
                       GeometRi3D.AlmostEqual(L3, b.L3);
            }
            else
            {
                double tol = GeometRi3D.Tolerance;
                GeometRi3D.Tolerance = tol * this.Diagonal;
                GeometRi3D.UseAbsoluteTolerance = true;
                bool result = this.Equals(b);
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
            int hash_code = GeometRi3D.HashFunction(_lx.GetHashCode(), _ly.GetHashCode(), _lz.GetHashCode());
            return GeometRi3D.HashFunction(_center.GetHashCode(), _r.GetHashCode(), hash_code);
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

            if (coord == null) { coord = Coord3d.GlobalCS; }
            Point3d p = _center.ConvertTo(coord);

            string str = string.Format("Box3d (reference coord.sys. ") + coord.Name + "):" + nl;
            str += string.Format("Center -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", p.X, p.Y, p.Z) + nl;
            str += string.Format("Lx, Ly, Lz -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", _lx, _ly, _lz) + nl;
            str += _r.ToString(coord);
            return str;
        }

        // Operators overloads
        //-----------------------------------------------------------------
        public static bool operator ==(Box3d b1, Box3d b2)
        {
            return b1.Equals(b2);
        }
        public static bool operator !=(Box3d b1, Box3d b2)
        {
            return !b1.Equals(b2);
        }
    }
}
