using System.Diagnostics;

namespace Wordle
{
    /// <summary>
    /// The GameManager class handles all game logic.
    /// </summary>
    public class GameController
    {
        DisplayView displayView = new DisplayView();

        public GameState GameState { get; private set; }
        private const int MaxAttempt = 6;
        private int playerAttempts = 0;

        public struct PlayerMessage
        {
            public int MessageColorType;
            // 0 - none: normal
            // 1 - good: green
            // 2 - bad: red

            public string Message;
        };

        private PlayerMessage message;
        private BoardModel board;
        private WordGenerator wordGenerator;
        private string currentWord;
        private Statistics playerStats;

        public void StartGame()
        {
            InitGame();

            while(true)
            {
                if(GameState == GameState.RoundStarted)
                {
                    for(playerAttempts = 0; playerAttempts < MaxAttempt; playerAttempts++)
                    {
                        PerformOneTurn();
                    }
                    if(playerAttempts == MaxAttempt)
                    {
                        GameState = GameState.RoundLoss;
                        message.Message = $"You were not able to guess the word \"{currentWord}\"\n";
                        message.MessageColorType = 2;
                        DisplayGameBoard();
                    }

                }
                else if(GameState == GameState.RoundWin || (GameState == GameState.RoundLoss))
                {
                    playerStats.RecordGame(GameState);

                    GameState = GameState.WaitingForUserInput;
                    DisplayView.Write("Press any key to play again. Press Escape to exit the game", 20);
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                    if(keyInfo.Key == ConsoleKey.Escape)
                    {
                        GameState = GameState.GameOver;
                        DisplayStats();
                        Console.WriteLine("\n");
                        DisplayView.Write("Press any key to close the game", 20);
                        Console.ReadKey(true);
                        break; //ends game
                    }
                    else
                    {
                        StartNewRound();
                        continue;
                    }
                }
                else if(GameState == GameState.GameOver)
                {
                    Console.WriteLine("Game Over");
                    break; //backup game ender
                }
                else
                {
                    Debug.WriteLine("Error with the GameState");
                    Console.ReadKey(true);
                    break;
                }
            }
        }

        public void StartNewRound()
        {
            GameState = GameState.RoundStarted;

            wordGenerator = new HardcodedWordGenerator();
            currentWord = wordGenerator.GenerateWord();

            board = new BoardModel(MaxAttempt, currentWord);

            message = new PlayerMessage
            {
                MessageColorType = 0,
                Message = ""
            };
        }
        public void DisplayGameBoard()
        {
            Console.Clear();
            DisplayView.DisplayBoard(board);

            Console.WriteLine();
            KeyboardDisplay.DisplayColoredKeyboard(board);
            Console.WriteLine("\n");

            DisplayView.PrintPlayerMessage(message.Message, message.MessageColorType);
        }

        public void PerformOneTurn()
        {
            GameState = GameState.WaitingForUserInput;
            

            if(message.Message != "")
            {
                DisplayGameBoard();
                message.Message = "";
                message.MessageColorType = 0;
                DisplayView.WriteLine("Press any key to continue. . .", 5);
                Console.ReadKey(false);
            }
            DisplayGameBoard();

            if(playerAttempts == MaxAttempt - 1)
            {
                DisplayView.WriteLine($"Last Attempt!");
            }
            else
            {
                DisplayView.WriteLine($"Attempt #{playerAttempts + 1}");
            }
            DisplayView.WriteLine($"Enter a five letter word");
            string guess = GetUserInput();

            GameState = GameState.CheckingUserInput;

            if(String.IsNullOrEmpty(guess))
            {
                message.Message = "You must enter a word.";
                message.MessageColorType = 2;
                playerAttempts--;
                return;
            }

            if(guess.Length == currentWord.Length)
            {
                if(board.CheckGuess(guess, playerAttempts))
                {
                    //Player wins
                    playerStats.RecordDistribution(playerAttempts);

                    GameState = GameState.RoundWin;
                    playerAttempts = MaxAttempt;
                    message.Message = "Congratulations! You guess is correct!\n\n";
                    message.MessageColorType = 1;
                    DisplayGameBoard();
                }
            }
            else
            {
                message.Message = "You must enter valid 5 letter word.";
                message.MessageColorType = 2;
                playerAttempts--;
            }

        }

        private string GetUserInput()
        {
            const int Centering = DisplayView.BoardLength - 2;
            int gridLevel = 2 * playerAttempts + 1; 
            string fiveLetters = "";
            int letterCount = 0;
            ConsoleKeyInfo keyInfo;

            Console.WriteLine();
            Console.SetCursorPosition(((Console.WindowWidth - Centering) / 2) + letterCount, gridLevel);
            do
            {
                keyInfo = Console.ReadKey(true);

                if((keyInfo.Key == ConsoleKey.Backspace) && (letterCount > 0))
                {
                    letterCount--;
                    fiveLetters = fiveLetters.Substring(0, letterCount);
                    Console.SetCursorPosition(((Console.WindowWidth - Centering) / 2) + letterCount * 4, gridLevel);
                    Console.Write("   ");
                    Console.SetCursorPosition(((Console.WindowWidth - Centering) / 2) + letterCount * 4, gridLevel);
                }
                else if(((keyInfo.KeyChar >= 'a') && (keyInfo.KeyChar <= 'z') ||
                         (keyInfo.KeyChar >= 'A') && (keyInfo.KeyChar <= 'Z'))
                      && (letterCount < currentWord.Length))
                {
                    letterCount++;
                    Console.Write($" {keyInfo.Key} ");
                    Console.SetCursorPosition(((Console.WindowWidth - Centering) / 2) + letterCount * 4, gridLevel);
                    fiveLetters += keyInfo.Key;
                }

            } while(keyInfo.Key != ConsoleKey.Enter);

            return fiveLetters;
        }

        public void DisplayStats()
        {
            DisplayView.DisplayStats(playerStats);

        }

        public void InitGame()
        {
            playerStats = new Statistics();

            Console.SetCursorPosition((Console.WindowWidth - 50) / 2, Console.CursorTop);
            Console.WriteLine("\t\tWelcome to Wordle!\n");

            Console.SetCursorPosition((Console.WindowWidth - 50) / 2, Console.CursorTop);
            Console.WriteLine("You will have 6 chances to guess the 5 letter word.");

            Console.SetCursorPosition((Console.WindowWidth - 50) / 2, Console.CursorTop);
            Console.WriteLine("Each letter of your guess will be colored,");

            Console.SetCursorPosition((Console.WindowWidth - 60) / 2, Console.CursorTop);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.Write(" Yellow:");
            Console.ResetColor();
            Console.WriteLine(" the letter is in the word, but not in the right position.");

            Console.SetCursorPosition((Console.WindowWidth - 60) / 2, Console.CursorTop);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write(" Green:");
            Console.ResetColor();
            Console.WriteLine(" the letter is in the word, and in the right position.\n");

            DisplayView.Write("Press any key to begin.");
            Console.ReadKey(false);
            StartNewRound();
        }
    }
}
