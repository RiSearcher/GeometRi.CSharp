using System;
using static System.Math;
using System.Collections.Generic;

namespace GeometRi
{
    /// <summary>
    /// Arbitrary oriented 3D box, can be degenerated with one or more dimensions equal 0.
    /// </summary>
    public class Box3d : IFiniteObject
    {

        private Point3d _center;
        private double _lx, _ly, _lz;
        private Rotation _r;

        #region "Constructors"
        /// <summary>
        /// Default constructor, initializes unit box in the origin of the reference coordinate system aligned with coordinate axes.
        /// </summary>
        /// <param name="coord"></param>
        public Box3d(Coord3d coord = null)
        {
            coord = (coord == null) ? Coord3d.GlobalCS : coord;
            _center = new Point3d(coord);
            _lx = _ly = _lz = 1.0;
            _r = new Rotation(coord.Axes.Transpose());
        }

        public Box3d(Point3d center, double lx, double ly, double lz, Rotation r)
        {
            _center = center.Copy();
            _lx = lx;
            _ly = ly;
            _lz = lz;
            _r = r.Copy();
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
        public Point3d Center
        {
            get { return _center.Copy(); }
            set { _center = value.Copy(); }
        }

        public double Lx
        {
            get { return _lx; }
            set { _lx = value; }
        }

        public double Ly
        {
            get { return _ly; }
            set { _ly = value; }
        }

        public double Lz
        {
            get { return _lz; }
            set { _lz = value; }
        }

        public Rotation Orientation
        {
            get { return _r.Copy(); }
            set { _r = value.Copy(); }
        }

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

        public List<Point3d> ListOfPoints
        {
            get { return new List<Point3d> { P1, P2, P3, P4, P5, P6, P7, P8 }; }
        }

        public double Volume
        {
            get { return _lx * _ly * _lz; }
        }

        public double Area
        {
            get { return 2.0 * (_lx*_ly + _lx*_lz + _ly*_lz); }
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
        public Box3d BoundingBox(Coord3d coord)
        {
            Point3d c = _center.ConvertTo(coord);
            Rotation r = new Rotation(coord);
            double mx = c.X;
            double my = c.Y;
            double mz = c.Z;

            foreach (Point3d p in this.ListOfPoints)
            {
                if (p.X < mx) mx = p.X;
                if (p.Y < my) my = p.Y;
                if (p.Z < mz) mz = p.Z;
            }

            return new Box3d(c, 2.0 * (c.X - mx), 2.0 * (c.Y - my), 2.0 * (c.Z - mz), r);
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
            return this.Center == b.Center &&  _r == b.Orientation &&
                   GeometRi3D.AlmostEqual(Lx, b.Lx) &&
                   GeometRi3D.AlmostEqual(Ly, b.Ly) &&
                   GeometRi3D.AlmostEqual(Lz, b.Lz);
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
