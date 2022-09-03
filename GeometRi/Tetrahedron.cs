using System;
using System.Collections.Generic;
using System.Text;
using static System.Math;

namespace GeometRi
{
    public class Tetrahedron : FiniteObject, IFiniteObject
    {

        private Point3d[] vertices;

        private Coord3d _local_coord = null;
        private Box3d _aabb = null;

        private List<Triangle> _list_t = null;
        private List<Segment3d> _list_e = null;

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

        /// <summary>
        /// Creates copy of the object
        /// </summary>
        public Tetrahedron Copy()
        {
            return new Tetrahedron(vertices[0], vertices[1], vertices[2], vertices[3]);
        }


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
            if (_aabb == null)
            {
                _aabb = Box3d.AABB(vertices, coord);
            }
            return _aabb;
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

        #region "Distance"

        /// <summary>
        /// Distance from tetrahedron to point (zero will be returned for point located inside box)
        /// </summary>
        public double DistanceTo(Point3d p)
        {
            return ClosestPoint(p).DistanceTo(p);
        }

        /// <summary>
        /// Point on tetrahedron (including interior points) closest to target point "p".
        /// </summary>
        public Point3d ClosestPoint(Point3d p)
        {
            if (p.BelongsTo(this))
            {
                return p.Copy();
            }
            Point3d closest_point = null;
            double dist = double.PositiveInfinity;

            foreach (Triangle t in this.ListOfFaces)
            {
                Point3d temp_point = t.ClosestPoint(p);
                double temp_dist = p.DistanceTo(temp_point);
                if (temp_dist < dist)
                {
                    dist = temp_dist;
                    closest_point = temp_point;
                }
            }
            return closest_point;
        }

        /// <summary>
        /// Distance between two tetrahedrones
        /// </summary>
        public double DistanceTo(Tetrahedron t)
        {
            if (t.A.BelongsTo(this) || t.B.BelongsTo(this) || t.C.BelongsTo(this) || t.D.BelongsTo(this))
            {
                return 0;
            }
            if (this.A.BelongsTo(t) || this.B.BelongsTo(t) || this.C.BelongsTo(t) || this.D.BelongsTo(t))
            {
                return 0;
            }
            double dist = double.PositiveInfinity;
            foreach (Triangle t1 in this.ListOfFaces)
            {
                foreach (Segment3d t2 in t.ListOfEdges)
                {
                    double temp_dist = t1.DistanceTo(t2);
                    if (temp_dist == 0.0)
                    {
                        return 0;
                    } else if (temp_dist < dist)
                    {
                        dist = temp_dist;
                    }
                }
            }
            foreach (Triangle t1 in t.ListOfFaces)
            {
                foreach (Segment3d t2 in this.ListOfEdges)
                {
                    double temp_dist = t1.DistanceTo(t2);
                    if (temp_dist == 0.0)
                    {
                        return 0;
                    }
                    else if (temp_dist < dist)
                    {
                        dist = temp_dist;
                    }
                }
            }
            return dist;
        }

        #endregion

        #region "Intersection"

        /// <summary>
        /// Check intersection of two tetrahedrons
        /// </summary>
        public bool Intersects(Tetrahedron t)
        {
            if (!this.BoundingBox().Intersects(t.BoundingBox()))
            {
                return false;
            }

            if (t.A.BelongsTo(this) || t.B.BelongsTo(this) || t.C.BelongsTo(this) || t.D.BelongsTo(this))
            {
                return true;
            }

            foreach (Triangle bt in this.ListOfFaces)
            {
                if (bt.Intersects(t)) return true;
            }

            return false;
        }

        /// <summary>
        /// Check intersection of two tetrahedrons
        /// </summary>
        public bool IntersectsFast(Tetrahedron t)
        {
            // From "Intersection of Convex Objects: The Method of Separating Axes"
            // by David Eberly, Geometric Tools, Redmond WA 98052, https://www.geometrictools.com/
            // https://www.geometrictools.com/Documentation/MethodOfSeparatingAxes.pdf
            // Licensed under the Creative Commons Attribution 4.0 International License

            throw new NotImplementedException();
        }

        /// <summary>
        /// Check intersection of tetrahedron with triangle
        /// </summary>
        public bool Intersects(Triangle t)
        {
            if (t.A.BelongsTo(this) || t.B.BelongsTo(this) || t.C.BelongsTo(this))
            {
                return true;
            }

            foreach (Triangle bt in this.ListOfFaces)
            {
                if (bt.Intersects(t)) return true;
            }

            return false;
        }

