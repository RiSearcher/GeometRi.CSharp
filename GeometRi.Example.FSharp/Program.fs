open GeometRi
open System

let sample1() = 
    printfn "Ptolemy's construction of a pentagon"
    printfn "Draw a regular pentagon inside the circle"
    printfn ""

    // We will follow the notation from 
    // https://www.cut-the-knot.org/pythagoras/pentagon.shtml
    // Read this article for detailed description of the algorithm

    // Create circle with center in point O and choose arbitrary point C on circle
    // We will start our pentagon from this point
    let O = Point3d(0.0, 0.0, 0.0)
    let circle = Circle3d(O, 1.0, Vector3d(0.0, 0.0, 1.0))
    let C = circle.ParametricForm(0.0)

    // Draw a diameter through point C (point D is the opposite point of the diameter)
    let l = Line3d(C, O)
    let CD = l.IntersectionWith circle |> unbox<Segment3d>

    // Need to check if we get the correct point
    let D = if CD.P1 = C then CD.P2 else CD.P1

    // Draw a orthogonal diameter AB

    // First, draw two circles with centers in points C and D
    // The intersection of these circles gives a line, orthogonal to the segment CD
    let circle1 = Circle3d(C, CD.Length, circle.Normal)
    let circle2 = Circle3d(D, CD.Length, circle.Normal)
    let line = (circle1.IntersectionWith circle2 |> unbox<Segment3d>).ToLine

    // Now find the intersection of line with our circle
    // This gives points A and B
    let AB = line.IntersectionWith circle |> unbox<Segment3d>
    let A = AB.P1
    let B = AB.P2

    // Find the center point of the segment OB (this will be point M)
    let M = (O + B) / 2.0

    // Draw a circle with center in point M and radius MC
    let radius = M.DistanceTo C
    let tmp_circle0 = Circle3d(M, radius, circle.Normal)

    // Find the intersection with diameter AB (this will be point N)
    let ON = tmp_circle0.IntersectionWith AB |> unbox<Segment3d>
    let N = if ON.P1 = O then ON.P2 else ON.P1

    // CN is the length of the pentagon's side
    let side_len = C.DistanceTo N 

    // The first point of the pentagon is point C
    let P1 = C

    // Draw a circle from point C with radius 'side_len' and this gives us two more points
    let tmp_circle1 = Circle3d(C, side_len, circle.Normal)
    let tmp_segment = tmp_circle1.IntersectionWith circle |> unbox<Segment3d>
    let P2 = tmp_segment.P1
    let P5 = tmp_segment.P2

    // Repeat this two more times to get remaining points
    let tmp_circle2 = Circle3d(P2, side_len, circle.Normal)
    let tmp_segment2 = tmp_circle2.IntersectionWith circle |> unbox<Segment3d>
    let P3 = if tmp_segment2.P1 = P1 then tmp_segment2.P2 else tmp_segment2.P1

    let tmp_circle3 = Circle3d(P3, side_len, circle.Normal);
    let tmp_segment3 = tmp_circle3.IntersectionWith circle |> unbox<Segment3d>
    let P4 = if tmp_segment3.P1 = P2 then tmp_segment3.P2 else tmp_segment3.P1 

    // Points P1, P2, P3, P4 and P5 define our pentagon
    // Let's check if interion angles are equal 108 degrees (with accuracy 1e-8)
    let angle1 = Vector3d(P1, P2).AngleToDeg(Vector3d(P1, P5))
    let angle2 = Vector3d(P2, P1).AngleToDeg(Vector3d(P2, P3))
    let angle3 = Vector3d(P3, P2).AngleToDeg(Vector3d(P3, P4))
    let angle4 = Vector3d(P4, P3).AngleToDeg(Vector3d(P4, P5))
    let angle5 = Vector3d(P5, P4).AngleToDeg(Vector3d(P5, P1))

    let tolerance = 1e-8;
    if Math.Abs(angle1 - 108.0) < tolerance &&
       Math.Abs(angle2 - 108.0) < tolerance &&
       Math.Abs(angle3 - 108.0) < tolerance &&
       Math.Abs(angle4 - 108.0) < tolerance &&
       Math.Abs(angle5 - 108.0) < tolerance 
    then
        printfn "Your construction is correct!"
        printfn "%A" P1
        printfn "%A" P2
        printfn "%A" P3
        printfn "%A" P4
        printfn "%A" P5

let sample2() = 
    printfn "Rytz's construction example"

    // We will follow the notation from 
    // https://en.wikipedia.org/wiki/Rytz%27s_construction
    // Read this article for detailed description of the algorithm

    // Define arbitrary circle and plane
    let circle = Circle3d(Point3d(2.0, 3.0, -1.0), 5.0, Vector3d(1.0, 1.0, 1.0))
    let plane = Plane3d(Point3d(-1.0, -5.0, 0.0), Vector3d(3.0, 0.0, 1.0))

    // Get two points on the circle at orthogonal diameters
    let d1 = circle.ParametricForm 0.0
    let d2 = circle.ParametricForm(Math.PI / 2.0)

    //Project onto plane
    let C = circle.Center.ProjectionTo plane
    let P = d1.ProjectionTo plane
    let Q = d2.ProjectionTo plane

    // Rotate point P by 90 degrees
    let r = Rotation(plane.Normal, Math.PI / 2.0)
    let P_prime = P.Rotate(r, C)

    // Determine the center D of the line segment P'-Q
    let D = (P_prime + Q) / 2.0

    // Draw the circle with center D throuugh point C
    let radius = D.DistanceTo C
    let circle_DC = Circle3d(D, radius, plane.Normal)

    // Intersect the circle and the line P'-Q. Intersection points are A and B.
    // (some additional treatment is needed when circle is parallel to plane,
    // not going to do it here)
    let line = Line3d(P_prime, Q)
    let AB = line.IntersectionWith(circle_DC) |> unbox<Segment3d>
    let A = AB.P1
    let B = AB.P2

    // Find the length of semi-axes
    let semi_axis_a = Q.DistanceTo A
    let semi_axis_b = Q.DistanceTo B

    // Find the direction of semi-axes
    let tvec_CA = Vector3d(C, A)
    let tvec_CB = Vector3d(C, B)

    // renormalize vectors
    let vec_CA = semi_axis_b * tvec_CA.Normalized;
    let vec_CB = semi_axis_a * tvec_CB.Normalized;

    // Now, construct the projection
    let ellipse = Ellipse(C, vec_CA, vec_CB)

    // Compare with more efficient numerical algorithm
    if ellipse = circle.ProjectionTo plane then
        printfn "Your construction is correct!"
        printfn "%A" ellipse

