﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace battleship
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	/// 
	public partial class	mainGameWindow : Window
	{
		//value to represent the X axis
		public int gridX = 10;
		//value to represent the Y axis
		public int gridY = 10;
		// public double size = 0; was used when we resized window, but feature was removed
		public int difficulty = 0;
		//general timer of the game
		public System.Timers.Timer T = new Timer();
		//time for the amount of time the user has been playing
		public System.Timers.Timer PT = new Timer();
		//the game time passed
		public int GameTime = 0;
		//the time that has passed for the round of the player
		public int PlayTime = 0;
		//when end value == 17 (17 marks to hit to win) the game ends
		public int endValue = 0;
		//if the ennemy wins by taking down all your 17 marks, end game
		public int friendlyDamage = 0;
		//time when each round will end
		public int expireTime = 20;
        //creating human
        private String[] saved = new String[250];
		public Player human;
        //boolean to see if its horizental or not
        public List<Boolean> horizental;

        public List<Boolean> isHorizental
        {
            get { return horizental; }
            set {; }
        }

        Leaderboard leaderboard = new Leaderboard();
		public mainGameWindow()
		{
            
			InitializeComponent();
			//Call method to change value of GameTime when event is met
			T.Elapsed += new ElapsedEventHandler(OnTimedEvent);
			//set the interval to 1000
			T.Interval = 1000;
			//Call method to change value of PlayTime when event is met
			PT.Elapsed += new ElapsedEventHandler(OnTimedEvent);
			//set the interval to 1000
			PT.Interval = 1000;
			//small text used to justify whos turn it is
			Turn.Content = "YOUR \r\nTURN";
		}



		//This method was used when window was resized, doesnt work, is outdated
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
			//if the round for the player has started, start adding time to its counter
			if (PT.Enabled == true)
				PlayTime++;
			//invoke the time_passed textbox and change its values with the new value of gamtime
			this.Dispatcher.Invoke(() =>
			{
				//increment the value of the Time label by 1 
				_Time.Text = GameTime.ToString();
				if (GameTime > 0)
				{
					//disable change of difficulty once game has started
					Easy.IsEnabled = false;
					Medium.IsEnabled = false;
					Hard.IsEnabled = false;
				}
			});

		}

		//method for the when the user choses easy difficulty, changes the value to 1
		private void Easy_Click(object sender, RoutedEventArgs e)
		{
			difficulty = 1;
		}
		//method for the when the user choses medium difficulty, changes the value to 2
		private void Medium_Click(object sender, RoutedEventArgs e)
		{
			difficulty = 2;
		}
		//method for the when the user choses hard difficulty, changes the value to 3
		private void Hard_Click(object sender, RoutedEventArgs e)
		{
			difficulty = 3;
		}
		//method for the when the user starts the game (option to load + start timer)
		private void Start_Click(object sender, RoutedEventArgs e)
		{
			T.Enabled = true;
		}
		//method for the when the user stops the game (automatically saves + stops timer)
		private void Stop_Click(object sender, RoutedEventArgs e)
		{
            T.Enabled = false;
            if (File.Exists(@"../../DataFile.txt"))
            {
                File.Delete(@"../../DataFile.txt");
            }
            saved[0] = _Score.Text + '*';
            saved[1] = _Time.Text + '*';
            saved[2] = difficulty.ToString() + '*';
            // saved[3] = Name.Text +'*';
            // saved[4] = Credit.Text +'*';
            //saved[5] = empire.text +'*';
            //saved[6]= language.text +'*';
            //saved[7]= cheats.text +'*';

            //SAVE THE ELEMENTS FOR THE GRID 
            /*   for (int i = 0; i < GameGrid.Text.Length; i++)
               {
                   saved[i + 4] = secret[i];

               }
               for (int j = 0; j < reveal.Length; j++)
               {
                   saved[j + 20] = Convert.ToString(reveal[j]);

               }*/
            FileStream fs = new FileStream("../../DataFile.txt", FileMode.Create, FileAccess.ReadWrite);
            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, saved);
            }
            catch (SerializationException em)
            {
                Console.WriteLine("Failed to serialize. Reason: " + em.Message);

            }
            finally
            {
                fs.Close();
            }
        }
		//method which shows the leaderboard
		private void Leaderboard_Click(object sender, RoutedEventArgs e)
		{
            leaderboard.Visibility = Visibility.Visible;
		}
		//method which changes the game language to french
		private void French(object sender, RoutedEventArgs e)
		{

		}
		//method which changes the game language to english
		private void English(object sender, RoutedEventArgs e)
		{

		}
		//method which allows for cheats (for debugg reasons or if you want to augment your self esteem)
		private void CheatsOn(object sender, RoutedEventArgs e)
		{
			if (On.IsChecked)
				Off.IsChecked = false;


		}
		//method which disables those cheats
		private void CheatsOff(object sender, RoutedEventArgs e)
		{
			if (Off.IsChecked)
				On.IsChecked = false;
		}
		//the ship skins is of the USA
		private void USA(object sender, RoutedEventArgs e)
		{

		}
		//the ship skins is of japan
		private void JPN(object sender, RoutedEventArgs e)
		{


		}
		//the ship skins is of russia
		private void RUS(object sender, RoutedEventArgs e)
		{

		}
		//the ship skins is of germany
		private void GER(object sender, RoutedEventArgs e)
		{

		}
