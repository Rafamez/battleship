using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

//INSPIRED FROM JAMESJRG CODE FROM https://github.com/jamesjrg/battleship/tree/master/Battleship
namespace battleship
{
    //ENUMS REPRESENTING THE VALUE THE SQUARE CAN BE (CAN BE UNKNOWN, WATER, UNDAMAGED, DAMAGED OR SUNK)
    public enum SquareType {Unknown, Water, Undamaged, Miss, Sunk }
    [Serializable]
    public class Board : DependencyObject
    {
        //GET THE ROW OF THE BOARD
        public int Row { get; private set; }
        //GET THE COL OF THE BOARD
        public int Col { get; private set; }
        //INDEX TO SEE IF SHIP ON IT OR NOT (-1 MEANS YES)
        public int ShipIndex { get; set; }

        //PARAMETER WHICH RETURNS A TYPE BASED ON THE TYPE OF BOARD(LOCATION ON GRID)
        public SquareType Type
        {
            //RETURNS TYPEPROERTY (TYPE OF A BOARD)
            get { return (SquareType)GetValue(TypeProperty); }
            //SET THE TYPE OF THE BOARD
            set { SetValue(TypeProperty, value); }
        }
        //READ ONLY VALUE WHICH SETS THE PROPERTY OF A BOARD
        public static readonly DependencyProperty TypeProperty =
        DependencyProperty.Register("Type", typeof(SquareType), typeof(Board), null);


        //CREATE A BOARD USING ROWS AND COLS
        public Board(int row, int col)
        {
            Row = row;
            Col = col;
        }

        //RESET A BOARD, SET SHIPINDEX TO -1, SET THE TYPE TO WHATEVER USERS SAYS
        public void Reset(SquareType type)
        {
            Type = type;
            ShipIndex = -1;
        }
    }
}
