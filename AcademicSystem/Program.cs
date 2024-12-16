using System;
using System.Windows.Forms;

namespace AcademicSystem
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1()); // Inicia tu formulario principal
        }
    }
}