let sample3() = 
    printfn "Mascheroni Construction of a Regular Pentagon"
    printfn ""

    // We will follow the notation from 
    // https://www.cut-the-knot.org/pythagoras/MascheroniPentagon.shtml
    // Read this article for detailed description of the algorithm

    // Start with arbitrary points A and B (in plane XY)
    let A = Point3d(0.0, 0.0, 0.0)
    let B = Point3d(5.0, 0.0, 0.0)

    // Draw a circle C1 with center in point A through point B
    let C1 = Circle3d(A, A.DistanceTo(B), Vector3d(0.0, 0.0, 1.0))

    // Draw a circle C2 with center in point B through point A
    let C2 = Circle3d(B, A.DistanceTo(B), Vector3d(0.0, 0.0, 1.0))

    // Intersection of C1 and C2 gives points C and D
    let segment = C1.IntersectionWith C2 |> unbox<Segment3d>
    let C = segment.P1
    let D = segment.P2

    // Draw a circle C3 with center in point C through point D
    let C3 = Circle3d(C, C.DistanceTo(D), Vector3d(0.0, 0.0, 1.0))

    // Intersection of C3 and C1 gives point E
    let segment2 = C3.IntersectionWith C1 |> unbox<Segment3d> 
    // if E == D, choose another point of intersection
    let E = if segment2.P1 = D then segment2.P2 else segment2.P1


    // Intersection of C3 and C2 gives point F
    let segment3 = C3.IntersectionWith C2 |> unbox<Segment3d> 
    // if F == D, choose another point of intersection
    let F = if segment3.P1 = D then segment3.P2 else segment3.P1

    // Draw a circle C4 with center in point A through point F
    let C4 = Circle3d(A, A.DistanceTo(F), Vector3d(0.0, 0.0, 1.0))

    // Draw a circle C5 with center in point B through point E
    let C5 = Circle3d(B, B.DistanceTo(E), Vector3d(0.0, 0.0, 1.0))

    // Intersection of C4 and C5 gives points G and H
    let segment4 = C4.IntersectionWith C5 |> unbox<Segment3d> 
    // point G is closer to point D
    let G, H = 
        if segment4.P1.DistanceTo D < segment4.P2.DistanceTo D then
            segment4.P1, segment4.P2
        else
            segment4.P2, segment4.P1;

    // Draw a circle C6 with center in point G through point C
    let C6 = Circle3d(G, G.DistanceTo(C), Vector3d(0.0, 0.0, 1.0))

    // Intersection of C6 and C3 gives points I and J
    // we need to choose correct side for I and J
    let segment5 = C6.IntersectionWith C3 |> unbox<Segment3d> 
    let I, J = 
        if segment5.P1.DistanceTo A < segment5.P1.DistanceTo B then
            segment5.P1, segment5.P2
        else
            segment5.P1, segment5.P2

    // Draw a circle C7 with center in point H through point C
    let C7 = Circle3d(H, H.DistanceTo(C), Vector3d(0.0, 0.0, 1.0))

    // Intersection of C7 and C3 gives points K and L
    let segment6 = C7.IntersectionWith C3 |> unbox<Segment3d> 
    let K, L = 
        if segment6.P1.DistanceTo I < segment6.P1.DistanceTo J then
            segment6.P1, segment6.P2
        else
            segment6.P2, segment6.P1

    // Points DIKLJ define our pentagon
    // Let's check if interion angles are equal 108 degrees (with accuracy 1e-8)
    let angle1 = Vector3d(D, I).AngleToDeg(Vector3d(D, J))
    let angle2 = Vector3d(I, D).AngleToDeg(Vector3d(I, K))
    let angle3 = Vector3d(K, I).AngleToDeg(Vector3d(K, L))
    let angle4 = Vector3d(L, K).AngleToDeg(Vector3d(L, J))
    let angle5 = Vector3d(J, L).AngleToDeg(Vector3d(J, D))

    let tolerance = 1e-8
    if Math.Abs(angle1 - 108.0) < tolerance &&
       Math.Abs(angle2 - 108.0) < tolerance &&
       Math.Abs(angle3 - 108.0) < tolerance &&
       Math.Abs(angle4 - 108.0) < tolerance &&
       Math.Abs(angle5 - 108.0) < tolerance 
    then
        printfn "Your construction is correct!"
        printfn "%A" D
        printfn "%A" I
        printfn "%A" K
        printfn "%A" L
        printfn "%A" J

sample1()
sample2()
sample3()