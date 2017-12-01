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
        int tailFound;
        int headFound;
        int[] search;
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
                tailFound = 0;
                search = new int[4];
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

        public void AITurn()
        {
            int skill = 0;
            //Easy AI shoots randomly
            if (this.getDifficulty == 1)
            {
                skill = random.Next(-100, 0);
            }
            //Medium AI shoots randomly, but will his smarter if it has hit.
            else if (this.getDifficulty == 2)
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
                //AI will shoot randomly until it has found a hit that it has not killed.
                else
                {

                    skill = random.Next(-100, 0);
                    tailFound = shoot(skill);
                    //If return is 102, then that area has been shot already.
                    while (tailFound == 102)
                    {
                        skill = random.Next(-100, 0);
                        tailFound = shoot(skill);
                    }
                }
            }
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

        public int shoot(int number)
        {
           return 1;
        }

        //used to deserialize data, not ready (keep difficulty)
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }       
    }
}