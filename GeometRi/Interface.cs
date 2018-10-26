using System;

namespace GeometRi
{

    /// <summary>
    /// Interface for 1D objects (vector, line, ray, segment)
    /// </summary>
    public interface ILinearObject
    {
        Vector3d Direction { get; }
        bool IsOriented { get; }
        Line3d ToLine { get; }
    }

    /// <summary>
    /// Interface for 2D objects (plane, circle, ellipse, triangle)
    /// </summary>
    public interface IPlanarObject
    {
        Vector3d Normal { get; }
        bool IsOriented { get; }
        Plane3d ToPlane { get; }
    }

    /// <summary>
    /// Interface for finite objects
    /// </summary>
    public interface IFiniteObject
    {
        Box3d BoundingBox(Coord3d coord);
        Box3d MinimumBoundingBox { get; }
        Sphere BoundingSphere { get; }
    }
}
