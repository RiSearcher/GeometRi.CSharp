using System;
using System.Runtime;
using GeometRi;


namespace GeometRi_Example
{
    class Program
    {
        public static void Main()
        {
            // Examples of basic operations with GeometRi

            // Global coordinate system is created automatically and can be accessed as "Coord3d.GlobalCS"
            Console.WriteLine("Number of defined coordinate systems: {0}", Coord3d.Counts);
            Console.WriteLine();
            Console.WriteLine("Default coordinate system: ");
            Console.WriteLine(Coord3d.GlobalCS.ToString());



            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("!!! Find intersection of plane with line !!!");

            // Define point and vector in global CS
            Point3d p1 = new Point3d(1, -5, -1);
            Vector3d v1 = new Vector3d(-2, 3, 4);

            // Define line using point and vector
            Line3d l1 = new Line3d(p1, v1);

            // Define plane using general equation in 3D space in the form "A*x+B*y+C*z+D=0"
            Plane3d s1 = new Plane3d(-2, 2, 3, -29);

            // Find the intersection of line with plane.
            // The results could be point, line or nothing, therefore get result as general object
            // and determine it's type.
            object obj = l1.IntersectionWith(s1);
            if (obj != null)
            {
                if (obj.GetType() == typeof(Line3d))
                {
                    Console.WriteLine("Intersection is line");
                    Line3d l2 = (Line3d)obj;
                    Console.WriteLine(l2.ToString());
                }
                else if (obj.GetType() == typeof(Point3d))
                {
                    Console.WriteLine("Intersection is point");
                    Point3d p2 = (Point3d)obj;
                    Console.WriteLine(p2.ToString());
                }
            }

            // Short variant
            // Will cause "InvalidCastException" if the intersection is not a point
            Point3d p3 = (Point3d)l1.IntersectionWith(s1);
            Console.WriteLine(p3.ToString());



            Line3d ll1 = new Line3d(new Point3d(2, 2, 2), new Vector3d(1, 1, 1));
            Line3d ll2 = new Line3d(new Point3d(201, 200, 200), new Vector3d(-10, -10, -10));

            GeometRi3D.Tolerance = 0.01;
            GeometRi3D.UseAbsoluteTolerance = false;
            if (ll1.Equals(ll2))
            {  };


            Console.ReadLine();


        }

    }
}

