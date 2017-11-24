using System;


namespace battleship
{
	public class AI
{
		//public class for the AI (ennemy)
		public class AI : Player, ISerializable
		{
			public int getDifficulty
			{
				get { return difficulty; }
				set { value = difficulty; }
			}

			public AI(int difficulty)
			{
				this.difficulty = difficulty;
			}

			//used to deserialize data, not ready (keep difficulty)
			public void GetObjectData(SerializationInfo info, StreamingContext context)
			{
				throw new NotImplementedException();
			}
		}



	}
}
