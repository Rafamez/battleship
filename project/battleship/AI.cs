using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace battleship
{
    //public class for the AI (ennemy)
    public class AI : ISerializable
    {
		int difficulty;

        Random random = new Random();
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

        public AI(int difficulty, Player player)
        {
			this.human = player;
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
				position = human.TakeTurnAutomated(x, y);
			}
            return position;
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
                    for (int i = 0; i < ship.Length; i++)
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
                            for (int j = 0; j < ship.Length; j++)
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

        //used to deserialize data, not ready (keep difficulty)
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
