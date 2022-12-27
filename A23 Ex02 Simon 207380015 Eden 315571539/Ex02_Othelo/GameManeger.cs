using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ex02.ConsoleUtils;

namespace Ex02_Othelo
{
    public class GameManeger
    {
        #region Members
        private Board m_Board;
        private Player Player1;
        private Player Player2;
        #endregion

        #region Constructor
        public GameManeger()
        {
            Player1 = new Player('X');
            Player1.name = getPlayerName();
            if(getAiOrTwoPlayers())
            {
                Player2 = new AI('O');
            } 
            else
            {
                Player2 = new Player('O');
                Player2.name = getPlayerName();
            }
            setBoardSize();
            clearScreen();
        }
        #endregion

        #region Public Methods

        public void StartGmae()
        {
            displayBoard();
            while (!m_Board.IsGameOver())
            {
                clearScreen();
                displayBoard();
                Player currentPlayer = m_Board.currentPlayer == 'X' ? Player1 : Player2;
                currentPlayer.MakeMove(m_Board);
                m_Board.SwitchPlayer();
            }
            char winner = m_Board.LeadPlayer();
            if(winner == ' ')
            {
                Console.WriteLine($"The Game is Done! its a darw!");
            }
            else
            {
                Console.WriteLine($"The Game is Done! the winner is {(winner == Player1.piece ? Player1.name : Player2.name)}");
            }
            Console.WriteLine("Would you like a rematch ? (Y/N)");
            string input = Console.ReadLine();
            while(input.ToLower() != "y" && input.ToLower() != "n")
            {
                Console.WriteLine("Input is not valid, Would you like a rematch ? (Y/N)");
                input = Console.ReadLine();
            }
            if(input.ToLower() == "y")
            {
                m_Board.Initialize();
                StartGmae();
            }
            else
            {
                Environment.Exit(0);
            }
        }
        #endregion

        #region Private Methods
        private void displayBoard()
        {
            Console.WriteLine($"{Player1.name} - X | {Player2.name} - O");
            m_Board.Display();
        }
        private void setBoardSize()
        {
            Console.WriteLine("What board size Would you like to play ? (6x6 - 1 / 8x8 - 2)");
            string input = Console.ReadLine();
            while (string.IsNullOrEmpty(input) || !input.All(x => char.IsDigit(x) && (x == '1' || x == '2')))
            {
                Console.WriteLine("Input is not valid, What board size Would you like to play ? (6x6 - 1 / 8x8 - 2)");
                input = Console.ReadLine();
            }
            if (input == "2")
            {
                m_Board = new Board(8);
            }
            else
            {
                m_Board = new Board(6);

            }
            m_Board.Initialize();
        }
        private string getPlayerName()
        {
            string input;
            Console.WriteLine("Please enter player name :");
            input = Console.ReadLine();
            while (string.IsNullOrEmpty(input) || !input.All(x => char.IsLetter(x)))
            {
                Console.WriteLine("Name is not valid, Please enter first player name :");
                input = Console.ReadLine();
            }
            return input;
        }
        private bool getAiOrTwoPlayers()
        {
            bool result = true;
            Console.WriteLine("Would you like to play against AI or another player ? (AI - 1 / Player - 2)");
            string input = Console.ReadLine();
            while (string.IsNullOrEmpty(input) || !input.All(x => char.IsDigit(x) && (x == '1' || x == '2')))
            {
                Console.WriteLine("Input is not valid, Would you like to play against AI or another player ? (AI - 1 / Player - 2)");
                input = Console.ReadLine();
            }
            if(input == "2")
            {
                result = false;
            }
            return result;
        }

        private void clearScreen()
        {
            Screen.Clear();
        }
        #endregion


    }
}
