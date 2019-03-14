using System;
using System.Diagnostics;
using GeometRi;

namespace GeometRi.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {

            //Profile("Test", 10000, () =>
            //{
            //    Box3d box = new Box3d(new Point3d(0.5, 0.5, 0.5), 1, 1, 1);
            //    Circle3d c = new Circle3d(new Point3d(0.957494668177094, 1.08987119472114, -0.11622424522239),
            //                              0.154926580712558,
            //                              new Vector3d(0.362303959251271, 0.267138656415756, 0.892957322249635));
            //    object obj = c.Intersects(box);
            //});

            //Profile("Test", 20000, () =>
            //{
            //    Circle3d c1 = new Circle3d(new Point3d(0, 0, 0), 5, new Vector3d(0, 0, 1));
            //    Circle3d c2 = new Circle3d(new Point3d(11, 0, 0), 5, new Vector3d(0, 0, 1));

            //    bool t1 = GeometRi3D.AlmostEqual(c1.DistanceTo(c2), 1);

            //    c2 = new Circle3d(new Point3d(11, 0, 0), 5, new Vector3d(0, 2, 1));
            //    bool t2 = GeometRi3D.AlmostEqual(c1.DistanceTo(c2), 1);

            //});

            //Circle3d c = new Circle3d(new Point3d(2, 22, 43), 5, new Vector3d(0, 0, 2));
            //Point3d p = new Point3d(12, -22, -43);
            //Profile("Test", 2000000, () =>
            //{
            //    double dist = p.DistanceTo(c);

            //});


            Circle3d c = new Circle3d(new Point3d(2, 22, 43), 5, new Vector3d(0, 0, 2));
            Point3d p1 = new Point3d(12, -22, -43);
            Point3d p2 = new Point3d(3, 14, -6);
            Point3d p3 = new Point3d(-12, 11, 4);
            Triangle t = new Triangle(p1, p2, p3);
            Profile("Test", 2000000, () =>
            {
                bool test = c.Intersects(t);
            });


            Console.ReadLine();
        }

        static void Profile(string description, int iterations, Action action)
        {
            try
            {
                Console.WriteLine($"Running benchmark '{description}' for {iterations} iterations... ");

                // clean up
                GC.Collect();
                GC.WaitForPendingFinalizers();

                // warm up 
                action.Invoke();

                Stopwatch watch = Stopwatch.StartNew();
                for (int i = 0; i < iterations; i++)
                {
                    action.Invoke();
                }
                watch.Stop();
                Console.Write(description);
                Console.WriteLine("Time Elapsed: {0} ms", watch.ElapsedMilliseconds);
                Console.WriteLine($"Time per iteration: {watch.ElapsedMilliseconds / iterations} ms.");
            }
            catch (OutOfMemoryException)
            {
                Console.WriteLine("Out of memory!");
            }

        }
    }
}
