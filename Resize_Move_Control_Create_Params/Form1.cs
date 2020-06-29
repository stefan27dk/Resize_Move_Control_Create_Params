using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Resize_Move_Control_Create_Params
{
    public partial class Form1 : Form
    {
        MyPanel MyPanel1;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Moving_Panel();

            panel1.MouseDown += new MouseEventHandler(MyPanel_MouseDown);
        }



      


    // Moving_Reizable Panel
    private void Moving_Panel()
        {
            MyPanel1 = new MyPanel();
            MyPanel1.BackColor = Color.ForestGreen;
            MyPanel1.Size = new Size(350, 350);

            //MyPanel1.MouseDown += new MouseEventHandler(MyPanel_MouseDown);

            this.Controls.Add(MyPanel1);
            MyPanel1.BringToFront();

        }


        // Mouse Down
        private void MyPanel_MouseDown(object sender, MouseEventArgs e)
        {
            MyPanel1.BackColor = Color.Orange;
            panel1.BackColor = Color.Orange;

            // Paste the below code in the your label control MouseDown event
       
        }

       
    }
}
