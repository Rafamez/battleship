using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//INSPIRED FROM JAMESJRG CODE FROM https://github.com/jamesjrg/battleship/tree/master/Battleship
namespace battleship
{
    //ENUMS REPRESENTING SHIPS
    public enum ShipType { Battleship, Cruiser, Destroyer, Submarine, Carrier };

	public class Ship
	{
		//VALUE REPRESENTING HEALTH OF SHIPS (LENGTH - DAMAGE TAKEN)
		private int health;

		public int healthReturn
		{
			get { return health; }
		} 
        //VALUE FOR TYPE OF THE SHIP
        public readonly ShipType type;
        //PARAMETER FOR BOOLEANS, ADDING LENGTH VALUE TO THEIR NAMES
        private static readonly Dictionary<ShipType, int> shipLengths =
            new Dictionary<ShipType, int>()
        {
            {ShipType.Carrier, 5},
            {ShipType.Battleship, 2},
            {ShipType.Destroyer, 3},
            {ShipType.Submarine, 4},
            {ShipType.Cruiser, 3}
        };

		//CONSTRUCTOR TO CREATE SHIP
		public Ship(ShipType type)
		{
			//GIVE IT THE TYPE GIVEN
			this.type = type;
			//GIVE IT HEALTH
			Reincarnate();
		}

		//METHOD TO SET HEALTH VALUE DEPENDING ON HEIGHT
		public void Reincarnate()
		{
			health = shipLengths[type];
		}

		//METHOD TO GET LENGTH OF SHIP
		public int Length
		{
			get
			{
				return shipLengths[type];
			}
		}

		//METHOD TO SEE IF BOAT SUNK
		public bool IsSunk
		{
			get
			{
				return health == 0 ? true : false;
			}
		}

		//IF YOU GET FIRED AT, TAKE -1 TO HEALTH  AND VERIFY IF SHIP IS SUNK
		public bool FiredAt()
		{
			health--;
			return IsSunk;
		}
	}
}