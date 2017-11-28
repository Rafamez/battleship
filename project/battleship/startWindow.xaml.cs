using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
using System.Windows.Shapes;

namespace battleship
{
    /// <summary>
    /// Interaction logic for startWindow.xaml
    /// </summary>
    public partial class startWindow : Window
    {
        private bool click = true;
        public System.Timers.Timer T = new Timer();
        mainGameWindow app = new mainGameWindow();
        public startWindow()
        {
            InitializeComponent();
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            app.Visibility = Visibility.Visible;

        }

        private void LoadGame_Click(object sender, RoutedEventArgs e)
        {
            click = !click;
            T.Enabled = true;
            FileStream Fs = new FileStream("../../DataFile.txt", FileMode.Open, FileAccess.Read);
            if (click)
            {
                MessageBoxResult dialogResult = MessageBox.Show("Do you want to load the file", "Load Game", MessageBoxButton.YesNo);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    //do something
                    try
                    {
                        BinaryFormatter F = new BinaryFormatter();

                        string a = (string)F.Deserialize(Fs);
                    }
                    catch (SerializationException em)
                    {
                        Console.WriteLine("Failed to deserialize. Reason: " + em.Message);

                    }
                    finally
                    {
                        Fs.Close();
                    }
                }

                else if (dialogResult == MessageBoxResult.No)
                {


                }

            }
    }
}
