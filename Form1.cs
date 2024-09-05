using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tekla.Structures.Model;
using Tekla.Structures.Model.UI;
using Tekla.Structures.Geometry3d;
using ModelObjectSelector = Tekla.Structures.Model.UI.ModelObjectSelector;


namespace TeKlaWeld
{
    public partial class Form1 : Form
    {
        Model Model;
        public Form1()
        {
            InitializeComponent();
            Model= new Model();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Model==null)
            {
                Model = new Model();
            }
            ModelObjectSelector se= new ModelObjectSelector();
            var enumm=se.GetSelectedObjects();
            List<Assembly> assemblies = new List<Assembly>();
            while (enumm.MoveNext())
            {
                var  part= enumm.Current as Assembly;
                if (part==null)
                {
                    continue;
                }
                var array= part.GetSecondaries();
                array.Add(part.GetMainPart());
                Method.SetAssembly(array);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
                Picker picker = new Picker();
            var p = picker.PickObject(Picker.PickObjectEnum.PICK_ONE_PART) as Part;
            var p2 = picker.PickObject(Picker.PickObjectEnum.PICK_ONE_PART) as Part;
            var po1= Method.GetPoints(p);
            var po2 = Method.GetPoints(p2);
            var i = 0;
            foreach (var item in po1)
            {
                foreach (var item1 in po2)
                {
                    //if (Method.IsContain(item.Value,item1.Value) )
                    //{
                    //    i++;

                    //}
                }
            }
        }
    }
}
