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
	public class Player : mainGameWindow
	{
        public const int GRID_SIZE = 10;

        static public Random rnd = new Random();

        public String username;

        public List<List<Board>> MyGrid { get; set; }
        public List<List<Board>> EnemyGrid { get; set; }

        private List<Ship> myShips = new List<Ship>();
        private List<Ship> enemyShips = new List<Ship>();

        public Player(String name)
        {
            username = name;
            MyGrid = new List<List<Board>>();
            EnemyGrid = new List<List<Board>>();
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

            foreach (ShipType type in Enum.GetValues(typeof(ShipType)))
            {
                myShips.Add(new Ship(type));
                enemyShips.Add(new Ship(type));
            }
    }


        private bool SquareFree(int row, int col)
        {
            return (MyGrid[row][col].ShipIndex == -1) ? true : false;
        }

        private bool PlaceVertical(int shipIndex, int remainingLength, int x, int y)
        {
            int startPosRow = x;
            int startPosCol = y;

            Func<bool> PlacementPossible = () =>
            {
                int tmp = remainingLength;
                for (int row = startPosRow; tmp != 0; ++row)
                {
                    if (!SquareFree(row, startPosCol))
                        return false;
                    --tmp;
                }
                return true;
            };

            if (PlacementPossible())
            {
                for (int row = startPosRow; remainingLength != 0; ++row)
                {
                    MyGrid[row][startPosCol].Type = SquareType.Undamaged;
                    MyGrid[row][startPosCol].ShipIndex = shipIndex;
                    --remainingLength;
                }
                return true;
            }

            return false;
        }

        private bool PlaceHorizontal(int shipIndex, int remainingLength, int x, int y)
        {
            int startPosRow = x;
            int startPosCol = y;

            Func<bool> PlacementPossible = () =>
            {
                int tmp = remainingLength;
                for (int col = startPosCol; tmp != 0; ++col)
                {
                    if (!SquareFree(startPosRow, col))
                        return false;
                    --tmp;
                }
                return true;
            };

            if (PlacementPossible())
            {
                for (int col = startPosCol; remainingLength != 0; ++col)
                {
                    MyGrid[startPosRow][col].Type = SquareType.Undamaged;
                    MyGrid[startPosRow][col].ShipIndex = shipIndex;
                    --remainingLength;
                }
                return true;
            }

            return false;
        }


        private void SinkShip(int i, List<List<Board>> grid)
        {
            foreach (var row in grid)
            {
                foreach (var square in row)
                {
                    if (square.ShipIndex == i)
                        square.Type = SquareType.Sunk;
                }
            }
        }

        private void MineSunk(int i)
        {
            SinkShip(i, MyGrid);
        }

        public void EnemySunk(int i)
        {
            SinkShip(i, EnemyGrid);
        }


        protected void Fire(int row, int col, Player otherPlayer)
        {
            int damagedIndex;
            bool isSunk;
            SquareType newType = otherPlayer.FiredAt(row, col, out damagedIndex, out isSunk);
            EnemyGrid[row][col].ShipIndex = damagedIndex;

            if (isSunk)
                EnemySunk(damagedIndex);
            else
                EnemyGrid[row][col].Type = newType;
        }

        public SquareType FiredAt(int row, int col, out int damagedIndex, out bool isSunk)
        {
            isSunk = false;
            damagedIndex = -1;

            switch (MyGrid[row][col].Type)
            {
                case SquareType.Water:
                    return SquareType.Water;
                case SquareType.Undamaged:
                    var square = MyGrid[row][col];
                    damagedIndex = square.ShipIndex;
                    if (myShips[damagedIndex].FiredAt())
                    {
                        MineSunk(square.ShipIndex);
                        isSunk = true;
                    }
                    else
                    {
                        square.Type = SquareType.Damaged;
                    }
                    return square.Type;
                case SquareType.Damaged:
                    goto default;
                case SquareType.Unknown:
                    goto default;
                case SquareType.Sunk:
                    goto default;
                default:
                    throw new Exception("fail");
            }
        }

        public bool Lost()
        {
            return myShips.All(ship => ship.IsSunk);
        }

        public bool Win()
        {
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


    private void PlaceShips(int x, int y)
        {

            //CALL METHOD HERE WHICH ASKS USER FOR POSITION
            ///TAKE OFF X AND Y FROM PLACESHIPS AND CALLING METHOD FROM MAINGAMEWINDOW TO GET LOCATION OF IMAGE CLICK
            bool startAgain = false;

            for (int i = 0; i != myShips.Count && !startAgain; ++i)
            {
                bool vertical = horizental[i];
                bool placed = false;

                int loopCounter = 0;
                for (; !placed && loopCounter != 10000; ++loopCounter)
                {
                    int remainingLength = myShips[i].Length;

                    if (vertical)
                        placed = PlaceVertical(i, remainingLength, x, y);
                    else
                        placed = PlaceHorizontal(i, remainingLength, x, y);
                }

                if (loopCounter == 10000)
                    startAgain = true;
            }

            if (startAgain)
                PlaceShips(x, y);
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