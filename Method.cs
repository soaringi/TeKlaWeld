using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Solid;
using Parallel = Tekla.Structures.Geometry3d.Parallel;
using Point = Tekla.Structures.Geometry3d.Point;

namespace TeKlaWeld
{
    internal class Method
    {
        public static Dictionary<Face, List<Point>> GetPoints(Part part)//获取零件轮廓面及轮廓点
        {
            var dic = new Dictionary<Face, List<Point>>();
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

                dic.Add(p, points);
            }
            return dic;
        }
        public static bool IsContain(List<Point> pointsone, List<Point> pointstwo)
        {
            ChangePositions(ref pointsone);
            ChangePositions(ref pointstwo);
            var list = pointsone.Union(pointstwo);
            var q =list.Select(x => x.X).Distinct().Count();
            var qq=list.Select(x => x.Y).Distinct().Count();
             var qqq=list.Select(x => x.Z).Distinct().Count();
            GeometricPlane plane1 = new GeometricPlane(
                new Point(pointsone[0]),
                new Vector(pointsone[1] - pointsone[0]),
                new Vector(pointsone[2] - pointsone[0])
                );
            GeometricPlane plane2 = new GeometricPlane(
                new Point(pointstwo[0]),
                new Vector(pointstwo[1] - pointstwo[0]),
                new Vector(pointstwo[2] - pointstwo[0])
                 );
            if (q==1||qq==1||qqq==1)
            {
                if (Parallel.PlaneToPlane(plane1, plane2, 0))
                {
                    return true;
                }
            }

            return false;
        }

        static void ChangePositions(ref List<Point> p)
        {
            for (int i = 0; i < p.Count - 1; i++)
            {
                p[i].X = Math.Round(p[i].X, 2);
                p[i].Y = Math.Round(p[i].Y, 2);
                p[i].Z = Math.Round(p[i].Z, 2);
            }
        }

    }
}
