using battleship;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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

//INSPIRED FROM JAMESJRG CODE FROM https://github.com/jamesjrg/battleship/tree/master/Battleship
namespace battleship
{
	public class Player
	{
		//INT FOR THE SIZE OF THE GRID
		public const int GRID_SIZE = 10;

		//USERNAME FOR PLAYER
		public String username;

		//HUMAN BOARD
		public List<List<Board>> MyGrid { get; set; }

		Grid grid;
		String skin;

        public String setString {
            get
            {
                return this.skin;
            }
            set
            { 
                this.skin = value; ; }
            }

        //HUMAN SHIPS
        public List<Ship> myShips = new List<Ship>();


		//VALUE TO GET THE LIST USED
		public int ship = 0;
		//value to chose which image to put on grid
		private int imagePos = 1;

		public String gettingShot;


		//CREATE PLAYER CLASS
		public Player(String name, Grid grid, String skin)
		{

			//SET USERNAME
			username = name;
			this.grid = grid;
			this.skin = skin;
			//SET FRIENDLY GRID
			MyGrid = new List<List<Board>>();
			//LOOP TO ADD BOARDS TO THE GRIDS
			for (int i = 0; i != GRID_SIZE; ++i)
			{
				MyGrid.Add(new List<Board>());

				for (int j = 0; j != GRID_SIZE; ++j)
				{
					MyGrid[i].Add(new Board(i, j));
				}
			}
			//ADD SHIPS TO THE BOARD'S TYPE
			foreach (ShipType type in Enum.GetValues(typeof(ShipType)))
			{
				myShips.Add(new Ship(type));
			}
			//ADD SHIPS TO THE BOARDS WHO HAVE SHIP PROERTIES
			Reset();
		}


		//METHOD USED TO CHANGE THE TYPE VALUE OF THE BOARDS WHO HAVE SHIPS ON THEM + SET THE SHIPS
		public void Reset()
		{
			//SET EVERY BOARD ALL BOARD TO WATER AND ENNEMY BOARD TO UNKNOWN
			for (int i = 0; i != GRID_SIZE; ++i)
			{
				for (int j = 0; j != GRID_SIZE; ++j)
				{
					MyGrid[i][j].Reset(SquareType.Water);
				}
			}
			//CREATE AND SET MY SHIPS AND ENNEMY SHIPS
			myShips.ForEach(s => s.Reincarnate());
		}
		//METHOD RETUNRS BOOL TO SEE IF SQUARE IS FREE
		private bool SquareFree(int row, int col)
		{
			try
			{
				return (MyGrid[row][col].ShipIndex == -1) ? true : false;
			}
			catch (Exception e)
			{
				return false;
			}

		}




		//CREATING FUN TO SEE IF ITS POSSIBLE TO PLACE SHIP
		private bool PlacementPossibleV(int shipIndex, int remainingLength, int x, int y)
		{
			{
				//X AND Y TO PLACE
				int startPosRow = x;
				int startPosCol = y;
				//VALUE FOR REMANING LENGTH OF SHIP
				int tmp = remainingLength;
				//LOOP TO PLACE SHIP, BASED ON REMAINING OF GRID
				for (int row = startPosRow; tmp != 0; ++row)
				{
					//IF SQUARE IS NOT FREE, RETURN FALSE
					if (!SquareFree(row, startPosCol))
						return false;
					//SQUARE WAS FREE, -1 TO TEMP
					--tmp;
				}

				//RETURN TRUE IF PLACEMENT IS DONE CORRECTLY
				return true;
			}
		}


