using Wordle;

namespace WordleTests
{
    [TestClass]
    public class StatisticsTest
    {
        private Statistics stats;

        [TestInitialize]
        public void Initialize()
        {
            stats = new Statistics();
        }

        [TestMethod]
        public void TestRecordGameIncrementCurrentStreakWhenRoundWin()
        {
            stats.RecordGame(GameState.RoundWin);
            Assert.AreEqual(1, stats.gamesPlayed);
            Assert.AreEqual(1, stats.currentStreak);
        }

        [TestMethod]
        public void TestRecordGameResetCurrentStreakWhenRoundLoss()
        {
            stats.RecordGame(GameState.RoundLoss);
            Assert.AreEqual(1, stats.gamesPlayed);
            Assert.AreEqual(0, stats.currentStreak);
        }

        [TestMethod]
        public void TestRecordGameDoesNotUpdateWhenInvalidGameState()
        {
            stats.RecordGame(GameState.RoundStarted);
            Assert.AreEqual(0, stats.gamesPlayed);
            Assert.AreEqual(0, stats.currentStreak);
        }

        [TestMethod]
        public void TestRecordGameUpdatesMaxStreak()
        {
            stats.RecordGame(GameState.RoundWin);
            stats.RecordGame(GameState.RoundWin);
            stats.RecordGame(GameState.RoundWin);
            stats.RecordGame(GameState.RoundLoss);
            stats.RecordGame(GameState.RoundWin);
            Assert.AreEqual(1, stats.currentStreak);
            Assert.AreEqual(5, stats.gamesPlayed);
            Assert.AreEqual(3, stats.maxStreak);
        }

        [TestMethod]
        public void TestRecordGameUpdatesWinPercentageWhenRoundOver()
        {
            stats.RecordGame(GameState.RoundWin);
            stats.RecordGame(GameState.RoundWin);
            stats.RecordGame(GameState.RoundWin);
            stats.RecordGame(GameState.RoundLoss);
            stats.RecordGame(GameState.RoundWin);
            Assert.AreEqual(0.8, stats.winPercentage);
        }

        [TestMethod]
        public void TestRecordGameDoesNotUpdateWinPercentageWhenInvalidState()
        {
            stats.RecordGame(GameState.RoundStarted);
            Assert.AreEqual(0.0, stats.winPercentage);
        }

        [TestMethod]
        public void TestRecordDistributionUpdatesGuessDistributions()
        {
            stats.RecordDistribution(1);
            stats.RecordDistribution(2);
            stats.RecordDistribution(3);
            stats.RecordDistribution(4);
            stats.RecordDistribution(5);
            stats.RecordDistribution(6);
            CollectionAssert.AreEqual(new int[] { 1, 1, 1, 1, 1, 1 }, stats.distribution);
        }
    }
}
