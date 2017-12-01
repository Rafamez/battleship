using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace battleship
{
    //class for the ships

    public class Ship : mainGameWindow, ISerializable

    {
        //list for the length of each ship
        private int[] shipLength = new int[] { 2, 3, 3, 4, 5 };
        //list for each ship
        private String[] shipName = new String[] { "Patrol Boat (2)", "Submarine (3)", "Destroyer (3)", "Battleship (4)", "Aircraft Carrier (5)" };
        //lists for images
        public List<Image> boatImages;
        public List<Image> hBoat;
        public List<Image> vBoat;
        //the boat clicked
        int boat;
        Boolean boatClicked;
        int boatChosen;
        Boolean horizental = true;


        //used to deserialize data, not ready (keep location + alive)
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
        public void choseShip(object sender, MouseButtonEventArgs e)
        {
            Image newimage = new Image();
            newimage.Source = ((Image)sender).Source;
            if (newimage.Source.ToString() == "Images/USA/patrolBoat.png")
                boatChosen = 1;
            if (newimage.Source.ToString() == "Images/USA/submarine.png")
                boatChosen = 2;
            if (newimage.Source.ToString() == "Images/USA/destroyer.png")
                boatChosen = 3;
            if (newimage.Source.ToString() == "Images/USA/aircraftCarrier.png")
                boatChosen = 4;

            boatChosen = -1;
        }


        public void setShip(object sender, MouseButtonEventArgs e)
        {
            if (boatClicked)
            {
                Image newimage = new Image();
                newimage.Source = ((Image)sender).Source;
                try
                {
                    if (boatChosen == 1)
                    {
                        if (horizental)
                        {
                            newimage.Source = Convert;
                        }
                        else
                        {

                        }

                    }
                    if (boatChosen == 2)
                    {
                        if (horizental)
                        {
                            newimage.Source = Convert;
                        }
                        else
                        {

                        }

                    }
                    if (boatChosen == 3)
                    {
                        if (horizental)
                        {
                            newimage.Source = Convert;
                        }
                        else
                        {

                        }

                    }
                    if (boatChosen == 4)
                    {
                        if (horizental)
                        {
                            newimage.Source = Convert;
                        }
                        else
                        {

                        }

                    }
                }
                catch (IndexOutOfRangeException)
                {
                    return;
                }
            }
            return;

        }


        //method used to adding images in a list which will be used to put the ships on the grid
        public void boatFace()
        {
            if (!horizental)
            {
                verticalFace();
                hBoat = null;
            }
            else
            {
                HorizentalFace();
                vBoat = null;
            }
        }

        private void HorizentalFace()
        {
            string directory = @".\Images\" + skin + "\\horizental";
            foreach (string myFile in
                      Directory.GetFiles(directory, "*.png", SearchOption.AllDirectories))
            {
                Image image = new Image();
                BitmapImage source = new BitmapImage();
                source.BeginInit();
                source.UriSource = new Uri(myFile, UriKind.Relative);
                source.EndInit();
                image.Source = source;

                hBoat.Add(image);
            }
        }

        private void verticalFace()
        {
            string directory = @".\Images\" + skin + "\vertical";
            foreach (string myFile in
                      Directory.GetFiles(directory, "*.png", SearchOption.AllDirectories))
            {
                Image image = new Image();
                BitmapImage source = new BitmapImage();
                source.BeginInit();
                source.UriSource = new Uri(myFile, UriKind.Relative);
                source.EndInit();
                image.Source = source;

                vBoat.Add(image);
            }
        }



    }

}