		//METHOD TO PLACE SHIP DOWN, RETURNS BOOLEAN TRUE IF PLACED CORRECTLY
		private bool PlaceVertical(int shipIndex, int remainingLength, int x, int y)
		{
			//reset imagePos
			imagePos = 1;
			//X AND Y POSITION TO PLACE
			int startPosRow = x;
			int startPosCol = y;



			//IF IT IS POSSIBLE
			if (PlacementPossibleV(shipIndex, remainingLength, x, y))
			{
				//LOOP TO SEE IF SQUARE IS OCCUPIED
				for (int row = startPosRow; remainingLength != 0; ++row)
				{
					//SET THE BOARD TO UNDAMAGED
					MyGrid[row][startPosCol].Type = SquareType.Undamaged;
					//GIVE IT A SHIP INDEX
					MyGrid[row][startPosCol].ShipIndex = shipIndex;
					Image image = new Image();
					switch (ship)
					{
						case 0:
							image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/vertical/battleship" + imagePos.ToString() + ".png");
							image.Stretch = Stretch.Fill;
							Grid.SetRow(image, row);
							Grid.SetColumn(image, startPosCol);
							grid.Children.Add(image);
							break;
						case 1:
							image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/vertical/cruiser" + imagePos.ToString() + ".png");
							image.Stretch = Stretch.Fill;
							Grid.SetRow(image, row);
							Grid.SetColumn(image, startPosCol);
							grid.Children.Add(image);
							break;
						case 2:
							image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/vertical/destroyer" + imagePos.ToString() + ".png");
							image.Stretch = Stretch.Fill;
							Grid.SetRow(image, row);
							Grid.SetColumn(image, startPosCol);
							grid.Children.Add(image);
							break;
						case 3:
							image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/vertical/submarine" + imagePos.ToString() + ".png");
							image.Stretch = Stretch.Fill;
							Grid.SetRow(image, row);
							Grid.SetColumn(image, startPosCol);
							grid.Children.Add(image);
							break;
						case 4:
							image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/vertical/carrier" + imagePos.ToString() + ".png");
							image.Stretch = Stretch.Fill;
							Grid.SetRow(image, row);
							Grid.SetColumn(image, startPosCol);
							grid.Children.Add(image);
							break;
					}
					//REMAINING LENGTH -1
					--remainingLength;
					//imagePos +1
					++imagePos;

				}
				//RETURN TRUE IF PLACEMENT WAS POSSIBLE
				return true;
			}
			//RETURN FALSE IF IT WASNT
			return false;
		}


		private bool PlacementPossibleH(int shipIndex, int remainingLength, int x, int y)
		{
			{
				//X AND Y TO PLACE
				int startPosRow = x;
				int startPosCol = y;
				//VALUE FOR REMANING LENGTH OF SHIP
				int tmp = remainingLength;
				//LOOP TO PLACE SHIP, BASED ON REMANING LENGTH OF THE GRID
				for (int col = startPosCol; tmp != 0; ++col)
				{
					//IF THE SQUARE IS NOT FREE
					if (!SquareFree(startPosRow, col))
						//RETURN FALSE
						return false;
					// IF IT IS FREE, TAKE OFF 1 TO REMAINING LENGTH
					--tmp;
				}
				//RETURN TRUE IF PLACEMENT IS POSSIBLE
				return true;
			}
		}



		//METHOD TO PLACE BOAT HORIZONTALLY
		private bool PlaceHorizontal(int shipIndex, int remainingLength, int x, int y)
		{
			//reset imagePos
			imagePos = 1;
			//X AND Y TO PLACE
			int startPosRow = x;
			int startPosCol = y;
			//IF PLACEMENT IS POSSIBLE
			if (PlacementPossibleH(shipIndex, remainingLength, x, y))

			{
				//LOOP TO CHANGE VALUE OF BOARD
				for (int col = startPosCol; remainingLength != 0; ++col)
				{
					//CHANGE VALUE OF BOARD TO UNDAMAGED
					MyGrid[startPosRow][col].Type = SquareType.Undamaged;
					//GIVE IT A SHIP INDEX
					MyGrid[startPosRow][col].ShipIndex = shipIndex;
					Image image = new Image();
					switch (ship)
					{
						case 0:
							image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/horizental/battleship" + imagePos.ToString() + ".png");
							image.Stretch = Stretch.Fill;
							Grid.SetColumn(image, col);
							Grid.SetRow(image, startPosRow);
							grid.Children.Add(image);
							break;
						case 1:
							image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/horizental/cruiser" + imagePos.ToString() + ".png");
							image.Stretch = Stretch.Fill;
							Grid.SetColumn(image, col);
							Grid.SetRow(image, startPosRow);
							grid.Children.Add(image);
							break;
						case 2:
							image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/horizental/destroyer" + imagePos.ToString() + ".png");
							image.Stretch = Stretch.Fill;
							Grid.SetColumn(image, col);
							Grid.SetRow(image, startPosRow);
							grid.Children.Add(image);
							break;
						case 3:
							image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/horizental/submarine" + imagePos.ToString() + ".png");
							image.Stretch = Stretch.Fill;
							Grid.SetColumn(image, col);
							Grid.SetRow(image, startPosRow);
							grid.Children.Add(image);
							break;
						case 4:
							image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/" + skin + "/horizental/carrier" + imagePos.ToString() + ".png");
							image.Stretch = Stretch.Fill;
							Grid.SetColumn(image, col);
							Grid.SetRow(image, startPosRow);
							grid.Children.Add(image);
							break;
					}
					// -1 TO LENGTH
					--remainingLength;
					//+1 to imagePos
					++imagePos;
				}

				//RETURN TRUE IF THIS WAS ABLE TO BE DONE
				return true;
			}
			//RETURN FALSE IF IT WASNT
			return false;
		}

