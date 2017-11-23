using battleship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
    public partial class gameWindow : Window
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



        public gameWindow()
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
        }
        //method which shows the leaderboard
        private void Leaderboard_Click(object sender, RoutedEventArgs e)
        {

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
        //method which shows your credit only if you have some (accumulated over multiple games, related to username)
        private void showCredits(Human player1)
        {
            if (player1.getPoints > 0)
            {
                Credits.Visibility = Visibility.Visible;
                CreditsValue.Visibility = Visibility.Visible;
                CreditsValue.Text = player1.getPoints.ToString();
            }
        }
        //test method to see what happens when a label has focus (may be used to initate "clicks")
        private void TextBlock0_GotFocus(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("hi");
        }


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

    }

    //class for the human player (you)
    public class Human : Player, ISerializable
    {
        //variables for username, points
        //parameters for username (getter) and points (getter + setter)
        private String username;
        public String getName
        {
            get { return username; }
            set {; }
        }

        private int points = 0;
        public int getPoints
        {
            get { return points; }
            set { this.points = value; }
        }

        //instantiate human player
        public Human(String name)
        {
            //if name is already in leaderboard, add points to his total (points!=score)
            this.username = name;
            if (name == this.username)
            {
                //   this.points+= //ADD SCORE VALUE FROM ETHANS CODE
            }

        }
        //used to deserialize data, not ready (keep username + points)
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        //public class for the AI (ennemy)
        public class AI : Player, ISerializable
        {
            public int getDifficulty
            {
                get { return difficulty; }
                set { value = difficulty; }
            }

            public AI(int difficulty)
            {
                this.difficulty = difficulty;
            }

            //used to deserialize data, not ready (keep difficulty)
            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                throw new NotImplementedException();
            }
        }



    }
}
//class for the ships
public class Ship : Player, ISerializable
{
    //list for the length of each ship
    private int[] shipLength = new int[] { 2, 3, 3, 4, 5 };
    //list for each ship
    private String[] shipName = new String[] { "Patrol Boat (2)", "Submarine (3)", "Destroyer (3)", "Battleship (4)", "Aircraft Carrier (5)" };

    //used to deserialize data, not ready (keep location + alive)
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        throw new NotImplementedException();
    }

    public void setShip()
    {

    }
}
//general class extended by 2 players (AI + you)
public class Player : gameWindow
{
    //list to see which location the ai shot, the human shot, which textblock has been hit, whose turn is it
    //parameter for the turn
    private List<List<Boolean>> AIgotShot;
    List<TextBlock> hitZone;
    private List<List<Boolean>> gotShot;
    public bool playerTurn;
    public bool turn
    {
        get { return turn; }
        set {; }
    }

    //shoot method which requires x and y axis 
    public void shoot(int x, int y)
    {
        //set playtime timer to 0 + begin timer
        PlayTime = 0;
        PT.Enabled = true;
        //spot=the location you will have (61st label for example)
        int spot = x * 10 + y;
        //loop to continue until time expires or the player makes a valid turn
        while (PlayTime % expireTime != 0)
        {
            //if he shoots in a location not yet used
            if (!gotShot[0][x] && !gotShot[1][y])
            {
                //make it used
                gotShot[0][x] = true;
                gotShot[1][y] = true;

                //if he hit an object with a background (ship)
                if (hitZone[spot].Background.Opacity == 0)
                {
                    //next turn
                    playerTurn = !playerTurn;
                    //switch user depending on who played the turn
                    if (playerTurn)
                        Turn.Content = "YOUR \r\nTURN";
                    else
                        Turn.Content = "AI \r\nTURN";
                    //add an image of a cross on the location hit and put the opacity to 100
                    hitZone[spot].Background = new ImageBrush(new BitmapImage(new Uri(@"Images/cross.png", UriKind.Relative)));
                    hitZone[spot].Background.Opacity = 100;
                    //increment end value by 1
                    endValue++;
                    //verify if it is the end of the game
                    end();
                    //disable timer
                    PT.Enabled = false;
                    //return
                    return;
                }
                //if he did not hit anything
                else
                {
                    //add the image of an X on the location he hit, put its opacity to 100
                    hitZone[spot].Background = new ImageBrush(new BitmapImage(new Uri(@"Images/X.png", UriKind.Relative)));
                    hitZone[spot].Background.Opacity = 100;
                    //disable timer
                    PT.Enabled = false;
                    //return
                    return;
                }
            }
            else
            {
                //if he did not use a valid space, end timer and return, since turns havent been changed yet, he can still chose another spot to hit until the timer runs out
                PT.Enabled = false;
                return;
            }
        }
    }
    public void shoot()
    {
        //create a random variable so the AI choses a random spot to it
        Random random = new Random();
        //available rows to chose from (10), chose a random one
        int x = random.Next(10);
        //available columns to chose from (10), chose a random one
        int y = random.Next(10);
        //if the difficulty is at 1 (lowest)
        if (difficulty == 1)
        {
            //until the AI hits a valid spot (not yet used)
            while (!AIgotShot[0][x] && !AIgotShot[1][y])
            {
                //make him shoot using the 2 random variables he got
                shoot(x, y);
                //if the x axis is not valid, look for another random value
                if (!AIgotShot[0][x])
                    x = random.Next(10);
                //if the y axis is not valid, look for another random value
                if (!AIgotShot[1][y])
                    y = random.Next(10);
            }
            //once he gets a valid spot, make it so the spot he hit is offically marked as used
            AIgotShot[0][x] = true;
            AIgotShot[1][y] = true;
            //increase friendly value by 1
            friendlyDamage++;
            //verify if its the end of the game
            end();
        }
        //if difficulty is 2
        if (difficulty == 2)
        {

        }
        //if difficulty is 3
        if (difficulty == 3)
        {

        }
    }
}


