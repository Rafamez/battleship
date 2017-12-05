using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace battleship
{
    //public class for the AI (ennemy)
    public class AI : ISerializable
    {
        int difficulty;
        //INT FOR THE SIZE OF THE GRID
        public const int GRID_SIZE = 10;

        //AI BOARD
        public List<List<Board>> MyGrid { get; set; }

        String skin;

        Grid grid;


        //AI SHIPS
        public List<Ship> myShips = new List<Ship>();


        //VALUE TO GET THE LIST USED
        private static int ship = -1;

        private Random random = new Random();
        //Will be used only by the medium difficulty and above
        int[] tailFound;
        int[] headFound;
        int[,] search;
        String direction = "";
        Boolean reversed;
        //Will be used by hard difficulty only
        int missCount;
        Boolean even;
        int[] line;
        int currentLine;
        int[] secondLine;
        Player human;

        public AI(int difficulty, Player player, Grid grid, String skin)
        {
            this.human = player;
            this.grid = grid;
            this.skin = skin;

            this.difficulty = difficulty;
            if (difficulty >= 2)
            {
                tailFound = new int[2];
                tailFound[1] = 100;
                tailFound[2] = 100;
                search = new int[4, 2];
            }
            if (difficulty == 3)
            {
                if (random.Next(0, 2) == 0)
                {
                    even = true;
                }
                else
                {
                    even = false;
                }
                line = new int[10];
                for (int i = 0; i < line.Length; i++)
                {
                    line[i] = 0;
                }
                currentLine = random.Next(0, 10);
                secondLine = new int[10];
                for (int i = 0; i < secondLine.Length; i++)
                {
                    secondLine[i] = 0;
                }
            }
            //SET FRIENDLY GRID
            MyGrid = new List<List<Board>>();
            //LOOP TO ADD BOARDS TO THE GRIDS
            for (int i = 0; i != GRID_SIZE; ++i)
            {
                MyGrid.Add(new List<Board>());

                for (int j = 0; j != GRID_SIZE; ++j)
                {
                    MyGrid[i].Add(new Board(i, j));
                }
            }
            //ADD SHIPS TO THE BOARD'S TYPE
            foreach (ShipType type in Enum.GetValues(typeof(ShipType)))
            {
                myShips.Add(new Ship(type));
            }

            MakeMyGrid(getShipPlacement());
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
                }
            }
            //CREATE AND SET MY SHIPS AND ENNEMY SHIPS
            myShips.ForEach(s => s.Reincarnate());
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
        public void MineSunk(int i)
        {
            SinkShip(i, MyGrid);
        }

        //METHOD RETUNRS BOOL TO SEE IF SQUARE IS FREE
        private bool SquareFree(int row, int col)
        {
            return (MyGrid[row][col].ShipIndex == -1) ? true : false;
        }

        /**
         * This code will help decide what position the AI will shoot, based on difficulty
         * 
         * @Throw exception when an unexpected error occures
         * */
        public void AITurn()
        {
            //Easy AI shoots randomly
            if (this.difficulty == 1)
            {
                int[] number = shoot(random.Next(0, 10), random.Next(0, 10));
                while (number[0] == -2)
                {
                    number = shoot(random.Next(0, 10), random.Next(0, 10));
                }
            }
            //End of easy AI





            //Medium AI shoots randomly, but will his smarter if it has hit.
            else
            {

                //If a direction was found for the ship, AI will guess that direction until corrected.
                if (!(direction == ""))
                {
                    int[] number = new int[2];

                    //Picks shoot area based on direction, and adapt the direction to get ready for the next step
                    switch (direction)
                    {
                        case "Left":
                            if (reversed == false)
                            {

                                headFound[0] = headFound[0] - 1;
                                number = shoot(headFound[0], headFound[1]);
                            }
                            else
                            {
                                tailFound[0] = tailFound[0] + 1;
                                number = shoot(tailFound[0], tailFound[1]);
                            }
                            break;
                        case "Right":
                            if (reversed == false)
                            {
                                headFound[0] = headFound[0] + 1;
                                number = shoot(headFound[0], headFound[1]);
                            }
                            else
                            {
                                tailFound[0] = tailFound[0] - 1;
                                number = shoot(tailFound[0], tailFound[1]);
                            }
                            break;
                        case "Up":
                            if (reversed == false)
                            {
                                headFound[1] = headFound[1] - 1;
                                number = shoot(headFound[0], headFound[1]);
                            }
                            else
                            {
                                tailFound[1] = tailFound[1] + 1;
                                number = shoot(tailFound[0], tailFound[1]);
                            }
                            break;
                        case "Down":
                            if (reversed == false)
                            {
                                headFound[1] = headFound[1] + 1;
                                number = shoot(headFound[0], headFound[1]);
                            }
                            else
                            {
                                tailFound[1] = tailFound[1] - 1;
                                number = shoot(tailFound[0], tailFound[1]);
                            }
                            break;
                        default:
                            throw new Exception("AI made it to a direction not valid in the code");
                    }

                    //If the area is invalid or already shot
                    if (number[0] == -3 || number[0] == -2)
                    {
                        reversed = true;
                        switch (direction)
                        {
                            case "Left":
                                tailFound[0] = tailFound[0] + 1;
                                number = shoot(tailFound[0], tailFound[1]);
                                break;
                            case "Right":
                                tailFound[0] = tailFound[0] - 1;
                                number = shoot(tailFound[0], tailFound[1]);
                                break;
                            case "Up":
                                tailFound[1] = tailFound[1] + 1;
                                number = shoot(tailFound[0], tailFound[1]);
                                break;
                            case "Down":
                                tailFound[1] = tailFound[1] - 1;
                                number = shoot(tailFound[0], tailFound[1]);
                                break;
                            default:
                                throw new Exception("AI made it to a direction not valid in the code");
                        }
                        number = shoot(tailFound[0], tailFound[1]);
                    }

                    //Will add to make sure that it switches when need be (Second line if opposite of current check).
                    if (even)
                    {
                        if (number[1] % 2 == 0)
                        {
                            line[number[0]] = line[number[0]] + 1;
                        }
                        else
                        {
                            secondLine[number[0]] = secondLine[number[0]] + 1;
                        }
                    }
                    else
                    {
                        if (!(number[1] % 2 == 0))
                        {
                            line[number[0]] = line[number[0]] + 1;
                        }
                        else
                        {
                            secondLine[number[0]] = secondLine[number[0]] + 1;
                        }
                    }

                    //If area is miss
                    if (number[0] == -1)
                    {
                        reversed = true;
                    }

                    //Check if the ship has sunk
                    else if (number[0] == -4)
                    {
                        //Will reset to start search anew
                        tailFound[0] = 100;
                        tailFound[1] = 100;
                        direction = "";
                        reversed = false;
                    }

                }

                //If AI has found only a single hit, which is represented by shoot method not returning 0
                else if ((!(tailFound[0] == 100)) && (!(tailFound[0] == -1)))
                {
                    search[0, 0] = tailFound[0] - 1;
                    search[0, 1] = tailFound[1];
                    search[1, 0] = tailFound[0] + 1;
                    search[1, 1] = tailFound[1];
                    search[2, 0] = tailFound[0];
                    search[2, 1] = tailFound[1] - 1;
                    search[3, 0] = tailFound[0];
                    search[3, 1] = tailFound[1] + 1;
                    int check = random.Next(0, 4);
                    int[] number = shoot(search[check, 0], search[check, 1]);

                    //If return is -2, then that area has been shot already, or if number is -3, is invalid, and code will restart.
                    while (number[0] == -2 || number[0] == -3)
                    {
                        check = random.Next(0, 4);
                        number = shoot(search[check, 0], search[check, 1]);
                    }

                    //Will add to make sure that it switches when need be (Second line if opposite of current check).
                    if (even)
                    {
                        if (number[1] % 2 == 0)
                        {
                            line[number[0]] = line[number[0]] + 1;
                        }
                        else
                        {
                            secondLine[number[0]] = secondLine[number[0]] + 1;
                        }
                    }
                    else
                    {
                        if (!(number[1] % 2 == 0))
                        {
                            line[number[0]] = line[number[0]] + 1;
                        }
                        else
                        {
                            secondLine[number[0]] = secondLine[number[0]] + 1;
                        }
                    }

                    //If -4 is returned, it represents that the ship has sunk.
                    if (number[0] == -4)
                    {
                        tailFound[0] = 100;
                        tailFound[1] = 100;
                    }

                    //If the area shot is a hit
                    else if (!(number[0] == -1))
                    {
                        //Checks which direction the boat is most likley going
                        if ((tailFound[0] == search[check, 0]) && (tailFound[1] - 1 == search[check, 1]))
                        {
                            direction = "Up";
                        }
                        else if ((tailFound[0] == search[check, 0]) && (tailFound[1] + 1 == search[check, 1]))
                        {
                            direction = "Down";
                        }
                        else if ((tailFound[0] - 1 == search[check, 0]) && (tailFound[1] == search[check, 1]))
                        {
                            direction = "Left";
                        }
                        else
                        {
                            direction = "Right";
                        }
                        headFound[0] = search[check, 0];
                        headFound[1] = search[check, 1];
                        reversed = false;
                    }
                }

                //Medium AI will shoot randomly until it has found a hit that it has not killed.
                if (this.difficulty == 2)
                {
                    tailFound = shoot(random.Next(0, 10), random.Next(0, 10));

                    //If return is -2,-2, then area has already been shot
                    while (tailFound[0] == -2)
                    {
                        tailFound = shoot(random.Next(0, 10), random.Next(0, 10));
                    }
                }
                else
                {
                    //Will check before running if the current line has reached maximum
                    if (line[currentLine] == 5)
                    {
                        Boolean test = true;

                        //Checks to see if all lines were checked twice (or once during third go)
                        for (int i = 0; i < line.Length; i++)
                        {
                            if (line[i] < 5)
                            {
                                test = false;
                            }
                        }

                        //If all lines have been checked for the max times, it will extend or change
                        if (test)
                        {

                            //If all numbers of even/odd have been checked, then the reverse will have to be checked.                    
                            for (int i = 0; i < line.Length; i++)
                            {
                                line[i] = secondLine[i];
                            }
                            even = !even;
                        }

                        //Will switch the line that is currently on.
                        currentLine = random.Next(0, 10);
                        while (line[currentLine] == 5)
                        {
                            currentLine = random.Next(0, 10);
                        }
                    }

                    //This will change the line if it misses twice
                    if (missCount == 2)
                    {
                        missCount = 0;
                        int nope = currentLine;
                        while (currentLine == nope)
                        {
                            currentLine = random.Next(0, 10);
                        }
                    }

                    tailFound = shoot(currentLine, random.Next(0, 10));

                    //Will check to make sure random shots are checkered.
                    if (even)
                    {
                        while (!(tailFound[1] % 2 == 0))
                        {
                            tailFound = shoot(currentLine, random.Next(0, 10));
                        }
                    }
                    else
                    {
                        while (tailFound[1] % 2 == 0)
                        {
                            tailFound = shoot(currentLine, random.Next(0, 10));
                        }
                    }

                    //Represents that the area has already been shot
                    while (tailFound[1] == -2)
                    {
                        if (even)
                        {
                            while (!(tailFound[1] % 2 == 0))
                            {
                                tailFound = shoot(currentLine, random.Next(0, 10));
                            }
                        }
                        else
                        {
                            while (tailFound[1] % 2 == 0)
                            {
                                tailFound = shoot(currentLine, random.Next(0, 10));
                            }
                        }
                    }

                    if (tailFound[0] == -1)
                    {
                        missCount = missCount + 1;
                    }
                    line[currentLine] = line[currentLine] + 1;
                }

            }
            //End of medium AI and hard AI            
        }

        /**
         * This shoot method is intended for the computer to shoot at the human.
         * 
         * @paramater an int representing the column, an int representing the row
         * @return an int[] representing the area if it has been shot.
         * return -1,-1 when area is miss.
         * return -2,-2 when area has already been shot.
         * return -3,-3 when area is invalid shot area.
         * return -4,-4 when area is destroyed ship.
         * */
        public int[] shoot(int x, int y)
        {
            int[] position = new int[2];

            //Verifies if it is possible for the place to be shot.
            if (x > 9 || x < 0 || y > 9 || y < 0)
            {
                position[1] = -3;
                position[2] = -3;
            }
            else
            {
                bool isSunk;
                int damagedIndex;
                position = human.FiredAt(x, y, out damagedIndex, out isSunk);
            }
            return position;
        }

        public SquareType FiredAt(int row, int col, out int damagedIndex, out bool isSunk)
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
                //IF ITS AN UNDAMAGED SHIP
                case SquareType.Undamaged:
                    //VALUE TO GET TYPE OF VALUE AT [ROW][SQUARE] OF THE GRID
                    var square = MyGrid[row][col];
                    //SHIPINDEX = THE SHIPINDEX OF THAT SQUARE
                    damagedIndex = square.ShipIndex;
                    //IF SHIPINDEX IS <-1 (GOT HIT), CHANGE SQUARE TO SUNK
                    if (myShips[damagedIndex].FiredAt())
                    {
                        //CHANGE SUNK TO TRUE
                        square.Type = SquareType.Sunk;
                        isSunk = true;
                        Image image = new Image();
                        image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Images/cross.jpg");
                        image.Stretch = Stretch.UniformToFill;
                        Grid.SetRow(image, row);
                        Grid.SetColumn(image, col);
                        grid.Children.Add(image);
                        if (myShips[damagedIndex].healthReturn == 0)
                        {
                            MessageBox.Show(myShips[damagedIndex].ToString() + " has been sunk");
                            //IS SUNK IS TRUE, AND MINESUNK IS THE SHIP INDEX OF THE SQUARE
                            MineSunk(square.ShipIndex);
                        }
                    }
                    else
                    {
                        //SET THE TYPE OF THE SQUARE TO DAMAGED
                        square.Type = SquareType.Miss;
                        Image image = new Image();
                        image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Images/X.jpg");
                        image.Stretch = Stretch.UniformToFill;
                        Grid.SetRow(image, row);
                        Grid.SetColumn(image, col);
                        grid.Children.Add(image);
                    }
                    return square.Type;
                //IF ITS DAMAGED, RETURN ERROR
                case SquareType.Miss:
                    goto default;
                //IF ITS UNKNOWN RETURN ERROR
                //IF ITS SUNK RETURN ERROR
                case SquareType.Sunk:
                    goto default;
                default:
                    throw new Exception("fail");
            }
        }

        /**
		 * This code will return a int representation of how the AI ship should be made
		 * 1 = Carrier (Size 5)
		 * 2 = Battleship (Size 4)
		 * 3 = Cruiser (Size 3)
		 * 4 = Submarine (Size 3)
		 * 5 = Destroyer (Size 2)
		 * @ return an int[10,10] representing the grid
		 * */
        public int[,] getShipPlacement()
        {
            int[,] grid = new int[10, 10];
            int[] ship = new int[5];
            ship[0] = 1;
            ship[1] = 2;
            ship[2] = 3;
            ship[3] = 4;
            ship[4] = 5;
            int check;
            for (int count = 0; count < 5; count++)
            {
                check = random.Next(0, 5);
                int number = ship[check];
                while (number == 0)
                {
                    check = random.Next(0, 5);
                    number = ship[check];
                }
                ship[check] = 0;
                int size = 0;
                switch (number)
                {
                    case 1:
                        size = 5;
                        break;
                    case 2:
                        size = 4;
                        break;
                    case 3:
                    case 4:
                        size = 3;
                        break;
                    case 5:
                        size = 2;
                        break;
                }
                int[,] addShip = new int[size, 2];
                while (true)
                {
                    addShip[0, 0] = random.Next(0, 10);
                    addShip[0, 1] = random.Next(0, 10);
                    int direction = random.Next(0, 4);
                    switch (direction)
                    {
                        //Up
                        case 0:
                            for (int i = 1; i < size; i++)
                            {
                                addShip[i, 0] = addShip[0, 0];
                                addShip[i, 1] = addShip[i - 1, 1] - 1;
                            }
                            break;
                        //Down
                        case 1:
                            for (int i = 1; i < size; i++)
                            {
                                addShip[i, 0] = addShip[0, 0];
                                addShip[i, 1] = addShip[i - 1, 1] + 1;
                            }
                            break;
                        //Left
                        case 2:
                            for (int i = 1; i < size; i++)
                            {
                                addShip[i, 0] = addShip[i - 1, 0] - 1;
                                addShip[i, 1] = addShip[0, 1];
                            }
                            break;
                        case 3:
                            for (int i = 1; i < size; i++)
                            {
                                addShip[i, 0] = addShip[i - 1, 0] + 1;
                                addShip[i, 1] = addShip[0, 1];
                            }
                            break;
                    }
                    Boolean valid = true;
                    for (int i = 0; i < size; i++)
                    {
                        if (addShip[i, 0] < 0 || addShip[i, 0] > 9 || addShip[i, 1] < 0 || addShip[i, 1] > 9)
                        {
                            valid = false;
                            break;
                        }
                        if (!(grid[addShip[i, 0], addShip[i, 1]] == 0))
                        {
                            valid = false;
                            break;
                        }
                    }
                    if (valid)
                    {
                        if (this.difficulty == 3)
                        {
                            for (int j = 0; j < size; j++)
                            {
                                if (!((addShip[j, 0] - 1) < 0))
                                {
                                    if (!(grid[addShip[j, 0] - 1, addShip[j, 1]] == 0))
                                    {
                                        valid = false;
                                        break;
                                    }
                                }
                                if (!((addShip[j, 0] + 1) > 9))
                                {
                                    if (!(grid[addShip[j, 0] + 1, addShip[j, 1]] == 0))
                                    {
                                        valid = false;
                                        break;
                                    }
                                }
                                if (!((addShip[j, 1] - 1) < 0))
                                {
                                    if (!(grid[addShip[j, 0], addShip[j, 1] - 1] == 0))
                                    {
                                        valid = false;
                                        break;
                                    }
                                }
                                if (!((addShip[j, 1] + 1) > 9))
                                {
                                    if (!(grid[addShip[j, 0], addShip[j, 1] + 1] == 0))
                                    {
                                        valid = false;
                                        break;
                                    }
                                }
                                if ((!((addShip[j, 0] - 1) < 0)) && (!((addShip[j, 1] - 1) < 0)))
                                {
                                    if (!(grid[addShip[j, 0] - 1, addShip[j, 1] - 1] == 0))
                                    {
                                        valid = false;
                                        break;
                                    }
                                }
                                if ((!((addShip[j, 0] - 1) < 0)) && (!((addShip[j, 1] + 1) > 9)))
                                {
                                    if (!(grid[addShip[j, 0] - 1, addShip[j, 1] + 1] == 0))
                                    {
                                        valid = false;
                                        break;
                                    }
                                }
                                if ((!((addShip[j, 0] + 1) > 9)) && (!((addShip[j, 1] - 1) < 0)))
                                {
                                    if (!(grid[addShip[j, 0] + 1, addShip[j, 1] - 1] == 0))
                                    {
                                        valid = false;
                                        break;
                                    }
                                }
                                if ((!((addShip[j, 0] + 1) > 9)) && (!((addShip[j, 1] + 1) > 9)))
                                {
                                    if (!(grid[addShip[j, 0] + 1, addShip[j, 1] + 1] == 0))
                                    {
                                        valid = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (valid == false)
                    {
                        continue;
                    }
                    for (int i = 0; i < size; i++)
                    {
                        grid[addShip[i, 0], addShip[i, 1]] = number;
                    }
                    break;
                }
            }
            return grid;
        }


        public bool Lost()
        {
            //if all your ships are sunk, end game
            return myShips.All(ship => ship.IsSunk);
        }

        /**
         * This code will let the player build the grid.
         * 
         * @Parameter an int[,] representing what the gird will become.
         * 
         * */
        public void MakeMyGrid(int[,] grid)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    MyGrid[i][j].Type = SquareType.Unknown;
                    switch (grid[i, j])
                    {
                        case 0:
                            MyGrid[i][j].ShipIndex = -1;
                            break;
                        case 1:
                            MyGrid[i][j].ShipIndex = 4;
                            break;
                        case 2:
                            MyGrid[i][j].ShipIndex = 0;
                            break;
                        case 3:
                            MyGrid[i][j].ShipIndex = 1;
                            break;
                        case 4:
                            MyGrid[i][j].ShipIndex = 3;
                            break;
                        case 5:
                            MyGrid[i][j].ShipIndex = 2;
                            break;
                    }
                }
            }
        }
        //used to deserialize data, not ready (keep difficulty)
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        public void reveal()
        {
            int battleShip = 1;
            int cruiser = 1;
            int destroyer = 1;
            int submarine = 1;
            int carrier = 1;
            for (int i = 0; i < MyGrid[i].Count; i++)
            {
                for (int j = 0; j < MyGrid[j].Count; j++)
                {
                    if (MyGrid[i][j].Type == SquareType.Unknown)
                    {
                        Boolean horizontal = true;
                        switch (MyGrid[i][j].ShipIndex)
                        {
                            case -1:
                                MyGrid[i][j].Type = SquareType.Water;
                                break;
                            case 0:
                                MyGrid[i][j].Type = SquareType.Undamaged;                                
                                if (!(i - 1 < 0))
                                {
                                    if (MyGrid[i][j].ShipIndex == MyGrid[i-1][j].ShipIndex)
                                    {
                                        horizontal = false;
                                    }
                                }
                                if (!(i + 1 > 9))
                                {
                                    if (MyGrid[i][j].ShipIndex == MyGrid[i + 1][j].ShipIndex)
                                    {
                                        horizontal = false;
                                    }
                                }
                                if (horizontal)
                                {
                                    Image image = new Image();
                                    image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "horizental/battleship" + battleShip.ToString() + ".png");
                                    image.Stretch = Stretch.UniformToFill;
                                    Grid.SetRow(image, i);
                                    Grid.SetColumn(image, j);
                                    grid.Children.Add(image);
                                    battleShip++;
                                }
                                else
                                {
                                    Image image = new Image();
                                    image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "vertical/battleship" + battleShip.ToString() + ".png");
                                    image.Stretch = Stretch.UniformToFill;
                                    Grid.SetRow(image, i);
                                    Grid.SetColumn(image, j);
                                    grid.Children.Add(image);
                                    battleShip++;
                                }
                                break;
                            case 1:
                                MyGrid[i][j].Type = SquareType.Undamaged;
                                if (!(i - 1 < 0))
                                {
                                    if (MyGrid[i][j].ShipIndex == MyGrid[i - 1][j].ShipIndex)
                                    {
                                        horizontal = false;
                                    }
                                }
                                if (!(i + 1 > 9))
                                {
                                    if (MyGrid[i][j].ShipIndex == MyGrid[i + 1][j].ShipIndex)
                                    {
                                        horizontal = false;
                                    }
                                }
                                if (horizontal)
                                {
                                    Image image = new Image();
                                    image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "horizental/cruiser" + cruiser.ToString() + ".png");
                                    image.Stretch = Stretch.UniformToFill;
                                    Grid.SetRow(image, i);
                                    Grid.SetColumn(image, j);
                                    grid.Children.Add(image);
                                    cruiser++;
                                }
                                else
                                {
                                    Image image = new Image();
                                    image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "vertical/cruiser" + cruiser.ToString() + ".png");
                                    image.Stretch = Stretch.UniformToFill;
                                    Grid.SetRow(image, i);
                                    Grid.SetColumn(image, j);
                                    grid.Children.Add(image);
                                    cruiser++;
                                }
                                break;
                            case 2:
                                MyGrid[i][j].Type = SquareType.Undamaged;
                                if (!(i - 1 < 0))
                                {
                                    if (MyGrid[i][j].ShipIndex == MyGrid[i - 1][j].ShipIndex)
                                    {
                                        horizontal = false;
                                    }
                                }
                                if (!(i + 1 > 9))
                                {
                                    if (MyGrid[i][j].ShipIndex == MyGrid[i + 1][j].ShipIndex)
                                    {
                                        horizontal = false;
                                    }
                                }
                                if (horizontal)
                                {
                                    Image image = new Image();
                                    image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "horizental/destroyer" + destroyer.ToString() + ".png");
                                    image.Stretch = Stretch.UniformToFill;
                                    Grid.SetRow(image, i);
                                    Grid.SetColumn(image, j);
                                    grid.Children.Add(image);
                                    destroyer++;
                                }
                                else
                                {
                                    Image image = new Image();
                                    image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "vertical/destroyer" + destroyer.ToString() + ".png");
                                    image.Stretch = Stretch.UniformToFill;
                                    Grid.SetRow(image, i);
                                    Grid.SetColumn(image, j);
                                    grid.Children.Add(image);
                                    destroyer++;
                                }
                                break;
                            case 3:
                                MyGrid[i][j].Type = SquareType.Undamaged;
                                if (!(i - 1 < 0))
                                {
                                    if (MyGrid[i][j].ShipIndex == MyGrid[i - 1][j].ShipIndex)
                                    {
                                        horizontal = false;
                                    }
                                }
                                if (!(i + 1 > 9))
                                {
                                    if (MyGrid[i][j].ShipIndex == MyGrid[i + 1][j].ShipIndex)
                                    {
                                        horizontal = false;
                                    }
                                }
                                if (horizontal)
                                {
                                    Image image = new Image();
                                    image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "horizental/submarine" + submarine.ToString() + ".png");
                                    image.Stretch = Stretch.UniformToFill;
                                    Grid.SetRow(image, i);
                                    Grid.SetColumn(image, j);
                                    grid.Children.Add(image);
                                    submarine++;
                                }
                                else
                                {
                                    Image image = new Image();
                                    image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "vertical/submarine" + submarine.ToString() + ".png");
                                    image.Stretch = Stretch.UniformToFill;
                                    Grid.SetRow(image, i);
                                    Grid.SetColumn(image, j);
                                    grid.Children.Add(image);
                                    submarine++;
                                }
                                break;
                            case 4:
                                MyGrid[i][j].Type = SquareType.Undamaged;
                                if (!(i - 1 < 0))
                                {
                                    if (MyGrid[i][j].ShipIndex == MyGrid[i - 1][j].ShipIndex)
                                    {
                                        horizontal = false;
                                    }
                                }
                                if (!(i + 1 > 9))
                                {
                                    if (MyGrid[i][j].ShipIndex == MyGrid[i + 1][j].ShipIndex)
                                    {
                                        horizontal = false;
                                    }
                                }
                                if (horizontal)
                                {
                                    Image image = new Image();
                                    image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "horizental/carrier" + carrier.ToString() + ".png");
                                    image.Stretch = Stretch.UniformToFill;
                                    Grid.SetRow(image, i);
                                    Grid.SetColumn(image, j);
                                    grid.Children.Add(image);
                                    carrier++;
                                }
                                else
                                {
                                    Image image = new Image();
                                    image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "vertical/carrier" + carrier.ToString() + ".png");
                                    image.Stretch = Stretch.UniformToFill;
                                    Grid.SetRow(image, i);
                                    Grid.SetColumn(image, j);
                                    grid.Children.Add(image);
                                    carrier++;
                                }
                                break;
                        }
                    }
                }
            }
        }

        public void hide()
        {
            for (int i = 0; i < MyGrid[i].Count; i++)
            {
                for (int j = 0; j < MyGrid[j].Count; j++)
                {
                    switch (MyGrid[i][j].Type)
                    {
                        case SquareType.Undamaged:
                        case SquareType.Water:
                            MyGrid[i][j].Type = SquareType.Unknown;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}

