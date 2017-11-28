using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace battleship
{
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

		//instantiate human player
		public Human(String name)
		{
			this.username = name;
		}
		//used to deserialize data, not ready (keep username + points)
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotImplementedException();
		}

        public void powerUps()
        {

        }

	}
}
