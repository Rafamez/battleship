using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        userInput app = new userInput();
        public startWindow()
        {
            InitializeComponent();
			WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
			MediaElement media = new MediaElement();
			media.LoadedBehavior = MediaState.Manual;
			media.UnloadedBehavior = MediaState.Manual;
			media.Source = new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "../../Images/oceanStartPage.wmv"));
			media.Stretch = Stretch.Fill;
			media.MediaEnded += Video_MediaEnded;
			Panel.SetZIndex(media, -1);
			mom.Children.Add(media);
			media.Play();

		}
    
    private void Video_MediaEnded(object sender, RoutedEventArgs e)
    {
			MediaElement media = (MediaElement)sender;
			media.Position = TimeSpan.FromSeconds(0);
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
						MessageBox.Show("Failed to deserialize. Reason: " + em.Message);

					}
					catch (FileNotFoundException fnfe)
					{
						MessageBox.Show("The file was not found");
					}
				//	catch (Exception excp) {
				//		MessageBox.Show("An error was found");
				//	}
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

    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            bool wasCodeClosed = new StackTrace().GetFrames().FirstOrDefault(x => x.GetMethod() == typeof(Window).GetMethod("Close")) != null;
            if (!wasCodeClosed)
            {
                Application.Current.Shutdown();
            }
           

            base.OnClosing(e);
        }
    }
}
