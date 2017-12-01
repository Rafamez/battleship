using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace battleship
{
    //class for the ships

    public class Ship : mainGameWindow, ISerializable

    {
        //list for the length of each ship
        private int[] shipLength = new int[] { 2, 3, 3, 4, 5 };
        //list for each ship
        private String[] shipName = new String[] { "patrolBoat", "submarine", "submarine2", "destroyer", "aircraftCarrier" };
        //lists for images
        public List<List<Image>> hBoat;
        public List<List<Image>> vBoat;
		public List<Image> iconsH;
		public List<Image> iconsV;
        //the boat clicked
        List<Boolean> boatClicked= new List<Boolean> { false, false, false, false, false};
		List<Boolean> horizental= new List<Boolean>{ true, true, true, true, true };


        //used to deserialize data, not ready (keep location + alive)
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
        public void choseShip(object sender)
        {
            Image newimage = new Image();
            newimage.Source = ((Image)sender).Source;
			if (newimage.Source.ToString() == "Images/" + skin + "/patrolBoatH.png" || newimage.Source.ToString() == "Images/" + skin + "/patrolBoatV.png")
			{
				boatClicked[0] = true;
				boatClicked[1] = false;
				boatClicked[2] = false;
				boatClicked[3] = false;
				boatClicked[4] = false;

			}
			if (newimage.Source.ToString() == "Images/" + skin + "/submarineH.png" || newimage.Source.ToString() == "Images/" + skin + "/submarineV.png")
			{
				boatClicked[0] = false;
				boatClicked[1] = true;
				boatClicked[2] = false;
				boatClicked[3] = false;
				boatClicked[4] = false;

			}
			if (newimage.Source.ToString() == "Images/" + skin + "/battleshipH.png" || newimage.Source.ToString() == "Images/" + skin + "/battleshipV.png")
			{
				boatClicked[0] = false;
				boatClicked[1] = false;
				boatClicked[2] = true;
				boatClicked[3] = false;
				boatClicked[4] = false;

			}
			if (newimage.Source.ToString() == "Images/" + skin + "/battleshipH.png" || newimage.Source.ToString() == "Images/" + skin + "/battleshipV.png")
			{
				boatClicked[0] = false;
				boatClicked[1] = false;
				boatClicked[2] = false;
				boatClicked[3] = true;
				boatClicked[4] = false;

			}
			if (newimage.Source.ToString() == "Images/" + skin + "/aircraftCarrierH.png" || newimage.Source.ToString() == "Images/" + skin + "/aircraftCarrierV.png")
			{

				boatClicked[0] = false;
				boatClicked[1] = false;
				boatClicked[2] = false;
				boatClicked[3] = false;
				boatClicked[4] = true;


			}
			boatClicked[0] = false;
			boatClicked[1] = false;
			boatClicked[2] = false;
			boatClicked[3] = false;
			boatClicked[4] = false;
        }


        public void setShip(Object sender, int x)
        {
			Image newimage = new Image();
			newimage.Source = ((Image)sender).Source;
				boatFace(x);

			

		}


        //method used to adding images in a list which will be used to put the ships on the grid
        public void boatFace(int x, int y)
        {
			Image image=null;
            if (!horizental[x])
            {
				try
				{
					image = (Image)GameGrid.FindName("AITextBlock" + (x * y).ToString());
				}
				catch(Exception){}
				image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("Images\\patrolV.png");
				horizental[x] = !horizental[x];
            }
            else
            {

				horizental[x] = !horizental[x];
            }
        }

		private void setLocation(Object sender, int x)
		{
			if (horizental[x]) {

				Image newimage = new Image();
				newimage.Source = ((Image)sender).Source;

				//newimage.Source = hBoat[x];
					}
		}



		private void Setup(int x)
		{
			iconsHFill(x);
			iconsVFill(x);
			boatsHFill(x);
			boatsVFill(x);
		}

		private void iconsHFill(int x) {
			string directoryH = @".\Images\" + skin;
			foreach (string myFile in
					  Directory.GetFiles(directoryH, shipName[x] + "*H.png", SearchOption.AllDirectories))
			{
				Image image = new Image();
				BitmapImage source = new BitmapImage();
				source.BeginInit();
				source.UriSource = new Uri(myFile, UriKind.Relative);
				source.EndInit();
				image.Source = source;

				image.Opacity = 100;

				iconsH.Add(image);
			}

		}

		private void iconsVFill(int x)
		{
			string directoryV = @".\Images\" + skin;
			foreach (string myFile in
					  Directory.GetFiles(directoryV, shipName[x] + "*V.png", SearchOption.AllDirectories))
			{
				Image image = new Image();
				BitmapImage source = new BitmapImage();
				source.BeginInit();
				source.UriSource = new Uri(myFile, UriKind.Relative);
				source.EndInit();
				image.Source = source;

				image.Opacity = 100;

				iconsH.Add(image);
			}

		}

		private void boatsHFill(int x)
		{
			string directory = @".\Images\" + skin + "\\horizental";
			foreach (string myFile in
					  Directory.GetFiles(directory, shipName[x] + "*.png", SearchOption.AllDirectories))
			{
				Image image = new Image();
				BitmapImage source = new BitmapImage();
				source.BeginInit();
				source.UriSource = new Uri(myFile, UriKind.Relative);
				source.EndInit();
				image.Source = source;

				image.Opacity = 99;

				hBoat[x].Add(image);
			}
		}

		private void boatsVFill(int x)
		{
			string directory2 = @".\Images\" + skin + "\vertical";
			foreach (string myFile in
					  Directory.GetFiles(directory2, shipName[x] + "*.png", SearchOption.AllDirectories))
			{
				Image image = new Image();
				BitmapImage source = new BitmapImage();
				source.BeginInit();
				source.UriSource = new Uri(myFile, UriKind.Relative);
				source.EndInit();
				image.Source = source;

				image.Opacity = 99;

				vBoat[x].Add(image);
			}
		}

    }

}