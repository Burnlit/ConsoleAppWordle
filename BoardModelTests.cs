using Wordle;

namespace WordleTests
{
    [TestClass]
    public class BoardModelTests
    {
        private BoardModel board;
        private int level = 0;

        [TestInitialize] 
        public void Initialize() 
        {
            string word = "APPLE";
            board = new BoardModel(6, word);
        }

        [TestMethod]
        public void TestCheckGuessReturnsTrue()
        {
            bool isSolved = board.CheckGuess("APPLE", level);
            Assert.IsTrue(isSolved);
        }

        [TestMethod]
        public void TestCheckGuessReturnsFalse()
        {
            bool isSolved = board.CheckGuess("AVERT", level);
            Assert.IsFalse(isSolved);
        }

        [TestMethod]
        public void TestCheckGuessIgnoresCase()
        {
            bool isSolved = board.CheckGuess("avert", level);
            Assert.IsFalse(isSolved);
        }

        [TestMethod]
        public void TestCheckGuessUpdatesBoard()
        {
            bool isSolved = board.CheckGuess("APPLE", level);
            Assert.AreEqual('A', board.board[0, 0]);
        }

        [TestMethod]
        public void TestCheckGuessUpdatesColorBoard()
        {
            bool isSolved = board.CheckGuess("APPLE", level);
            Assert.AreEqual(3, board.colorBoard[0, 0]);
        }

        [TestMethod]
        public void TestCheckGuessColorsRepeatedLettersOnlyOnce()
        {
            string word = "PLEAD";
            board = new BoardModel(6, word);
            bool isSolved = board.CheckGuess("APPLE", level);
            Assert.AreEqual(2, board.colorBoard[0, 1]);
            Assert.AreEqual(1, board.colorBoard[0, 2]);
        }

        [TestMethod]
        public void TestCheckGuessColorsDoesNotOverrideLetterInCorrectPosition()
        {
            string word = "PLEAD";
            board = new BoardModel(6, word);
            bool isSolved = board.CheckGuess("PPPPP", level);
            Assert.AreEqual(3, board.colorBoard[0, 0]);
            Assert.AreEqual(1, board.colorBoard[0, 1]);
            Assert.AreEqual(1, board.colorBoard[0, 2]);
            Assert.AreEqual(1, board.colorBoard[0, 3]);
            Assert.AreEqual(1, board.colorBoard[0, 4]);
        }

        [TestMethod]
        public void TestCheckGuessColorsDuplicateLetters()
        {
            string word = "PLEAP";
            board = new BoardModel(6, word);
            bool isSolved = board.CheckGuess("PPPPA", level);
            Assert.AreEqual(3, board.colorBoard[0, 0]);
            Assert.AreEqual(2, board.colorBoard[0, 1]);
            Assert.AreEqual(1, board.colorBoard[0, 2]);
            Assert.AreEqual(1, board.colorBoard[0, 3]);
            Assert.AreEqual(2, board.colorBoard[0, 4]);

            word = "CYNIC";
            board = new BoardModel(6, word);
            isSolved = board.CheckGuess("CCCDD", level);
            Assert.AreEqual(3, board.colorBoard[0, 0]);
            Assert.AreEqual(2, board.colorBoard[0, 1]);
            Assert.AreEqual(1, board.colorBoard[0, 2]);
            Assert.AreEqual(1, board.colorBoard[0, 3]);
            Assert.AreEqual(1, board.colorBoard[0, 4]);
        }
    }
}