using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace battleship
{
    //public class for the AI (ennemy)
    public class AI : Player, ISerializable
    {
        //Note that following will have to be added to shoot that returns int of position number. 0 = miss. 101 = ship destroyed. 102 = Already shot. 103 = Invalid.
        //Might add extentions for different AI levels -Anthony
        public int getDifficulty
        {
            get { return difficulty; }
            set { value = difficulty; }
        }

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

        public AI(int difficulty)
        {
            this.difficulty = difficulty;
            if (difficulty >= 2)
            {
                tailFound = new int[2];
                tailFound[1] = 100;
                tailFound[2] = 100;
                search = new int[4,2];
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
            if (this.getDifficulty == 1)
            {
                int[] number = shoot(random.Next(0, 10), random.Next(0,10));
                while (number[0] == -2)
                {
                    number = shoot(random.Next(0, 10), random.Next(0, 10));
                }
            }
            //End of easy AI





            //Medium AI shoots randomly, but will his smarter if it has hit.
            else if (this.getDifficulty == 2)
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
                                number = tailFound - 1;
                                tailFound = tailFound - 1;
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
                                number = tailFound + 1;
                                tailFound = tailFound + 1;
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
                                number = tailFound - 10;
                                tailFound = tailFound + 10;
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
                                number = tailFound + 10;
                                tailFound = tailFound + 10;
                            }
                            break;
                        default:
                            throw new Exception("AI made it to a direction not valid in the code");
                    }

                    //If the area is invalid or already shot
                    if (check == 103 || check == 102)
                    {
                        reversed = true;
                        switch (direction)
                        {
                            case "Left":
                                number = tailFound - 1;
                                tailFound = tailFound - 1;
                                break;
                            case "Right":
                                number = tailFound + 1;
                                tailFound = tailFound + 1;
                                break;
                            case "Up":
                                number = tailFound - 10;
                                tailFound = tailFound - 10;
                                break;
                            case "Down":
                                number = tailFound + 10;
                                tailFound = tailFound + 10;
                                break;
                            default:
                                throw new Exception("AI made it to a direction not valid in the code");
                        }
                        check = shoot(number);
                    }

                    //If area is miss
                    if (check == 0)
                    {
                        reversed = true;
                    }

                    //Check if the ship has sunk
                    else if (check == 101)
                    {
                        //Will reset to start search anew
                        tailFound[0] = 100;
                        tailFound[1] = 100;
                        direction = "";
                        reversed = false;
                    }

                }

                //If AI has found only a single hit, which is represented by shoot method not returning 0
                else if (!(tailFound[0] == 100))
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
                        if ((tailFound[0] == search[check,0]) && (tailFound[1] - 1 == search[check,1]))
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

                //AI will shoot randomly until it has found a hit that it has not killed.
                else
                {
                    tailFound = shoot(random.Next(0,10),random.Next(0,10));

                    //If return is -2,-2, then area has already been shot
                    while (tailFound[0] == -2)
                    {
                        tailFound = shoot(random.Next(0, 10), random.Next(0, 10));
                    }
                }
            }
            //End of medium AI





            //Hard AI will follow a stragedy when picking randomly
            else
            {

                //If a direction was found for the ship, AI will guess that direction until corrected.
                if (!(direction == ""))
                {
                    int number = 0;

                    //Picks shoot area based on direction, and adapt the direction to get ready for the next step
                    switch (direction)
                    {
                        case "Left":
                            if (reversed == false)
                            {
                                number = headFound + 1;
                                headFound = headFound + 1;
                            }
                            else
                            {
                                number = tailFound - 1;
                                tailFound = tailFound - 1;
                            }
                            break;
                        case "Right":
                            if (reversed == false)
                            {
                                number = headFound - 1;
                                headFound = headFound - 1;
                            }
                            else
                            {
                                number = tailFound + 1;
                                tailFound = tailFound + 1;
                            }
                            break;
                        case "Up":
                            if (reversed == false)
                            {
                                number = headFound + 10;
                                headFound = headFound + 10;
                            }
                            else
                            {
                                number = tailFound - 10;
                                tailFound = tailFound + 10;
                            }
                            break;
                        case "Down":
                            if (reversed == false)
                            {
                                number = headFound - 10;
                                headFound = headFound - 10;
                            }
                            else
                            {
                                number = tailFound + 10;
                                tailFound = tailFound + 10;
                            }
                            break;
                        default:
                            throw new Exception("AI made it to a direction not valid in the code");
                    }
                    int check = shoot(number);

                    //If the area is invalid or already shot
                    if (check == 103 || check == 102)
                    {
                        reversed = true;
                        switch (direction)
                        {
                            case "Left":
                                number = tailFound - 1;
                                tailFound = tailFound - 1;
                                break;
                            case "Right":
                                number = tailFound + 1;
                                tailFound = tailFound + 1;
                                break;
                            case "Up":
                                number = tailFound - 10;
                                tailFound = tailFound - 10;
                                break;
                            case "Down":
                                number = tailFound + 10;
                                tailFound = tailFound + 10;
                                break;
                            default:
                                throw new Exception("AI made it to a direction not valid in the code");
                        }
                        check = shoot(number);
                    }

                    //Will add to make sure that it switches when need be (Second line if opposite of current check).
                    if (even)
                    {
                        //TO DO LATER
                    }
                    else
                    {
                        //TO DO LATER
                    }
                    //If area is miss
                    if (check == 0)
                    {
                        reversed = true;
                    }

                    //Check if the ship has sunk
                    else if (check == 101)
                    {

                        //Will reset to start search anew
                        tailFound = 0;
                        headFound = 0;
                        search[0] = 0;
                        search[1] = 0;
                        search[2] = 0;
                        search[3] = 0;
                        direction = "";
                        reversed = false;
                    }
                }

                //If AI has found only a single hit, which is represented by shoot method not returning 0
                else if (!(tailFound == 0))
                {
                    search[0] = tailFound - 1;
                    search[1] = tailFound + 1;
                    search[2] = tailFound - 10;
                    search[3] = tailFound + 10;
                    int check;
                    check = random.Next(0, 4);
                    skill = search[check];
                    int number;
                    number = shoot(skill);

                    //If return is 102, then that area has been shot already, or if number is 103, is invalid, and code will restart.
                    while (number == 102 || number == 103)
                    {
                        check = random.Next(0, 4);
                        skill = search[check];
                        number = shoot(skill);
                    }

                    //Will add to make sure that it switches when need be (Second line if opposite of current check).
                    if (even)
                    {
                        //TO DO LATER
                    }
                    else
                    {
                        //TO DO LATER
                    }

                    //If 101 is returned, it represents that the ship has sunk.
                    if (number == 101)
                    {
                        tailFound = 0;
                        search[0] = 0;
                        search[1] = 0;
                        search[2] = 0;
                        search[3] = 0;
                        direction = "";
                    }

                    //If the area shot is a hit
                    else if (!(number == 0))
                    {
                        if (tailFound + 10 == search[check])
                        {
                            direction = "Up";
                        }
                        else if (tailFound - 10 == search[check])
                        {
                            direction = "Down";
                        }
                        else if (tailFound + 1 == search[check])
                        {
                            direction = "Left";
                        }
                        else
                        {
                            direction = "Right";
                        }
                        headFound = search[check];
                        reversed = false;
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
                                line[i] = 0 + secondLine[i];
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

                    skill = (currentLine * 10) + random.Next(0, 10);

                    //Will check to make sure random shots are checkered.
                    if (even)
                    {
                        while (!(skill % 2 == 0))
                        {
                            skill = (currentLine * 10) + random.Next(0, 10);
                        }
                    }
                    else
                    {
                        while (skill % 2 == 0)
                        {
                            skill = (currentLine * 10) + random.Next(0, 10);
                        }
                    }
                    int check = shoot(skill);

                    //Represents that the area has already been shot
                    while (check == 102)
                    {
                        if (even)
                        {
                            while (!(skill % 2 == 0))
                            {
                                skill = (currentLine * 10) + random.Next(0, 10);
                            }
                        }
                        else
                        {
                            while (skill % 2 == 0)
                            {
                                skill = (currentLine * 10) + random.Next(0, 10);
                            }
                        }
                        check = shoot(skill);
                    }
                    if (check == 0)
                    {
                        missCount = missCount + 1;
                    }
                    line[currentLine] = line[currentLine] + 1;
                }
            }
        }

        /**
         * This shoot method is intended for the computer to shoot at the human.
         * 
         * @paramater an int representing the column, an int representing the row
         * @return an int[] representing the area if it has been shot.
         * return -1,-1 when area is miss. (NOT INSERTED)
         * return -2,-2 when area has already been shot. (NOT INSERTED)
         * return -3,-3 when area is invalid shot area.
         * return -4,-4 when area is destroyed area. (NOT INSERTED)
         * */
        public int[] shoot(int x, int y)
        {
           int[] position = new int[2];
           if (x > 9 || x < 0 || y > 9 || y < 0)
           {
                position[1] = -3;
                position[2] = -3;
           }
           return position;
        }

        //used to deserialize data, not ready (keep difficulty)
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }       
    }
}