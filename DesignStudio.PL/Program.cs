using System;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DesignStudio.PL
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}
