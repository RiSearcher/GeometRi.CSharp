using System;
using System.Collections.Generic;
using System.Text;
using static System.Math;

namespace GeometRi
{
    public class Tetrahedron : FiniteObject, IFiniteObject
    {

        private Point3d[] vertices;

        private Point3d _center;
        private double _lx, _ly, _lz;
        private Rotation _r;
        private Coord3d _local_coord = null;

        private List<Point3d> _list_p = null;
        private List<Triangle> _list_t = null;
        private List<Segment3d> _list_e = null;
        private List<Plane3d> _list_plane = null;

        #region "Constructors"
        /// <summary>
        /// Regular tetrahedron with vertices (0, 0, 0), (0, 1, 1), (1, 0, 1), (1, 1, 0)
        /// </summary>
        public Tetrahedron()
        {
            this.vertices = new Point3d[4];
            vertices[0] = new Point3d(0, 0, 0);
            vertices[1] = new Point3d(0, 1, 1);
            vertices[2] = new Point3d(1, 0, 1);
            vertices[3] = new Point3d(1, 1, 0);
        }

        public Tetrahedron(Point3d v1, Point3d v2, Point3d v3, Point3d v4)
        {
            this.vertices = new Point3d[4];
            vertices[0] = v1;
            vertices[1] = v2;
            vertices[2] = v3;
            vertices[3] = v4;
        }
        #endregion


        #region "Properties"

        public Point3d Center
        {
            get 
            {
                return 0.25 * (vertices[0] + vertices[1] + vertices[2] + vertices[3]);
            }

        }

        public Point3d A
        {
            get
            {
                return vertices[0];
            }
        }

        public Point3d B
        {
            get
            {
                return vertices[1];
            }
        }

        public Point3d C
        {
            get
            {
                return vertices[2];
            }
        }

        public Point3d D
        {
            get
            {
                return vertices[3];
            }
        }

        /// <summary>
        /// List of faces forming the tetrahedron
        /// </summary>
        public List<Triangle> ListOfFaces
        {
            get
            {
                if (_list_t == null)
                {
                    _list_t = new List<Triangle> { };
                    _list_t.Add(new Triangle(vertices[0], vertices[1], vertices[2]));
                    _list_t.Add(new Triangle(vertices[0], vertices[3], vertices[1]));
                    _list_t.Add(new Triangle(vertices[1], vertices[3], vertices[2]));
                    _list_t.Add(new Triangle(vertices[2], vertices[3], vertices[0]));

                    return _list_t;
                }
                else
                {
                    return _list_t;
                }
            }
        }


        /// <summary>
        /// List of edges forming the tetragedron
        /// </summary>
        public List<Segment3d> ListOfEdges
        {
            get
            {
                if (_list_e == null)
                {
                    _list_e = new List<Segment3d> { };
                    _list_e.Add(new Segment3d(vertices[0], vertices[1]));
                    _list_e.Add(new Segment3d(vertices[1], vertices[2]));
                    _list_e.Add(new Segment3d(vertices[2], vertices[0]));
                    _list_e.Add(new Segment3d(vertices[0], vertices[3]));
                    _list_e.Add(new Segment3d(vertices[1], vertices[3]));
                    _list_e.Add(new Segment3d(vertices[2], vertices[3]));
                    return _list_e;
                }
                else
                {
                    return _list_e;
                }
            }
        }

        /// <summary>
        /// Volume of the tetrahedron.
        /// </summary>
        public double Volume
        {
            get 
            {
                Vector3d a = new Vector3d(vertices[3], vertices[0]);
                Vector3d b = new Vector3d(vertices[3], vertices[1]);
                Vector3d c = new Vector3d(vertices[3], vertices[2]);
                return Abs(a*(b.Cross(c))) / 6;
            }
        }

        /// <summary>
        /// Surface area of the tetrahedron.
        /// </summary>
        public double Area
        {
            get
            {
                double area = 0.0;
                foreach (Triangle t in ListOfFaces)
                {
                    area += t.Area;
                }
                return area;
            }
        }

        #endregion

        #region "BoundingBox"
        /// <summary>
        /// Return minimum bounding box.
        /// </summary>
        public Box3d MinimumBoundingBox
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Return Axis Aligned Bounding Box (AABB) in given coordinate system.
        /// </summary>
        public Box3d BoundingBox(Coord3d coord = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return bounding sphere.
        /// </summary>
        public Sphere BoundingSphere
        {
            get 
            { 
                throw new NotImplementedException();
            }

        }
        #endregion


        #region "Intersection"

        #endregion

        internal override int _PointLocation(Point3d p)
        {

            int v1 = _SameSide(vertices[0], vertices[1], vertices[2], vertices[3], p);
            int v2 = _SameSide(vertices[1], vertices[2], vertices[3], vertices[0], p);
            int v3 = _SameSide(vertices[2], vertices[3], vertices[0], vertices[1], p);
            int v4 = _SameSide(vertices[3], vertices[0], vertices[1], vertices[2], p);

            int sum = v1 + v2 + v3 + v4;
            int product = v1 * v2 * v3 * v4;

            if (sum == 4)
            {
                return 1; // Point is strictly inside tetrahedron
            }
            else if (product == 0 && sum < 4)
            {
                return 0; // Point is on boundary
            }
            else
            {
                return -1; // Point is outside
            }
        }

        internal int _SameSide(Point3d a, Point3d b, Point3d c, Point3d d, Point3d p)
        {
            Vector3d normal = (b - a).ToVector.Cross((c - a).ToVector);
            double dot_d = normal * (d - a).ToVector;
            double dot_p = normal * (p - a).ToVector;
            if (GeometRi3D.AlmostEqual(dot_p,0.0))
            {
                return 0; // Point is on face
            }
            else if (Sign(dot_d) == Sign(dot_p))
            {
                return 1; // points are on the same side
            } else
            {
                return 999;
            }
        }

    }
}
