using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace battleship
{
	//class for the ships
	public class Ship : Player, ISerializable
	{
		//list for the length of each ship
		private int[] shipLength = new int[] { 2, 3, 3, 4, 5 };
		//list for each ship
		private String[] shipName = new String[] { "Patrol Boat (2)", "Submarine (3)", "Destroyer (3)", "Battleship (4)", "Aircraft Carrier (5)" };
        private List<Image> boatImages;
        private List<Image> hBoat;
        private List<Image> vBoat;

        public void useVertical(Boolean horizental)
        {
            if (!horizental)
            {
                string directory = @".\Images\";
                foreach (string myFile in
                          Directory.GetFiles(directory, "*.png", SearchOption.AllDirectories))
                {
                    Image image = new Image();
                    BitmapImage source = new BitmapImage();
                    source.BeginInit();
                    source.UriSource = new Uri(myFile, UriKind.Relative);
                    source.EndInit();
                    image.Source = source;

                    HandCards.Add(image);
                }
            }
            hBoat = null;
        }

		//used to deserialize data, not ready (keep location + alive)
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotImplementedException();
		}

		public void setShip()
		{
            
		}
	}
}