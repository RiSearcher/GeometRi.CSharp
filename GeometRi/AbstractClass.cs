using System;
using static System.Math;
using System.Collections.Generic;

namespace GeometRi
{

#if NET20_OR_GREATER
    [Serializable]
#endif
    abstract public class FiniteObject
    {
        abstract internal int _PointLocation(Point3d p);
    }
#if NET20_OR_GREATER
    [Serializable]
#endif
    abstract public class PlanarFiniteObject : FiniteObject
    {
        abstract internal Plane3d Plane { get; }
    }
#if NET20_OR_GREATER
    [Serializable]
#endif
    abstract public class LinearFiniteObject : FiniteObject
    {
        abstract internal Line3d Line { get; }
        abstract internal Vector3d Vector { get; }
        abstract internal Ray3d Ray { get; }
    }
}
