using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using TraceLab.UI.WPF.Views.PackageBuilder;

namespace PackageBuilder
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var app = new Application();
            Window wind = new PackageBuilderMainWindow();

            app.Run(wind);
        }
    }
}
