# GeometRi
### 简单、轻量级的.Net 计算几何库

GeometRi 库的主要目的是处理基本
几何基元，例如三维空间中的点、线、平面、球体、三角形：
平移和旋转运算、距离计算、交叉点、
一个物体到另一个物体的正交投影等。物体可以定义为
在全局或局部坐标系中并转换形成一个坐标
系统转变为另一个系统。

该库的构建尽可能简单直观。用户不必记住参考坐标
每个对象的系统。对象存储它们定义的坐标系和所有变换
必要时将会隐性地执行。

主要目标是代码的简单性和可读性，因此速度和稳健性不是优先考虑的。
全局容差属性用于接近度检查，而不是精确的稳健算法。

## [发布说明](./ReleaseNotes.md)

* [安装](https://github.com/RiSearcher/GeometRi.CSharp#installation)
* [示例](https://github.com/RiSearcher/GeometRi.CSharp#examples)
* [类](https://github.com/RiSearcher/GeometRi.CSharp#classes)
* [Point3d](https://github.com/RiSearcher/GeometRi.CSharp#point3d)
* [Vector3d](https://github.com/RiSearcher/GeometRi.CSharp#vector3d)
* [Line3d](https://github.com/RiSearcher/GeometRi.CSharp#line3d)
* [Ray3d](https://github.com/RiSearcher/GeometRi.CSharp#ray3d)
* [Segment3d](https://github.com/RiSearcher/GeometRi.CSharp#segment3d)
* [Plane3d](https://github.com/RiSearcher/GeometRi.CSharp#plane3d)
* [球体](https://github.com/RiSearcher/GeometRi.CSharp#sphere)
* [Circle3d](https://github.com/RiSearcher/GeometRi.CSharp#circle3d)
* [椭圆](https://github.com/RiSearcher/GeometRi.CSharp#ellipse)
* [椭圆体](https://github.com/RiSearcher/GeometRi.CSharp#ellipsoid)
* [Box3d](https://github.com/RiSearcher/GeometRi.CSharp#box3d)
* [三角形](https://github.com/RiSearcher/GeometRi.CSharp#triangle)
* [四面体](https://github.com/RiSearcher/GeometRi.CSharp#tetrahedron)
* [凸多面体](https://github.com/RiSearcher/GeometRi.CSharp#convexpolyhedron)
* [Coord3d](https://github.com/RiSearcher/GeometRi.CSharp#coord3d)
* [Matrix3d](https://github.com/RiSearcher/GeometRi.CSharp#matrix3d)
* [四元数](https://github.com/RiSearcher/GeometRi.CSharp#quaternion)
* [旋转](https://github.com/RiSearcher/GeometRi.CSharp#rotation)
* [GeometRi3D](https://github.com/RiSearcher/GeometRi.CSharp#geometri3d)

## 安装
使用 NuGet 安装库。在 NuGet 包管理器中搜索 __GeometRi__ 或在包管理器控制台中输入：
```
Install-Package GeometRi
```

## 示例

* 托勒密在圆内构造的五边形：[C#](https://dotnetfiddle.net/0Is1ZV)
* 马斯切罗尼构造正五边形：[C#](https://dotnetfiddle.net/45iLTa)
* Rytz 的构造：[C#](https://dotnetfiddle.net/WqeS69)、[VB.Net](https://dotnetfiddle.net/2c04c0)

## 课程

* __Point3d__ 和 __Vector3d__ 是两个基类，分别表示三维空间中的点和向量。
Point3d 或 Vector3d 类型的对象可以在全局或局部坐标系中定义。

* __Line3d__、__Ray3d__、__Segment3d__、__Plane3d__、__Circle3d__、__Sphere__、__Ellipse__、__Ellipsoid__、__Box3d__、__Triangle__ 和 __Tetrahedron__
是复合类，根据点和向量定义。

* __Coord3d__、__Rotation__、__Quaternion__ 和 __Matrix3d__ 是辅助类。

* __GeometRi3d__ 是一个抽象类，它定义了一些常见的功能，例如全局公差属性(GeometRi3d.Tolerance)
用于其他类的邻近操作。实现基于容差的相等方法：AlmostEqual(double, double)、NotEqual(double,double)、
大于(双精度，双精度)和小于(双精度，双精度)。

## Point3d – 点3D

基类之一，可以由三个双精度数(X、Y 和 Z)或双精度数组构造。
每个构造函数都有可选参数“coord”，用于定义点的局部坐标系。
默认情况下，所有点都在全球坐标系中定义。
### 特性
* __X__ - 参考坐标系中的 X 坐标
* __Y__ - 参考坐标系中的 Y 坐标
* __Z__ - 参考坐标系中的 Z 坐标
* __Coord__ - 参考坐标系
* __ToVector__ - 点的半径向量
### 方法
* __Copy__ - 创建对象的副本
* __ConvertTo__ - 将点转换为本地坐标系
* __ConvertToGlobal__ - 将点转换为全局坐标系
* __Add__ - 添加两个点
* __Subtract__ - 从另一个点中减去一个点
* __Scale__ - 按给定数字缩放点
* __DistanceTo__ - 从点到其他物体的最短距离
* __ClosestPoint__ - 圆、盒子、三角形、球体、椭圆、椭圆体、四面体上的最近点
* __ProjectionTo__ - 点到线、平面或球体的正交投影
* __BelongsTo__ - 测试点是否位于对象的 epsilon 邻域内
* __IsInside__ - 测试点是否严格位于对象内部(不在边界的 epsilon 邻域内)
* __IsOutside__ - 测试点是否位于对象的 epsilon 邻域之外
* __IsOnBoundary__ - 测试点是否位于对象边界的 epsilon 邻域内
* __Translate__ - 通过向量平移点
* __Rotate__ - 围绕原点或其他点旋转点
* __反射__ - 在点、线或面中反射点
* __Equals__ - 检查两个点是否相等
* __ToString__ - 全局或局部坐标系中点的字符串表示
### 静态方法
* __CollinearPoints__ - 检查三个点是否共线
### 重载运算符
* __+__ - 添加两点
* __-__ - 从另一点中减去一分
* __-__ - 一元运算符
* __*__ - 按数字缩放点
* __/__ - 按数字缩放点
* __=__ - 平等检查
* __<>__ - 不平等检查

## Vector3d – 矢量3D

第二个基类，表示三维空间中的矢量。由三个分量(X、Y 和 Z)或双精度数组构成
(带有可选的 'coord' 参数用于本地坐标系)。此外，可以通过点构建，
表示该点的半径矢量，或者用两个点表示从第一点到另一点的矢量。在这种情况下
该向量将在与第一个操作数相同的坐标系中定义。
### 特性
* __X__ - 参考坐标系中的 X 分量
* __Y__ - 参考坐标系中的 Y 分量
* __Z__ - 参考坐标系中的 Z 分量
* __Coord__ - 参考坐标系
* __Norm__ - 向量范数
* __ToPoint__ - 点，由从原点开始的向量表示
* __OrthogonalVector__ - 返回任意向量，与当前向量正交
### 方法
* __Copy__ - 创建对象的副本
* __ConvertTo__ - 将矢量转换为局部坐标系
* __ConvertToGlobal__ - 将向量转换为全局坐标系
* __Normalize__ - 标准化当前向量
* __Normalized__ - 返回标准化向量
* __IsParallelTo__ - 检查两个物体是否平行
* __IsNotParallelTo__ - 检查两个物体是否不平行
* __IsOrthogonalTo__ - 检查两个对象是否正交
* __AngleTo__ - 两个物体之间的角度
* __AngleToDeg__ - 两个物体之间的角度(以度为单位)
* __Add__ - 重载，添加数字或向量
* __Subtract__ - 重载，减去数字或向量
* __Mult__ - 重载，乘以数字或向量
* __Dot__ - 两个向量的点积
* __ProjectionTo__ - 返回当前向量到第二个向量的投影
* __Rotate__ - 围绕原点旋转矢量
* __Reflect__ - 在点、线或平面中反射矢量
* __Equals__ - 检查两个向量是否相等
* __ToString__ - 全局或局部坐标系中矢量的字符串表示

## Line3d – 无限线3D

三维空间中的无限线，由线上的任意点和方向向量定义。
### 特性
* __Point__ - 线的基点
* __Direction__ - 线的方向向量
### 方法
* __Copy__ - 创建对象的副本
* __DistanceTo__ - 到点、线、射线或线段的最短距离
* __PerpendicularTo__ - 指向第二条线的垂直点
* __IntersectionWith__ - 线与平面、椭圆体、椭圆、圆、三角形、线段或球体的交点
* __ProjectionTo__ - 直线到平面的正交投影
* __IsParallelTo__ - 检查两个物体是否平行
* __IsNotParallelTo__ - 检查两个物体是否不平行
* __IsOrthogonalTo__ - 检查两个对象是否正交
* __IsCoplanarTo__ - 检查线是否与其他线性或平面物体共面
* __AngleTo__ - 两个物体之间的角度
* __AngleToDeg__ - 两个物体之间的角度(以度为单位)
* __Translate__ - 通过向量翻译线
* __Rotate__ - 围绕原点或其他点旋转线
* __Reflect__ - 在点、线或平面中反射线
* __Equals__ - 检查两行是否相等
* __ToString__ - 全局或局部坐标系中线的字符串表示

## Ray3d – 射线3D

表示三维空间中的射线，由起点和方向向量定义。
### 特性
* __Point__ - 射线基点
* __Direction__ - 射线的方向向量
* __ToLine__ - 将射线转换为线
### 方法
* __Copy__ - 创建对象的副本
* __DistanceTo__ - 到点、线、线段或其他射线的最短距离
* __PerpendicularTo__ - 垂直于线的点
* __IntersectionWith__ - 射线与平面的交点
* __ProjectionTo__ - 射线到平面的正交投影
* __IsParallelTo__ - 检查两个物体是否平行
* __IsNotParallelTo__ - 检查两个物体是否不平行
* __IsOrthogonalTo__ - 检查两个对象是否正交
* __IsCoplanarTo__ - 检查射线是否与其他线性或平面物体共面
* __AngleTo__ - 两个物体之间的角度
* __AngleToDeg__ - 两个物体之间的角度(以度为单位)
* __Translate__ - 通过矢量平移射线
* __Rotate__ - 围绕原点或其他点旋转射线
* __反射__ - 在点、线或平面上反射射线
* __Equals__ - 检查两条射线是否相等
* __ToString__ - 全局或局部坐标系中射线的字符串表示

## Segment3d – 线段3D

表示三维空间中的一条线段，由两个点定义。
### 特性
* __P1__ - 线段的第一个点
* __P2__ - 线段的第二个点
* __Length__ - 段的长度
* __ToVector__ - 将线段转换为向量
* __ToRay__ - 将线段转换为射线
* __ToLine__ - 将段转换为线
* __MinimumBoundingBox__ - 对象的最小边界框
* __BoundingSphere__ - 物体的边界球
### 方法
* __Copy__ - 创建对象的副本
* __BoundingBox__ - 给定坐标系中的轴对齐边界框 (AABB)
* __DistanceTo__ - 到点、线、射线、平面或其他线段的最短距离
* __IntersectionWith__ - 线段与线、平面、椭圆、三角形、椭圆体、球体、圆或其他线段的交点
* __ProjectionTo__ - 线段到线或平面的正交投影
* __IsParallelTo__ - 检查两个物体是否平行
* __IsNotParallelTo__ - 检查两个物体是否不平行
* __IsOrthogonalTo__ - 检查两个对象是否正交
* __IsCoplanarTo__ - 检查线段是否与其他线性或平面物体共面
* __AngleTo__ - 两个物体之间的角度
* __AngleToDeg__ - 两个物体之间的角度(以度为单位)
* __Translate__ - 通过向量翻译片段
* __Rotate__ - 围绕原点或其他点旋转线段
* __反射__ - 在点、线或平面中反射线段
* __Equals__ - 检查两个段是否相等
* __ToString__ - 全局或局部坐标系中字符串的表示形式

## Plane3d – 平面3D

由平面上的任意点和法线向量定义的 3D 平面。
也可以通过平面一般方程(Ax + By + Cz + D = 0)中的系数来定义，通过三个点
或通过平面上的点和两个向量。
### 特性
* __Point__ - 平面上的点
* __Normal__ - 平面的法线向量
* __A/B/C/D__ - 一般平面方程中的系数 A、B、C 和 D
### 方法
* __Copy__ - 创建对象的副本
* __SetCoord__ - 设置一般平面方程的参考坐标系
* __IntersectionWith__ - 平面与线、平面、线段、球体、椭圆、椭圆体、圆或两个其他平面的交点
* __IsParallelTo__ - 检查两个物体是否平行
* __IsNotParallelTo__ - 检查两个物体是否不平行
* __IsOrthogonalTo__ - 检查两个对象是否正交
* __IsCoplanarTo__ - 检查平面是否与其他线性或平面物体共面
* __AngleTo__ - 两个物体之间的角度
* __AngleToDeg__ - 两个物体之间的角度(以度为单位)
* __Translate__ - 通过矢量平移平面
* __Rotate__ - 围绕原点或其他点旋转平面
* __反射__ - 在点、线或平面中反射平面
* __Equals__ - 检查两个平面是否相等
* __ToString__ - 全局或局部坐标系中平面的字符串表示

## Sphere - 球体

定义三维空间中的球体。实现与线、平面和其他球体的相交、投影到线和平面，以及
常见的平移、旋转和反射方法。
### 特性
* __Center__ - 球体的中心
* __R__ - 球体的半径
* __Area__ - 球体的面积
* __Volume__ - 球体的体积
* __MinimumBoundingBox__ - 对象的最小边界框
* __BoundingSphere__ - 物体的边界球
### 方法
* __Copy__ - 创建对象的副本
* __BoundingBox__ - 给定坐标系中的轴对齐边界框 (AABB)
* __IsInside__ - 检查球体是否位于盒子内部
* __ClosestPoint__ - 球体表面上距离目标点最近的点
* __DistanceTo__ - 到点、线、射线、线段、平面、圆、球体或盒子的最短距离
* __Intersects__ - 与圆和四面体的相交检查
* __IntersectionWith__ - 球面与线、平面、线段或其他球面的交点
* __ProjectionTo__ - 球体到线或平面的正交投影
* __Translate__ - 通过矢量平移球体
* __Rotate__ - 围绕原点或其他点旋转球体
* __Reflect__ - 在点、线或平面上反射球体
* __Equals__ - 检查两个球体是否相等
* __ToString__ - 全局或局部坐标系中球体的字符串表示

## Circle3d - 圆形3d

通过中心点、半径和法线向量在三维空间中定义一个圆。
### 特性
* __Center__ - 圆心
* __R__ - 圆的半径
* __Normal__ - 圆的法线
* __Perimeter__ - 圆的周长
* __Area__ - 圆的面积
* __ToEllipse__ - 将圆转换为等效椭圆
* __MinimumBoundingBox__ - 对象的最小边界框
* __BoundingSphere__ - 物体的边界球
### 方法
* __Copy__ - 创建对象的副本
* __BoundingBox__ - 给定坐标系中的轴对齐边界框 (AABB)
* __IsInside__ - 检查圆是否位于盒子内部
* __ClosestPoint__ - 圆表面上距离目标点最近的点
* __ParametricForm__ - 返回给定参数“t”的圆上的点
* __ProjectionTo__ - 圆到平面或线的正交投影
* __DistanceTo__ - 到点、平面、圆、球体、盒子或三角形的最短距离
* __Intersects__ - 与盒子、三角形、圆形和球体进行相交检查
* __IntersectionWith__ - 圆与线、平面、线段或其他圆的交点
* __IsParallelTo__ - 检查两个物体是否平行
* __IsNotParallelTo__ - 检查两个物体是否不平行
* __IsOrthogonalTo__ - 检查两个对象是否正交
* __IsCoplanarTo__ - 检查圆是否与其他线性或平面物体共面
* __AngleTo__ - 两个物体之间的角度
* __AngleToDeg__ - 两个物体之间的角度(以度为单位)
* __Translate__ - 通过矢量平移圆
* __Rotate__ - 围绕原点或其他点旋转圆
* __反射__ - 在点、线或平面上反射圆
* __Equals__ - 检查两个圆是否相等
* __ToString__ - 全局或局部坐标系中圆的字符串表示

## Ellipse - 椭圆

三维空间中的椭圆，由中心点和两个正交向量(长半轴和短半轴)定义。
### 特性
* __Center__ - 椭圆的中心
* __MajorSemiaxis__ - 椭圆的长半轴
* __MinorSemiaxis__ - 椭圆的短半轴
* __Normal__ - 椭圆的法线
* __A__ - 长半轴的长度
* __B__ - 短半轴的长度
* __F__ - 从中心到焦点的距离
* __F1__ - 第一个焦点
* __F2__ - 第二焦点
* __e__ - 椭圆的偏心率
* __周长__ - 椭圆的近似周长
* __Area__ - 椭圆的面积
* __MinimumBoundingBox__ - 对象的最小边界框
* __BoundingSphere__ - 物体的边界球
### 方法
* __Copy__ - 创建对象的副本
* __BoundingBox__ - 给定坐标系中的轴对齐边界框 (AABB)
* __ParametricForm__ - 返回给定参数“t”的椭圆上的点
* __ProjectionTo__ - 椭圆到平面或线的正交投影
* __IntersectionWith__ - 椭圆与线、平面或线段的交点
* __ClosestPoint__ - 计算椭圆边界上距离给定点最近的点
* __IsParallelTo__ - 检查两个物体是否平行
* __IsNotParallelTo__ - 检查两个物体是否不平行
* __IsOrthogonalTo__ - 检查两个对象是否正交
* __IsCoplanarTo__ - 检查椭圆是否与其他线性或平面物体共面
* __AngleTo__ - 两个物体之间的角度
* __AngleToDeg__ - 两个物体之间的角度(以度为单位)
* __Translate__ - 通过矢量平移椭圆
* __Rotate__ - 围绕原点或其他点旋转椭圆
* __反射__ - 在点、线或平面上反射椭圆
* __Equals__ - 检查两个椭圆是否相等
* __ToString__ - 全局或局部坐标系中椭圆的字符串表示

## Ellipsoid - 椭圆体

由中心点和三个相互正交的向量定义的椭圆体物体。
### 特性
* __Center__ - 椭圆体的中心
* __SemiaxisA/B/C__ - 椭圆体的半轴
* __A/B/C__ - 椭圆体半轴的长度
* __Area__ - 椭圆体的近似表面积
* __Volume__ - 椭圆体的体积
* __MinimumBoundingBox__ - 对象的最小边界框
* __BoundingSphere__ - 物体的边界球
### 方法
* __Copy__ - 创建对象的副本
* __BoundingBox__ - 给定坐标系中的轴对齐边界框 (AABB)
* __ProjectionTo__ - 椭圆体到线的正交投影
* __IntersectionWith__ - 椭圆体与线、平面或线段的交点
* __ClosestPoint__ - 计算椭圆体边界上距离给定点最近的点
* __Translate__ - 通过矢量平移椭圆体
* __Rotate__ - 围绕原点或其他点旋转椭圆体
* __Reflect__ - 在点、线或平面上反射椭圆体
* __Equals__ - 检查两个椭圆体是否相等
* __ToString__ - 全局或局部坐标系中椭圆体的字符串表示

## Box3d – 长方体

由中心点、三维尺寸和空间方向定义的盒子物体。
### 特性
* __Center__ - 盒子的中心点
* __L1/L2/L3__ - 盒子的尺寸
* __V1/V2/V3__ - 盒子的方向向量
* __Orientation__ - 盒子方向
* __P1/P2/P3/P4/P5/P6/P7/P8__ - 盒子的角点
* __ListOfPoints__ - 框的角点列表
* __ListOfTriangles__ - 构成盒子表面的三角形列表
* __ListOfPlanes__ - 构成盒子表面的平面列表
* __ListOfEdges__ - 边列表
* __Area__ - 盒子的面积
* __Volume__ - 盒子的体积
* __MinimumBoundingBox__ - 对象的最小边界框
* __BoundingSphere__ - 物体的边界球
* __IsAxisAligned__ - 检查框是否为 AABB
### 方法
* __Copy__ - 创建对象的副本
* __BoundingBox__ - 给定坐标系中的轴对齐边界框 (AABB)
* __DistanceTo__ - 到点、圆或球的最短距离
* __Intersects__ - 与盒子、圆形、四面体或三角形进行交点检查
* __IntersectionWith__ - 框与线、射线或线段的交点
* __Translate__ - 通过矢量翻译框
* __Rotate__ - 围绕原点或其他点旋转盒子
* __Reflect__ - 在点、线或平面上反射框
* __Equals__ - 检查两个椭圆体是否相等
* __ToString__ - 全局或局部坐标系中椭圆体的字符串表示
### 静态方法
* __AABB__ - 点云的轴对齐边界框

## 反义词

与轴对齐的 3D 框，可以退化为一个或多个等于 0 的维度。仅在 Global CS 中定义。
### 特性
* __Center__ - 盒子的中心点
* __L1/L2/L3__ - 盒子的尺寸
* __V1/V2/V3__ - 盒子的方向向量
* __Orientation__ - 盒子方向
* __P1/P2/P3/P4/P5/P6/P7/P8__ - 盒子的角点
* __ListOfPoints__ - 框的角点列表
* __ListOfTriangles__ - 构成盒子表面的三角形列表
* __ListOfPlanes__ - 构成盒子表面的平面列表
* __ListOfEdges__ - 边列表
* __Area__ - 盒子的面积
* __Volume__ - 盒子的体积
### 方法
* __DistanceTo__ - 到点、圆或球的最短距离
* __Intersects__ - 与盒子、圆形、四面体或三角形进行交点检查
* __IntersectionWith__ - 框与线、射线或线段的交点
* __Translate__ - 通过矢量翻译框
* __Rotate__ - 围绕原点或其他点旋转盒子
* __Reflect__ - 在点、线或平面上反射框
* __Equals__ - 检查两个椭圆体是否相等
* __ToString__ - 全局或局部坐标系中椭圆体的字符串表示

## Triangle - 三角形

在三维空间中定义一个三角形。实现常见的平移、旋转和反射方法。计算大部分标准
三角形属性：角平分线、中线、高、内心、外心、重心、垂心等。
### 特性
* __A/B/C__ - 三角形的顶点
* __AB/AC/BC__ - 三角形边的长度
* __周长__ - 三角形的周长
* __Area__ - 三角形的面积
* __Circumcircle__ - 三角形的外接圆
* __Angle_A/B/C__ - 顶点 A/B/C 处的角度
* __Bisector_A/B/C__ - 顶点 A/B/C 处的角平分线
* __内心__ - 三角形内心
* __Centroid__ - 三角形的质心
* __垂心__ - 三角形的垂心
* __外心__ - 三角形的外心
* __Incircle__ - 三角形的内切圆
* __Altitude_A/B/C__ - 顶点 A/B/C 处的高度
* __Median_A/B/C__ - 顶点 A/B/C 处的中位数
* __IsEquilateral__ - 检查三角形的所有边是否长度相同
* __IsIsosceles__ - 检查三角形的两条边是否长度相同
* __IsScalene__ - 检查所有边是否不相等
* __IsRight__ - 检查一个角度是否等于 90 度
* __IsObtuse__ - 检查一个角度是否大于 90 度
* __IsAcute__ - 检查所有角度是否小于 90 度
* __MinimumBoundingBox__ - 对象的最小边界框
* __BoundingSphere__ - 物体的边界球
### 方法
* __Copy__ - 创建对象的副本
* __BoundingBox__ - 给定坐标系中的轴对齐边界框 (AABB)
* __DistanceTo__ - 到点、线段、三角形和圆的最短距离
* __IntersectionWith__ - 三角形与线、平面、射线或线段的交点
* __Intersects__ - 与三角形、盒子、四面体、球体或圆形进行交点检查
* __ProjectionTo__ - 三角形到线的正交投影
* __IsParallelTo__ - 检查两个物体是否平行
* __IsNotParallelTo__ - 检查两个物体是否不平行
* __IsOrthogonalTo__ - 检查两个对象是否正交
* __IsCoplanarTo__ - 检查三角形是否与其他线性或平面物体共面
* __AngleTo__ - 两个物体之间的角度
* __AngleToDeg__ - 两个物体之间的角度(以度为单位)
* __Translate__ - 通过向量平移三角形
* __Rotate__ - 围绕原点或其他点旋转三角形
* __反射__ - 在点、线或平面上反射三角形
* __Equals__ - 检查两个三角形是否相等
* __ToString__ - 全局或局部坐标系中三角形的字符串表示

## Tetrahedron - 四面体

在三维空间中定义一个四面体。实现常见的平移、旋转和反射方法。
### 特性
* __A/B/C/D__ - 四面体的顶点
* __中心__ - 四面体的质心
* __ListOfEdges__ - 边列表
* __ListOfFaces__ - 面孔列表
* __Area__ - 四面体的面积
* __Volume__ - 四面体的体积
### 方法
* __Copy__ - 创建对象的副本
* __BoundingBox__ - 给定坐标系中的边界框
* __AABB__ - 轴对齐边界框 (AABB)
* __DistanceTo__ - 到点和四面体的最短距离
* __ClosestPoint__ - 计算四面体边界上距离给定点最近的点
* __Intersects__ - 与三角形、四面体、直线、射线、线段、盒子和球体进行相交检查
* __IsInside__ - 检查物体是否位于给定的框内
* __Translate__ - 通过矢量平移四面体
* __Rotate__ - 围绕原点或其他点旋转四面体
* __反射__ - 在点、线或平面上反射四面体
* __Scale__ - 比例四面体
* __Equals__ - 检查两个四面体是否相等
* __ToString__ - 全局或局部坐标系中四面体的字符串表示

## ConvexPolyhedron - 凸多面体

在 3D 空间中定义一个凸多面体，其面沿逆时针方向(从外部看)。
### 特性
* __中心__ - 多面体的质心
* __ListOfEdges__ - 边列表
* __Area__ - 多面体的面积
* __Volume__ - 多面体的体积
### 方法
* __Copy__ - 创建对象的副本
* __BoundingBox__ - 给定坐标系中的边界框
* __AABB__ - 轴对齐边界框 (AABB)
* __DistanceTo__ - 到点、三角形和多面体的最短距离
* __Intersects__ - 与多面体和盒子进行相交检查
* __IsInside__ - 检查物体是否位于给定的框内
* __Translate__ - 通过向量平移多面体
* __Rotate__ - 围绕原点或其他点旋转多面体
* __Scale__ - 比例多面体
### 静态方法
* __FromTetrahedron__ - 从 Tetrahedron 对象创建 ConvexPolyhedron 对象
* __FromBox__ - 从 Box3d 对象创建 ConvexPolyhedron 对象
* __Octahedron__ - 创建以原点为中心的正八面体
* __二十面体__ - 创建以原点为中心的正二十面体
* __Dodecahedron__ - 创建以原点为中心的正十二面体

## Coord3d - 坐标3d

表示正交笛卡尔三维坐标系的类。由原点和变换矩阵定义
(以行格式存储的三个正交单位向量)。默认定义一个全局坐标系(Coord3d.GlobalCS)，
用户可以定义任意数量的局部坐标系。
### 特性
* __Origin__ - 坐标系的原点
* __Axes__ - 轴的单位向量，存储为行矩阵(3x3)
* __Name__ - 坐标系的名称
* __Counts__ - 定义的坐标系总数
* __X/Y/Zaxis__ - 坐标系的 X/Y/Z 轴
* __XY/XZ/YZ_plane__ - 坐标系的平面
### 方法
* __Copy__ - 创建对象的副本
* __Rotate/RotateDeg__ - 绕旋转轴旋转坐标系
* __Equals__ - 检查两个坐标系是否相等
* __ToString__ - 坐标系的字符串表示

## Matrix3d - 矩阵3d

### 特性
* __Item(i,j)__ - 矩阵元素
* __Row1/2/3__ - 矩阵的行
* __Column1/2/3__ - 矩阵的列
* __Det__ - 矩阵的行列式
* __MaxNorm__ - 矩阵的最大范数
* __IsZero__ - 检查矩阵是否为零矩阵
* __IsIdentity__ - 检查矩阵是否为单位矩阵
* __IsOrthogonal__ - 检查矩阵是否正交
### 方法
* __Inverse__ - 矩阵的逆
* __Transpose__ - 矩阵的转置
* __RotationMatrix__ - 围绕给定轴旋转
* __Equals__ - 检查两个矩阵是否相等
* __ToString__ - 矩阵的字符串表示
### 静态方法
* __Identity__ - 创建新的单位矩阵
* __DiagonalMatrix__ - 创建对角矩阵

## Quaternion - 四元数
单位四元数 (W + X*i + Y*j + Z*k)
### 特性
* __W/X/Y/Z__ - 参考坐标系中的四元数分量
* __Coord__ - 参考坐标系
* __Norm__ - 四元数的范数
* __SquareNorm__ - 四元数范数的平方
* __Conjugate__ - 四元数的共轭
* __ToAxis__ - 参考坐标系中的旋转轴
* __ToAngle__ - 参考坐标系中的旋转角度
* __Normalized__ - 返回标准化四元数
### 方法
* __Copy__ - 创建对象的副本
* __ConvertTo__ - 将四元数转换为局部坐标系
* __ConvertToGlobal__ - 将四元数转换为全局坐标系
* __Normalize__ - 标准化当前四元数
* __Add/Subtract/Mult__ - 算术运算
* __Scale__ - 按数字缩放四元数
* __Inverse__ - 逆四元数
* __ToRotationMatrix__ - 转换为旋转矩阵
* __Equals__ - 检查两个四元数是否相等
* __ToString__ - 全局或局部坐标系中四元数的字符串表示
### 静态方法
* __SLERP__ - 两个四元数的球面线性插值

## Rotation – 旋转
在全局或局部参考系中定义的 3D 空间中的旋转(内部由旋转矩阵表示)
### 特性
* __Coord__ - 参考坐标系
* __ToAxis__ - 参考坐标系中的旋转轴
* __ToAngle__ - 参考坐标系中的旋转角度
* __ToRotationMatrix__ - 旋转矩阵
* __ToQuaternion__ - 转换为四元数
### 方法
* __Copy__ - 创建对象的副本
* __ConvertTo__ - 将旋转转换为局部坐标系
* __ConvertToGlobal__ - 将旋转转换为全局坐标系
* __Mult__ - 对点或矢量应用旋转
* __ToEulerAngles__ - 将旋转矩阵分解为三个元素旋转的乘积
* __Equals__ - 检查两个旋转是否相等
* __ToString__ - 全局或局部坐标系中旋转的字符串表示
### 静态方法
* __FromEulerAngles__ - 根据欧拉角或 Tait-Bryan 角创建旋转对象
* __SLERP__ - 两个旋转的球面线性插值

## GeometRi3D - 容差

### 特性
* __Tolerance__ - 用于比较操作的容差(默认 1e-12)
### 方法
* __AlmostEqual__ - 基于容差的平等检查
* __NotEqual__ - 基于容差的不平等检查
* __Greater__ - 基于公差的比较
* __Smaller__ - 基于公差的比较
