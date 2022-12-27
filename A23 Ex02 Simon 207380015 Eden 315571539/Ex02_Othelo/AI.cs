using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ex02_Othelo
{
    public class AI : Player
    {
        public AI(char i_Piece) : base(i_Piece)
        {
            base.name = "AI";
        }

        public override void MakeMove(Board board)
        {
            Console.WriteLine("AI's turn");
            Thread.Sleep(2000);
            List<(int row, int col)> possibleMoves = board.GetPossibleMoves();
            if(possibleMoves.Count > 0)
            {
                int maxScore = int.MinValue;
                int bestRow = -1;
                int bestCol = -1;

                foreach (var move in possibleMoves)
                {
                    Board tempBoard = board.Clone();
                    tempBoard.m_Board[move.row, move.col] = (eCellState)piece;
                    tempBoard.FlipPieces(move.row, move.col);
                    int score = Minimax(tempBoard, 3, false);
                    if (score > maxScore)
                    {
                        maxScore = score;
                        bestRow = move.row;
                        bestCol = move.col;
                    }
                }
                Console.WriteLine($"AI chose {(char)(bestCol + 'A')}{bestRow + 1}");
                Thread.Sleep(3000);
                board.TryPlacePiece(bestRow, bestCol);
            }
            else
            {
                Console.WriteLine($"{name} has no possible moves! press any key to continue.");
                Console.ReadKey();
            }
            
        }
        private int Minimax(Board tempBoard, int depth, bool maximizingPlayer)
        {
            if (depth == 0 || tempBoard.IsGameOver())
            {
                return EvaluateBoard(tempBoard.m_Board);
            }

            if (maximizingPlayer)
            {
                int maxScore = int.MinValue;
                var possibleMoves = tempBoard.GetPossibleMoves();
                foreach (var move in possibleMoves)
                {
                    Board newBoard = tempBoard.Clone();
                    newBoard.m_Board[move.row, move.col] = (eCellState)piece;
                    newBoard.FlipPieces(move.row, move.col);
                    int score = Minimax(newBoard, depth - 1, false);
                    maxScore = Math.Max(maxScore, score);
                }
                return maxScore;
            }
            else
            {
                int minScore = int.MaxValue;
                var possibleMoves = tempBoard.GetPossibleMoves();
                foreach (var move in possibleMoves)
                {
                    Board newBoard = tempBoard.Clone();
                    newBoard.m_Board[move.row, move.col] = (eCellState)opponent();
                    newBoard.FlipPieces(move.row, move.col);
                    int score = Minimax(newBoard, depth - 1, true);
                    minScore = Math.Min(minScore, score);
                }
                return minScore;
            }
        }
        private int EvaluateBoard(eCellState[,] tempBoard)
        {
            int score = 0;
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if (tempBoard[row, col] == (eCellState)piece)
                    {
                        score++;
                    }
                    else if (tempBoard[row, col] == (eCellState)opponent())
                    {
                        score--;
                    }
                }
            }
            return score;
        }
        private char opponent()
        {
            return piece == 'X' ? 'O' : 'X';
        }
    }
}
