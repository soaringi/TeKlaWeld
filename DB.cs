using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;

namespace TeKlaWeld
{
    public class Weld
    { 
        public string AssNo {  get; set; }
        public string profile { get; set; }
        public string Material { get; set; }
        public decimal MainLength {  get; set; }
        public string WeldNo { get; set; }
        public string ConnectPart { get; set; }
        public string WeldLength {  get; set; }
        public PolygonWeld weld { get; set; }
    }
}
