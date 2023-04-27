using System;
using System.Windows.Forms;
using Comun;
using Microsoft.VisualBasic;

namespace MPSER
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!new _MPCLogica().Main()) return;
            Sistema.sysDBInUse = Sistema.sysDBBase;
            //Console.WriteLine("PUCHVALL" + "month" +"today" +"Hora mas 20");
            Application.Run(new MPTCP());
        }
    }
}
