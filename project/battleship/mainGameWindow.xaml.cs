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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace battleship
{
    [Serializable]
    public partial class mainGameWindow : Window
    {
        private static MediaPlayer player = new MediaPlayer();
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
        public int expireTime2 = 0;

        public string skin = "usa";

        int ennemyPlacedShips = 0;

        public int attempts = 0;

        //creating human
        private String[] saved = new String[250];
        private string saveData;

        private Boolean clicked;
        //creating human
        public Player human;
        public AI otherPlayer;

        private int shotsFired = 0;

        public Boolean english = true;

        private Boolean musicPlaying = true;

        public List<Boolean> horizental = new List<Boolean> { true, true, true, true, true };




        public List<Boolean> isHorizental
        {
            get { return horizental; }
            set {; }
        }

        List<Boolean> boatClicked = new List<Boolean> { false, false, false, false, false };
        private String[] a = new String[2];

        public mainGameWindow()
        {
            InitializeComponent();

            //deserializes and adds the username and the timeleft 


            /* FileStream Fs = new FileStream("../../userInput.txt", FileMode.Open, FileAccess.Read);
             try
             {
                 BinaryFormatter F = new BinaryFormatter();

                 a = (String[])F.Deserialize(Fs);
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
             expireTime = Convert.ToInt32( a[1]);
             expireTime2 = expireTime;*/
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            MediaElement media = new MediaElement();
            media.LoadedBehavior = MediaState.Manual;
            media.UnloadedBehavior = MediaState.Manual;
            media.Source = new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "../../Images/gamebackground.mp4"));
            media.Stretch = Stretch.Fill;
            media.MediaEnded += Video_MediaEnded;
            Panel.SetZIndex(media, -1);
            mom.Children.Add(media);
            media.Play();

            //MUSIC
            player.Open(new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "../../Images/MainOST.mp3")));
            player.Play();

            //player.Pause();
            human = new Player("Bob", HumanGrid, skin);

            Leaderboard lb = new Leaderboard();




            UnitedStates.IsChecked = true;
            //Call method to change value of GameTime when event is met
            T.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            //set the interval to 1000
            T.Interval = 1000;
            //Call method to change value of PlayTime when event is met
            PT.Elapsed += new ElapsedEventHandler(OnTimedEventExpire);
            PT.Interval = expireTime * 1000;

            //small text used to justify whos turn it is
            Easy.Background = Brushes.WhiteSmoke;
            Medium.Background = Brushes.DarkGray;
            Hard.Background = Brushes.DarkGray;

        }

        //when the elapsedEventArgs e is met (1000 milliseconds elapsed)
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (T.Enabled)
            {


                //getting the play 
                if (expireTime == 0)
                {


                    expireTime = expireTime2;
                    otherPlayer.AITurn();

                }
                expireTime--;
                //increment 1 to gametime
                GameTime++;
                //if the round for the player has started, start adding time to its counter
                if (PT.Enabled == true)
                {
                    PlayTime++;


                }
                //invoke the time_passed textbox and change its values with the new value of gamtime
                this.Dispatcher.Invoke(() =>
                {
                    //increment the value of the Time label by 1 
                    Time.Text = GameTime.ToString();
                });

            }
        }

        //ETHAN USE THIS METHOD TO CHANGE THE VALUE OF THE TIMER.TXT ON MAINWINDOWSGAME
        private void OnTimedEventExpire(object source, ElapsedEventArgs e)
        {
            //invoke the time_passed textbox and change its values with the new value of gamtime
            this.Dispatcher.Invoke(() =>
            {
                //increment the value of the Time label by 1 
                Time.Text = GameTime.ToString();
            });
        }

        private void Video_MediaEnded(object sender, RoutedEventArgs e)
        {
            MediaElement media = (MediaElement)sender;
            media.Position = TimeSpan.FromSeconds(0);
        }
        private void Video_MediaEnded2(object sender, RoutedEventArgs e)
        {
            MediaPlayer media = (MediaPlayer)sender;
            media.Position = TimeSpan.FromSeconds(0);
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

            //deserializes and adds the username and the timeleft 

          //  FileStream Fs = new FileStream("../../userInput.txt", FileMode.Open, FileAccess.Read);
       /*     try
            {
                BinaryFormatter F = new BinaryFormatter();

                a = (String[])F.Deserialize(Fs);
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
            timeLeft.Text = a[1].ToString();
            human.username = a[0];

            */
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
            otherPlayer = new AI(difficulty, human, AIGrid, skin);
            if (ennemyPlacedShips == 0)
                otherPlayer.getShipPlacement();
            ennemyPlacedShips++;
        }
        //method for the when the user stops the game (automatically saves + stops timer)
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
       /*     T.Enabled = false;
            if (File.Exists(@"../../DataFile.txt"))
            {

                File.Delete(@"../../DataFile.txt");
            }
            if (File.Exists(@"../../DataFile.ser"))
            {
                File.Delete(@"../../DataFile.ser");
            }
            saved[0] = Score.Text;
            saved[1] = Time.Text;
            saved[2] = difficulty.ToString();
            saved[3] = AttemptValues.Text;
            saved[4] = timeLeft.Text;
            saved[5] = skin.ToString();
            saved[6] = human.username;

            Serialize(TimeLabel.Content, "../../DataFile.ser");
            Serialize(Language, "../../DataFile.ser");
            Serialize(AttemptsCount.Content, "../../DataFile.ser");
            Serialize(Easy.Content, "../../DataFile.ser");
            Serialize(Medium.Content, "../../DataFile.ser");
            Serialize(Hard.Content, "../../DataFile.ser");
            Serialize(Cheats, "../../DataFile.ser");
            Serialize(Skins, "../../DataFile.ser");
            Serialize(HumanGrid, "../../DataFile.ser");
            Serialize(AIGrid, "../../DataFile.ser");
            Serialize(TurnTimeLeft.Content, "../../DataFile.ser");


            Serialize(human.myShips, "../../DataFile.ser");
            Serialize(otherPlayer.myShips, "../../DataFile.ser");
            Serialize(human.MyGrid, "../../DataFile.ser");
            Serialize(otherPlayer.MyGrid, "../../DataFile.ser");



            //saved[5] = empire.text +'*';
            //saved[6]= language.text +'*';
            //saved[7]= cheats.text +'*';
            /*
             * skins
             * difficulty
             * cheats 
             * human grid
             * ai grid 
             * human ships 
             * ai ships
             * ai my grid 
             * human my grid
             * username
             * */
            //SAVE THE ELEMENTS FOR THE GRID 
            /*   for (int i = 0; i < GameGrid.Text.Length; i++)
               {
                   saved[i + 4] = secret[i];

               }
               for (int j = 0; j < reveal.Length; j++)
               {
                   saved[j + 20] = Convert.ToString(reveal[j]);

               }*/
          /*  FileStream fs = new FileStream("../../DataFile.txt", FileMode.Create, FileAccess.ReadWrite);
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
            }*/
        }
        //method which shows the leaderboard
        private void Leaderboard_Click(object sender, RoutedEventArgs e)
        {
            clicked = !clicked;
            if (clicked)
            {
                //app2.Visibility = Visibility.Visible;

            }
        }
        //method which changes the game language to french
        private void French(object sender, RoutedEventArgs e)
        {
            english = false;
            TurnTimeLeft.Content = "TourneTempsRestant";
            reset.Content = "Réinitialiser";
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
            TurnTimeLeft.Content = "TurnTimeLeft";
            reset.Content = "Reset";
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
            if (T.Enabled || GameTime != 0)
                otherPlayer.reveal();
        }
        //method which disables those cheats
        private void CheatsOff(object sender, RoutedEventArgs e)
        {
            if (T.Enabled || GameTime != 0)
                otherPlayer.hide();
        }

        //the ship skins is of the USA
        private void USA(object sender, RoutedEventArgs e)
        {
            skin = "usa";
            Japan.IsChecked = false;
            Germany.IsChecked = false;
            Russia.IsChecked = false;
            UnitedStates.IsChecked = true;
            human.setString = skin;
            checkImage();


        }
        //the ship skins is of japan
        private void JPN(object sender, RoutedEventArgs e)
        {
            skin = "japan";
            Germany.IsChecked = false;
            Russia.IsChecked = false;
            UnitedStates.IsChecked = false;
            Japan.IsChecked = true;
            human.setString = skin;
            checkImage();



        }
        //the ship skins is of russia
        private void RUS(object sender, RoutedEventArgs e)
        {
            skin = "russia";
            Japan.IsChecked = false;
            Germany.IsChecked = false;
            UnitedStates.IsChecked = false;
            Russia.IsChecked = true;
            human.setString = skin;
            checkImage();
        }
        //the ship skins is of germany
        private void GER(object sender, RoutedEventArgs e)
        {
            skin = "germany";
            UnitedStates.IsChecked = false;
            Japan.IsChecked = false;
            Russia.IsChecked = false;
            Germany.IsChecked = true;
            human.setString = skin;
            checkImage();


        }

        private void checkImage()
        {
            if (!horizental[0])
                BattleShip.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/battleshipV.png");
            if (horizental[0])
                BattleShip.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/battleshipH.png");
            if (!horizental[1])
                Cruiser.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/cruiserV.png");
            if (horizental[1])
                Cruiser.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/cruiserH.png");
            if (!horizental[2])
                Destroyer.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/destroyerV.png");
            if (horizental[2])
                Destroyer.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/destroyerH.png");
            if (!horizental[3])
                Submarine.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/submarineV.png");
            if (horizental[3])
                Submarine.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/submarineH.png");
            if (!horizental[4])
                AircraftCarrier.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/carrierV.png");
            if (horizental[4])
                AircraftCarrier.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/carrierH.png");
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
                ++xAxis;
                if (xAxis == 10)
                    break;
            }

            // calc col mouse was over
            foreach (var columnDefinition in HumanGrid.ColumnDefinitions)
            {
                accumulatedWidth += columnDefinition.ActualWidth;
                if (accumulatedWidth >= point.X)
                    break;
                ++yAxis;
                if (yAxis == 10)
                    break;
            }
            human.PlaceShips(xAxis, yAxis, horizental);
            setVisibility();
            Console.WriteLine("Clicked at {0}, {1}", yAxis, xAxis);

        }

        private void setVisibility()
        {
            switch (human.ship - 1)
            {
                case (0):
                    BattleShip.Visibility = Visibility.Hidden;
                    BBButton.Visibility = Visibility.Hidden;
                    Cruiser.Visibility = Visibility.Visible;
                    CButton.Visibility = Visibility.Visible;
                    break;
                case (1):
                    Cruiser.Visibility = Visibility.Hidden;
                    CButton.Visibility = Visibility.Hidden;
                    Destroyer.Visibility = Visibility.Visible;
                    DButton.Visibility = Visibility = Visibility;
                    break;
                case (2):
                    Destroyer.Visibility = Visibility.Hidden;
                    DButton.Visibility = Visibility.Hidden;
                    Submarine.Visibility = Visibility.Visible;
                    SButton.Visibility = Visibility = Visibility;
                    break;
                case (3):
                    Submarine.Visibility = Visibility.Hidden;
                    SButton.Visibility = Visibility.Hidden;
                    AircraftCarrier.Visibility = Visibility.Visible;
                    ACButton.Visibility = Visibility = Visibility;
                    break;
                case (4):
                    AircraftCarrier.Visibility = Visibility.Hidden;
                    ACButton.Visibility = Visibility.Hidden;
                    break;


            }
        }



        private void boatClickedSet()
        {
            for (int i = 0; i < boatClicked.Count; i++)
            {
                boatClicked[i] = false;
            }
            boatClicked[human.ship] = true;
        }


        private void CallChoseShip(object sender, MouseButtonEventArgs e)
        {
            {
                Image newimage = (Image)sender;

                if (newimage.Source.ToString() == "pack://application:,,,/battleship;component/Images/" + skin + "/battleshipH.png" || newimage.Source.ToString() == "../../Images/" + skin + "/battleshipV.png")
                {
                    if (!boatClicked[0])
                        boatClickedSet();

                }
                if (newimage.Source.ToString() == "pack://application:,,,/battleship;component/Images/" + skin + "/cruiserH.png" || newimage.Source.ToString() == "../../Images/" + skin + "/cruiserV.png")
                {
                    if (!boatClicked[1])
                        boatClickedSet();

                }
                if (newimage.Source.ToString() == "pack://application:,,,/battleship;component/Images/" + skin + "/destroyerH.png" || newimage.Source.ToString() == "../../Images/" + skin + "/destroyerV.png")
                {
                    if (!boatClicked[2])
                        boatClickedSet();
                }
                if (newimage.Source.ToString() == "pack://application:,,,/battleship;component/Images/" + skin + "/submarineH.png" || newimage.Source.ToString() == "../../Images/" + skin + "/submarineV.png")
                {
                    if (!boatClicked[3])
                        boatClickedSet();
                }
                if (newimage.Source.ToString() == "pack://application:,,,/battleship;component/Images/" + skin + "/carrierH.png" || newimage.Source.ToString() == "../../Images/" + skin + "/carrierV.png")
                {
                    if (!boatClicked[4])
                        boatClickedSet();
                }
            }
        }

        private void ImageGotClicked(object sender, MouseButtonEventArgs e)
        {
            var point = Mouse.GetPosition(AIGrid);

            xAxis = 0;
            yAxis = 0;
            double accumulatedHeight = 0.0;
            double accumulatedWidth = 0.0;

            // calc row mouse was over
            foreach (var rowDefinition in AIGrid.RowDefinitions)
            {
                accumulatedHeight += rowDefinition.ActualHeight;
                if (accumulatedHeight >= point.Y)
                    break;
                ++xAxis;
                if (xAxis == 10)
                    break;
            }

            // calc col mouse was over
            foreach (var columnDefinition in AIGrid.ColumnDefinitions)
            {
                accumulatedWidth += columnDefinition.ActualWidth;
                if (accumulatedWidth >= point.X)
                    break;
                ++yAxis;
                if (yAxis == 10)
                    break;
            }
            if (T.Enabled || GameTime > 0)
            {
                if (otherPlayer.MyGrid[xAxis][yAxis].Type != SquareType.Sunk && otherPlayer.MyGrid[xAxis][yAxis].Type != SquareType.Miss)
                {
                    actions.Text += human.Fire(xAxis, yAxis, otherPlayer);
                    actions.Text += Environment.NewLine;
                    otherPlayer.AITurn();
                    shotsFired++;
                    attempts++;
                    AttemptValues.Text = attempts.ToString();
                    end();
                }
            }
        }





        //method which sees if the game is about to end or not
        public void end()
        {
            //if you lose
            if (human.Lost() || otherPlayer.Lost())
            {
                //Serializing the highscores 
                int score = 0;
                int boatsLeft = 0;
                int attempts = Convert.ToInt32(AttemptValues.Text);
                int timeCount = Convert.ToInt32(AttemptValues.Text);
                for (int j = 0; j < human.myShips.Count; j++)
                {
                    if (!(human.myShips[j].healthReturn == 0))
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
                saveData = "Name: " + ' ' + score.ToString() + ' ' + Time.Text + '*';
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
            }
            if (human.Lost())
            {
                if (english)
                    MessageBox.Show("You lost the game in " + GameTime + " seconds, git gud!");
                else
                    MessageBox.Show("Vous avez perdu le jeu en " + GameTime + " secondes, vous êtes mauvais!");
                System.Windows.Application.Current.Shutdown();
            }
            else if (otherPlayer.Lost())
            {
                if (english)
                    MessageBox.Show("You won the game in " + GameTime + " seconds, Congratulations!");
                else
                    MessageBox.Show("Vous avez gagnez le jeu en " + GameTime + " secondes, Bravo!");
                System.Windows.Application.Current.Shutdown();
            }
        }


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            if (!T.Enabled && GameTime == 0)
            {
                BattleShip.Visibility = Visibility.Visible;
                BBButton.Visibility = Visibility.Visible;
                Cruiser.Visibility = Visibility.Hidden;
                CButton.Visibility = Visibility.Hidden;
                Destroyer.Visibility = Visibility.Hidden;
                DButton.Visibility = Visibility.Hidden;
                Submarine.Visibility = Visibility.Hidden;
                SButton.Visibility = Visibility.Hidden;
                AircraftCarrier.Visibility = Visibility.Hidden;
                ACButton.Visibility = Visibility.Hidden;
                human.ship = 0;
                int size = HumanGrid.Children.Count - 1;

                human.RemoveAll();
                HumanGrid.Children.Clear();
            }
        }
        private void Mute_click(object sender, MouseButtonEventArgs e)
        {
            if (musicPlaying)
            {
                player.Pause();
                musicPlaying = false;
                Mute_Click.Visibility = Visibility.Hidden;
                Volume_Click.Visibility = Visibility.Visible;
            }
        }
        private void Volume_click(object sender, MouseButtonEventArgs e)
        {
            if (!musicPlaying)
            {
                player.Play();
                musicPlaying = true;
                Mute_Click.Visibility = Visibility.Visible;
                Volume_Click.Visibility = Visibility.Hidden;
            }
        }

        private void gameWindow_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        public static void Serialize(Object o, string filespec)
        {
            IFormatter f = new BinaryFormatter();
            Stream writer = new FileStream(filespec, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            f.Serialize(writer, o);
            writer.Close();
        }

        public static Object DeSerialize(string filespec)
        {
            IFormatter f = new BinaryFormatter();
            Stream reader = new FileStream(filespec, FileMode.Open, FileAccess.Read, FileShare.None);
            Object o = f.Deserialize(reader);
            reader.Close();

            return o;
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

