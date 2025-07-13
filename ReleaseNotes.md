# GeometRi
## Release notes

### 1.4.1.4 (13/7/2025)
* Intersection check of ellipsoid and sphere.
* Distance between two ellipsoids.

### 1.4.1.3 (27/5/2025)
* Fixed AABB local coordinate system.
* Create prism by extruding face of convex polyhedron.

### 1.4.1.2 (5/3/2025)
* Distance from convex polyhedron to segment, sphere, and circle.
* Several optimizations.

### 1.4.1.1 (24/10/2024)
* Fixed closest points locations in ConvexPoly distance calculations.
* Fixed circle-circle intersection check.

### 1.4.1.0 (16/4/2024)
* Convex polyhedron class.
* Axis aligned bounding box (AABB) class.

### 1.4.0.1 (13/9/2022)
* Fixed segment direction normalization.

### 1.4.0.0 (8/9/2022)
* Added tetrahedron class.
* More robust traingle and plane/line intersection algorithm.
* Breaking changes: Line.PerpendicuarTo(Line) now returns 'null' for parallel lines.

### 1.3.5.16 (7/5/2021)
* Fixed distance from triangle to point calculation.

### 1.3.5.15 (1/6/2021)
* Intersection check of triangle with triangle and box.

### 1.3.5.13 (1/10/2019)
* Many optimizations.
* Distance from line to circle's boundary.
* Fixed circle/triangle intersection check.

### 1.3.5.12 (18/6/2019)
* Fixed "Rotation.ToAxis/Angle" method.

### 1.3.5.11 (17/5/2019)
* More robust implementation of Point3d.BelongsTo(Segment3d).

### 1.3.5.10 (14/5/2019)
* Added "Serializable" attribute for .Net Framework 2.0 build.
* Fixed bug in circle/circle intersection.

### 1.3.5.9 (14/3/2019)
* Shortest distance calculation for various objects.

### 1.3.5.8 (10/1/2019)
* Distance from sphere to point, plane, circle or other sphere.
* Closest points on two circles/spheres calculation.

### 1.3.5.7 (15/12/2018)
* Distance from circle to point, plane or other circle.

### 1.3.5.6 (12/12/2018)
* Added Circle.IsInside(Box) method.
* Fixed segment with segment intersection method.

### 1.3.5.5 (2/11/2018)
* Coplanarity test for different objects.

### 1.3.5.4 (14/10/2018)
* Fixed point in triangle test.

### 1.3.5.3 (15/9/2018)
* Box3d intersection with line, ray or segment.
* Box3d translate, rotate, reflect methods.

### 1.3.5.2 (7/7/2018)
* Ray3d intersection with line, segment, circle, ellipse, sphere, ellipsoid and triangle.

### 1.3.5.1 (30/6/2018)
* Added circle-circle intersection method.

### 1.3.5 (20/6/2018)
* New intersection methods: line/segment intersection with circle, ellipse, triangle, ellipsoid.

### 1.3.4 (4/5/2018)
* New test methods to check if point is located in the epsilon neighborhood of the object.

### 1.3.3 (21/4/2018)
* Added relative tolerance equality checks.

### 1.3.2 (27/11/2017)
* Changed behavior of "new Vector3d(point)" and "Point3d.ToVector" methods. By default vectors in global CS will be created.
* Fixed "Segment3d.DistanceTo(Segment3d)" (tolerance parameter decreased).

### 1.3.1 (6/10/2017)
* Added "Rotation.ToEulerAngles()" method.

### 1.3.0 (18/9/2017)
* Added "Box3d" class and "BoundingBox" methods.

### 1.2.1 (11/9/2017)
* Added "Rotation.SLERP" and "Rotation.FromEulerAngles" methods.

### 1.2.0 (26/8/2017)
* Added "Rotation" and "Quaternion" classes.

### 1.1.1 (4/8/2017)

### 1.1.0 (30/7/2017)

### 1.0.1 (23/7/2017)

### 0.9.6 (21/6/2017)

### 0.9.5 (3/6/2017)

### 0.9.4 (31/5/2017)

### 0.9.3 (21/5/2017)

### 0.9.2 (29/4/2017)

### 0.9.1 (25/4/2017)

### 0.9.0 (23/4/2017)
