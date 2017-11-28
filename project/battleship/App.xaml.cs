using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace battleship
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>

    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            startWindow start = new startWindow();
            mainGameWindow app = new mainGameWindow();
            Leaderboard leaderboard = new Leaderboard();

            start.Visibility = Visibility.Visible;
            app.Visibility = Visibility.Hidden;
            leaderboard.Visibility = Visibility.Hidden;
        }
    }
}
