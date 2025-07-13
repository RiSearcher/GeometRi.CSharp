using System;
using System.Diagnostics;
using System.Collections.Generic;
using GeometRi;

namespace GeometRi.Benchmarks
{
    class IntersectionBenchmark
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


            //Circle3d c = new Circle3d(new Point3d(2, 22, 43), 5, new Vector3d(0, 0, 2));
            //Point3d p1 = new Point3d(12, -22, -43);
            //Point3d p2 = new Point3d(3, 14, -6);
            //Point3d p3 = new Point3d(-12, 11, 4);
            //Triangle t = new Triangle(p1, p2, p3);
            //Profile("Test", 200000, () =>
            //{
            //    bool test = c.Intersects(t);
            //});

            //Line3d s = new Line3d(new Point3d(1, 2, 0), new Vector3d(-5, 7, 0));
            //Point3d p1 = new Point3d(12, -22, 0);
            //Point3d p2 = new Point3d(3, 14, 0);
            //Point3d p3 = new Point3d(-12, 11, 0);
            //Triangle t = new Triangle(p1, p2, p3);
            //Circle3d c1 = new Circle3d(new Point3d(2, 22, 43), 50, new Vector3d(0, 0, 2));
            //Circle3d c2 = new Circle3d(new Point3d(22, -3, 8), 50, new Vector3d(-1, 2, -4));
            //Box3d box = new Box3d();
            //Circle3d c3 = new Circle3d(new Point3d(0.3, 0.55, 0.3), 0.1, new Vector3d(0, 0, 2));
            //Sphere sph = new Sphere(new Point3d(2.3, 1.55, 0.3), 0.1);
            //double dist1 = sph.DistanceTo(box);

            //List<ConvexPolyhedron> list = new List<ConvexPolyhedron>();
            //Random rnd = new Random();
            //for (int i=0; i < 5; i++)
            //{
            //    ConvexPolyhedron cp = ConvexPolyhedron.Octahedron();
            //    Rotation r = new Rotation(new Vector3d(rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble()), rnd.NextDouble());
            //    cp = cp.Rotate(r, cp.Center);
            //    cp = cp.Translate(new Vector3d(5 * rnd.NextDouble(), 5 * rnd.NextDouble(), 5 * rnd.NextDouble()));
            //    list.Add(cp);
            //}
            //for (int i = 0; i < 5; i++)
            //{
            //    ConvexPolyhedron cp = ConvexPolyhedron.Icosahedron();
            //    Rotation r = new Rotation(new Vector3d(rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble()), rnd.NextDouble());
            //    cp = cp.Rotate(r, cp.Center);
            //    cp = cp.Translate(new Vector3d(5 * rnd.NextDouble(), 5 * rnd.NextDouble(), 5 * rnd.NextDouble()));
            //    list.Add(cp);
            //}
            //for (int i = 0; i < 5; i++)
            //{
            //    ConvexPolyhedron cp = ConvexPolyhedron.Dodecahedron();
            //    Rotation r = new Rotation(new Vector3d(rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble()), rnd.NextDouble());
            //    cp = cp.Rotate(r, cp.Center);
            //    cp = cp.Translate(new Vector3d(5 * rnd.NextDouble(), 5 * rnd.NextDouble(), 5 * rnd.NextDouble()));
            //    list.Add(cp);
            //}

            //Profile("Test2", 50, () =>
            //{
            //    foreach(ConvexPolyhedron cp1 in list)
            //    {
            //        foreach(ConvexPolyhedron cp2 in list)
            //        {
            //            Point3d pp1 = new Point3d();
            //            Point3d pp2 = new Point3d();
            //            double dist = cp1.DistanceTo(cp2, out pp1, out pp2);
            //        }
            //    }
            //});


            Random rnd = new Random();
            List<Point3d> list = new List<Point3d>();
            for (int i = 0; i < 300000; i++)
            {
                Rotation r = new Rotation(new Vector3d(rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble()), rnd.NextDouble());
                Point3d origin = new Point3d(rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble());
                Coord3d coord = new Coord3d(origin, r.ToRotationMatrix.Column1, r.ToRotationMatrix.Column2);
                list.Add(new Point3d(rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble(), coord));
            }

            Profile("Test2", 5, () =>
            {
                foreach (Point3d p in list)
                {
                    Point3d p2 = p.ConvertToGlobal();
                }
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
