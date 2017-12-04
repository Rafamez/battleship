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

//INSPIRED FROM JAMESJRG CODE FROM https://github.com/jamesjrg/battleship/tree/master/Battleship
namespace battleship
{
    public class Player : mainGameWindow
    {
        //INT FOR THE SIZE OF THE GRID
        public const int GRID_SIZE = 10;
        //RANDOM VARIABLE TO PLACE ENNEMY SHIPS
        static public Random rnd = new Random();
        //USERNAME FOR PLAYER
        public String username;

        //HUMAN BOARD
        public List<List<Board>> MyGrid { get; set; }
        //AI BOARD
        public List<List<Board>> EnemyGrid { get; set; }

        //HUMAN SHIPS
        List<Ship> myShips = new List<Ship>();
        //AI SHIPS
        List<Ship> enemyShips = new List<Ship>();


        //VALUE TO GET THE LIST USED
        private static int ship = -1;

        //CREATE PLAYER CLASS
        public Player(String name)
        {
            //SET USERNAME
            username = name;
            //SET FRIENDLY GRID
            MyGrid = new List<List<Board>>();
            //SET ENNEMY GRID
            EnemyGrid = new List<List<Board>>();
            //LOOP TO ADD BOARDS TO THE GRIDS
            for (int i = 0; i != GRID_SIZE; ++i)
            {
                MyGrid.Add(new List<Board>());
                EnemyGrid.Add(new List<Board>());

                for (int j = 0; j != GRID_SIZE; ++j)
                {
                    MyGrid[i].Add(new Board(i, j));
                    EnemyGrid[i].Add(new Board(i, j));
                }
            }
            //ADD SHIPS TO THE BOARD'S TYPE
            foreach (ShipType type in Enum.GetValues(typeof(ShipType)))
            {
                myShips.Add(new Ship(type));
                enemyShips.Add(new Ship(type));
            }
            //ADD SHIPS TO THE BOARDS WHO HAVE SHIP PROERTIES
            Reset();
        }

        //METHOD USED TO CHANGE THE TYPE VALUE OF THE BOARDS WHO HAVE SHIPS ON THEM + SET THE SHIPS
        public void Reset()
        {
            //SET EVERY BOARD ALL BOARD TO WATER AND ENNEMY BOARD TO UNKNOWN
            for (int i = 0; i != GRID_SIZE; ++i)
            {
                for (int j = 0; j != GRID_SIZE; ++j)
                {
                    MyGrid[i][j].Reset(SquareType.Water);
                    EnemyGrid[i][j].Reset(SquareType.Unknown);
                }
            }
            //CREATE AND SET MY SHIPS AND ENNEMY SHIPS
            myShips.ForEach(s => s.Reincarnate());
            enemyShips.ForEach(s => s.Reincarnate());
            PlaceShips();
        }
        //METHOD RETUNRS BOOL TO SEE IF SQUARE IS FREE
        private bool SquareFree(int row, int col)
        {
            return (MyGrid[row][col].ShipIndex == -1) ? true : false;
        }
        //METHOD TO PLACE SHIP DOWN, RETURNS BOOLEAN TRUE IF PLACED CORRECTLY
        private bool PlaceVertical(int shipIndex, int remainingLength)
        {
            //X AND Y POSITION TO PLACE
            int startPosRow = xAxis;
            int startPosCol = yAxis;

            //CREATING FUN TO SEE IF ITS POSSIBLE TO PLACE SHIP
            Func<bool> PlacementPossible = () =>
            {
                //VALUE FOR REMAINING LENGTH OF SHIP
                int tmp = remainingLength;
                //LOOP TO PLACE SHIP, BASED ON REMAINING OF GRID
                for (int row = startPosRow; tmp != 0; ++row)
                {
                    //IF SQUARE IS NOT FREE, RETURN FALSE
                    if (!SquareFree(row, startPosCol))
                        return false;
                    //SQUARE WAS FREE, -1 TO TEMP
                    --tmp;
                }

                //RETURN TRUE IF PLACEMENT IS DONE CORRECTLY
                return true;
            };

            //IF IT IS POSSIBLE
            if (PlacementPossible())
            {
                //LOOP TO SEE IF SQUARE IS OCCUPIED
                for (int row = startPosRow; remainingLength != 0; ++row)
                {
                    //SET THE BOARD TO UNDAMAGED
                    MyGrid[row][startPosCol].Type = SquareType.Undamaged;
                    //GIVE IT A SHIP INDEX
                    MyGrid[row][startPosCol].ShipIndex = shipIndex;
					Image image = new Image();
                    switch (ship)
                    {
                        case 0:
                            image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Images/" + skin + "/vertical/battleship/" + remainingLength.ToString() + ".png");
							image.Stretch = Stretch.UniformToFill;
							Grid.SetColumn(image, row);
							HumanGrid.Children.Add(image);
                            break;
                        case 1:
							image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Images/" + skin + "/vertical/cruiser/" + remainingLength.ToString() + ".png");
							image.Stretch = Stretch.UniformToFill;
							Grid.SetColumn(image, row);
							HumanGrid.Children.Add(image);
							break;
                        case 2:
							image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Images/" + skin + "/vertical/destroyer/" + remainingLength.ToString() + ".png");
							image.Stretch = Stretch.UniformToFill;
							Grid.SetColumn(image, row);
							HumanGrid.Children.Add(image);
							break;
                        case 3:
							image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Images/" + skin + "/vertical/submarine/" + remainingLength.ToString() + ".png");
							image.Stretch = Stretch.UniformToFill;
							Grid.SetColumn(image, row);
							HumanGrid.Children.Add(image);
							break;
						case 4:
							image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Images/" + skin + "/vertical/carrier/" + remainingLength.ToString() + ".png");
							image.Stretch = Stretch.UniformToFill;
							Grid.SetColumn(image, row);
							HumanGrid.Children.Add(image);
							break;
                    }
                    //REMAINING LENGTH -1
                    --remainingLength;
                }
                //RETURN TRUE IF PLACEMENT WAS POSSIBLE
                return true;
            }
            //RETURN FALSE IF IT WASNT
            return false;
        }