		//METHOD USED SINK THE SHIP
		private void SinkShip(int i, List<List<Board>> grid)
		{
			//NESTED LOOP TO GET EVERY SQUARE IN THE ROW IF SHIP IS HORIZENTAL
			foreach (var row in grid)
			{
				foreach (var square in row)
				{
					if (square.ShipIndex == i)
						square.Type = SquareType.Sunk;
				}
			}
			//NESTED LOOP TO GET EVERY SQUARE IN THE COLUMN IF SHIP IS HORIZENTAL
			foreach (var col in grid)
			{
				foreach (var square in col)
				{
					if (square.ShipIndex == i)
						square.Type = SquareType.Sunk;
				}
			}
		}

		//METHOD USED TO SINK ALLY SHIP
		public void MineSunk(int i)
		{
			SinkShip(i, MyGrid);
		}

		//METHOD USED TO FIRE
		public String Fire(int row, int col, AI otherPlayer)
		{
			//VALUE FOR LOCATION DAMAGED
			int damagedIndex;
			//BOOLEAN FOR IF LOCATION HAS A SUNKEN SHIP
			bool isSunk;
			//SQUARETYPE TO VERIFY THE TYPE OF THE HITTEN LOCATION, AND IF IT IS SUNK
			SquareType newType = otherPlayer.FiredAt(row, col, out damagedIndex, out isSunk);
			//CHANGE THE SHIPINDEX TO DAMAGE INDEX
			otherPlayer.MyGrid[row][col].ShipIndex = damagedIndex;

			//IF LOCATION ISSUNK IS TRUE (HITTING A SHIP)
			if (damagedIndex>-1)
				return username + " hit " + otherPlayer.myShips[damagedIndex].type.ToString() + " on location (" + row.ToString() + "," + col.ToString() + ")";
			if (isSunk)
				otherPlayer.MineSunk(damagedIndex);
			else
			{
				//IF ISSUNK IS FALSE (NOT HITTING A SHIP)
				//CHANGE THE TYPE OF ENENMYGRID AT LOCATION TO MISS
				otherPlayer.MyGrid[row][col].Type = newType;
				return username + " missed his shot on locations (" + row.ToString() + "," + col.ToString() + ")";
			}
			return null;
		}

