using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Wordle
{
    internal class KeyboardDisplay
    {
        private const int BoardLength = 30;
        private static char[] _letters = 
            ['Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P', 
            'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 
            'Z', 'X', 'C', 'V', 'B', 'N', 'M'];
        // Display the keyboard with color coded letters
        // pass in parameter for the guessed letters
        public static void DisplayColoredKeyboard(BoardModel board)
        {
            Console.SetCursorPosition((Console.WindowWidth - BoardLength) / 2, Console.CursorTop);
            Console.Write("  ");
            var guessedLetters = board.guessedLetters;

            Console.SetCursorPosition((Console.WindowWidth - 45) / 2, Console.CursorTop);
            Console.Write(" ");
            for (int i = 0; i < _letters.Length; i++)
            {
                if (i == 10)
                {
                    Console.Write("\n");
                    Console.SetCursorPosition((Console.WindowWidth - 37) / 2, Console.CursorTop);
                }
                if (i == 19)
                {
                    Console.Write("\n");
                    Console.SetCursorPosition((Console.WindowWidth - 31) / 2, Console.CursorTop);
                }
                // Add a check to see if the letter has been guessed and change the color of the letter when applicable
                if (guessedLetters.ContainsKey(_letters[i]))
                {
                    guessedLetters.TryGetValue(_letters[i], out int color);
                    switch (color)
                    {
                        case 1:
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Black;
                            break;
                        case 2:
                            Console.BackgroundColor = ConsoleColor.DarkYellow;
                            Console.ForegroundColor = ConsoleColor.Black;
                            break;
                        case 3:
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.ForegroundColor = ConsoleColor.Black;
                            break;
                    }
                }
                else
                {
                    Console.ResetColor();
                }
                Console.Write(" " + _letters[i] + " ");
                Console.ResetColor();
                Console.Write(" ");
            }
        }
    }
}