        /// <summary>
        /// Check intersection of tetrahedron with line
        /// </summary>
        public bool Intersects(Line3d l)
        {
            foreach (Triangle bt in this.ListOfFaces)
            {
                if (l.IntersectionWith(bt) != null) return true;
            }
            return false;
        }

        /// <summary>
        /// Check intersection of tetrahedron with ray
        /// </summary>
        public bool Intersects(Ray3d r)
        {
            foreach (Triangle bt in this.ListOfFaces)
            {
                if (r.IntersectionWith(bt) != null) return true;
            }
            return false;
        }

        /// <summary>
        /// Check intersection of tetrahedron with segment
        /// </summary>
        public bool Intersects(Segment3d s)
        {
            if (s.P1.BelongsTo(this) || s.P2.BelongsTo(this))
            {
                return true;
            }
            foreach (Triangle bt in this.ListOfFaces)
            {
                if (s.IntersectionWith(bt) != null) return true;
            }
            return false;
        }

        /// <summary>
        /// Check intersection of tetrahedron with box
        /// </summary>
        public bool Intersects(Box3d box)
        {
            if (!this.BoundingBox().Intersects(box))
            {
                return false;
            }
            foreach (Triangle face in this.ListOfFaces)
            {
                if (face.Intersects(box)) return true;
            }
            return false;
        }

        /// <summary>
        /// Get intersection of line with tetrahedron.
        /// Returns 'null' (no intersection) or object of type 'Point3d' or 'Segment3d'.
        /// </summary>
        public object IntersectionWith(Line3d l)
        {
            throw new NotImplementedException();
        }

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

        internal int _WhichSide(Tetrahedron tetra, Point3d p, Vector3d d)
        {
            // From "Intersection of Convex Objects: The Method of Separating Axes"
            // by David Eberly, Geometric Tools, Redmond WA 98052, https://www.geometrictools.com/
            // https://www.geometrictools.com/Documentation/MethodOfSeparatingAxes.pdf
            // Licensed under the Creative Commons Attribution 4.0 International License

            int positive = 0;
            int negative = 0;

            for (int i = 0; i <= 3; i++)
            {
                double t = d * (tetra.vertices[i] - p).ToVector;
                if (t>0)
                {
                    positive++;
                }
                if (t<0)
                {
                    negative++;
                }
                if (positive > 0 && negative >0)
                {
                    // this is not separating axis
                    return 0; 
                }
            }

            return positive > 0 ? +1 : -1;
        }


        /// <summary>
        /// Check if tetrahedron is located inside box with tolerance defined by global tolerance property (GeometRi3D.Tolerance).
        /// </summary>
        public bool IsInside(Box3d box)
        {
            // Relative tolerance ================================
            if (!GeometRi3D.UseAbsoluteTolerance)
            {
                double tol = GeometRi3D.Tolerance;
                GeometRi3D.Tolerance = tol * Sqrt(Max(this.A.DistanceSquared(this.D),Max(this.A.DistanceSquared(this.B), this.A.DistanceSquared(this.C))));
                GeometRi3D.UseAbsoluteTolerance = true;
                bool result = this.IsInside(box);
                GeometRi3D.UseAbsoluteTolerance = false;
                GeometRi3D.Tolerance = tol;
                return result;
            }
            //====================================================

            if (!this.A.IsInside(box)) return false;
            if (!this.B.IsInside(box)) return false;
            if (!this.C.IsInside(box)) return false;
            if (!this.D.IsInside(box)) return false;

            return true;
        }


        #region "TranslateRotateReflect"
        /// <summary>
        /// Translate tetrahedron by a vector
        /// </summary>
        public Tetrahedron Translate(Vector3d v)
        {
            return new Tetrahedron(vertices[0].Translate(v), vertices[1].Translate(v), vertices[2].Translate(v), vertices[3].Translate(v));
        }

        /// <summary>
        /// Rotate tetrahedron around point 'p' as a rotation center.
        /// </summary>
        public Tetrahedron Rotate(Rotation r, Point3d p)
        {
            Point3d p1 = r.ToRotationMatrix * (vertices[0] - p) + p;
            Point3d p2 = r.ToRotationMatrix * (vertices[1] - p) + p;
            Point3d p3 = r.ToRotationMatrix * (vertices[2] - p) + p;
            Point3d p4 = r.ToRotationMatrix * (vertices[3] - p) + p;
            return new Tetrahedron(p1, p2, p3, p4);
        }