		//IF YOURE GETTING FIRED AT
		public int[] FiredAt(int row, int col, out int damagedIndex, out bool isSunk)
		{
			//VALUE TO SEE IF LOCATION GOT SUNK
			isSunk = false;
			//DAMAGE INDEX
			damagedIndex = -1;
			int[] location = new int[2];
			Image image = new Image();

			//SWITCH TO SEE THE TYPE OF THE LOCATION HIT
			switch (MyGrid[row][col].Type)
			{
				//IF ITS WATER, RETURN WATER
				case SquareType.Water:
					location[0] = -1;
					location[1] = -1;
					MyGrid[row][col].Type=SquareType.Miss;
					image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/X.png");
					image.Stretch = Stretch.Fill;
					Grid.SetRow(image, row);
					Grid.SetColumn(image, col);
					grid.Children.Add(image);
					return location;
				//IF ITS AN UNDAMAGED SHIP
				case SquareType.Undamaged:
					//VALUE TO GET TYPE OF VALUE AT [ROW][SQUARE] OF THE GRID
					var square = MyGrid[row][col];
					//SHIPINDEX = THE SHIPINDEX OF THAT SQUARE
					damagedIndex = square.ShipIndex;
					//IF SHIPINDEX IS <-1 (GOT HIT), CHANGE SQUARE TO SUNK
					if (myShips[damagedIndex].FiredAt())
					{
						//IS SUNK IS TRUE, AND MINESUNK IS THE SHIP INDEX OF THE SQUARE
						MineSunk(square.ShipIndex);
						//CHANGE SUNK TO TRUE
						isSunk = true;
						image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/cross.png");
						image.Stretch = Stretch.Fill;
						Grid.SetRow(image, row);
						Grid.SetColumn(image, col);
						grid.Children.Add(image);
						if (myShips[damagedIndex].healthReturn == 0)
						{
							MessageBox.Show(myShips[damagedIndex].ToString() + " has been sunk");
							location[0] = -4;
							location[1] = -4;
							return location;
						}
					}
					else
					{
						//SET THE TYPE OF THE SQUARE TO DAMAGED
						square.Type = SquareType.Miss;
						image.Source = (ImageSource)new ImageSourceConverter().ConvertFrom("../../Images/X.png");
						image.Stretch = Stretch.Fill;
						Grid.SetRow(image, row);
						Grid.SetColumn(image, col);
						grid.Children.Add(image);
					}
					location[0] = row;
					location[1] = col;
					return location;
				//IF ITS DAMAGED, RETURN ERROR
				case SquareType.Miss:
					goto default;
				//IF ITS UNKNOWN RETURN ERROR
				//IF ITS SUNK RETURN ERROR
				case SquareType.Sunk:
					goto default;
				default:
					location[0] = -2;
					location[1] = -2;
					return location;
			}
		}


		//Method to verifiy is you have lost
		public bool Lost()
		{
			//if all your ships are sunk, end game
			return myShips.All(ship => ship.IsSunk);
		}

		public Boolean findShip(Ship arg, int x, int y)
		{
			return true;
		}
		//METHOD USED TO PLACE SHIPS
		public void PlaceShips(int x, int y, List<bool> horizental)
		{
			if (ship < 5)
			{
				//CALL METHOD HERE WHICH ASKS USER FOR POSITION
				//TAKE OFF X AND Y FROM PLACESHIPS AND CALLING METHOD FROM MAINGAMEWINDOW TO GET LOCATION OF IMAGE CLICK
				//BOOLEAN TO GET THE VALUE HORIZENTAL (IF ITS HORIZENTAL OR VERTICAL)
				bool vertical = !horizental[ship];
				//BOOLEAN TO SEE IF SHIP WAS PLACED
				bool placed = false;
				//LOOP TO PLACE SHIPS
				{
					//INT FOR LENGTH OF SHIP
					int remainingLength = myShips[ship].Length;
					//IF VERTICAL IS TRUE (PLACE SHIP VERTICALLY)
					//INSERT IMAGES HERE
					if (vertical)
					{
						if (PlacementPossibleV(ship, remainingLength, x, y))
						{
							//PLACE THE SHIP VERTICALLY. BOOLEAN TO SEE IF IT SUCCEEDED OR NOT
							placed = PlaceVertical(ship, remainingLength, x, y);
							ship++;
						}
					}
					else
					{
						if (PlacementPossibleH(ship, remainingLength, x, y))
						{
							//PLACE THE SHIP VERTICALLY, BOOLEAN TO SEE IF IT SUCCEEDED
							placed = PlaceHorizontal(ship, remainingLength, x, y);
							ship++;
						}
					}
				}
			}
		}

		//Use when the player clicks reset to reset his sea
		public void RemoveAll() {
			for (int i = 0; i != GRID_SIZE; ++i)
			{
				for (int j = 0; j != GRID_SIZE; ++j)
				{
					MyGrid[i][j].Reset(SquareType.Water);
				}
			}
		}
	}
}