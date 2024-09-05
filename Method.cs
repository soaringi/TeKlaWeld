using NetTopologySuite.Geometries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Solid;
using Parallel = Tekla.Structures.Geometry3d.Parallel;
using Intersection = Tekla.Structures.Geometry3d.Intersection;
using Point = Tekla.Structures.Geometry3d.Point;
using Vector = Tekla.Structures.Geometry3d.Vector;
using NetTopologySuite.Operation.Union;

namespace TeKlaWeld
{
    internal class Method
    {
        static bool IsPlaneInsideAnotherPlane(List<Point> plane1, List<Point> plane2)
        {
            // 找到每个平面的最小和最大的 X、Y、Z 值
            double minX1 = plane1.Min(p => p.X);
            double minY1 = plane1.Min(p => p.Y);
            double minZ1 = plane1.Min(p => p.Z);
            double maxX1 = plane1.Max(p => p.X);
            double maxY1 = plane1.Max(p => p.Y);
            double maxZ1 = plane1.Max(p => p.Z);
            double minX2 = plane2.Min(p => p.X);
            double minY2 = plane2.Min(p => p.Y);
            double minZ2 = plane2.Min(p => p.Z);
            double maxX2 = plane2.Max(p => p.X);
            double maxY2 = plane2.Max(p => p.Y);
            double maxZ2 = plane2.Max(p => p.Z);

            // 检查第一个平面的最小值和最大值是否都在第二个平面的范围内
            if (minX1 >= minX2 && minY1 >= minY2 && minZ1 >= minZ2 &&
                maxX1 <= maxX2 && maxY1 <= maxY2 && maxZ1 <= maxZ2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static List<List<Point>> GetPoints(Part part)//获取零件轮廓面及轮廓点
        {
            var dic = new List<List<Point>>();
            var solid = part.GetSolid();
            var face = solid.GetFaceEnumerator();
            while (face.MoveNext())
            {
                var p = face.Current as Face;
                var pp = p.GetLoopEnumerator();
                List<Point> points = new List<Point>();
                while (pp.MoveNext())
                {
                    var ppp = pp.Current as Loop;
                    var vertex = ppp.GetVertexEnumerator();
                    while (vertex.MoveNext())
                    {
                        var point = vertex.Current;
                        points.Add(point);
                    }
                }

                dic.Add(points);
            }
            return dic;
        }
        public static int IsContain(List<List<Point>> pointsone, List<List<Point>> pointstwo)
        {
            int num = 0;
            Dictionary<List<Point>, List<Point>> key = new Dictionary<List<Point>, List<Point>>();
            List<List<Point>> os = new List<List<Point>>();
            for (int i = 0; i < pointsone.Count; i++)
            {
                for (int j = 0; j < pointstwo.Count; j++)
                {
                    pointsone[i] = ChangePositions(pointsone[i]);
                    pointstwo[j] = ChangePositions(pointstwo[j]);
                    var list = pointsone[i].Union(pointstwo[j]);
                    var x1 = list.Select(x => x.X).Distinct().Count();
                    var y1 = list.Select(x => x.Y).Distinct().Count();
                    var z1 = list.Select(x => x.Z).Distinct().Count();
                    GeometricPlane plane1 = new GeometricPlane(
                        new Point(pointsone[i][0]),
                        new Vector(pointsone[i][1] - pointsone[i][0]),
                        new Vector(pointsone[i][2] - pointsone[i][0])
                        );
                    GeometricPlane plane2 = new GeometricPlane(
                        new Point(pointstwo[j][0]),
                        new Vector(pointstwo[j][1] - pointstwo[j][0]),
                        new Vector(pointstwo[j][2] - pointstwo[j][0])
                         );
                    if (x1 == 1 || y1 == 1 || z1 == 1)
                    {
                        if (Parallel.PlaneToPlane(plane1, plane2, 0))
                        {
                            if (IsPlaneInsideAnotherPlane(pointsone[i], pointstwo[j])
                                || IsPlaneInsideAnotherPlane(pointstwo[j], pointsone[i]))
                            {
                                List<Point> points = new List<Point>();
                                Point point = new Point();
                                Point point1 = new Point();
                                if (IsPlaneInsideAnotherPlane(pointsone[i], pointstwo[j]))
                                {
                                    var xmin = x1 == 1 ? pointsone[i].Average(x => x.X) : pointsone[i].Min(x => x.X);
                                    var xmax = x1 == 1 ? pointsone[i].Average(x => x.X) : pointsone[i].Max(x => x.X);
                                    var ymin = y1 == 1 ? pointsone[i].Average(x => x.Y) : pointsone[i].Min(x => x.Y);
                                    var ymax = y1 == 1 ? pointsone[i].Average(x => x.Y) : pointsone[i].Max(x => x.Y);
                                    var zmin = z1 == 1 ? pointsone[i].Average(x => x.Z) : pointsone[i].Min(x => x.Z);
                                    var zmax = z1 == 1 ? pointsone[i].Average(x => x.Z) : pointsone[i].Min(x => x.Z);
                                    point = new Point(xmin, ymin, zmin);
                                    point1=new Point(xmax, ymax, zmax);
                                    points.Add(point);
                                    points.Add(point1);
                                }
                                if (IsPlaneInsideAnotherPlane(pointstwo[j], pointsone[i]))
                                {
                                    var xmin = x1 == 1 ? pointstwo[i].Average(x => x.X) : pointstwo[i].Min(x => x.X);
                                    var xmax = x1 == 1 ? pointstwo[i].Average(x => x.X) : pointstwo[i].Max(x => x.X);
                                    var ymin = y1 == 1 ? pointstwo[i].Average(x => x.Y) : pointstwo[i].Min(x => x.Y);
                                    var ymax = y1 == 1 ? pointstwo[i].Average(x => x.Y) : pointstwo[i].Max(x => x.Y);
                                    var zmin = z1 == 1 ? pointstwo[i].Average(x => x.Z) : pointstwo[i].Min(x => x.Z);
                                    var zmax = z1 == 1 ? pointstwo[i].Average(x => x.Z) : pointstwo[i].Min(x => x.Z);
                                    point = new Point(xmin, ymin, zmin);
                                    point1 = new Point(xmax, ymax, zmax);
                                    points.Add(point);
                                    points.Add(point1);
                                }
                            }

                            num++;
                        }
                    }


                }
            }
            if (num > 1)
            {

            }
            return num; ;
        }

        static List<Point> ChangePositions(List<Point> p)
        {
            for (int i = 0; i < p.Count - 1; i++)
            {
                p[i].X = Math.Round(p[i].X, 2);
                p[i].Y = Math.Round(p[i].Y, 2);
                p[i].Z = Math.Round(p[i].Z, 2);
            }
            return p;
        }
        public static void SetAssembly(ArrayList array)
        {
            ArrayList arrayList = new ArrayList();
            arrayList.CopyTo(array.ToArray());
            foreach (Part partOne in array)
            {

                foreach (Part partScend in arrayList)
                {
                    if (partOne != partScend)
                    {
                        var dic = GetPoints(partOne);
                        var dic2 = GetPoints(partScend);
                        IsContain(dic, dic2);
                    }
                }
            }
        }
        static void ChangePositions()
        {
        }
    }
}