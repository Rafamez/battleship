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
    public partial class mainGameWindow : Window
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
        public Player human;
        public string skin = "usa";
        //creating human
        private String[] saved = new String[250];

        public Boolean english = true;

        public List<Boolean> horizental;

        public List<Boolean> isHorizental
        {
            get { return horizental; }
            set {; }
        }


        public mainGameWindow()
        {
            InitializeComponent();
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
            Turn.Content = "YOUR \r\nTURN";
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



        //method which sees if the game is about to end or not
        public void end()
        {
            //if you win
            if (endValue == 17)
            {
                if (english)
                    MessageBox.Show("You finished the game in " + GameTime + " seconds, congratulations!");
                else
                    MessageBox.Show("Vous avez fini le jeu en " + GameTime + " secondes, bravo!");
                System.Windows.Application.Current.Shutdown();
            }
            //if you lose
            if (friendlyDamage == 17)
            {
                if (english)
                    MessageBox.Show("You lost the game in " + GameTime + " seconds, git gud!");
                else
                    MessageBox.Show("Vous avez perdu le jeu en " + GameTime + " secondes, vous êtes mauvais!");
                System.Windows.Application.Current.Shutdown();

            }
        }



        private void PBButton_Click(object sender, RoutedEventArgs e)
        {
            horizental[0] = !horizental[0];
            if (!horizental[0])
                PatrolBoat.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Images\\patrolV.png");
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

		private void callChoseShip(object sender, RoutedEventArgs e)
		{
			
		}


	}
}



