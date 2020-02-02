using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace StringsExplorer
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //  Set maximum priority
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;

        }
    }
}
