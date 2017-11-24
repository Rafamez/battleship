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

	}
}
