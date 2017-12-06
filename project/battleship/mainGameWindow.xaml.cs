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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace battleship
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	/// 
	public partial class mainGameWindow : Window,ISerializable
    {
		public int xAxis = 0;
		public int yAxis = 0;
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
		//time when each round will end
		public int expireTime = 20;

		public string skin = "usa";

		public int attempts = 0;
		//creating human
		private String[] saved = new String[250];
		private Boolean clicked;
		//creating human
		public Player human;
		public AI otherPlayer;

		public Boolean english = true;

		public List<Boolean> horizental = new List<Boolean> { true, true, true, true, true };

		public List<Boolean> isHorizental
		{
			get { return horizental; }
			set {; }
		}

		public bool yourTurn = true;
		public int shipsUsed = -1;
		List<Boolean> boatClicked = new List<Boolean> { false, false, false, false, false };
        private string saveData;

        public mainGameWindow()
		{
			InitializeComponent();


			human = new Player("Bob", HumanGrid, skin);
			otherPlayer = new AI(difficulty, human, AIGrid, skin);


			UnitedStates.IsChecked = true;
			//Call method to change value of GameTime when event is met
			T.Elapsed += new ElapsedEventHandler(OnTimedEvent);
			//set the interval to 1000
			T.Interval = 1000;
			//Call method to change value of PlayTime when event is met
			PT.Elapsed += new ElapsedEventHandler(OnTimedEvent);

			//set the interval to 1000
			PT.Interval = 1000;
			//small text used to justify whos turn it is
			Easy.Background = Brushes.WhiteSmoke;
			Medium.Background = Brushes.DarkGray;
			Hard.Background = Brushes.DarkGray;

		}

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
			});

		}

		//method for the when the user choses easy difficulty, changes the value to 1
		private void Easy_Click(object sender, RoutedEventArgs e)
		{
			difficulty = 1;
			Hard.Background = Brushes.DarkGray;
			Medium.Background = Brushes.DarkGray;
			Easy.Background = Brushes.WhiteSmoke;
		}
		//method for the when the user choses medium difficulty, changes the value to 2
		private void Medium_Click(object sender, RoutedEventArgs e)
		{
			difficulty = 2;
			Hard.Background = Brushes.DarkGray;
			Easy.Background = Brushes.DarkGray;
			Medium.Background = Brushes.WhiteSmoke;

		}
		//method for the when the user choses hard difficulty, changes the value to 3
		private void Hard_Click(object sender, RoutedEventArgs e)
		{
			difficulty = 3;
			Easy.Background = Brushes.DarkGray;
			Medium.Background = Brushes.DarkGray;
			Hard.Background = Brushes.WhiteSmoke;

		}
		//method for the when the user starts the game (option to load + start timer)
		private void Start_Click(object sender, RoutedEventArgs e)
		{
			T.Enabled = true;
			//disable change of difficulty once game has started
			Easy.Click -= Easy_Click;
			Medium.Click -= Medium_Click;
			Hard.Click -= Hard_Click;
			UnitedStates.Click -= USA;
			Japan.Click -= JPN;
			Russia.Click -= RUS;
			Germany.Click -= GER;
			UnitedStates.IsCheckable = false;
			Japan.IsCheckable = false;
			Russia.IsCheckable = false;
			Germany.IsCheckable = false;
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
			clicked = !clicked;
			if (clicked)
			{

			}
		}
		//method which changes the game language to french
		private void French(object sender, RoutedEventArgs e)
		{
			english = false;
			Title.Content = "BattleShip: Le jeu";
			TimeLabel.Content = "Temps";
			Difficulty.Content = "Difficulté";
			Easy.Content = "_Facile";
			Medium.Content = "_Moyen";
			Hard.Content = "_Difficile";
			Start.Content = "_Commencer";
			Stop.Content = "_Pauser";
			Leaderboard.Content = "_Classement";
			Cheats.Header = "_Tricher";
			Languages.Header = "Langues";
			Skins.Header = "Déguisements";
			UnitedStates.Header = "Etats-Unis";
			Germany.Header = "Allemagne";
			Russia.Header = "Russie";
			Japan.Header = "Japon";

		}
		//method which changes the game language to english
		private void English(object sender, RoutedEventArgs e)
		{
			english = true;

			Title.Content = "BattleShip: The Game";
			TimeLabel.Content = "Time";
			Difficulty.Content = "Difficulty";
			Easy.Content = "_Easy";
			Medium.Content = "_Medium";
			Hard.Content = "_Hard";
			Start.Content = "_Start";
			Stop.Content = "Sto_p";
			Leaderboard.Content = "_Leaderboard";
			Cheats.Header = "_cheats";
			Languages.Header = "_Languages";
			Skins.Header = "S_kins";
			UnitedStates.Header = "USA";
			Germany.Header = "Germany";
			Russia.Header = "Russia";
			Japan.Header = "Japan";

		}
		//method which allows for cheats (for debugg reasons or if you want to augment your self esteem)
		private void CheatsOn(object sender, RoutedEventArgs e)
		{
			if (On.IsChecked)
				Off.IsChecked = false;
			var values = Enum.GetValues(typeof(SquareType));

			for (int i = 0; i < AIGrid.RowDefinitions.Count * AIGrid.ColumnDefinitions.Count; i++)
			{

			}


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
			skin = "usa";
			Japan.IsChecked = false;
			Germany.IsChecked = false;
			Russia.IsChecked = false;
			UnitedStates.IsChecked = true;


		}
		//the ship skins is of japan
		private void JPN(object sender, RoutedEventArgs e)
		{
			skin = "japan";
			Germany.IsChecked = false;
			Russia.IsChecked = false;
			UnitedStates.IsChecked = false;
			Japan.IsChecked = true;



		}
		//the ship skins is of russia
		private void RUS(object sender, RoutedEventArgs e)
		{
			skin = "russia";
			Japan.IsChecked = false;
			Germany.IsChecked = false;
			UnitedStates.IsChecked = false;
			Russia.IsChecked = true;


		}
		//the ship skins is of germany
		private void GER(object sender, RoutedEventArgs e)
		{
			skin = "germany";
			UnitedStates.IsChecked = false;
			Japan.IsChecked = false;
			Russia.IsChecked = false;
			Germany.IsChecked = true;


		}


		private void BButton_Click(object sender, RoutedEventArgs e)
		{
			horizental[0] = !horizental[0];
			if (!horizental[0])
				BattleShip.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/battleshipV.png");
			if (horizental[0])
				BattleShip.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/battleshipH.png");
		}

		private void CButton_Click(object sender, RoutedEventArgs e)
		{
			horizental[1] = !horizental[1];
			if (!horizental[1])
				Cruiser.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/cruiserV.png");
			if (horizental[1])
				Cruiser.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/cruiserH.png");
		}

		private void DButton_Click(object sender, RoutedEventArgs e)
		{
			horizental[2] = !horizental[2];
			if (!horizental[2])
				Destroyer.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/destroyerV.png");
			if (horizental[2])
				Destroyer.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/destroyerH.png");
		}


		private void SButton_Click(object sender, RoutedEventArgs e)
		{
			horizental[3] = !horizental[3];
			if (!horizental[3])
				Submarine.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/submarineV.png");
			if (horizental[3])
				Submarine.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/submarineH.png");
		}


		private void ACButton_Click(object sender, RoutedEventArgs e)
		{
			horizental[4] = !horizental[4];
			if (!horizental[4])
				AircraftCarrier.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/carrierV.png");
			if (horizental[4])
				AircraftCarrier.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/carrierH.png");
		}

		private void LayShip(object sender, MouseButtonEventArgs e)
		{
			if (GameTime > 0)
			{
				var point = Mouse.GetPosition(HumanGrid);

				xAxis = 0;
				yAxis = 0;
				double accumulatedHeight = 0.0;
				double accumulatedWidth = 0.0;

				// calc row mouse was over
				foreach (var rowDefinition in HumanGrid.RowDefinitions)
				{
					accumulatedHeight += rowDefinition.ActualHeight;
					if (accumulatedHeight >= point.Y)
						break;
					xAxis++;
					if (xAxis == 10)
						break;
				}

				// calc col mouse was over
				foreach (var columnDefinition in HumanGrid.ColumnDefinitions)
				{
					accumulatedWidth += columnDefinition.ActualWidth;
					if (accumulatedWidth >= point.X)
						break;
					yAxis++;
					if (yAxis==10)
						break;
				}
				human.PlaceShips(xAxis, yAxis, horizental);
				Console.WriteLine("Clicked at {0}, {1}", yAxis, xAxis);
			}
		}

		private void boatClickedSet()
		{
			for (int i = 0; i < boatClicked.Count; i++) {
				boatClicked[i] = false;
			}
			boatClicked[shipsUsed] = true;
		}


		private void CallChoseShip(object sender, MouseButtonEventArgs e)
		{
			if (GameTime != 0)
			{
				Image newimage = (Image) sender;
				
				if (newimage.Source.ToString() == "pack://application:,,,/battleship;component/Images/usa/battleshipH.png" || newimage.Source.ToString() == "../../Images/usa/battleshipV.png")
				{
					if (!boatClicked[0])
						shipsUsed++;
					boatClickedSet();

				}
				if (newimage.Source.ToString() == "pack://application:,,,/battleship;component/Images/usa/cruiserH.png" || newimage.Source.ToString() == "../../Images/usa/cruiserV.png")
				{
					if (!boatClicked[1])
						shipsUsed++;
					boatClickedSet();

				}
				if (newimage.Source.ToString() == "pack://application:,,,/battleship;component/Images/usa/destroyerH.png" || newimage.Source.ToString() == "../../Images/usa/destroyerV.png")
				{
					if (!boatClicked[2])
						shipsUsed++;
					boatClickedSet();
				}
				if (newimage.Source.ToString() == "pack://application:,,,/battleship;component/Images/usa/submarineH.png" || newimage.Source.ToString() == "../../Images/usa/submarineV.png")
				{
					if (!boatClicked[3])
						shipsUsed++;
					boatClickedSet();
				}
				if (newimage.Source.ToString() == "pack://application:,,,/battleship;component/Images/usa/carrierH.png" || newimage.Source.ToString() == "../../Images/usa/carrierV.png")
				{
					if (!boatClicked[4])
						shipsUsed++;
					boatClickedSet();
				}
			}
		}

		private void Image_GotFocus(object sender, MouseButtonEventArgs e)
		{
			if (yourTurn && shipsUsed == 5)
			{
				Image newimage = (Image)sender;
				xAxis = Grid.GetRow(newimage);
				yAxis = Grid.GetColumn(newimage);
				if (yourTurn)
					actions.Text+= human.Fire(xAxis, yAxis, otherPlayer);
				yourTurn = !yourTurn;
				attempts++;
				AttemptValues.Text = attempts.ToString();
				if (yourTurn)
				{
					if (english)
						Turn.Content = "YOUR \r\nTURN";
					else
						Turn.Content = "VOTRE \r\nTOURS";

				}
				else
				{
					if (english)
						Turn.Content = "AI \r\nTURN";
					else
						Turn.Content = "TOURS \r\n DU ROBOT";
				}

			}

		}




		//method which sees if the game is about to end or not
		public void end()
		{
			//if you win
			if (otherPlayer.Lost())
			{
				if (english)
					MessageBox.Show("You finished the game in " + GameTime + " seconds, congratulations!");
				else
					MessageBox.Show("Vous avez fini le jeu en " + GameTime + " secondes, bravo!");

                //Serializing the highscores 

                //Serializing the highscores 
                int score = 0;
                int boatsLeft = 0;
                int attempts = Convert.ToInt32(AttemptValues.Text);
                int timeCount = Convert.ToInt32(AttemptValues.Text);
                for (int i = 0; i < human.myShips.Count; i++)
                {
                    if (!(human.myShips[i].healthReturn == 0))
                        boatsLeft++;
                }
                if (difficulty == 1)
                {
                    score = 2500 - (timeCount * (attempts / 17) * (2 - (boatsLeft / 5)));
                }
                if (difficulty == 2)
                {
                    score = 5000 - (timeCount * (attempts / 17) * (2 - (boatsLeft / 5)));
                }
                if (difficulty == 3)
                {
                    score = 10000 - (timeCount * (attempts / 17) * (2 - (boatsLeft / 5)));
                }
                saveData = "Name: " + ' ' + score.ToString() + ' ' + _Time.Text + '*';
                FileStream fs = new FileStream("../../highscores.txt", FileMode.Create, FileAccess.ReadWrite);
                // Construct a BinaryFormatter and use it to serialize the data to the stream.
                BinaryFormatter formatter = new BinaryFormatter();
                try
                {
                    
                    formatter.Serialize(fs, saveData);
                }
                catch (SerializationException em)
                {
                    Console.WriteLine("Failed to serialize. Reason: " + em.Message);

                }
                finally
                {
                    fs.Close();
                }
                System.Windows.Application.Current.Shutdown();
            }
            //if you lose
            if (human.Lost())
			{
				if (english)
					MessageBox.Show("You lost the game in " + GameTime + " seconds, git gud!");
				else
					MessageBox.Show("Vous avez perdu le jeu en " + GameTime + " secondes, vous êtes mauvais!");
				System.Windows.Application.Current.Shutdown();

			}
		}

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}