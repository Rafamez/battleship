using battleship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace battleship
{
	public class Player : mainGameWindow
	{
		//list to see which location the ai shot, the human shot, which textblock has been hit, whose turn is it
		//parameter for the turn
		private List<Boolean> AIgotShot;
		List<TextBlock> hitZone;
		private List<Boolean> gotShot;
		public bool playerTurn;
		public bool turn
		{
			get { return turn; }
			set {; }
		}

		int spot;

		//shoot method which requires x and y axis 
		public void shoot(int x, int y)
		{
			//set playtime timer to 0 + begin timer
			PlayTime = 0;
			PT.Enabled = true;
			//spot=the location you will have (61st label for example)
			spot = x * 10 + y;
			//loop to continue until time expires or the player makes a valid turn
			while (PlayTime % expireTime != 0)
			{
				//if he shoots in a location not yet used
				if (!gotShot[spot])
				{
					//make it used
					gotShot[spot] = true;

					//if he hit an object with a background (ship)
					if (hitZone[spot].Background.Opacity == 0)
					{
						//next turn
						playerTurn = !playerTurn;
						//switch user depending on who played the turn
						if (playerTurn)
							Turn.Content = "YOUR \r\nTURN";
						else
							Turn.Content = "AI \r\nTURN";
						//add an image of a cross on the location hit and put the opacity to 100
						hitZone[spot].Background = new ImageBrush(new BitmapImage(new Uri(@"Images/cross.png", UriKind.Relative)));
						hitZone[spot].Background.Opacity = 100;
						//increment end value by 1
						endValue++;
						//verify if it is the end of the game
						end();
						//disable timer
						PT.Enabled = false;
						//return
						return;
					}
					//if he did not hit anything
					else
					{
						//add the image of an X on the location he hit, put its opacity to 100
						hitZone[spot].Background = new ImageBrush(new BitmapImage(new Uri(@"Images/X.png", UriKind.Relative)));
						hitZone[spot].Background.Opacity = 100;
						//disable timer
						PT.Enabled = false;
						//return
						return;
					}
				}
				else
				{
					//if he did not use a valid space, end timer and return, since turns havent been changed yet, he can still chose another spot to hit until the timer runs out
					PT.Enabled = false;
					return;
				}
			}
		}
		public void shoot()
		{
			//create a random variable so the AI choses a random spot to it
			Random random = new Random();
			//available rows to chose from (10), chose a random one
			int x = random.Next(10);
			//available columns to chose from (10), chose a random one
			int y = random.Next(10);
			//if the difficulty is at 1 (lowest)
			if (difficulty == 1)
			{
				//until the AI hits a valid spot (not yet used)
				while (!AIgotShot[spot])
				{
					//make him shoot using the 2 random variables he got
					shoot(x, y);
					//if the the spot is not valid, look for another random value
					if (!AIgotShot[spot])
					{
						x = random.Next(10);
						y = random.Next(10);
					}

					//once he gets a valid spot, make it so the spot he hit is offically marked as used
					AIgotShot[spot] = true;
					//increase friendly value by 1
					friendlyDamage++;
					//verify if its the end of the game
					end();
				}
				//if difficulty is 2
				if (difficulty == 2)
				{

				}
				//if difficulty is 3
				if (difficulty == 3)
				{

				}
			}
		}
	}
}