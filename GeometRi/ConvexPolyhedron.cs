using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace GeometRi
{
    /// <summary>
    /// Convex polyhedron with counterclockwise oriented faces (seen from outside).
    /// </summary>
#if NET20
    [Serializable]
#endif
    public class ConvexPolyhedron : FiniteObject, IFiniteObject
    {
        public int numVertices, numEdges, numFaces;
        public Point3d[] vertex;
        public Edge[] edge;
        public Face[] face;

        private AABB _aabb = null;
        private List<Segment3d> _list_e = null;


        /// <summary>
        /// Edge of a convex polyhedron.
        /// </summary>
#if NET20
    [Serializable]
#endif
        public struct Edge
        {
            public int p1, p2;
            internal ConvexPolyhedron parent;
            public Edge(int P1, int P2)
            {
                p1 = P1;
                p2 = P2;
                parent = null;
            }

            public Point3d P1
            {
                get
                {
                    return parent.vertex[p1];
                }
            }

            public Point3d P2
            {
                get
                {
                    return parent.vertex[p2];
                }
            }
        }
        public interface IVertex
        {
            Point3d this[int index] { get; }
        }

#if NET20
    [Serializable]
#endif
        public struct Face : IVertex
        {
            public int numVertices;
            public int[] vertex;
            public Vector3d normal;
            internal ConvexPolyhedron parent;
            public Face(int numVertices, int[] vertex)
            {
                this.numVertices = numVertices;
                this.vertex = vertex;
                parent = null;
                normal = null;
            }

            public double Area
            {
                get
                {
                    Vector3d v1, v2;
                    v1 = new Vector3d(parent.vertex[vertex[0]], parent.vertex[vertex[1]]);
                    double area = 0;
                    for (int i = 0; i < vertex.Length - 2; i++)
                    {
                        v2 = new Vector3d(parent.vertex[vertex[0]], parent.vertex[vertex[i + 2]]);
                        area += v1.Cross(v2).Norm;
                        v1 = v2;
                    }
                    return area / 2;
                }
            }

            Point3d IVertex.this[int index]
            {
                get
                {
                    return parent.vertex[vertex[index]];
                }
            }
            /// <summary>
            /// Returns a Point3d object for vertex [i]
            /// </summary>
            public IVertex Vertex
            {
                get
                {
                    return this;
                }
            }

            internal void UpdateNormal()
            {
                normal = new Vector3d(parent.vertex[vertex[0]], parent.vertex[vertex[1]]).Cross(new Vector3d(parent.vertex[vertex[0]], parent.vertex[vertex[2]])).Normalized;
            }
        }



        #region "Constructors"

        /// <summary>
        /// Creates general convex polyhedron from a lists of vertices, edges, and faces
        /// </summary>
        /// <param name="numVertices">Number of vertices</param>
        /// <param name="numEdges">Number of edges</param>
        /// <param name="numFaces">Number of faces</param>
        /// <param name="vertices">List of vertices</param>
        /// <param name="edges">List of edges</param>
        /// <param name="faces">List of faces</param>
        /// <param name="check_face_orientation">Check and invert incorrectly oriented faces</param>
        public ConvexPolyhedron(int numVertices, int numEdges, int numFaces, Point3d[] vertices, Edge[] edges, Face[] faces, bool check_face_orientation = false)
        {
            this.numVertices = numVertices;
            this.numEdges = numEdges;
            this.numFaces = numFaces;
            this.vertex = vertices;
            this.edge = edges;
            this.face = faces;

            // Initialize fields
            for (int i = 0; i < edges.Length; i++)
            {
                edges[i].parent = this;
            }
            for (int i = 0; i < faces.Length; i++)
            {
                faces[i].parent = this;
                faces[i].UpdateNormal();
            }

            if (check_face_orientation)
            {
                CheckFaceOrientation();
            }

        }

        /// <summary>
        /// Create ConvexPolyhedron object from a Tetrahedron object
        /// </summary>
        public static ConvexPolyhedron FromTetrahedron(Tetrahedron t)
        {
            Point3d[] vertices = new Point3d[4];
            vertices[0] = t.A.Copy();
            vertices[1] = t.B.Copy();
            vertices[2] = t.C.Copy();
            vertices[3] = t.D.Copy();

            Edge[] edges = new Edge[6];
            edges[0] = new Edge(0, 1);
            edges[1] = new Edge(1, 2);
            edges[2] = new Edge(2, 0);
            edges[3] = new Edge(0, 3);
            edges[4] = new Edge(1, 3);
            edges[5] = new Edge(2, 3);

            Face[] faces = new Face[4];
            faces[0] = new Face(3, new int[] { 0, 1, 2 });
            faces[1] = new Face(3, new int[] { 0, 1, 3 });
            faces[2] = new Face(3, new int[] { 1, 2, 3 });
            faces[3] = new Face(3, new int[] { 2, 0, 3 });

            ConvexPolyhedron cp = new ConvexPolyhedron(4, 6, 4, vertices, edges, faces);
            cp.CheckFaceOrientation();

            return cp;
        }

        /// <summary>
        /// Create ConvexPolyhedron object from a Box3d object
        /// </summary>
        public static ConvexPolyhedron FromBox(Box3d box)
        {
            Point3d[] vertices = new Point3d[8];
            vertices[0] = box.P1.Copy();
            vertices[1] = box.P2.Copy();
            vertices[2] = box.P3.Copy();
            vertices[3] = box.P4.Copy();
            vertices[4] = box.P5.Copy();
            vertices[5] = box.P6.Copy();
            vertices[6] = box.P7.Copy();
            vertices[7] = box.P8.Copy();

            Edge[] edges = new Edge[12];
            edges[0] = new Edge(0, 1);
            edges[1] = new Edge(1, 2);
            edges[2] = new Edge(2, 3);
            edges[3] = new Edge(3, 0);
            
            edges[4] = new Edge(4, 5);
            edges[5] = new Edge(5, 6);
            edges[6] = new Edge(6, 7);
            edges[7] = new Edge(7, 4);
            
            edges[8] = new Edge(0, 4);
            edges[9] = new Edge(1, 5);
            edges[10] = new Edge(2, 6);
            edges[11] = new Edge(3, 7);

            Face[] faces = new Face[6];
            faces[0] = new Face(4, new int[] { 0, 3, 2, 1 });
            faces[1] = new Face(4, new int[] { 4, 5, 6, 7 });
            faces[2] = new Face(4, new int[] { 0, 1, 5, 4 });
            faces[3] = new Face(4, new int[] { 2, 3, 7, 6 });
            faces[4] = new Face(4, new int[] { 1, 2, 6, 5 });
            faces[5] = new Face(4, new int[] { 0, 4, 7, 3 });

            return new ConvexPolyhedron(8, 12, 6, vertices, edges, faces);
        }

        /// <summary>
        /// Creates regular octahedron centered at origin with vertices:
        /// <para>(±1,  0,  0)</para>
        /// <para>( 0, ±1,  0)</para>
        /// <para>( 0,  0, ±1)</para>
        /// </summary>
        public static ConvexPolyhedron Octahedron()
        {
            Point3d[] vertices = new Point3d[6];
            vertices[0] = new Point3d(1, 0, 0);
            vertices[1] = new Point3d(-1, 0, 0);
            vertices[2] = new Point3d(0, 1, 0);
            vertices[3] = new Point3d(0, -1, 0);
            vertices[4] = new Point3d(0, 0, 1);
            vertices[5] = new Point3d(0, 0, -1);

            Edge[] edges = new Edge[12];
            edges[0] = new Edge(0, 2);
            edges[1] = new Edge(2, 1);
            edges[2] = new Edge(1, 3);
            edges[3] = new Edge(3, 0);

            edges[4] = new Edge(0, 4);
            edges[5] = new Edge(2, 4);
            edges[6] = new Edge(1, 4);
            edges[7] = new Edge(3, 4);

            edges[8] = new Edge(0, 5);
            edges[9] = new Edge(2, 5);
            edges[10] = new Edge(1, 5);
            edges[11] = new Edge(3, 5);

            Face[] faces = new Face[8];
            faces[0] = new Face(3, new int[] { 0, 2, 4 });
            faces[1] = new Face(3, new int[] { 2, 1, 4 });
            faces[2] = new Face(3, new int[] { 1, 3, 4 });
            faces[3] = new Face(3, new int[] { 3, 0, 4 });
            faces[4] = new Face(3, new int[] { 0, 3, 5 });
            faces[5] = new Face(3, new int[] { 3, 1, 5 });
            faces[6] = new Face(3, new int[] { 1, 2, 5 });
            faces[7] = new Face(3, new int[] { 2, 0, 5 });

            return new ConvexPolyhedron(6, 12, 8, vertices, edges, faces);
        }


        /// <summary>
        /// Creates regular icosahedron centered at origin with vertices:
        /// <para>( 0, ±f, ±1)</para>
        /// <para>(±f, ±1,  0)</para>
        /// <para>(±1,  0, ±f)</para>
        /// <para>with 'f' equal to golden ratio (1+Sqrt(5))/2</para>
        /// </summary>
        public static ConvexPolyhedron Icosahedron()
        {
            double f = (1 + Math.Sqrt(5)) / 2;

            Point3d[] vertices = new Point3d[12];
            vertices[0] = new Point3d(0, f, 1);
            vertices[1] = new Point3d(0, f, -1);
            vertices[2] = new Point3d(0, -f, 1);
            vertices[3] = new Point3d(0, -f, -1);

            vertices[4] = new Point3d(f, 1, 0);
            vertices[5] = new Point3d(f, -1, 0);
            vertices[6] = new Point3d(-f, 1, 0);
            vertices[7] = new Point3d(-f, -1, 0);

            vertices[8] = new Point3d(1, 0, f);
            vertices[9] = new Point3d(-1, 0, f);
            vertices[10] = new Point3d(1, 0, -f);
            vertices[11] = new Point3d(-1, 0, -f);

            Edge[] edges = new Edge[30];
            edges[0] = new Edge(0, 1);
            edges[1] = new Edge(0, 4);
            edges[2] = new Edge(0, 6);
            edges[3] = new Edge(0, 8);
            edges[4] = new Edge(0, 9);

            edges[5] = new Edge(1, 4);
            edges[6] = new Edge(1, 6);
            edges[7] = new Edge(1, 10);
            edges[8] = new Edge(1, 11);

            edges[9] = new Edge(2, 3);
            edges[10] = new Edge(2, 5);
            edges[11] = new Edge(2, 7);
            edges[12] = new Edge(2, 8);
            edges[13] = new Edge(2, 9);

            edges[14] = new Edge(3, 5);
            edges[15] = new Edge(3, 7);
            edges[16] = new Edge(3, 10);
            edges[17] = new Edge(3, 11);

            edges[18] = new Edge(4, 5);
            edges[19] = new Edge(4, 8);
            edges[20] = new Edge(4, 10);

            edges[21] = new Edge(5, 8);
            edges[22] = new Edge(5, 10);

            edges[23] = new Edge(6, 7);
            edges[24] = new Edge(6, 9);
            edges[25] = new Edge(6, 11);

            edges[26] = new Edge(7, 9);
            edges[27] = new Edge(7, 11);

            edges[28] = new Edge(8, 9);
            edges[29] = new Edge(10, 11);

            Face[] faces = new Face[20];
            faces[0] = new Face(3, new int[] { 0, 4, 1 });
            faces[1] = new Face(3, new int[] { 0, 1, 6 });
            faces[2] = new Face(3, new int[] { 0, 6, 9 });
            faces[3] = new Face(3, new int[] { 0, 9, 8 });
            faces[4] = new Face(3, new int[] { 0, 8, 4 });

            faces[5] = new Face(3, new int[] { 1, 4, 10 });
            faces[6] = new Face(3, new int[] { 1, 10, 11 });
            faces[7] = new Face(3, new int[] { 1, 11, 6 });

            faces[8] = new Face(3, new int[] { 2, 3, 5 });
            faces[9] = new Face(3, new int[] { 2, 5, 8 });
            faces[10] = new Face(3, new int[] { 2, 8, 9 });
            faces[11] = new Face(3, new int[] { 2, 9, 7 });
            faces[12] = new Face(3, new int[] { 2, 7, 3 });

            faces[13] = new Face(3, new int[] { 3, 10, 5 });
            faces[14] = new Face(3, new int[] { 3, 7, 11 });
            faces[15] = new Face(3, new int[] { 3, 11, 10 });

            faces[16] = new Face(3, new int[] { 4, 8, 5 });
            faces[17] = new Face(3, new int[] { 4, 5, 10 });
            faces[18] = new Face(3, new int[] { 6, 7, 9 });
            faces[19] = new Face(3, new int[] { 6, 11, 7 });

            return new ConvexPolyhedron(12, 30, 20, vertices, edges, faces);
        }

        /// <summary>
        /// Creates regular dodecahedron centered at origin with vertices:
        /// <para>(±1, ±1, ±1)</para>
        /// <para>( 0, ±φ, ±1/φ)</para>
        /// <para>(±φ, ±1/φ,  0)</para> 
        /// <para>(±1/φ,  0, ±φ)</para>
        /// <para>with 'φ' equal to golden ratio (1+Sqrt(5))/2</para>
        /// </summary>
        public static ConvexPolyhedron Dodecahedron()
        {
            double f = (1 + Math.Sqrt(5)) / 2;

            Point3d[] vertices = new Point3d[20];
            vertices[0] = new Point3d(-1, 1, 1);
            vertices[1] = new Point3d(-1, -1, 1);
            vertices[2] = new Point3d(1, -1, 1);
            vertices[3] = new Point3d(1, 1, 1);
            vertices[4] = new Point3d(-1, 1, -1);
            vertices[5] = new Point3d(-1, -1, -1);
            vertices[6] = new Point3d(1, -1, -1);
            vertices[7] = new Point3d(1, 1, -1);

            vertices[8] = new Point3d(0, f, 1 / f);
            vertices[9] = new Point3d(0, -f, 1 / f);
            vertices[10] = new Point3d(0, f, -1 / f);
            vertices[11] = new Point3d(0, -f, -1 / f);

            vertices[12] = new Point3d(f, 1 / f, 0);
            vertices[13] = new Point3d(f, -1 / f, 0);
            vertices[14] = new Point3d(-f, 1 / f, 0);
            vertices[15] = new Point3d(-f, -1 / f, 0);

            vertices[16] = new Point3d(1 / f, 0, f);
            vertices[17] = new Point3d(-1 / f, 0, f);
            vertices[18] = new Point3d(1 / f, 0, -f);
            vertices[19] = new Point3d(-1 / f, 0, -f);

            Edge[] edges = new Edge[30];
            edges[0] = new Edge(0, 8);
            edges[1] = new Edge(0, 14);
            edges[2] = new Edge(0, 17);
            edges[3] = new Edge(1, 9);
            edges[4] = new Edge(1, 15);
            edges[5] = new Edge(1, 17);
            edges[6] = new Edge(2, 9);
            edges[7] = new Edge(2, 13);
            edges[8] = new Edge(2, 16);
            edges[9] = new Edge(3, 8);
            edges[10] = new Edge(3, 12);
            edges[11] = new Edge(3, 16);

            edges[12] = new Edge(4, 10);
            edges[13] = new Edge(4, 14);
            edges[14] = new Edge(4, 19);
            edges[15] = new Edge(5, 11);
            edges[16] = new Edge(5, 15);
            edges[17] = new Edge(5, 19);
            edges[18] = new Edge(6, 11);
            edges[19] = new Edge(6, 13);
            edges[20] = new Edge(6, 18);
            edges[21] = new Edge(7, 10);
            edges[22] = new Edge(7, 12);
            edges[23] = new Edge(7, 18);

            edges[24] = new Edge(8, 10);
            edges[25] = new Edge(9, 11);
            edges[26] = new Edge(12, 13);
            edges[27] = new Edge(14, 15);
            edges[28] = new Edge(16, 17);
            edges[29] = new Edge(18, 19);

            Face[] faces = new Face[12];
            faces[0] = new Face(5, new int[] { 0, 14, 15, 1, 17 });
            faces[1] = new Face(5, new int[] { 1, 15, 5, 11, 9 });
            faces[2] = new Face(5, new int[] { 1, 9, 2, 16, 17 });
            faces[3] = new Face(5, new int[] { 2, 9, 11, 6, 13 });

            faces[4] = new Face(5, new int[] { 0, 17, 16, 3, 8 });
            faces[5] = new Face(5, new int[] { 2, 13, 12, 3, 16 });
            faces[6] = new Face(5, new int[] { 0, 8, 10, 4, 14 });
            faces[7] = new Face(5, new int[] { 3, 12, 7, 10, 8 });
            faces[8] = new Face(5, new int[] { 4, 10, 7, 18, 19 });
            faces[9] = new Face(5, new int[] { 4, 19, 5, 15, 14 });
            faces[10] = new Face(5, new int[] { 6, 18, 7, 12, 13 });
            faces[11] = new Face(5, new int[] { 5, 19, 18, 6, 11 });


            return new ConvexPolyhedron(20, 30, 12, vertices, edges, faces);
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
                        Vector3d v1 = new Vector3d(this.vertex[0], f.Vertex[0]);
                        Vector3d v2 = new Vector3d(this.vertex[0], f.Vertex[i + 1]);
                        Vector3d v3 = new Vector3d(this.vertex[0], f.Vertex[i + 2]);
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

        /// <summary>
        /// List of edges forming the polyhedron
        /// </summary>
        public List<Segment3d> ListOfEdges
        {
            get
            {
                if (_list_e == null)
                {
                    _list_e = new List<Segment3d> { };
                    foreach (Edge e in edge)
                    {
                        _list_e.Add(new Segment3d(e.P1, e.P2));
                    }
                    return _list_e;
                }
                else
                {
                    return _list_e;
                }
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
                vertex_copy[i] = vertex[i].Copy();
            }

            for (int i = 0; i < edge.Length; i++)
            {
                edge_copy[i].p1 = edge[i].p1;
                edge_copy[i].p2 = edge[i].p2;
            }
            for (int i = 0; i < face.Length; i++)
            {
                face_copy[i].normal = face[i].normal;
                face_copy[i].numVertices = face[i].numVertices;
                face_copy[i].vertex = new int[face[i].vertex.Length];

                for (int j = 0; j < face[i].vertex.Length; j++)
                {
                    face_copy[i].vertex[j] = face[i].vertex[j];
                }
            }
            return new ConvexPolyhedron(numVertices, numEdges, numFaces, vertex_copy, edge_copy, face_copy);
        }

        private void CheckFaceOrientation()
        {
            for (int i = 0; i < face.Length; i++)
            {
                Vector3d normal = face[i].normal;
                Vector3d to_center = new Vector3d(face[i].Vertex[0], this.Center);
                if (to_center * normal > 0)
                {
                    face[i] = ReverseFace(face[i]);
                }
            }
        }

        private Face ReverseFace(Face face)
        {
            int[] tmp = new int[face.numVertices];
            for (int j = 0; j < face.numVertices; j++)
            {
                tmp[j] = face.vertex[face.numVertices - j - 1];
            }
            face.vertex = tmp;
            face.UpdateNormal();
            return face;
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
        /// Return Bounding Box in given coordinate system.
        /// </summary>
        public Box3d BoundingBox(Coord3d coord = null)
        {
            return Box3d.BoundingBox(vertex, coord);
        }

        /// <summary>
        /// Return Axis Aligned Bounding Box (AABB).
        /// </summary>
        public AABB AABB()
        {
            if (_aabb == null)
            {
                _aabb = GeometRi.AABB.BoundingBox(vertex);
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
        /// Distance from polyhedron to point (zero will be returned for point located inside polyhedron)
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
                if (face[i].normal * new Vector3d(face[i].Vertex[0], p) < 0)
                {
                    continue;
                }

                for (int j = 0; j < face[i].vertex.Length - 2; j++)
                {
                    Triangle t = new Triangle(face[i].Vertex[0], face[i].Vertex[j + 1], face[i].Vertex[j + 2]);
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
        /// Distance between two polyhedrons
        /// </summary>
        public double DistanceTo_old(ConvexPolyhedron cp)
        {
            if (this.Intersects(cp))
            {
                return 0;
            }
            Point3d p1, p2;
            return _distance_to_not_intersecting(cp, out p1, out p2);
        }

        /// <summary>
        /// Distance between two polyhedrons
        /// </summary>
        /// <param name="c">Target polyhedron</param>
        public double DistanceTo(ConvexPolyhedron c)
        {
            // Use "Method of Separating Axes" to test intersection combined with distance calculation

            // Intersection of Convex Objects: The Method of Separating Axes
            // David Eberly, Geometric Tools, Redmond WA 98052
            // Creative Commons Attribution 4.0 International License

            double dist = double.PositiveInfinity;
            bool intersecting = true;

            // Test faces of this CP for separation. Because of the counterclockwise ordering,
            // the projection interval for this CP is (-inf, 0].
            // Determine whether 'c' is on the positive side of the line
            for (int i = 0; i < this.numFaces; i++)
            {
                Vector3d P = this.face[i].Vertex[0].ToVector;
                Vector3d N = this.face[i].normal;
                if (WhichSide(c.vertex, P, N) > 0)
                {
                    // 'c' is entirely on the positive side of the line P + t * N
                    // Calculate min projection distance to face's plane
                    intersecting = false;
                    double square_proj_dist = double.PositiveInfinity;
                    Point3d best_proj_point = this.face[i].Vertex[0];

                    Plane3d plane = new Plane3d(this.face[i].Vertex[0], this.face[i].normal);
                    foreach (Point3d point in c.vertex)
                    {
                        Point3d projection = point.ProjectionTo(plane);
                        double tmp_dist = projection.DistanceSquared(point);
                        if (tmp_dist < square_proj_dist)
                        {
                            square_proj_dist = tmp_dist;
                            best_proj_point = projection;
                        }
                    }
                    // test if best projection of c.vertex is inside the face
                    bool inside = true;
                    for (int l = 0; l < this.face[i].vertex.Length; l++)
                    {
                        Vector3d edge = new Vector3d(this.face[i].Vertex[l], this.face[i].Vertex[0]);
                        if (l < this.face[i].vertex.Length - 1)
                        {
                            edge = new Vector3d(this.face[i].Vertex[l], this.face[i].Vertex[l + 1]);
                        }
                        Vector3d v = new Vector3d(this.face[i].Vertex[l], best_proj_point);
                        if (edge.Cross(v).Dot(this.face[i].normal) < 0)
                        {
                            // projection ourside of face
                            inside = false;
                            break;
                        }

                    }
                    if (inside)
                    {
                        double tmp_dist = Math.Sqrt(square_proj_dist);
                        if (tmp_dist < dist) { dist = tmp_dist; }
                    }
                }
            }

            // Test faces 'c' for separation. Because of the counterclockwise ordering,
            // the projection interval for 'c' is (-inf, 0].
            // Determine whether this CP is on the positive side of the line
            for (int i = 0; i < c.numFaces; i++)
            {
                Vector3d P = c.face[i].Vertex[0].ToVector;
                Vector3d N = c.face[i].normal;
                if (WhichSide(this.vertex, P, N) > 0)
                {
                    // this CP is entirely on the positive side of the line P + t * N
                    // Calculate min projection distance to face's plane
                    intersecting = false;
                    double square_proj_dist = double.PositiveInfinity;
                    Point3d best_proj_point = c.face[i].Vertex[0];

                    Plane3d plane = new Plane3d(c.face[i].Vertex[0], c.face[i].normal);
                    foreach (Point3d point in this.vertex)
                    {
                        Point3d projection = point.ProjectionTo(plane);
                        double tmp_dist = projection.DistanceSquared(point);
                        if (tmp_dist < square_proj_dist)
                        {
                            square_proj_dist = tmp_dist;
                            best_proj_point = projection;
                        }
                    }
                    // test if best projection of c.vertex is inside the face
                    bool inside = true;
                    for (int l = 0; l < c.face[i].vertex.Length; l++)
                    {
                        Vector3d edge = new Vector3d(c.face[i].Vertex[l], c.face[i].Vertex[0]);
                        if (l < c.face[i].vertex.Length - 1)
                        {
                            edge = new Vector3d(c.face[i].Vertex[l], c.face[i].Vertex[l + 1]);
                        }
                        Vector3d v = new Vector3d(c.face[i].Vertex[l], best_proj_point);
                        if (edge.Cross(v).Dot(c.face[i].normal) < 0)
                        {
                            // projection ourside of face
                            inside = false;
                            break;
                        }

                    }
                    if (inside)
                    {
                        double tmp_dist = Math.Sqrt(square_proj_dist);
                        if (tmp_dist < dist) { dist = tmp_dist; }
                    }

                }
            }

            // Test cross products of pairs of edge directions
            // one edge direction from each polyhedron

            for (int i = 0; i < this.numEdges; i++)
            {
                Vector3d D0 = new Vector3d(this.edge[i].P1, this.edge[i].P2);
                Vector3d P = this.edge[i].P1.ToVector;
                Segment3d s1 = new Segment3d(this.vertex[this.edge[i].p1], this.vertex[this.edge[i].p2]);
                for (int j = 0; j < c.numEdges; j++)
                {
                    Vector3d D1 = new Vector3d(c.edge[j].P1, c.edge[j].P2);
                    Vector3d N = D0.Cross(D1);
                    Segment3d s2 = new Segment3d(c.vertex[c.edge[j].p1], c.vertex[c.edge[j].p2]);

                    if (N.X != 0 || N.Y != 0 || N.Z != 0)
                    {
                        int side0 = WhichSide(this.vertex, P, N, 1e-16);
                        if (side0 == 0)
                        {
                            continue;
                        }
                        int side1 = WhichSide(c.vertex, P, N, 1e-16);
                        if (side1 == 0)
                        {
                            continue;
                        }

                        if (side0 * side1 < 0)
                        {
                            // The projections of this CP and 'c' onto the line P + t * N are on opposite sides of the projection of P.
                            intersecting = false;
                            double tmp_dist = s1.DistanceTo(s2);
                            if (tmp_dist < dist) {  dist = tmp_dist; }
                        }
                    }
                }
            }

            if (intersecting)
            {
                return 0;
            }
            else
            {
                return dist;
            }
        }

        /// <summary>
        /// Distance between two polyhedrons
        /// <para> The output points may be not unique in case of intersecting objects.</para>
        /// </summary>
        /// <param name="cp">Target polyhedron</param>
        /// <param name="point_on_this_cp">Closest point on this convex polyhedron</param>
        /// <param name="point_on_target_cp">Closest point on target convex polyhedron</param>
        public double DistanceTo(ConvexPolyhedron cp, out Point3d point_on_this_cp, out Point3d point_on_target_cp)
        {
            if (this.Intersects(cp))
            {
                // check if one CP is inside other CP
                Point3d c1 = this.Center;
                point_on_this_cp = c1;
                point_on_target_cp = c1;
                if (c1.BelongsTo(cp))
                {
                    return 0;
                }
                Point3d c2 = cp.Center;
                if (c2.BelongsTo(this))
                {
                    point_on_this_cp = c2;
                    point_on_target_cp = c2;
                    return 0;
                }

                // If CPs are partially intersecting, return any common point
                Point3d common_point = _get_common_point(cp);
                if (common_point != null)
                {
                    point_on_this_cp = common_point;
                    point_on_target_cp = common_point;
                    return 0;
                }
            }

            return _distance_to_not_intersecting(cp, out point_on_this_cp, out point_on_target_cp);
        }

        internal double _distance_to_not_intersecting(ConvexPolyhedron cp, out Point3d point_on_this_cp, out Point3d point_on_target_cp)
        {
            double dist = double.PositiveInfinity;
            Point3d c1 = new Point3d();
            Point3d c2 = new Point3d();
            point_on_this_cp = c1;
            point_on_target_cp = c1;

            // test vertices of this cp
            for (int i = 0; i < numVertices; i++)
            {
                for (int k = 0; k < cp.numFaces; k++)
                {
                    for (int l = 0; l < cp.face[k].vertex.Length - 2; l++)
                    {
                        Triangle t2 = new Triangle(cp.face[k].Vertex[0], cp.face[k].Vertex[l + 1], cp.face[k].Vertex[l + 2]);

                        double tmp_dist = vertex[i].DistanceTo(t2, out c1);
                        if (tmp_dist < dist)
                        {
                            point_on_this_cp = vertex[i];
                            point_on_target_cp = c1;
                            dist = tmp_dist;
                        }
                    }

                }
            }

            // test vertices of target cp
            for (int i = 0; i < cp.numVertices; i++)
            {
                for (int k = 0; k < this.numFaces; k++)
                {
                    for (int l = 0; l < this.face[k].vertex.Length - 2; l++)
                    {
                        Triangle t2 = new Triangle(this.face[k].Vertex[0], this.face[k].Vertex[l + 1], this.face[k].Vertex[l + 2]);

                        double tmp_dist = cp.vertex[i].DistanceTo(t2, out c1);
                        if (tmp_dist < dist)
                        {
                            point_on_this_cp = c1;
                            point_on_target_cp = cp.vertex[i];
                            dist = tmp_dist;
                        }
                    }

                }
            }

            // test edges
            for (int i = 0; i < this.numEdges; i++)
            {
                Segment3d s1 = new Segment3d(this.vertex[this.edge[i].p1], this.vertex[this.edge[i].p2]);
                for (int j = 0; j < cp.numEdges; j++)
                {
                    Segment3d s2 = new Segment3d(cp.vertex[cp.edge[j].p1], cp.vertex[cp.edge[j].p2]);
                    double tmp_dist = s1.DistanceTo(s2, out c1, out c2);
                    if (tmp_dist < dist)
                    {
                        point_on_this_cp = c1;
                        point_on_target_cp = c2;
                        dist = tmp_dist;
                    }
                }
            }

            return dist;
        }

        internal Point3d _get_common_point(ConvexPolyhedron cp)
        {
            // test edges of this cp
            for (int i = 0; i < this.numEdges; i++)
            {
                Segment3d s1 = new Segment3d(this.vertex[this.edge[i].p1], this.vertex[this.edge[i].p2]);
                for (int k = 0; k < cp.numFaces; k++)
                {
                    for (int l = 0; l < cp.face[k].vertex.Length - 2; l++)
                    {
                        Triangle t2 = new Triangle(cp.face[k].Vertex[0], cp.face[k].Vertex[l + 1], cp.face[k].Vertex[l + 2]);

                        Point3d p1, p2;
                        double tmp_dist = s1.DistanceTo(t2, out p1, out p2);
                        if (tmp_dist == 0)
                        {
                            return p1;
                        }
                    }

                }
            }

            // test edges of target cp
            for (int i = 0; i < cp.numEdges; i++)
            {
                Segment3d s1 = new Segment3d(cp.vertex[cp.edge[i].p1], cp.vertex[cp.edge[i].p2]);
                for (int k = 0; k < this.numFaces; k++)
                {
                    for (int l = 0; l < this.face[k].vertex.Length - 2; l++)
                    {
                        Triangle t2 = new Triangle(this.face[k].Vertex[0], this.face[k].Vertex[l + 1], this.face[k].Vertex[l + 2]);

                        Point3d p1, p2;
                        double tmp_dist = s1.DistanceTo(t2, out p1, out p2);
                        if (tmp_dist == 0)
                        {
                            return p1;
                        }
                    }

                }
            }

            return null;
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
                    Triangle tmp = new Triangle(face[i].Vertex[0], face[i].Vertex[j + 1], face[i].Vertex[j + 2]);
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
        internal bool Intersects_Slow(ConvexPolyhedron cp)
        {
            Point3d c1 = this.Center;
            Point3d c2 = cp.Center;

            if (c1.BelongsTo(cp) || c2.BelongsTo(this))
            {
                return true;
            }

            for (int i = 0; i < numFaces; i++)
            {
                // test only visible faces
                //if (face[i].normal * new Vector3d(face[i].vertex[0], c2) < 0)
                //{
                //    continue;
                //}

                for (int j = 0; j < face[i].vertex.Length - 2; j++)
                {
                    Triangle t1 = new Triangle(face[i].Vertex[0], face[i].Vertex[j + 1], face[i].Vertex[j + 2]);

                    for (int k = 0; k < cp.numFaces; k++)
                    {
                        // test only visible faces
                        //if (cp.face[k].normal * new Vector3d(cp.face[k].vertex[0], c1) < 0)
                        //{
                        //    continue;
                        //}

                        for (int l = 0; l < cp.face[k].vertex.Length - 2; l++)
                        {
                            Triangle t2 = new Triangle(cp.face[k].Vertex[0], cp.face[k].Vertex[l + 1], cp.face[k].Vertex[l + 2]);

                            if (t1.Intersects(t2))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

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
                Vector3d P = this.face[i].Vertex[0].ToVector;
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
                Vector3d P = c.face[i].Vertex[0].ToVector;
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
                Vector3d D0 = new Vector3d(this.edge[i].P1, this.edge[i].P2);
                Vector3d P = this.edge[i].P1.ToVector;
                for (int j = 0; j < c.numEdges; j++)
                {
                    Vector3d D1 = new Vector3d(c.edge[j].P1, c.edge[j].P2);
                    Vector3d N = D0.Cross(D1);

                    if (N.X != 0 || N.Y != 0 || N.Z != 0)
                    {
                        int side0 = WhichSide(this.vertex, P, N, 1e-16);
                        if (side0 == 0)
                        {
                            continue;
                        }
                        int side1 = WhichSide(c.vertex, P, N, 1e-16);
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


        private int WhichSide(Point3d[] vertex, Vector3d P, Vector3d D, double tolerance = 0)
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
                if (t > tolerance)
                {
                    positive++;
                }
                else if (t < -tolerance)
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

        /// <summary>
        /// Check if polyhedron intersects box.
        /// </summary>
        public bool Intersects(Box3d box)
        {
            ConvexPolyhedron cp = ConvexPolyhedron.FromBox(box);
            return this.Intersects(cp);
        }


        #endregion




        internal override int _PointLocation(Point3d p)
        {
            int sum = 0;
            int product = 1;

            foreach (Face f in face)
            {
                int v = _SameSide(f.Vertex[0], f.normal, p);
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
            double dot_p = normal * new Vector3d(p, a);
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
                return -2;
            }
        }


        #region "TranslateRotateReflect"

        /// <summary>
        /// Translate polyhedron by a vector
        /// </summary>
        public ConvexPolyhedron Translate(Vector3d v)
        {
            for (int i = 0; i < vertex.Length; i++)
            {
                vertex[i] = vertex[i].Translate(v);
            }

            _list_e = null;
            _aabb = null;
            return this;
        }

        /// <summary>
        /// Rotate polyhedron around point 'p' as a rotation center.
        /// </summary>
        public ConvexPolyhedron Rotate(Rotation r, Point3d p)
        {
            Matrix3d m = r.ToRotationMatrix;

            for (int i = 0; i < vertex.Length; i++)
            {
                vertex[i] = m * (vertex[i] - p) + p;
            }

            for (int i = 0; i < face.Length; i++)
            {
                face[i].normal = m * face[i].normal;
            }
            _list_e = null;
            _aabb = null;
            return this;
        }

        /// <summary>
        /// Scale polyhedron relative to center point
        /// </summary>
        public ConvexPolyhedron Scale(double scale)
        {
            return Scale(scale, this.Center);
        }

        /// <summary>
        /// Scale polyhedron relative to given point
        /// </summary>
        public ConvexPolyhedron Scale(double scale, Point3d scaling_center)
        {
            for (int i = 0; i < vertex.Length; i++)
            {
                vertex[i] = scaling_center + scale * (vertex[i] - scaling_center);
            }

            _list_e = null;
            _aabb = null;
            return this;
        }

        /// <summary>
        /// Non-uniform scaling of polyhedron relative to given point
        /// </summary>
        public virtual ConvexPolyhedron Scale(double scale_x, double scale_y, double scale_z, Point3d scaling_center)
        {
            Point3d center = scaling_center;
            Matrix3d m = Matrix3d.DiagonalMatrix(scale_x, scale_y, scale_z);
            Matrix3d m_1 = Matrix3d.DiagonalMatrix(1.0 / scale_x, 1.0 / scale_y, 1.0 / scale_z);

            for (int i = 0; i < vertex.Length; i++)
            {
                vertex[i] = center.Translate(m * (vertex[i] - center).ToVector); ;
            }

            for (int i = 0; i < face.Length; i++)
            {
                face[i].normal = (Vector3d)(m_1 * face[i].normal).Normalized;
            }
            _list_e = null;
            _aabb = null;
            return this;
        }

        /// <summary>
        /// Non-uniform scaling of polyhedron relative to center point
        /// </summary>
        public virtual ConvexPolyhedron Scale(double scale_x, double scale_y, double scale_z)
        {
            return Scale(scale_x, scale_y, scale_z, this.Center);
        }

        #endregion


    }
}
