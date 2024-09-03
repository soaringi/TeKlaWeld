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
            ModelObjectSelector se= new ModelObjectSelector  ();
            var enumm=se.GetSelectedObjects();
            List<Assembly> assemblies = new List<Assembly>();
            while (enumm.MoveNext())
            {
                var  part= enumm.Current as Part;
                if (part==null)
                {
                    continue;
                }
                var ass=part.GetAssembly();

            }
        }
    }
}
