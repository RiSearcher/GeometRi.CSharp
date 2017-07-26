using System;

namespace GeometRi
{
    // Interface for 1D objects (vector, line, ray, segment)
    public interface ILinearObject
    {
        Vector3d Direction { get; }
        bool IsOriented { get; }
    }

    // Interface for 2D objects (plane, circle, ellipse, triangle)
    public interface IPlanarObject
    {
        Vector3d Normal { get; }
        bool IsOriented { get; }
    }
}
