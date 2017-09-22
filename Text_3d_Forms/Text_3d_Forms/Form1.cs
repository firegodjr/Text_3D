using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Text_3D_Renderer;
using Text_3D_Renderer.Rendering;

namespace Text_3d_Forms
{
    public partial class Form1 : Form
    {
        public static int framecount;
        public Form1()
        {
            InitializeComponent();
            Renderer.Setup(aspect: 1.3f);
            Renderer.screenMgr.changeScreenSize(90, 30);

            renderTick.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double deltatime = Renderer.deltatime;
            label1.Text = Renderer.Draw();
            //Render();
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            label1.Width = Width - 14;
            label1.Height = Height - 37;
        }

        public async Task Render()
        {
            label1.Text = await Task.FromResult(Renderer.Draw());
            framecount++;
        }


    }
}
