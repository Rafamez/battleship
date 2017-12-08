using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
	/// Interaction logic for userInput.xaml
	/// </summary>
	public partial class userInput : Window
	{

		String username;
		int expireDate;
		mainGameWindow app = new mainGameWindow();

		public userInput()
		{
			InitializeComponent();
			WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
		}

		private void Start_Click(object sender, RoutedEventArgs e)
		{
			if (!(string.IsNullOrWhiteSpace(username)) && username.Trim().Length != 0 && expireDate > -1) {
				//ETHAN SERIALIZE THE VALUES
				this.Close();
				app.Visibility = Visibility.Visible;
			}
		}

		private void Reset_Click(object sender, RoutedEventArgs e)
		{
			username = null;
			expireDate = -1;
			UsernameValue.Text = "";
			ExpireTimeValue.Text = "";
		}

		private void UsernameValue_KeyDown(object sender, KeyEventArgs e)
		{
			username += UsernameValue.Text.ToString();
		}

		private void ExpireTimeValue_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				expireDate += Convert.ToInt32(ExpireTimeValue.Text);
			}
			catch (Exception excp)
			{
				ExpireTimeValue.Text = "";
				expireDate = -1;

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
