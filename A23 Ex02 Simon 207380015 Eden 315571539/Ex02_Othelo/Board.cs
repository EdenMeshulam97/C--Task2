using System;
using System.Runtime;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ex02_Othelo
{
    public enum eCellState
    {
        Empty = ' ',
        Black = 'X',
        White = 'O'
    }
    public class Board
    {

        #region Members
        #endregion

        #region Properties

        public eCellState[,] m_Board;
        public char currentPlayer = 'X';
        public int m_BoardSize;
        
        #endregion

        #region Constructor
        public Board(int i_BoardSize)
        {
            m_BoardSize = i_BoardSize;
            m_Board = new eCellState[i_BoardSize, i_BoardSize];
        }
        #endregion
        public void Initialize()
        {
            // Initialize the board with starting pieces
            for(int row = 0; row<m_BoardSize; row++)
            {
                for(int column = 0; column < m_BoardSize; column++)
                {
                    m_Board[row, column] = eCellState.Empty;
                }
            }
            m_Board[m_BoardSize / 2 - 1, m_BoardSize / 2 -1] = eCellState.Black;
            m_Board[m_BoardSize / 2 - 1, m_BoardSize / 2] = eCellState.White;
            m_Board[m_BoardSize / 2, m_BoardSize / 2 - 1] = eCellState.White;
            m_Board[m_BoardSize / 2, m_BoardSize / 2] = eCellState.Black;
        }
        public bool ValidateMove(eCellState[,] tempBoard, int row, int col)
        {
            bool isValidMove = false;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    int x = row + i;
                    int y = col + j;
                    if (x >= 0 && x < m_BoardSize && y >= 0 && y < m_BoardSize && tempBoard[x, y] == (eCellState)opponent())
                    {
                        // There is an opponent's piece in this direction, check if we can capture it
                        while (true)
                        {
                            x += i;
                            y += j;
                            if (x < 0 || x >= m_BoardSize || y < 0 || y >= m_BoardSize || tempBoard[x, y] == eCellState.Empty)
                            {
                                break;
                            }
                            if (tempBoard[x, y] == (eCellState)currentPlayer)
                            {
                                isValidMove = true;
                                break;
                            }
                        }

                        // Flip the captured pieces (if any)
                        if (isValidMove)
                        {
                            x = row + i;
                            y = col + j;
                            while (x >= 0 && x < m_BoardSize && y >= 0 && y < m_BoardSize && tempBoard[x, y] != (eCellState)currentPlayer )
                            {
                                tempBoard[x, y] = (eCellState)currentPlayer;
                                x += i;
                                y += j;
                            }
                        }
                    }
                }
            }

            return isValidMove;
        }
        public void SwitchPlayer()
        {
            currentPlayer = opponent();
        }
        public void Display()
        {
            Console.Write("   ");
            for(int index = 0; index < m_BoardSize; index++)
            {
                Console.Write($" {(char)(65 + index)}  ");
            }
            Console.WriteLine();
            printSeperator();
            for(int row = 0; row < m_BoardSize; row++)
            {
                Console.Write($"{row + 1} |");
                for(int column = 0; column < m_BoardSize; column++)
                {
                    Console.Write($" {(char)m_Board[row,column]} |");
                }
                Console.WriteLine();
                printSeperator();
            }

        }
        public bool TryPlacePiece(int row, int col)
        {
            // Check if the specified position is on the board
            if (row < 0 || row >= 8 || col < 0 || col >= 8)
            {
                Console.WriteLine("Invalid move.");
                return false;
            }

            // Check if the specified position is empty
            if (m_Board[row, col] != eCellState.Empty)
            {
                Console.WriteLine("Invalid move.");
                return false;
            }

            // Place the piece and flip any captured pieces
            if (FlipPieces(row, col))
            {
                m_Board[row, col] = (eCellState)currentPlayer;
                return true;
            }
            else
            {
                Console.WriteLine("Invalid move.");
                return false;
            }
        }    
        public bool FlipPieces(int row, int col)
        {

            bool isValidMove = false;
            bool canFlip = false;

            for (int leftRighti = -1; leftRighti <= 1; leftRighti++)
            {
                for (int upDown = -1; upDown <= 1; upDown++)
                {
                    if (leftRighti == 0 && upDown == 0) continue;

                    int x = row + leftRighti;
                    int y = col + upDown;
                    if (x >= 0 && x < m_BoardSize && y >= 0 && y < m_BoardSize && m_Board[x, y] == (eCellState)opponent())
                    {
                        // There is an opponent's piece in this direction, check if we can capture it
                        while (true)
                        {
                            x += leftRighti;
                            y += upDown;
                            if (x < 0 || x >= m_BoardSize || y < 0 || y >= m_BoardSize || m_Board[x, y] == eCellState.Empty)
                            {
                                canFlip = false;
                                break;
                            }
                            if (m_Board[x, y] == (eCellState)currentPlayer)
                            {
                                isValidMove = true;
                                canFlip = true;
                                break;
                            }
                        }

                        // Flip the captured pieces
                        if (isValidMove && canFlip)
                        {
                            x = row + leftRighti;
                            y = col + upDown;
                            while (x >= 0 && x < m_BoardSize && y >= 0 && y < m_BoardSize && m_Board[x, y] != (eCellState)currentPlayer)
                            {
                                m_Board[x, y] = (eCellState)currentPlayer;
                                x += leftRighti;
                                y += upDown;
                            }
                        }
                    }
                }
            }

            return isValidMove;
        }
        public bool IsGameOver()
        {
            bool result = true;
            if (GetPossibleMoves().Count > 0) 
            { 
                result = false; 
            }
            currentPlayer = opponent();
            if (GetPossibleMoves().Count > 0)
            {
                result = false;
            }
            currentPlayer = opponent();
            return result;
        }
        public List<(int row, int col)> GetPossibleMoves()
        {
            var possibleMoves = new List<(int row, int col)>();

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if (/*m_Board[row, col] == eCellState.Empty && */ValidateMove((eCellState[,])m_Board.Clone(), row, col))
                    {
                        possibleMoves.Add((row, col));
                    }
                }
            }

            return possibleMoves;
        }
        public char LeadPlayer()
        {
            int Score = 0;
            foreach(eCellState cell in m_Board)
            {
                if(cell is eCellState.Black)
                {
                    Score++;
                }
                if(cell is eCellState.White)
                {
                    Score--;
                }
            }
            return Score == 0 ? ' ' : Score > 0 ? 'X' : 'O';
        }
       
        public Board Clone()
        {
            return new Board(m_BoardSize) { m_Board = (eCellState[,])m_Board.Clone(), currentPlayer = currentPlayer  };
        }
        #region Private Methods
        private char opponent()
        {
            return currentPlayer == 'X' ? 'O' : 'X';
        }
        private void printSeperator()
        {
            Console.WriteLine("  " + new string('=', 4 * m_BoardSize + 1));
        }
        #endregion
    }
}