        /// <summary>
        /// Reflect tetrahedron in given point
        /// </summary>
        public virtual Tetrahedron ReflectIn(Point3d p)
        {
            Point3d p1 = vertices[0].ReflectIn(p);
            Point3d p2 = vertices[1].ReflectIn(p);
            Point3d p3 = vertices[2].ReflectIn(p);
            Point3d p4 = vertices[3].ReflectIn(p);
            return new Tetrahedron(p1, p2, p3, p4);
        }

        /// <summary>
        /// Reflect tetrahedron in given line
        /// </summary>
        public virtual Tetrahedron ReflectIn(Line3d l)
        {
            Point3d p1 = vertices[0].ReflectIn(l);
            Point3d p2 = vertices[1].ReflectIn(l);
            Point3d p3 = vertices[2].ReflectIn(l);
            Point3d p4 = vertices[3].ReflectIn(l);
            return new Tetrahedron(p1, p2, p3, p4);
        }

        /// <summary>
        /// Reflect tetrahedron in given plane
        /// </summary>
        public virtual Tetrahedron ReflectIn(Plane3d s)
        {
            Point3d p1 = vertices[0].ReflectIn(s);
            Point3d p2 = vertices[1].ReflectIn(s);
            Point3d p3 = vertices[2].ReflectIn(s);
            Point3d p4 = vertices[3].ReflectIn(s);
            return new Tetrahedron(p1, p2, p3, p4);
        }

        /// <summary>
        /// Scale tetrahedron
        /// </summary>
        public virtual Tetrahedron Scale(double scale)
        {
            Point3d center = this.Center;
            Point3d p1 = center + scale * (vertices[0] - center);
            Point3d p2 = center + scale * (vertices[1] - center);
            Point3d p3 = center + scale * (vertices[2] - center);
            Point3d p4 = center + scale * (vertices[3] - center);
            return new Tetrahedron(p1, p2, p3, p4);
        }

        /// <summary>
        /// Scale tetrahedron
        /// </summary>
        public virtual Tetrahedron Scale(double scale_x, double scale_y, double scale_z)
        {
            Point3d center = this.Center;
            Matrix3d m = Matrix3d.DiagonalMatrix(scale_x, scale_y, scale_z);
            Point3d p1 = center.Translate(m * (vertices[0] - center).ToVector);
            Point3d p2 = center.Translate(m * (vertices[1] - center).ToVector);
            Point3d p3 = center.Translate(m * (vertices[2] - center).ToVector);
            Point3d p4 = center.Translate(m * (vertices[3] - center).ToVector);
            return new Tetrahedron(p1, p2, p3, p4);
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
            Tetrahedron t = (Tetrahedron)obj;

            if (GeometRi3D.UseAbsoluteTolerance)
            {
                int count = 0;
                foreach (Point3d p1 in this.vertices)
                {
                    foreach (Point3d p2 in t.vertices)
                    {
                        if (p1 == p2)
                        {
                            count++;
                        }
                    }
                }
                if (count == 4) return true;
                return false;
            }
            else
            {
                double tol = GeometRi3D.Tolerance;
                GeometRi3D.Tolerance = tol * this.A.DistanceTo(this.B);
                GeometRi3D.UseAbsoluteTolerance = true;
                bool result = this.Equals(t);
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
            return GeometRi3D.HashFunction(vertices[0].GetHashCode(), vertices[1].GetHashCode(), vertices[2].GetHashCode(), vertices[3].GetHashCode());
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
            Point3d p1 = vertices[0].ConvertTo(coord);
            Point3d p2 = vertices[1].ConvertTo(coord);
            Point3d p3 = vertices[2].ConvertTo(coord);
            Point3d p4 = vertices[3].ConvertTo(coord);

            string str = string.Format("Tetrahedron (reference coord.sys. ") + coord.Name + "):" + nl;
            str += string.Format("A -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", p1.X, p1.Y, p1.Z) + nl;
            str += string.Format("B -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", p2.X, p2.Y, p2.Z) + nl;
            str += string.Format("C -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", p3.X, p3.Y, p3.Z) + nl;
            str += string.Format("D -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", p4.X, p4.Y, p4.Z);
            return str;
        }

    }
}
