using Wordle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace Wordle
{
    internal class DisplayView
    {
        public const int BoardLength = 25;



        public static void DisplayBoard(BoardModel boardModel)
        {
            Console.SetCursorPosition((Console.WindowWidth - BoardLength) / 2, Console.CursorTop);
            PrintTop(boardModel);
            for(int i = 0; i < boardModel.board.GetLength(0); i++)
            {
                Console.SetCursorPosition((Console.WindowWidth - BoardLength) / 2, Console.CursorTop);
                for(int j = 0; j < boardModel.board.GetLength(1); j++)
                {
                    if(boardModel.board[i, j] == '\0')
                    {
                        Console.Write("║ " + "  ");
                    }
                    else
                    {
                        PrintLetter(boardModel, i, j);
                    }
                }
                Console.ResetColor();
                Console.WriteLine("║");

                if(i < boardModel.board.GetLength(0) - 1)
                {
                    Console.SetCursorPosition((Console.WindowWidth - BoardLength) / 2, Console.CursorTop);
                    PrintLine(boardModel);
                }
                else
                {
                    Console.SetCursorPosition((Console.WindowWidth - BoardLength) / 2, Console.CursorTop);
                    PrintBottom(boardModel);
                }

            }

        }
        public static void PrintLine(BoardModel boardModel)
        {
            string line = "╠═" + "══"; ;
            for(int i = 1; i < boardModel.board.GetLength(0)-1; i++)
            {
                line += "╬═" + "══";
            }
            Console.WriteLine(line + "╣");
        }
        public static void PrintTop(BoardModel boardModel)
        {
            string line = "╔═" + "══";
            for(int i = 1; i < boardModel.board.GetLength(0) - 1; i++)
            {
                line += "╦═" + "══";
            }
            Console.WriteLine(line + "╗");
        }
        public static void PrintBottom(BoardModel boardModel)
        {
            string line = "╚═" + "══";
            for(int i = 1; i < boardModel.board.GetLength(0) - 1; i++)
            {
                line += "╩═" + "══";
            }
            Console.WriteLine(line + "╝");
        }
        public static void PrintLetter(BoardModel board, int row, int col)
        {
            Console.Write("║");
            if(board.colorBoard[row, col] == 0)
            {
                Console.Write(" " + board.board[row, col] + " ");
            }
            else if(board.colorBoard[row, col] == 1)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write(" " + board.board[row, col] + " ");
            }
            else if(board.colorBoard[row, col] == 2)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.Write(" " + board.board[row, col] + " ");
            }
            else if(board.colorBoard[row, col] == 3)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Write(" " + board.board[row, col] + " ");
            }
            Console.ResetColor();
        }
        public static void PrintPlayerMessage(string message, int colorType)
        {
            Console.SetCursorPosition((Console.WindowWidth - (BoardLength)) / 2, Console.CursorTop);
            switch(colorType)
            {

                // 1 - good: green
                case 1:
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.WriteLine(message);
                    Console.ResetColor();
                    break;

                // 2 - bad: red
                case 2:
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine(message);
                    Console.ResetColor();
                    break;

                // 0 - none
                case 0:
                default:
                    Console.ResetColor();
                    Console.WriteLine(message);
                    break;
            }
        }
        public static void WriteLine(string message, int offset = 0)
        {
            Console.SetCursorPosition((Console.WindowWidth - (BoardLength)) / 2, Console.CursorTop);
            Console.WriteLine(message);
            Console.SetCursorPosition((Console.WindowWidth - (BoardLength)) / 2, Console.CursorTop);
        }
        public static void Write(string message, int offset = 0)
        {
            Console.SetCursorPosition((Console.WindowWidth - (BoardLength)) / 2, Console.CursorTop);
            Console.Write(message);

        }
        public static void DisplayStats(Statistics stats)
        {
            const int offset = 38;

            Console.Clear();
            DisplayView.WriteLine("Statistics:\n");
            int[] crossSecLoc = [15, 32, 49];
            string column = "║ Games Played ║ Win Percentage ║ Current Streak ║ Max Streak ║";

            //set top border
            string border = "╔";
            //border = "+" + new string('-', 61) + "+";
            for(int i = 1; i < column.Length - 1; i++)
            {
                switch(i)
                {
                    case 15:
                    case 32:
                    case 49:
                        border += "╦";
                        break;

                    default:
                        border += "═";
                        break;
                }
            }
            border += "╗";
            DisplayView.WriteLine(border, offset + 1);

            DisplayView.WriteLine("║ Games Played ║ Win Percentage ║ Current Streak ║ Max Streak ║", offset);
            string gamesPlayed = stats.gamesPlayed.ToString().PadLeft(7).PadRight(14);
            string winPer = $"{stats.winPercentage:P0}".PadLeft(8).PadRight(16);
            string curStrk = stats.currentStreak.ToString().PadLeft(8).PadRight(16);
            string maxStrk = stats.maxStreak.ToString().PadLeft(6).PadRight(12);
            string statsLine = $"║{gamesPlayed}║{winPer}║{curStrk}║{maxStrk}║";
            DisplayView.WriteLine(statsLine, offset + 1);

            //set bottom border
            border = "╚";
            for(int i = 1; i < column.Length - 1; i++)
            {
                switch(i)
                {
                    case 15:
                    case 32:
                    case 49:
                        border += "╩";
                        break;

                    default:
                        border += "═";
                        break;
                }
            }
            border += "╝";
            DisplayView.WriteLine(border, offset + 1);

            //Distribution
            int max = stats.GetDistrArr().Max();
            int maxIndex = Array.IndexOf(stats.GetDistrArr(), max);

            for(int i = 0; i < 6; i++)
            {
                string line = $"{i}: ";
                Console.SetCursorPosition((Console.WindowWidth - (BoardLength)) / 2, Console.CursorTop);
                Console.Write($"{i}: ");
                if(i == maxIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    for(int j = 0; j < stats.GetDistribution(i); j++)
                        Console.Write("─");
                    Console.ResetColor();
                }
                else
                    for(int j = 0; j < stats.GetDistribution(i); j++)
                        Console.Write("─");

                Console.WriteLine($" {stats.GetDistribution(i)}");
            }


        }
    }
}
