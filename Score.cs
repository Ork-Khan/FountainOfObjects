using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FountainOfObjects
{
    public class Score
    {
        public string PlayerName {  get; }
        public double GameScore { get; }
        public TimeSpan TimeSpent { get; }
        public int CaveSize { get; }

        public Score(string playerName, double gameScore, TimeSpan timeSpent, int caveSize)
        {
            PlayerName = playerName;
            GameScore = gameScore;
            TimeSpent = timeSpent;
            CaveSize = caveSize;
        }
    }
}