        //METHOD TO PLACE BOAT HORIZONTALLY
        private bool PlaceHorizontal(int shipIndex, int remainingLength)
        {
            //X AND Y TO PLACE
            int startPosRow = xAxis;
            int startPosCol = yAxis;

            //CREATING FUNCTION TO SEE IF IT IS POSSIBLE
            Func<bool> PlacementPossible = () =>
            {
                //VALUE FOR REMANING LENGTH OF SHIP
                int tmp = remainingLength;
                //LOOP TO PLACE SHIP, BASED ON REMANING LENGTH OF THE GRID
                for (int col = startPosCol; tmp != 0; ++col)
                {
                    //IF THE SQUARE IS NOT FREE
                    if (!SquareFree(startPosRow, col))
                        //RETURN FALSE
                        return false;
                    // IF IT IS FREE, TAKE OFF 1 TO REMAINING LENGTH
                    --tmp;
                }
                //RETURN TRUE IF PLACEMENT IS POSSIBLE
                return true;
            };
            //IF PLACEMENT IS POSSIBLE
            if (PlacementPossible())
            {
                //LOOP TO CHANGE VALUE OF BOARD
                for (int col = startPosCol; remainingLength != 0; ++col)
                {
                    //CHANGE VALUE OF BOARD TO UNDAMAGED
                    MyGrid[startPosRow][col].Type = SquareType.Undamaged;
                    //GIVE IT A SHIP INDEX
                    MyGrid[startPosRow][col].ShipIndex = shipIndex;
					Image image = new Image();
					switch (ship)
					{
						case 0:
							image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Images/" + skin + "vertical/battleship/" + remainingLength.ToString() + ".png");
							image.Stretch = Stretch.UniformToFill;
							Grid.SetRow(image, col);
							HumanGrid.Children.Add(image);
							break;
						case 1:
							image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Images/" + skin + "horizental/cruiser/" + remainingLength.ToString() + ".png");
							image.Stretch = Stretch.UniformToFill;
							Grid.SetRow(image, col);
							HumanGrid.Children.Add(image);
							break;
						case 2:
							image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Images/" + skin + "horizental/destroyer/" + remainingLength.ToString() + ".png");
							image.Stretch = Stretch.UniformToFill;
							Grid.SetRow(image, col);
							HumanGrid.Children.Add(image);
							break;
						case 3:
							image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Images/" + skin + "horizental/submarine/" + remainingLength.ToString() + ".png");
							image.Stretch = Stretch.UniformToFill;
							Grid.SetRow(image, col);
							HumanGrid.Children.Add(image);
							break;
						case 4:
							image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Images/" + skin + "horizental/carrier/" + remainingLength.ToString() + ".png");
							image.Stretch = Stretch.UniformToFill;
							Grid.SetRow(image, col);
							HumanGrid.Children.Add(image);
							break;
					}
					// -1 TO LENGTH
					--remainingLength;
                }
                //RETURN TRUE IF THIS WAS ABLE TO BE DONE
                return true;
            }
            //RETURN FALSE IF IT WASNT
            return false;
        }

