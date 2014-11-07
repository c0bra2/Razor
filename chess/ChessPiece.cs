using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chess
{
    class ChessPiece
    {
        private string piece = "";
        private string color = "";

        public ChessPiece(string mypiece, string mycolor)
        {
            piece = mypiece;
            color = mycolor;
        }

        public string getPiece()
        {
            return piece;
        }

        public void setColor(string newcolor)
        {
            color = newcolor;
        }
        public void setPiece(string newpiece)
        {
            piece = newpiece;
        }
        public string getColor()
        {
            return color;
        }
    }
}
