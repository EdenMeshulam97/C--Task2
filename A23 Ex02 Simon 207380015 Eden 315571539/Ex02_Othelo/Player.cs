using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02_Othelo
{
    public class Player
    {
        public char piece;
        public string name;

        public Player(char i_Piece)
        {
            this.piece = i_Piece;
        }

        public virtual void MakeMove(Board board)
        {
            if(board.GetPossibleMoves().Count > 0)
            {
                Console.WriteLine($"{name}'s turn. Enter row and column (e.g. E3, Q - Quit):");
                string input = Console.ReadLine();
                if(input.ToLower() == "q")
                {
                    Environment.Exit(0);
                }
                if (!tryParsePlayerMoveInput(input, out int row, out int column))
                {
                    Console.WriteLine("Input is not valid!");
                    MakeMove(board);
                }
                else if (!board.TryPlacePiece(row, column))
                {
                    MakeMove(board);
                }
            }
            else
            {
                Console.WriteLine($"{name} has no possible moves! press any key to continue.");
                Console.ReadKey();
            }

        }
        private bool tryParsePlayerMoveInput(string i_Input, out int o_Row, out int o_Column)
        {
            o_Row = 0;
            o_Column = 0;
            bool result = false;
            i_Input = i_Input.ToLower();
            if(!string.IsNullOrEmpty(i_Input) && i_Input.All(x => char.IsLetterOrDigit(x) && i_Input.Length == 2))
            {
                if(int.TryParse(i_Input.Where(x => char.IsDigit(x)).First().ToString(), out o_Row))
                {
                    o_Row--;
                    int letterAscii = (int)(i_Input.Where(x => char.IsLetter(x)).First());
                    if(letterAscii >= 'a' && letterAscii <= 'z')
                    {
                        o_Column = letterAscii - 'a';
                        result = true;
                    }
                }
            }
            return result;
        }
    }
}

