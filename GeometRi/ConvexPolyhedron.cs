using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace GeometRi
{
    /// <summary>
    /// Convex polyhedron with counterclockwise oriented faces (seen from outside).
    /// </summary>
    public class ConvexPolyhedron : FiniteObject, IFiniteObject
    {

        public struct Edge
        {
            public Point3d p1, p2;
            public Edge(Point3d P1, Point3d P2)
            {
                p1 = P1;
                p2 = P2;
            }
        }

        public struct Face
        {
            public int numVertices;
            public Point3d[] vertex;
            public Vector3d normal;
            public Face(int numVertices, Point3d[] vertex, Vector3d normal)
            {
                this.numVertices = numVertices;
                this.vertex = vertex;
                this.normal = normal;
            }

            public double Area
            {
                get
                {
                    Vector3d v1, v2;
                    v1 = new Vector3d(vertex[0], vertex[1]);
                    double area = 0;
                    for (int i = 0; i < vertex.Length - 2; i++)
                    {
                        v2 = new Vector3d(vertex[0], vertex[i + 2]);
                        area += v1.Cross(v2).Norm;
                        v1 = v2;
                    }
                    return area / 2;
                }
            }
        }

        public int numVertices, numEdges, numFaces;
        public Point3d[] vertex;
        public Edge[] edge;
        public Face[] face;

        private Box3d _aabb = null;

        #region "Constructors"

        public ConvexPolyhedron(int numVertices, int numEdges, int numFaces, Point3d[] verices, Edge[] edges, Face[] faces)
        {
            this.numVertices = numVertices;
            this.numEdges = numEdges;
            this.numFaces = numFaces;
            this.vertex = verices;
            this.edge = edges;
            this.face = faces;
        }

        public static ConvexPolyhedron FromTetrahedron(Tetrahedron t)
        {
            Point3d[] vertices = new Point3d[4];
            vertices[0] = t.A.Copy();
            vertices[1] = t.B.Copy();
            vertices[2] = t.C.Copy();
            vertices[3] = t.D.Copy();

            Edge[] edges = new Edge[6];
            edges[0] = new Edge(vertices[0], vertices[1]);
            edges[1] = new Edge(vertices[1], vertices[2]);
            edges[2] = new Edge(vertices[2], vertices[0]);
            edges[3] = new Edge(vertices[0], vertices[3]);
            edges[4] = new Edge(vertices[1], vertices[3]);
            edges[5] = new Edge(vertices[2], vertices[3]);

            Face[] faces = new Face[4];
            faces[0] = new Face(3, new Point3d[] { vertices[0], vertices[1], vertices[2] },
                                new Vector3d(vertices[0], vertices[1]).Cross(new Vector3d(vertices[1], vertices[2])).Normalized);
            faces[1] = new Face(3, new Point3d[] { vertices[0], vertices[1], vertices[3] },
                                new Vector3d(vertices[0], vertices[1]).Cross(new Vector3d(vertices[1], vertices[3])).Normalized);
            faces[2] = new Face(3, new Point3d[] { vertices[1], vertices[2], vertices[3] },
                                new Vector3d(vertices[1], vertices[2]).Cross(new Vector3d(vertices[2], vertices[3])).Normalized);
            faces[3] = new Face(3, new Point3d[] { vertices[2], vertices[0], vertices[3] },
                                new Vector3d(vertices[2], vertices[0]).Cross(new Vector3d(vertices[0], vertices[3])).Normalized);

            // Check orientation of faces
            for (int i = 0; i < faces.Length; i++)
            {
                foreach (Point3d p in vertices)
                {
                    Vector3d v = new Vector3d(faces[i].vertex[0], p);
                    if (faces[i].normal * v > GeometRi3D.Tolerance)
                    {
                        // Reverse face
                        faces[i].normal = -faces[i].normal;
                        Point3d tmp = faces[i].vertex[1];
                        faces[i].vertex[1] = faces[i].vertex[2];
                        faces[i].vertex[2] = tmp;
                    }
                }
            }

            return new ConvexPolyhedron(4, 6, 4, vertices, edges, faces);
        }
        #endregion

        #region "Properties"

        /// <summary>
        /// Center of mass.
        /// </summary>
        public Point3d Center
        {
            get
            {
                Point3d center = new Point3d(); ;
                foreach (Point3d p in vertex)
                {
                    center += p;
                }
                return center / vertex.Length;
            }
        }

        /// <summary>
        /// Volume of the polyhedron.
        /// </summary>
        public double Volume
        {
            get
            {
                double volume = 0;
                foreach (Face f in face)
                {
                    for (int i = 0; i < f.vertex.Length - 2; i++)
                    {
                        Vector3d v1 = new Vector3d(this.vertex[0], f.vertex[i]);
                        Vector3d v2 = new Vector3d(this.vertex[0], f.vertex[i + 1]);
                        Vector3d v3 = new Vector3d(this.vertex[0], f.vertex[i + 2]);
                        volume += v1 * (v2.Cross(v3));
                    }
                }
                return volume / 6;
            }
        }

        /// <summary>
        /// Surface area of the polyhedron.
        /// </summary>
        public double Area
        {
            get
            {
                double area = 0;
                foreach (Face f in face)
                {
                    area += f.Area;
                }
                return area;
            }
        }
        #endregion

        /// <summary>
        /// Creates copy of the object
        /// </summary>
        public ConvexPolyhedron Copy()
        {
            Point3d[] vertex_copy = new Point3d[vertex.Length];
            Edge[] edge_copy = new Edge[edge.Length];
            Face[] face_copy = new Face[face.Length];
            Dictionary<Int32, Point3d> dict = new Dictionary<Int32, Point3d>();

            for (int i = 0; i < vertex.Length; i++)
            {
                Point3d tmp = vertex[i].Copy();
                dict[RuntimeHelpers.GetHashCode(vertex[i])] = tmp;
                vertex_copy[i] = tmp;
            }

            for (int i = 0; i < edge.Length; i++)
            {
                edge_copy[i].p1 = dict[RuntimeHelpers.GetHashCode(edge[i].p1)];
            }
            for (int i = 0; i < face.Length; i++)
            {
                for (int j = 0; j < face[i].vertex.Length; j++)
                {
                    face_copy[i].vertex[j] = dict[RuntimeHelpers.GetHashCode(face[i].vertex[j])];
                }
            }
            return new ConvexPolyhedron(numVertices, numEdges, numFaces, vertex_copy, edge_copy, face_copy);
        }


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
                _aabb = Box3d.AABB(vertex, coord);
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
        /// Distance from polyhedron to point (zero will be returned for point located inside box)
        /// </summary>
        public double DistanceTo(Point3d p)
        {
            if (p.BelongsTo(this))
            {
                return 0;
            }

            double dist = double.PositiveInfinity;

            for (int i = 0; i < numFaces; i++)
            {
                // test only visible faces
                if (face[i].normal * new Vector3d(face[i].vertex[0], p) < 0)
                {
                    continue;
                }

                for (int j = 0; j < face[i].vertex.Length - 2; j++)
                {
                    Triangle t = new Triangle(face[i].vertex[0], face[i].vertex[j + 1], face[i].vertex[j + 2]);
                    double tmp_dist = t.DistanceTo(p);
                    if (tmp_dist < dist)
                    {
                        dist = tmp_dist;
                    }
                }

            }
            return dist;
        }

        /// <summary>
        /// Distance from polyhedron to triangle
        /// </summary>
        public double DistanceTo(Triangle t)
        {
            if (t.A.BelongsTo(this) || t.B.BelongsTo(this) || t.C.BelongsTo(this))
            {
                return 0;
            }

            double dist = double.PositiveInfinity;

            for (int i = 0; i < numFaces; i++)
            {
                for (int j = 0; j < face[i].vertex.Length - 2; j++)
                {
                    Triangle tmp = new Triangle(face[i].vertex[0], face[i].vertex[j + 1], face[i].vertex[j + 2]);
                    double tmp_dist = t.DistanceTo(tmp);
                    if (tmp_dist < dist)
                    {
                        dist = tmp_dist;
                    }
                }

            }
            return dist;
        }

        #endregion


        #region "Intersection"

        /// <summary>
        /// Intersection check between two polyhedrons.
        /// </summary>
        public bool Intersects(ConvexPolyhedron c)
        {

            // Algorithm from:
            // Intersection of Convex Objects: The Method of Separating Axes
            // David Eberly, Geometric Tools, Redmond WA 98052
            // Creative Commons Attribution 4.0 International License

            // Test faces of this CP for separation. Because of the counterclockwise ordering,
            // the projection interval for this CP is (-inf, 0].
            // Determine whether 'c' is on the positive side of the line
            for (int i = 0; i < this.numFaces; i++)
            {
                Vector3d P = this.face[i].vertex[0].ToVector;
                Vector3d N = this.face[i].normal;
                if (WhichSide(c.vertex, P, N) > 0)
                {
                    // 'c' is entirely on the positive side of the line P + t * N
                    return false;
                }
            }

            // Test faces 'c' for separation. Because of the counterclockwise ordering,
            // the projection interval for 'c' is (-inf, 0].
            // Determine whether this CP is on the positive side of the line
            for (int i = 0; i < c.numFaces; i++)
            {
                Vector3d P = c.face[i].vertex[0].ToVector;
                Vector3d N = c.face[i].normal;
                if (WhichSide(this.vertex, P, N) > 0)
                {
                    // this CP is entirely on the positive side of the line P + t * N
                    return false;
                }
            }

            // Test cross products of pairs of edge directions
            // one edge direction from each polyhedron
            for (int i = 0; i < this.numEdges; i++)
            {
                Vector3d D0 = new Vector3d(this.edge[i].p1, this.edge[i].p2);
                Vector3d P = this.edge[i].p1.ToVector;
                for (int j = 0; j < c.numEdges; j++)
                {
                    Vector3d D1 = new Vector3d(c.edge[j].p1, c.edge[j].p2);
                    Vector3d N = D0.Cross(D1);

                    if (N.X != 0 && N.Y != 0 && N.Z != 0)
                    {
                        int side0 = WhichSide(this.vertex, P, N);
                        if (side0 == 0)
                        {
                            continue;
                        }
                        int side1 = WhichSide(c.vertex, P, N);
                        if (side1 == 0)
                        {
                            continue;
                        }

                        if (side0 * side1 < 0)
                        {
                            // The projections of this CP and 'c' onto the line P + t * N are on opposite sides of the projection of P.
                            return false;
                        }
                    }
                }
            }

            return true;
        }


        private int WhichSide(Point3d[] vertex, Vector3d P, Vector3d D)
        {
            // The vertices are projected to the form P+t*D.
            // The return value is:
            //     +1 - if all t>0
            //     -1 - if all t<0
            //      0 - otherwise, in which case the line/plane splits the polygon/polyhedron projection

            int positive = 0, negative = 0;

            for (int i = 0; i < vertex.Length; i++)
            {
                // Project vertex onto the line
                double t = D * (vertex[i].ToVector - P);
                if (t > 0)
                {
                    positive++;
                }
                else if (t < 0)
                {
                    negative++;
                }
                if (positive > 0 && negative > 0)
                {
                    return 0;
                }
            }

            return positive > 0 ? 1 : -1;
        }


        /// <summary>
        /// Check if polyhedron is located inside box.
        /// </summary>
        public bool IsInside(Box3d box)
        {
            for (int i = 0; i < this.vertex.Length; i++)
            {
                if (!this.vertex[i].IsInside(box)) return false;
 
            }
            return true;
        }


        #endregion




        internal override int _PointLocation(Point3d p)
        {
            int sum = 0;
            int product = 1;

            foreach (Face f in face)
            {
                int v = _SameSide(f.vertex[0], f.normal, p);
                if (v < 0)
                {
                    return -1; // Point is outside
                }
                sum += v;
                product *= v;
            }

            if (sum == face.Length)
            {
                return 1; // Point is strictly inside tetrahedron
            }
            else
            {
                return 0; // Point is on boundary
            }
        }

        private int _SameSide(Point3d a, Vector3d normal, Point3d p)
        {
            double dot_p = normal * (p - a).ToVector;
            if (GeometRi3D.AlmostEqual(dot_p, 0.0))
            {
                return 0; // Point is on face
            }
            else if (dot_p > 0)
            {
                return 1; // point is on the back side for this face
            }
            else
            {
                return 2;
            }
        }


        #region "TranslateRotateReflect"

        /// <summary>
        /// Translate polyhedron by a vector
        /// </summary>
        public ConvexPolyhedron Translate(Vector3d v)
        {
            Dictionary<Int32, Point3d> dict = new Dictionary<Int32, Point3d>();

            for (int i = 0; i < vertex.Length; i++)
            {
                Point3d tmp = vertex[i].Translate(v);
                dict[RuntimeHelpers.GetHashCode(vertex[i])] = tmp;
                vertex[i] = tmp;
            }

            for (int i = 0; i < edge.Length; i++)
            {
                edge[i].p1 = dict[RuntimeHelpers.GetHashCode(edge[i].p1)];
            }
            for (int i = 0; i < face.Length; i++)
            {
                for (int j = 0; j < face[i].vertex.Length; j++)
                {
                    face[i].vertex[j] = dict[RuntimeHelpers.GetHashCode(face[i].vertex[j])];
                }
            }
            return this;
        }

        /// <summary>
        /// Rotate polyhedron around point 'p' as a rotation center.
        /// </summary>
        public ConvexPolyhedron Rotate(Rotation r, Point3d p)
        {
            Dictionary<Int32, Point3d> dict = new Dictionary<Int32, Point3d>();
            Matrix3d m = r.ToRotationMatrix;

            for (int i = 0; i < vertex.Length; i++)
            {
                Point3d tmp = m * (vertex[i] - p) + p;
                dict[RuntimeHelpers.GetHashCode(vertex[i])] = tmp;
                vertex[i] = tmp;
            }

            for (int i = 0; i < edge.Length; i++)
            {
                edge[i].p1 = dict[RuntimeHelpers.GetHashCode(edge[i].p1)];
            }
            for (int i = 0; i < face.Length; i++)
            {
                face[i].normal = m * face[i].normal;
                for (int j = 0; j < face[i].vertex.Length; j++)
                {
                    face[i].vertex[j] = dict[RuntimeHelpers.GetHashCode(face[i].vertex[j])];
                }
            }
            return this;
        }

        #endregion


    }
}
