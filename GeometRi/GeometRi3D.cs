using System;
using static System.Math;

namespace GeometRi
{
    /// <summary>
    /// Abstract class. Implements global tolerance property and tolerance based equality methods.
    /// </summary>
    public abstract class GeometRi3D
    {

        private static double _tolerance = 1E-12;
        private static bool _absolute = true;

        /// <summary>
        /// Tolerance used for comparison operations (default 1e-12)
        /// </summary>
        public static double Tolerance
        {
            get { return _tolerance; }
            set { _tolerance = value; }
        }

        /// <summary>
        /// Flag for switching absolute (TRUE) to relative (FALSE) tolerance comparison.
        /// </summary>
        public static bool UseAbsoluteTolerance
        {
            get { return _absolute; }
            set { _absolute = value; }
        }

        /// <summary>
        /// Tolerance based equality check
        /// </summary>
        public static bool AlmostEqual(double a, double b)
        {
            if (Math.Abs(a - b) <= _tolerance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Tolerance based unequality check
        /// </summary>
        public static bool NotEqual(double a, double b)
        {
            if (Math.Abs(a - b) > _tolerance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Tolerance based comparison
        /// </summary>
        public static bool Greater(double a, double b)
        {
            if ((a - b) > _tolerance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Tolerance based comparison
        /// </summary>
        public static bool Smaller(double a, double b)
        {
            if ((a - b) < -_tolerance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static internal int HashFunction(int n1, int n2)
        {
            return (n1 << 4) ^ (n1 >> 28) ^ n2;
        }

        static internal int HashFunction(int n1, int n2, int n3)
        {
            n1 = (n1 << 4) ^ (n1 >> 28) ^ n2;
            return (n1 << 4) ^ (n1 >> 28) ^ n3;
        }

        static internal int HashFunction(int n1, int n2, int n3, int n4)
        {
            n1 = (n1 << 4) ^ (n1 >> 28) ^ n2;
            n1 = (n1 << 4) ^ (n1 >> 28) ^ n3;
            return (n1 << 4) ^ (n1 >> 28) ^ n4;
        }

        #region "CalcAngle"
        static internal double GetAngle(ILinearObject obj1, ILinearObject obj2)
        {
            if (obj1.IsOriented && obj2.IsOriented)
            {
                double tmp = obj1.Direction.Dot(obj2.Direction) / obj1.Direction.Norm / obj2.Direction.Norm;
                if (tmp > 1)
                {
                    tmp = 1;
                } else if (tmp < -1)
                {
                    tmp = -1;
                }
                return Acos(tmp);
            }
            else
            {
                // return smalest angle
                double ang = GetAngle(obj1.Direction, obj2.Direction);
                if (ang <= PI / 2)
                {
                    return ang;
                }
                else
                {
                    return PI - ang;
                }
            }
        }

        static internal double GetAngle(ILinearObject obj1, IPlanarObject obj2)
        {
            if (obj1.IsOriented && obj2.IsOriented)
            {
                throw new NotImplementedException();
            }
            else
            {
                double tmp = obj1.Direction.Dot(obj2.Normal) / obj1.Direction.Norm / obj2.Normal.Norm;
                if (tmp > 1)
                {
                    tmp = 1;
                }
                else if (tmp < -1)
                {
                    tmp = -1;
                }
                double ang = Asin(tmp);
                return Abs(ang);
            }
        }

        static internal double GetAngle(IPlanarObject obj1, ILinearObject obj2)
        {
            return GetAngle(obj2, obj1);
        }

        static internal double GetAngle(IPlanarObject obj1, IPlanarObject obj2)
        {
            if (obj1.IsOriented && obj2.IsOriented)
            {
                throw new NotImplementedException();
            }
            else
            {
                // return smalest angle
                double ang = GetAngle(obj1.Normal, obj2.Normal);
                if (ang <= PI / 2)
                {
                    return ang;
                }
                else
                {
                    return PI - ang;
                }
            }
        }
        #endregion
    }
}
