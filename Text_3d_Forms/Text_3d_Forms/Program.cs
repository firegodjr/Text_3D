using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Text_3d_Forms.Files;

namespace Text_3D_Engine
{
    static class Program
    {
        public static SaveData settings;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            settings = new SaveData();
            settings.LoadXML("cfg.xml");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            GameWindow g2 = new GameWindow();
            Application.Run(g2);

            settings.SaveXML("cfg.xml");
        }
    }
}
