using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gdk;
using Gtk;

namespace PENTE
{
    public class Pente
    {
        static void Main() {
            Application.Init();
            MyWindow w = new MyWindow();
            w.ShowAll();
            Application.Run();
        }
    }
}

