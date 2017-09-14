using System;
using static System.Math;
using System.Collections.Generic;

namespace GeometRi
{
    public class Box3d : IFiniteObject
    {

        private Point3d _center;
        private double _lx, _ly, _lz;
        private Rotation _r;

        #region "Constructors"
        public Box3d(Coord3d coord = null)
        {
            _center = new Point3d(coord);
            _lx = _ly = _lz = 1.0;
            _r = new Rotation(coord);
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
        #endregion
    }
}
