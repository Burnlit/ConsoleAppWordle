using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Wordle
{
    public class BoardModel
    {
        private readonly string word;
        public char[,] board { get; }
        public int[,] colorBoard { get; }
        public Dictionary<char, int> guessedLetters;
        private Dictionary<char, int> letters;

        public BoardModel(int attempts, string word)
        {
            this.word = word.ToUpper();
            this.board = new char[attempts, word.Length];
            this.colorBoard = new int[attempts, word.Length];
            this.guessedLetters = new Dictionary<char, int>();
            this.letters = MapLetters(word);

        }

        private static Dictionary<char, int> MapLetters(string word)
        {
            Dictionary<char, int> letters = new Dictionary<char, int>(word.Length);
            foreach (char letter in word)
            {
                if (letters.ContainsKey(letter))
                {
                    letters[letter]++;
                }
                else
                {
                    letters.Add(letter, 1);
                }
            }
            return letters;
        }

        public bool CheckGuess(string guess, int level)
        {
            bool isSolved = true;
            string guessWord = guess.ToUpper();
            Dictionary<char, int> colorMap = new Dictionary<char, int>(word.Length);

            for (int i = 0; i < guessWord.Length; i++)
            {
                colorBoard[level, i] = 1;
                board[level, i] = guessWord[i];
                if (word[i] == guessWord[i])
                {
                    colorBoard[level, i] = 3;
                    guessedLetters[guessWord[i]] = 3;
                    colorMap[guessWord[i]] = colorMap.GetValueOrDefault(guessWord[i]) + 1;
                }
                else
                {
                    if (!colorMap.ContainsKey(guessWord[i]))
                    {
                        colorMap[guessWord[i]] = 0;
                    }
                    isSolved = false;
                }
            }

            for (int i = 0; i < guessWord.Length; i++)
            {
                if (word.Contains(guessWord[i]))
                {
                    if (colorMap[guessWord[i]] < letters[guessWord[i]] && colorBoard[level, i] != 3)
                    {
                        colorBoard[level, i] = 2;
                        guessedLetters.TryAdd(guessWord[i], 2);
                        colorMap[guessWord[i]] = colorMap.GetValueOrDefault(guessWord[i]) + 1;
                    }
                }
                else
                {
                    guessedLetters.TryAdd(guessWord[i], 1);
                }
            }

            return isSolved;
        }
    }
}
