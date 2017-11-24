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
		public int getDifficulty
		{
			get { return getDifficulty; }
			set { value = getDifficulty; }
		}

		public AI(int difficulty)
		{
			this.getDifficulty = difficulty;
		}

		//used to deserialize data, not ready (keep difficulty)
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotImplementedException();
		}
	}
}