using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace chess
{
    public partial class Form1 : Form
    {
        ChessSquare[,] board;
        int clickedSquares;
        string move;
        int halfMoves;
        int moveNumber;
        string allMoves;
        public Form1()
        {   
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            newGame(); // start a new game
        }

        private void incMoveNum()
        {
            if (halfMoves == 2)
            {
                halfMoves = 0;
                moveNumber++;
                rtfMoveOutput.AppendText("\n" + moveNumber + ". ");
            }
        }
        private void makeMove(string currentMove)
        {
            string firstSquare = currentMove[0].ToString() + currentMove[1].ToString();
            string secondSquare = currentMove[2].ToString() + currentMove[3].ToString();
            int sub1, sub2, sub3, sub4;
            sub1 = sub2 = sub3 = sub4 = 0;

            //find the location of the first and second squares that were clicked
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (board[x, y].getSquareName() == firstSquare)
                    {
                        sub1 = x;
                        sub2 = y;
                    }
                    else if (board[x, y].getSquareName() == secondSquare)
                    {
                        sub3 = x;
                        sub4 = y;
                    }
                }
            }

            //move the piece
            bool castled = false;
            // check for castling
            if (currentMove == "e1g1")
            {
                if (board[sub1, sub2].getChessPiece().getPiece() == "King")
                {
                    castled = true;
                    board[sub3, sub4].setChessPiece(board[sub1, sub2].getChessPiece().getPiece(), board[sub1, sub2].getChessPiece().getColor());
                    board[sub1, sub2].EmptySquare();
                    // move the rook to the other side of the king to complete castling
                    board[0, 5].setChessPiece(board[0, 7].getChessPiece().getPiece(), board[0, 7].getChessPiece().getColor());
                    board[0, 7].EmptySquare();
                }
            }
            else if (currentMove == "e1c1")
            {
                if (board[sub1, sub2].getChessPiece().getPiece() == "King")
                {
                    castled = true;
                    board[sub3, sub4].setChessPiece(board[sub1, sub2].getChessPiece().getPiece(), board[sub1, sub2].getChessPiece().getColor());
                    board[sub1, sub2].EmptySquare();
                    // move the rook to the other side of the king to complete castling
                    board[0, 3].setChessPiece(board[0, 0].getChessPiece().getPiece(), board[0, 0].getChessPiece().getColor());
                    board[0, 0].EmptySquare();
                }
            }
            else if (currentMove == "e8g8")
            {
                if (board[sub1, sub2].getChessPiece().getPiece() == "King")
                {
                    castled = true;
                    board[sub3, sub4].setChessPiece(board[sub1, sub2].getChessPiece().getPiece(), board[sub1, sub2].getChessPiece().getColor());
                    board[sub1, sub2].EmptySquare();
                    // move the rook to the other side of the king to complete castling
                    board[7, 5].setChessPiece(board[0, 7].getChessPiece().getPiece(), board[7, 7].getChessPiece().getColor());
                    board[7, 7].EmptySquare();
                }
            }
            else if (currentMove == "e8c8")
            {
                if (board[sub1, sub2].getChessPiece().getPiece() == "King")
                {
                    castled = true;
                    board[sub3, sub4].setChessPiece(board[sub1, sub2].getChessPiece().getPiece(), board[sub1, sub2].getChessPiece().getColor());
                    board[sub1, sub2].EmptySquare();
                    // move the rook to the other side of the king to complete castling
                    board[7, 3].setChessPiece(board[7, 0].getChessPiece().getPiece(), board[7, 0].getChessPiece().getColor());
                    board[7, 0].EmptySquare();
                }
            }

            if (!castled)
            {
                board[sub3, sub4].setChessPiece(board[sub1, sub2].getChessPiece().getPiece(), board[sub1, sub2].getChessPiece().getColor());
                board[sub1, sub2].EmptySquare();
            }
        }

        private void checkMove(string currentSquare)
        {
            move = move + currentSquare;
            clickedSquares++; //how many squares were clicked

            if (clickedSquares == 2)
            {
                clickedSquares = 0;
                halfMoves++;
                incMoveNum();

                //chess move logic here
                rtfMoveOutput.AppendText(move + " ");
                makeMove(move);
                allMoves = allMoves + move + " ";
                move = "";

                //computers move
                string computerMove = "";
                Process myProcess = new Process();
                myProcess.StartInfo.FileName = "engine.exe";
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.RedirectStandardInput = true;
                myProcess.StartInfo.RedirectStandardOutput = true;
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.Start();

                StreamWriter myStreamWriter = myProcess.StandardInput;
                myStreamWriter.WriteLine(allMoves);
                
                if (Convert.ToInt32(getTickerValue()) == 1) // if we are only playing at a depth of 1
                {
                    myStreamWriter.WriteLine("go depth " + getTickerValue());

                    computerMove = getComputersMove(myProcess.StandardOutput.ReadLine()); 
                    myStreamWriter.Close();
                    myProcess.WaitForExit();
                }
                else
                {
                    myStreamWriter.WriteLine("go depth " + getTickerValue());

                    string temp = "";
                    string engineoutput = "";
                    while (true)
                    {
                        temp = myProcess.StandardOutput.ReadLine();
                        if (temp == "")
                            break;
                        engineoutput = temp;
                    }

                    rtfMoveOutput.AppendText(engineoutput);
                    //rtfMoveOutput.AppendText(" " + myProcess.StandardOutput.ReadToEnd());
                    //computerMove = getComputersMove(myProcess.StandardOutput.ReadLine()); 
                    computerMove = "e7e5";
                    myStreamWriter.Close();
                    myProcess.WaitForExit();
                }

                halfMoves++;
                rtfMoveOutput.AppendText(computerMove + " ");
                makeMove(computerMove);
                allMoves = allMoves + computerMove + " ";

            }

        }

        private string getTickerValue()
        {
            int temp;
            temp = trkStrength.Value + 1;
            return temp.ToString();
        }

        private string getComputersMove(string inputstring)
        {
            bool inToken = false;
            string [] res = inputstring.Split(' ');
            return res[res.Length - 1];
        }

        private void newGame()
        {
            allMoves = "position startpos moves ";
            board = new ChessSquare[8, 8];
            clickedSquares = 0;
            move = "";
            halfMoves = -1;
            moveNumber = 1;
            rtfMoveOutput.Text = "";
            rtfMoveOutput.AppendText("1. ");
            // first rank
            board[0, 0] = new ChessSquare("a1", "Black", (new ChessPiece("Rook", "White")), picA1);
            board[0, 1] = new ChessSquare("b1", "White", (new ChessPiece("Knight", "White")), picB1);
            board[0, 2] = new ChessSquare("c1", "Black", (new ChessPiece("Bishop", "White")), picC1);
            board[0, 3] = new ChessSquare("d1", "White", (new ChessPiece("Queen", "White")), picD1);
            board[0, 4] = new ChessSquare("e1", "Black", (new ChessPiece("King", "White")), picE1);
            board[0, 5] = new ChessSquare("f1", "White", (new ChessPiece("Bishop", "White")), picF1);
            board[0, 6] = new ChessSquare("g1", "Black", (new ChessPiece("Knight", "White")), picG1);
            board[0, 7] = new ChessSquare("h1", "White", (new ChessPiece("Rook", "White")), picH1);

            // second rank
            board[1, 0] = new ChessSquare("a2", "White", (new ChessPiece("Pawn", "White")), picA2);
            board[1, 1] = new ChessSquare("b2", "Black", (new ChessPiece("Pawn", "White")), picB2);
            board[1, 2] = new ChessSquare("c2", "White", (new ChessPiece("Pawn", "White")), picC2);
            board[1, 3] = new ChessSquare("d2", "Black", (new ChessPiece("Pawn", "White")), picD2);
            board[1, 4] = new ChessSquare("e2", "White", (new ChessPiece("Pawn", "White")), picE2);
            board[1, 5] = new ChessSquare("f2", "Black", (new ChessPiece("Pawn", "White")), picF2);
            board[1, 6] = new ChessSquare("g2", "White", (new ChessPiece("Pawn", "White")), picG2);
            board[1, 7] = new ChessSquare("h2", "Black", (new ChessPiece("Pawn", "White")), picH2);

            // third rank
            board[2, 0] = new ChessSquare("a3", "Black", (new ChessPiece("Empty", "Black")), picA3);
            board[2, 1] = new ChessSquare("b3", "White", (new ChessPiece("Empty", "White")), picB3);
            board[2, 2] = new ChessSquare("c3", "Black", (new ChessPiece("Empty", "Black")), picC3);
            board[2, 3] = new ChessSquare("d3", "White", (new ChessPiece("Empty", "White")), picD3);
            board[2, 4] = new ChessSquare("e3", "Black", (new ChessPiece("Empty", "Black")), picE3);
            board[2, 5] = new ChessSquare("f3", "White", (new ChessPiece("Empty", "White")), picF3);
            board[2, 6] = new ChessSquare("g3", "Black", (new ChessPiece("Empty", "Black")), picG3);
            board[2, 7] = new ChessSquare("h3", "White", (new ChessPiece("Empty", "White")), picH3);

            // fourth rank
            board[3, 0] = new ChessSquare("a4", "White", (new ChessPiece("Empty", "White")), picA4);
            board[3, 1] = new ChessSquare("b4", "Black", (new ChessPiece("Empty", "Black")), picB4);
            board[3, 2] = new ChessSquare("c4", "White", (new ChessPiece("Empty", "White")), picC4);
            board[3, 3] = new ChessSquare("d4", "Black", (new ChessPiece("Empty", "Black")), picD4);
            board[3, 4] = new ChessSquare("e4", "White", (new ChessPiece("Empty", "White")), picE4);
            board[3, 5] = new ChessSquare("f4", "Black", (new ChessPiece("Empty", "Black")), picF4);
            board[3, 6] = new ChessSquare("g4", "White", (new ChessPiece("Empty", "White")), picG4);
            board[3, 7] = new ChessSquare("h4", "Black", (new ChessPiece("Empty", "Black")), picH4);

            // fifth rank
            board[4, 0] = new ChessSquare("a5", "Black", (new ChessPiece("Empty", "Black")), picA5);
            board[4, 1] = new ChessSquare("b5", "White", (new ChessPiece("Empty", "White")), picB5);
            board[4, 2] = new ChessSquare("c5", "Black", (new ChessPiece("Empty", "Black")), picC5);
            board[4, 3] = new ChessSquare("d5", "White", (new ChessPiece("Empty", "White")), picD5);
            board[4, 4] = new ChessSquare("e5", "Black", (new ChessPiece("Empty", "Black")), picE5);
            board[4, 5] = new ChessSquare("f5", "White", (new ChessPiece("Empty", "White")), picF5);
            board[4, 6] = new ChessSquare("g5", "Black", (new ChessPiece("Empty", "Black")), picG5);
            board[4, 7] = new ChessSquare("h5", "White", (new ChessPiece("Empty", "White")), picH5);

            // sixth rank
            board[5, 0] = new ChessSquare("a6", "White", (new ChessPiece("Empty", "White")), picA6);
            board[5, 1] = new ChessSquare("b6", "Black", (new ChessPiece("Empty", "Black")), picB6);
            board[5, 2] = new ChessSquare("c6", "White", (new ChessPiece("Empty", "White")), picC6);
            board[5, 3] = new ChessSquare("d6", "Black", (new ChessPiece("Empty", "Black")), picD6);
            board[5, 4] = new ChessSquare("e6", "White", (new ChessPiece("Empty", "White")), picE6);
            board[5, 5] = new ChessSquare("f6", "Black", (new ChessPiece("Empty", "Black")), picF6);
            board[5, 6] = new ChessSquare("g6", "White", (new ChessPiece("Empty", "White")), picG6);
            board[5, 7] = new ChessSquare("h6", "Black", (new ChessPiece("Empty", "Black")), picH6);

            // seventh rank
            board[6, 0] = new ChessSquare("a7", "Black", (new ChessPiece("Pawn", "Black")), picA7);
            board[6, 1] = new ChessSquare("b7", "White", (new ChessPiece("Pawn", "Black")), picB7);
            board[6, 2] = new ChessSquare("c7", "Black", (new ChessPiece("Pawn", "Black")), picC7);
            board[6, 3] = new ChessSquare("d7", "White", (new ChessPiece("Pawn", "Black")), picD7);
            board[6, 4] = new ChessSquare("e7", "Black", (new ChessPiece("Pawn", "Black")), picE7);
            board[6, 5] = new ChessSquare("f7", "White", (new ChessPiece("Pawn", "Black")), picF7);
            board[6, 6] = new ChessSquare("g7", "Black", (new ChessPiece("Pawn", "Black")), picG7);
            board[6, 7] = new ChessSquare("h7", "White", (new ChessPiece("Pawn", "Black")), picH7);

            // eight rank
            board[7, 0] = new ChessSquare("a8", "White", (new ChessPiece("Rook", "Black")), picA8);
            board[7, 1] = new ChessSquare("b8", "Black", (new ChessPiece("Knight", "Black")), picB8);
            board[7, 2] = new ChessSquare("c8", "White", (new ChessPiece("Bishop", "Black")), picC8);
            board[7, 3] = new ChessSquare("d8", "Black", (new ChessPiece("Queen", "Black")), picD8);
            board[7, 4] = new ChessSquare("e8", "White", (new ChessPiece("King", "Black")), picE8);
            board[7, 5] = new ChessSquare("f8", "Black", (new ChessPiece("Bishop", "Black")), picF8);
            board[7, 6] = new ChessSquare("g8", "White", (new ChessPiece("Knight", "Black")), picG8);
            board[7, 7] = new ChessSquare("h8", "Black", (new ChessPiece("Rook", "Black")), picH8);
        }
        // rank 1
        private void picA1_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("a1");
        }

        private void picB1_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("b1");
        }

        private void picC1_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("c1");
        }

        private void picD1_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("d1");
        }

        private void picE1_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("e1");
        }

        private void picF1_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("f1");
        }

        private void picG1_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("g1");
        }

        private void picH1_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("h1");
        }

        // rank 2
        private void picA2_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("a2");
        }

        private void picB2_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("b2");
        }

        private void picC2_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("c2");
        }

        private void picD2_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("d2");
        }

        private void picE2_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("e2");
        }

        private void picF2_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("f2");
        }

        private void picG2_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("g2");
        }

        private void picH2_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("h2");
        }

        // third rank
        private void picA3_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("a3");
        }

        private void picB3_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("b3");
        }

        private void picC3_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("c3");
        }

        private void picD3_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("d3");
        }

        private void picE3_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("e3");
        }

        private void picF3_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("f3");
        }

        private void picG3_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("g3");
        }

        private void picH3_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("h3");
        }

        // rank 4
        private void picA4_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("a4");
        }

        private void picB4_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("b4");
        }

        private void picC4_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("c4");
        }

        private void picD4_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("d4");
        }

        private void picE4_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("e4");
        }

        private void picF4_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("f4");
        }

        private void picG4_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("g4");
        }

        private void picH4_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("h4");
        }

        // rank 5
        private void picA5_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("a5");
        }

        private void picB5_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("b5");
        }

        private void picC5_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("c5");
        }

        private void picD5_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("d5");
        }

        private void picE5_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("e5");
        }

        private void picF5_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("f5");
        }

        private void picG5_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("g5");
        }

        private void picH5_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("h5");
        }

        // rank 6
        private void picA6_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("a6");
        }

        private void picB6_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("b6");
        }

        private void picC6_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("c6");
        }

        private void picD6_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("d6");
        }

        private void picE6_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("e6");
        }

        private void picF6_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("f6");
        }

        private void picG6_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("g6");
        }

        private void picH6_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("h6");
        }

        // rank 7
        private void picA7_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("a7");
        }

        private void picB7_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("b7");
        }

        private void picC7_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("c7");
        }

        private void picD7_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("d7");
        }

        private void picE7_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("e7");
        }

        private void picF7_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("f7");
        }

        private void picG7_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("g7");
        }

        private void picH7_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("h7");
        }

        // rank 8
        private void picA8_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("a8");
        }

        private void picB8_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("b8");
        }

        private void picC8_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("c8");
        }

        private void picD8_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("d8");
        }

        private void picE8_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("e8");
        }

        private void picF8_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("f8");
        }

        private void picG8_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("g8");
        }

        private void picH8_MouseClick(object sender, MouseEventArgs e)
        {
            checkMove("h8");
        }

        private void cmdNew_Click(object sender, EventArgs e)
        {
            newGame();
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdUndo_Click(object sender, EventArgs e)
        {

        }
        
    }
}
  