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
            var list = pointsone.Union(pointstwo).ToList();
            ChangePositions(ref list);
            var x = list.Select(p => p.X).Distinct().Count();
            var y = list.Select(p => p.Y).Distinct().Count();
            var z = list.Select(p => p.Z).Distinct().Count();
            LinearRing linear = new LinearRing(pointsone);
            return false;
        }
        LinearRing Chane(List<Point> p,string str)
        {
            Coordinate coordinate = new Coordinate();
            return new LinearRing();

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
