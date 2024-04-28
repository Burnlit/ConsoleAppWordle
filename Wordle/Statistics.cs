
namespace Wordle
{
    public class Statistics
    {
        public int gamesPlayed { get; private set; }
        public int currentStreak { get; private set; }
        public int maxStreak { get; private set; }
        public double winPercentage { get; private set; }
        private int totalWins { get; set; }
        public int[] distribution { get; private set; }

        public Statistics()
        {
            gamesPlayed = 0;
            currentStreak = 0;
            maxStreak = 0;
            winPercentage = 0;
            totalWins = 0;
            distribution = new int[6];
        }

        public void RecordGame(GameState roundState)
        {
            if (roundState == GameState.RoundWin)
            { 
                gamesPlayed++;
                currentStreak++;
                maxStreak = Math.Max(maxStreak, currentStreak);
                totalWins++;
            }
            else if (roundState == GameState.RoundLoss)
            {
                gamesPlayed++;
                currentStreak = 0;
            }

            if (gamesPlayed != 0) winPercentage = (double)totalWins / gamesPlayed;
        }

        public void RecordDistribution(int level)
        {
            distribution[level]++;
        }
        public int GetDistribution(int level)
        {
            return distribution[level];
        }
        public int[] GetDistrArr()
        {
            return distribution;
        }

        
    }
}
