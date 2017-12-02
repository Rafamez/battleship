using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace battleship
{
    enum ShipType { Battleship, Cruiser, Destroyer, Submarine, Carrier };

    class Ship
    {
        private int health;

        private readonly ShipType type;

        private static readonly Dictionary<ShipType, int> shipLengths =
            new Dictionary<ShipType, int>()
        {
            {ShipType.Carrier, 5},
            {ShipType.Battleship, 2},
            {ShipType.Destroyer, 3},
            {ShipType.Submarine, 4},
            {ShipType.Cruiser, 3}
        };

        public Ship(ShipType type)
        {
            this.type = type;
            Reincarnate();
        }

        public void Reincarnate()
        {
            health = shipLengths[type];
        }

        public int Length
        {
            get
            {
                return shipLengths[type];
            }
        }

        public bool IsSunk
        {
            get
            {
                return health == 0 ? true : false;
            }
        }

        public bool FiredAt()
        {
            health--;
            return IsSunk;
        }
    }
}