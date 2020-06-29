using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Resize_Move_Control_Create_Params
{
    class MyPanel : Panel
    {


        // Part of Moving Panel-----------------::START::-------------------------------------------------------------

        // Make the Minimum Size Work
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        // Part of Moving Panel-----------------::END::-------------------------------------------------------------









        // Constructor
        public MyPanel()
        {
            //SetStyle(ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor, true);
            //this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor, true);
            //typeof(MyPanel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, this, new object[] { true }); // Double buffer MyPanel
            TitleBar(); // Load TitleBar

            //this.Padding = new Padding(8);
            this.MinimumSize = new System.Drawing.Size(200, 200); // Minimum Size to The Panel
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }



   







        // Redraw Parrent so the Panel dont disapears on move "Code for better movement of the panel"
        protected override void OnMove(EventArgs e)
        {
            this.Parent.Invalidate();
            base.OnMove(e);

        }








   
        // ::Resize function for the panel - "Resizable Panel"::
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= 0x20000; // "Part of Borderless Resize"
                //cp.ExStyle = cp.ExStyle | 0x20;  // Transparrent
                //cp.Style |= (int)0x00040000L;  // Turn on WS_BORDER + WS_THICKFRAME
                //cp.Style |= (int)0x00C00000L;  // Move
                //cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;

            }
        }



     



        // ::The Title Bar::
        private void TitleBar()
        {
            //Panel titleBar = new Panel();
            //titleBar.BackColor = Color.Black;
            //titleBar.Size = new Size(this.Size.Width, 20);
            //titleBar.Region = new Region(new Rectangle(5,5, this.Size.Width-5, 20));
            //titleBar.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            //this.Controls.Add(titleBar);

            //titleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDownTitleBar); // Mouse Down - Event
            //typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, titleBar, new object[] { true }); // Double Buffered 

      
            
            //Graphics g = this.CreateGraphics();

            //Pen rectanglePen = new Pen(Color.Lime, 3);
            //Rectangle topREC = new Rectangle(0, 0, this.Width, this.Height);
            //g.DrawRectangle(rectanglePen, topREC);



            //this.Paint += (o, e) => {
            //    Graphics g = e.Graphics;
            //    using (SolidBrush selPen = new Brush(Color.Blue))
            //    {
            //        g.FillRectangle(selPen, 5, 5, this.Width, this.Height);
            //    }
            //};



            // TitleBar - Rectangle
            this.Paint += (o, e) => {
                Graphics g = e.Graphics;
                using (Brush brush1 = new SolidBrush(Color.Blue))
                {
                    g.FillRectangle(brush1, 0, 0, this.Width, 25);
                }
            };

          

        }






        // ::Move Panel::      // TitleBar Move Panel On Mouse Drag
        private void MouseDownTitleBar(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }








        // Part of "Make the Minumum Size - Work" ------::START::-----------------------------------------------------------------------------
        public const int WM_GETMINMAXINFO = 0x24;

        public struct POINTAPI
        {
            public Int32 X;
            public Int32 Y;
        }


        public struct MINMAXINFO
        {
            public POINTAPI ptReserved;
            public POINTAPI ptMaxSize;
            public POINTAPI ptMaxPosition;
            public POINTAPI ptMinTrackSize;
            public POINTAPI ptMaxTrackSize;
        }

        // Part of "Make the Minumum Size - Work" ------::END::-----------------------------------------------------------------------------








        // Rezize :: Make the Minimum size work     AND    // RESIZE Without Border
        protected override void WndProc(ref Message m)
        {
            

        

            const int RESIZE_HANDLE_SIZE = 5;// Resize Handle  "Part of Resize without Border"

            switch (m.Msg)
            {
                 

                // Make the Minimum Size Work -------::START::-------------------------------------------------------------------------------------
                case WM_GETMINMAXINFO:
                    MINMAXINFO mmi = (MINMAXINFO)System.Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, typeof(MINMAXINFO));
                    mmi.ptMinTrackSize.X = this.MinimumSize.Width;
                    mmi.ptMinTrackSize.Y = this.MinimumSize.Height;
                    System.Runtime.InteropServices.Marshal.StructureToPtr(mmi, m.LParam, true);
                    break;
                // Make the Minimum Size Work -------::END::-------------------------------------------------------------------------------------








                // Resize Panel without Border ----------------::START::---------------------------------------------------------------



                    
                case 0x0084: /*NCHITTEST*/  
                    base.WndProc(ref m);

              
                    if ((int)m.Result == 0x01/*HTCLIENT*/)
                    {
                        Point screenPoint = new Point(m.LParam.ToInt32());
                        Point clientPoint = this.PointToClient(screenPoint);

                        
                        // Top
                        if (clientPoint.Y <= RESIZE_HANDLE_SIZE) // TOP - Resize      //"+2 for Top area of the resize so the resize cursor will apear in smaller area if - and + for bigger"     // +50 the height of the resize container
                        {

                            if ((clientPoint.Y <= RESIZE_HANDLE_SIZE + 15) && (clientPoint.X <= RESIZE_HANDLE_SIZE + 10)) // Top - Left Corner
                                m.Result = (IntPtr)13/*HTTOPLEFT*/ ;


                            else if ((clientPoint.X >= (Size.Width - 20))) // Top - Right Corner
                                m.Result = (IntPtr)14/*HTTOPRIGHT*/ ;


                            else if (clientPoint.Y <= RESIZE_HANDLE_SIZE) // TOP
                                m.Result = (IntPtr)12/*HTTOP*/ ;

                        }



                        // Left / Right / Moving
                        else if (clientPoint.Y <= (Size.Height - RESIZE_HANDLE_SIZE)) // Left and Right
                        {

                            if (clientPoint.X <= RESIZE_HANDLE_SIZE + 8) // The left side is startig at 0
                                m.Result = (IntPtr)10/*HTLEFT*/ ;


                            else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE)) // This is the middle "Dont include Top, Left, Right, Bottom" only middle // The middle moves the panel on drag  
                                m.Result = (IntPtr)2/*HTCAPTION*/ ;
                                 
                         

                            if (clientPoint.X >= this.Width - 13)
                                m.Result = (IntPtr)11/*HTRIGHT*/ ;
                        }


                        // Bottom
                        if (clientPoint.Y >= (Size.Height - (RESIZE_HANDLE_SIZE + 8)))   // Bottom , Bottom Left Corner, Bottom Right Corner
                        {
                            if (clientPoint.X <= RESIZE_HANDLE_SIZE + 10)  //Left Corner
                                m.Result = (IntPtr)16/*HTBOTTOMLEFT*/ ;

                            else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE - 8))  // Bottom    // Checking the bottom not the left ot right corner  // Adding 8 Pixels to the corner area so its easier to select the resize
                                m.Result = (IntPtr)15/*HTBOTTOM*/ ;

                            else
                                m.Result = (IntPtr)17/*HTBOTTOMRIGHT*/ ;
                        }
                    }


                    return;
                    // Resize Panel without Border ----------------::END::---------------------------------------------------------------

            }

            base.WndProc(ref m);


        }








    }
}