        //METHOD USED SINK THE SHIP
        private void SinkShip(int i, List<List<Board>> grid)
        {
            //NESTED LOOP TO GET EVERY SQUARE IN THE ROW IF SHIP IS HORIZENTAL
            foreach (var row in grid)
            {
                foreach (var square in row)
                {
                    if (square.ShipIndex == i)
                        square.Type = SquareType.Sunk;
                }
            }
            //NESTED LOOP TO GET EVERY SQUARE IN THE COLUMN IF SHIP IS HORIZENTAL
            foreach (var col in grid)
            {
                foreach (var square in col)
                {
                    if (square.ShipIndex == i)
                        square.Type = SquareType.Sunk;
                }
            }
        }

        //METHOD USED TO SINK ALLY SHIP
        private void MineSunk(int i)
        {
            SinkShip(i, MyGrid);
        }
        //METHOD USED TO SINK ENNEMY SHIP
        public void EnemySunk(int i)
        {
            SinkShip(i, EnemyGrid);
        }

        //METHOD USED TO FIRE
        public void Fire(int row, int col, Player otherPlayer)
        {
            //VALUE FOR LOCATION DAMAGED
            int damagedIndex;
            //BOOLEAN FOR IF LOCATION HAS A SUNKEN SHIP
            bool isSunk;
            //SQUARETYPE TO VERIFY THE TYPE OF THE HITTEN LOCATION, AND IF IT IS SUNK
            SquareType newType = otherPlayer.FiredAt(row, col, out damagedIndex, out isSunk);
            //CHANGE THE SHIPINDEX TO DAMAGE INDEX
            EnemyGrid[row][col].ShipIndex = damagedIndex;

            //IF LOCATION ISSUNK IS TRUE (HITTING A SHIP)
            if (isSunk)
                EnemySunk(damagedIndex);
            else
                //IF ISSUNK IS FALSE (NOT HITTING A SHIP)
                //CHANGE THE TYPE OF ENENMYGRID AT LOCATION TO MISS
                EnemyGrid[row][col].Type = newType;
        }

