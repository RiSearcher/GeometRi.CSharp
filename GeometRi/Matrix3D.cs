using System;
using static System.Math;

namespace GeometRi
{
    /// <summary>
    /// General 3x3 matrix class.
    /// </summary>
    public class Matrix3d
    {


        private double[,] val;

        /// <summary>
        /// Default constructor, intializes zero matrix.
        /// </summary>
        public Matrix3d()
        {
            val = new double[3, 3];
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    this.val[i, j] = 0.0;
                }
            }
        }
        public Matrix3d(Vector3d row1, Vector3d row2, Vector3d row3)
        {
            val = new double[3, 3];
            Row1 = row1;
            Row2 = row2;
            Row3 = row3;
        }
        public Matrix3d(double[] row1, double[] row2, double[] row3)
        {
            val = new double[3, 3];
            Row1 = new Vector3d(row1);
            Row2 = new Vector3d(row2);
            Row3 = new Vector3d(row3);
        }

        public static Matrix3d Identity()
        {
            Matrix3d I = new Matrix3d();
            I[0, 0] = 1.0;
            I[1, 1] = 1.0;
            I[2, 2] = 1.0;
            return I;
        }

        public static Matrix3d DiagonalMatrix(double a11, double a22, double a33)
        {
            Matrix3d D = new Matrix3d();
            D[0, 0] = a11;
            D[1, 1] = a22;
            D[2, 2] = a33;
            return D;
        }

        /// <summary>
        /// Creates copy of the object
        /// </summary>
        public Matrix3d Copy()
        {
            return new Matrix3d(this.Row1, this.Row2, this.Row3);
        }

        public double this[int i, int j]
        {
            get { return this.val[i, j]; }
            set { this.val[i, j] = value; }
        }

        public Vector3d Row1
        {
            get { return new Vector3d(this.val[0, 0], this.val[0, 1], this.val[0, 2]); }
            set
            {
                this.val[0, 0] = value.X;
                this.val[0, 1] = value.Y;
                this.val[0, 2] = value.Z;
            }
        }
        public Vector3d Row2
        {
            get { return new Vector3d(this.val[1, 0], this.val[1, 1], this.val[1, 2]); }
            set
            {
                this.val[1, 0] = value.X;
                this.val[1, 1] = value.Y;
                this.val[1, 2] = value.Z;
            }
        }
        public Vector3d Row3
        {
            get { return new Vector3d(this.val[2, 0], this.val[2, 1], this.val[2, 2]); }
            set
            {
                this.val[2, 0] = value.X;
                this.val[2, 1] = value.Y;
                this.val[2, 2] = value.Z;
            }
        }

        public Vector3d Column1
        {
            get { return new Vector3d(this.val[0, 0], this.val[1, 0], this.val[2, 0]); }
            set
            {
                this.val[0, 0] = value.X;
                this.val[1, 0] = value.Y;
                this.val[2, 0] = value.Z;
            }
        }
        public Vector3d Column2
        {
            get { return new Vector3d(this.val[0, 1], this.val[1, 1], this.val[2, 1]); }
            set
            {
                this.val[0, 1] = value.X;
                this.val[1, 1] = value.Y;
                this.val[2, 1] = value.Z;
            }
        }
        public Vector3d Column3
        {
            get { return new Vector3d(this.val[0, 2], this.val[1, 2], this.val[2, 2]); }
            set
            {
                this.val[0, 2] = value.X;
                this.val[1, 2] = value.Y;
                this.val[2, 2] = value.Z;
            }
        }

        /// <summary>
        /// Determinant of the matrix
        /// </summary>
        public double Det
        {
            get
            {
                double k11 = this.val[2, 2] * this.val[1, 1] - this.val[2, 1] * this.val[1, 2];
                double k12 = this.val[2, 1] * this.val[0, 2] - this.val[2, 2] * this.val[0, 1];
                double k13 = this.val[1, 2] * this.val[0, 1] - this.val[1, 1] * this.val[0, 2];

                return this.val[0, 0] * k11 + this.val[1, 0] * k12 + this.val[2, 0] * k13;
            }
        }

        /// <summary>
        /// Trace of the matrix
        /// </summary>
        public double Trace
        {
            get { return val[0, 0] + val[1, 1] + val[2, 2]; }

        }

        /// <summary>
        /// Elementwise max norm of the matrix
        /// </summary>
        public double MaxNorm
        {
            get
            {
                double maxval = 0;
                maxval = Max(Max(Abs(this.val[0, 0]), Abs(this.val[0, 1])), Abs(this.val[0, 2]));
                maxval = Max(maxval, Max(Max(Abs(this.val[1, 0]), Abs(this.val[1, 1])), Abs(this.val[1, 2])));
                maxval = Max(maxval, Max(Max(Abs(this.val[2, 0]), Abs(this.val[2, 1])), Abs(this.val[2, 2])));
                return maxval;
            }
        }

        public bool IsZero
        {
            get { return this == new Matrix3d(); }
        }

        public bool IsIdentity
        {
            get { return this == Matrix3d.Identity(); }
        }

        public bool IsOrthogonal
        {
            get { return this.Transpose() * this == Matrix3d.Identity(); }
        }

        public Matrix3d Add(double a)
        {
            Matrix3d B = new Matrix3d();
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    B[i, j] = this.val[i, j] + a;
                }
            }
            return B;
        }
        public Matrix3d Add(Matrix3d a)
        {
            Matrix3d B = new Matrix3d();
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    B[i, j] = this.val[i, j] + a[i, j];
                }
            }
            return B;
        }
        public Matrix3d Subtract(Matrix3d a)
        {
            Matrix3d B = new Matrix3d();
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    B[i, j] = this.val[i, j] - a[i, j];
                }
            }
            return B;
        }

        public Matrix3d Mult(double a)
        {
            Matrix3d B = new Matrix3d();
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    B[i, j] = a * this.val[i, j];
                }
            }
            return B;
        }
        public Vector3d Mult(Vector3d a)
        {
            Vector3d b = new Vector3d(0, 0, 0, a.Coord);
            b[0] = this.val[0, 0] * a[0] + this.val[0, 1] * a[1] + this.val[0, 2] * a[2];
            b[1] = this.val[1, 0] * a[0] + this.val[1, 1] * a[1] + this.val[1, 2] * a[2];
            b[2] = this.val[2, 0] * a[0] + this.val[2, 1] * a[1] + this.val[2, 2] * a[2];
            return b;
        }
        public Point3d Mult(Point3d p)
        {
            double x = this.val[0, 0] * p.X + this.val[0, 1] * p.Y + this.val[0, 2] * p.Z;
            double y = this.val[1, 0] * p.X + this.val[1, 1] * p.Y + this.val[1, 2] * p.Z;
            double z = this.val[2, 0] * p.X + this.val[2, 1] * p.Y + this.val[2, 2] * p.Z;
            return new Point3d(x, y, z, p.Coord);
        }
        public Matrix3d Mult(Matrix3d a)
        {
            Matrix3d B = new Matrix3d();
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    for (int k = 0; k <= 2; k++)
                    {
                        B[i, j] = B[i, j] + this.val[i, k] * a[k, j];
                    }
                }
            }
            return B;
        }

        public Matrix3d Inverse()
        {
            Matrix3d B = new Matrix3d();

            double k11 = this.val[2, 2] * this.val[1, 1] - this.val[2, 1] * this.val[1, 2];
            double k12 = this.val[2, 1] * this.val[0, 2] - this.val[2, 2] * this.val[0, 1];
            double k13 = this.val[1, 2] * this.val[0, 1] - this.val[1, 1] * this.val[0, 2];
            double k21 = this.val[2, 0] * this.val[1, 2] - this.val[2, 2] * this.val[1, 0];
            double k22 = this.val[2, 2] * this.val[0, 0] - this.val[2, 0] * this.val[0, 2];
            double k23 = this.val[1, 0] * this.val[0, 2] - this.val[1, 2] * this.val[0, 0];
            double k31 = this.val[2, 1] * this.val[1, 0] - this.val[2, 0] * this.val[1, 1];
            double k32 = this.val[2, 0] * this.val[0, 1] - this.val[2, 1] * this.val[0, 0];
            double k33 = this.val[1, 1] * this.val[0, 0] - this.val[1, 0] * this.val[0, 1];

            double det = this.val[0, 0] * k11 + this.val[1, 0] * k12 + this.val[2, 0] * k13;

            if (det != 0.0)
            {
                B[0, 0] = k11 / det;
                B[0, 1] = k12 / det;
                B[0, 2] = k13 / det;

                B[1, 0] = k21 / det;
                B[1, 1] = k22 / det;
                B[1, 2] = k23 / det;

                B[2, 0] = k31 / det;
                B[2, 1] = k32 / det;
                B[2, 2] = k33 / det;
            }
            else
            {
                throw new Exception("Matrix is singular");
            }

            return B;
        }

        public Matrix3d Transpose()
        {
            Matrix3d T = this.Copy();
            T[0, 1] = this[1, 0];
            T[0, 2] = this[2, 0];
            T[1, 0] = this[0, 1];
            T[1, 2] = this[2, 1];
            T[2, 0] = this[0, 2];
            T[2, 1] = this[1, 2];
            return T;
        }

        /// <summary>
        /// Defines counterclockwise rotation around axis
        /// </summary>
        /// <param name="axis">Rotation axis</param>
        /// <param name="alpha">Angle of rotation (radians)</param>
        public static Matrix3d RotationMatrix(Vector3d axis, double alpha)
        {
            Matrix3d R = new Matrix3d();
            Vector3d v = axis.Normalized;
            double c = Cos(alpha);
            double s = Sin(alpha);

            R[0, 0] = c + Math.Pow(v.X, 2) * (1 - c);
            R[0, 1] = v.X * v.Y * (1 - c) - v.Z * s;
            R[0, 2] = v.X * v.Z * (1 - c) + v.Y * s;

            R[1, 0] = v.Y * v.X * (1 - c) + v.Z * s;
            R[1, 1] = c + Math.Pow(v.Y, 2) * (1 - c);
            R[1, 2] = v.Y * v.Z * (1 - c) - v.X * s;

            R[2, 0] = v.Z * v.X * (1 - c) - v.Y * s;
            R[2, 1] = v.Z * v.Y * (1 - c) + v.X * s;
            R[2, 2] = c + Math.Pow(v.Z, 2) * (1 - c);

            return R;
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
            if (GeometRi3D.UseAbsoluteTolerance)
            {
                return (this - m).MaxNorm < GeometRi3D.Tolerance;
            }
            else
            {
                return (this - m).MaxNorm / this.MaxNorm < GeometRi3D.Tolerance;
            }

        }

        /// <summary>
        /// Returns the hashcode for the object.
        /// </summary>
        public override int GetHashCode()
        {
            return GeometRi3D.HashFunction(Row1.GetHashCode(), Row2.GetHashCode(), Row3.GetHashCode());
        }

        /// <summary>
        /// String representation of an object.
        /// </summary>
        public override string ToString()
        {
            string str = string.Format("Row1 -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", this.val[0, 0], this.val[0, 1], this.val[0, 2]) + System.Environment.NewLine;
            str += string.Format("Row2 -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", this.val[1, 0], this.val[1, 1], this.val[1, 2]) + System.Environment.NewLine;
            str += string.Format("Row3 -> ({0,10:g5}, {1,10:g5}, {2,10:g5})", this.val[2, 0], this.val[2, 1], this.val[2, 2]);
            return str;
        }

        // Operators overloads
        //-----------------------------------------------------------------
        // "+"
        public static Matrix3d operator +(Matrix3d m, Matrix3d a)
        {
            return m.Add(a);
        }
        // "-"
        public static Matrix3d operator -(Matrix3d m)
        {
            return m.Mult(-1.0);
        }
        public static Matrix3d operator -(Matrix3d m, Matrix3d a)
        {
            return m.Subtract(a);
        }
        // "*"
        public static Matrix3d operator *(Matrix3d m, double a)
        {
            return m.Mult(a);
        }
        public static Matrix3d operator *(double a, Matrix3d m)
        {
            return m.Mult(a);
        }
        public static Vector3d operator *(Matrix3d m, Vector3d a)
        {
            return m.Mult(a);
        }
        public static Point3d operator *(Matrix3d m, Point3d p)
        {
            return m.Mult(p);
        }
        public static Matrix3d operator *(Matrix3d m, Matrix3d a)
        {
            return m.Mult(a);
        }

        public static bool operator ==(Matrix3d m1, Matrix3d m2)
        {
            if (object.ReferenceEquals(m1, null))
                return object.ReferenceEquals(m2, null);
            return m1.Equals(m2);
        }
        public static bool operator !=(Matrix3d m1, Matrix3d m2)
        {
            if (object.ReferenceEquals(m1, null))
                return !object.ReferenceEquals(m2, null);
            return !m1.Equals(m2);
        }

    }
}