<<<<<<< HEAD

=======
        //method which shows your credit only if you have some (accumulated over multiple games, related to username)
        /*
        private void showCredits(Human player1)
		{
			if (player1.getPoints > 0)
			{
				Credits.Visibility = Visibility.Visible;
				CreditsValue.Visibility = Visibility.Visible;
				CreditsValue.Text = player1.getPoints.ToString();
			}
		}
>>>>>>> 98b2b5f9d309625ac654e0d6b3093759053d6f33
		//test method to see what happens when a label has focus (may be used to initate "clicks")
		private void TextBlock0_GotFocus(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("hi");
		}

    */

		//method which sees if the game is about to end or not
		public void end()
		{
			//if you win
			if (endValue == 17)
			{
				MessageBox.Show("You finished the game in " + GameTime + " seconds, congratulations!");
				System.Windows.Application.Current.Shutdown();
			}
			//if you lose
			if (friendlyDamage == 17)
			{
				MessageBox.Show("You lost the game in " + GameTime + " seconds, git gud!");
				System.Windows.Application.Current.Shutdown();
			}
		}

        private void PBButton_Click(object sender, RoutedEventArgs e)
        {
            horizental[0] = !horizental[0];
            if (!horizental[0])
            PatrolBoat.Source= (ImageSource)new ImageSourceConverter().ConvertFrom("Images\\patrolV.png");
            if (horizental[0])
                PatrolBoat.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Images\\patrolH.png");
        }

        private void SButton_Click(object sender, RoutedEventArgs e)
        {
            horizental[1] = !horizental[1];
            if (!horizental[1])
                PatrolBoat.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Images\\submarineV.png");
            if (horizental[1])
                PatrolBoat.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Images\\submarineH.png");
        }

        private void BButton_Click(object sender, RoutedEventArgs e)
        {
            horizental[2] = !horizental[2];
            if (!horizental[2])
                PatrolBoat.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Images\\battleshipV.png");
            if (horizental[2])
                PatrolBoat.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Images\\battleshipH.png");
        }

        private void ACButton_Click(object sender, RoutedEventArgs e)
        {
            horizental[3] = !horizental[3];
            if (!horizental[0])
                PatrolBoat.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Images\\airshipcarrierV.png");
            if (horizental[0])
                PatrolBoat.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Images\\airshipcarrierH.png");
        }
    }
}

  

