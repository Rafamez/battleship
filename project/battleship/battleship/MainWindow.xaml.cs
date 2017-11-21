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

namespace battleship
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        double size = 0;
        int difficulty = 0;
        System.Timers.Timer T = new System.Timers.Timer();
        //the time passed
        int GameTime = 0;



        public MainWindow()
        {
            InitializeComponent();
            size = mainWindow.ActualWidth;
            //Call method to change value of GameTime when event is met
            T.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            //set the interval to 1000
            T.Interval = 1000;
        }

      /*  private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if ((PlayerGrid.Margin.Right + AIGrid.Margin.Left < PlayerGrid.ActualWidth + AIGrid.ActualWidth) && (size - mainWindow.ActualWidth > 0))
            {
                PlayerGrid.Margin = new Thickness(PlayerGrid.Margin.Left, PlayerGrid.Margin.Top, size - mainWindow.ActualWidth, PlayerGrid.Margin.Bottom);
                AIGrid.Margin = new Thickness(size - mainWindow.ActualWidth, AIGrid.Margin.Top, AIGrid.Margin.Right, AIGrid.Margin.Bottom);
            }
            else
            {
                PlayerGrid.Margin = new Thickness(PlayerGrid.Margin.Left, PlayerGrid.Margin.Top, PlayerGrid.ActualWidth / 2 + 400, PlayerGrid.Margin.Bottom);
                AIGrid.Margin = new Thickness(AIGrid.ActualWidth / 2 + 400, AIGrid.Margin.Top, AIGrid.Margin.Right, AIGrid.Margin.Bottom);
            }
        }
    */
        //when the elapsedEventArgs e is met (1000 milliseconds elapsed)
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            //increment 1 to gametime
            GameTime++;
            //invoke the time_passed textbox and change its values with the new value of gamtime
            this.Dispatcher.Invoke(() =>
            {
                _Time.Text = GameTime.ToString();
                if (GameTime > 0)
                {
                    Easy.IsEnabled = false;
                    Medium.IsEnabled = false;
                    Hard.IsEnabled = false;
                }
            });

        }

        private void Easy_Click(object sender, RoutedEventArgs e)
        {
            difficulty = 1;
        }

        private void Medium_Click(object sender, RoutedEventArgs e)
        {
            difficulty = 2;
        }

        private void Hard_Click(object sender, RoutedEventArgs e)
        {
            difficulty = 3;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            T.Enabled = true;
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            T.Enabled = false;
        }

        private void Leaderboard_Click(object sender, RoutedEventArgs e)
        {

        }

        private void French(object sender, RoutedEventArgs e)
        {

        }

        private void English(object sender, RoutedEventArgs e)
        {

        }

        private void CheatsOn(object sender, RoutedEventArgs e)
        {
            if (On.IsChecked)
                Off.IsChecked = false;


        }

        private void CheatsOff(object sender, RoutedEventArgs e)
        {
            if (Off.IsChecked)
                On.IsChecked = false;
        }

        private void USA(object sender, RoutedEventArgs e)
        {

        }

        private void JPN(object sender, RoutedEventArgs e)
        {


        }

        private void RUS(object sender, RoutedEventArgs e)
        {

        }

        private void GER(object sender, RoutedEventArgs e)
        {

        }


    }
}
