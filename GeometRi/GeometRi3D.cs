using System;

namespace GeometRi
{
    public abstract class GeometRi3D
    {

        private static double _tolerance = 1E-12;

        /// <summary>
        /// Tolerance used for comparison operations (default 1e-12)
        /// </summary>
        public static double Tolerance
        {
            get { return _tolerance; }
            set { _tolerance = value; }
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


    }
}






