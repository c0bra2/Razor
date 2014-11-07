using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace chess
{
    class ChessSquare
    {
        private ChessPiece pieceOnSquare;
        private string squareColor = "";
        private string squareName = "";
        private PictureBox picBox;
        
        public ChessSquare(string sqName, string sqcolor, ChessPiece piece, PictureBox picb)
        {
            pieceOnSquare = piece;
            squareColor = sqcolor;
            squareName = sqName;
            picBox = picb;
            setPic("graphics\\" + formatContents());
        }
        public void setPic(string fileName) { 
           picBox.Image = Image.FromFile(fileName);
        }

        public string getSquareName(){
            return squareName;
        }

        public ChessPiece getChessPiece()
        {
            return pieceOnSquare;
        }

        public void setChessPiece(string newpiece, string newcolor)
        {
            if (newpiece != "Empty")
            {
                pieceOnSquare.setPiece(newpiece);
                pieceOnSquare.setColor(newcolor);
                setPic("graphics\\" + formatContents());
            }
        }

        public void EmptySquare()
        {
            setPic("graphics\\" + squareColor + "Empty.png");
        }

        public string formatContents()
        {
            string res = "";
            res = res + pieceOnSquare.getColor() + pieceOnSquare.getPiece();

            if (pieceOnSquare.getPiece() == "Empty")
            {
                res = res + ".png";
                return res;
            }
            else
                if (squareColor == "Black")
                {
                    res = res + "BB.png";
                    return res;
                }
                else
                {
                    res = res + "WB.png";
                    return res;
                }
        }
    }
}