        //IF YOURE GETTING FIRED AT
        private SquareType FiredAt(int row, int col, out int damagedIndex, out bool isSunk)
        {
            //VALUE TO SEE IF LOCATION GOT SUNK
            isSunk = false;
            //DAMAGE INDEX
            damagedIndex = -1;

            //SWITCH TO SEE THE TYPE OF THE LOCATION HIT
            switch (MyGrid[row][col].Type)
            {
                //IF ITS WATER, RETURN WATER
                case SquareType.Water:
                    return SquareType.Water;
                //IF ITS WATER
                case SquareType.Undamaged:
                    //VALUE TO GET TYPE OF VALUE AT [ROW][SQUARE] OF THE GRID
                    var square = MyGrid[row][col];
                    //SHIPINDEX = THE SHIPINDEX OF THAT SQUARE
                    damagedIndex = square.ShipIndex;
                    //IF SHIPINDEX IS -1 (GOT HIT), CHANGE SQUARE TO SUNK
                    if (myShips[damagedIndex].FiredAt())
                    {
                        //IS SUNK IS TRUE, AND MINESUNK IS THE SHIP INDEX OF THE SQUARE (-1)
                        MineSunk(square.ShipIndex);
                        //CHANGE SUNK TO TRUE
                        isSunk = true;
						Image image = new Image();
						image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Images/cross.jpg");
						image.Stretch = Stretch.UniformToFill;
						Grid.SetRow(image, row);
						Grid.SetColumn(image, col);
						HumanGrid.Children.Add(image);
					}
                    else
                    {
                        //SET THE TYPE OF THE SQUARE TO DAMAGED
                        square.Type = SquareType.Damaged;
						Image image = new Image();
						image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Images/X.jpg");
						image.Stretch = Stretch.UniformToFill;
						Grid.SetRow(image, row);
						Grid.SetColumn(image, col);
						HumanGrid.Children.Add(image);
					}
                    return square.Type;
                //IF ITS DAMAGED, RETURN ERROR
                case SquareType.Damaged:
                    goto default;
                //IF ITS UNKNOWN RETURN ERROR
                case SquareType.Unknown:
                    goto default;
                //IF ITS SUNK RETURN ERROR
                case SquareType.Sunk:
                    goto default;
                default:
                    throw new Exception("fail");
            }
        }


        //Method to verifiy is you have lost
        public bool Lost()
        {
            //if all your ships are sunk, end game
            return myShips.All(ship => ship.IsSunk);
        }
        //METHOD TO VERIFY IF YOU HAVE WON
        public bool Win()
        {
            //IF ALL ENNEMY SHIPS ARE SUNK, END GAME
            return enemyShips.All(ship => ship.IsSunk);
        }

        //UI shoot here anthony
        public void TakeTurnAutomated(Player otherPlayer)
        {
            bool takenShot = false;
            while (!takenShot)
            {
                int row = rnd.Next(GRID_SIZE);
                int col = rnd.Next(GRID_SIZE);

                if (EnemyGrid[row][col].Type == SquareType.Unknown)
                {
                    Fire(row, col, otherPlayer);
                    takenShot = true;
                }
            }
        }

        //METHOD USED TO PLACE SHIPS
        public void PlaceShips()
        {

            //CALL METHOD HERE WHICH ASKS USER FOR POSITION
            //TAKE OFF X AND Y FROM PLACESHIPS AND CALLING METHOD FROM MAINGAMEWINDOW TO GET LOCATION OF IMAGE CLICK

            //BOOLEAN TO GET THE VALUE HORIZENTAL (IF ITS HORIZENTAL OR VERTICAL)
            bool vertical = !horizental[ship];
            //BOOLEAN TO SEE IF SHIP WAS PLACED
            bool placed = false;

            //LOOP TO PLACE SHIPS
            for (; !placed;)
            {
                //INT FOR LENGTH OF SHIP
                int remainingLength = myShips[ship].Length;
                //IF VERTICAL IS TRUE (PLACE SHIP VERTICALLY)
                //INSERT IMAGES HERE
                if (vertical)
                    //PLACE THE SHIP VERTICALLY. BOOLEAN TO SEE IF IT SUCCEEDED OR NOT
                    placed = PlaceVertical(ship, remainingLength);
                else
                    //PLACE THE SHIP VERTICALLY, BOOLEAN TO SEE IF IT SUCCEEDED
                    placed = PlaceHorizontal(ship, remainingLength);
            }
            ship++;
        }

        //method which sees if the game is about to end or not
        public void end()
        {
            //if you win
            if (Win())
            {
                if (english)
                    MessageBox.Show("You finished the game in " + GameTime + " seconds, congratulations!");
                else
                    MessageBox.Show("Vous avez fini le jeu en " + GameTime + " secondes, bravo!");
                System.Windows.Application.Current.Shutdown();
            }
            //if you lose
            if (Lost())
            {
                if (english)
                    MessageBox.Show("You lost the game in " + GameTime + " seconds, git gud!");
                else
                    MessageBox.Show("Vous avez perdu le jeu en " + GameTime + " secondes, vous êtes mauvais!");
                System.Windows.Application.Current.Shutdown();

            }
        }
    }
}