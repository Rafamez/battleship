using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using battleship;

namespace battleship
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        startWindow login = new startWindow();
        mainGameWindow application = new mainGameWindow();
        Leaderboard leaderboard = new Leaderboard();

        public MainWindow()
        {
            Hide();
            login.Visibility = Visibility.Visible;




















        }



    }
}