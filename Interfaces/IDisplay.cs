using FountainOfObjects.RoomContents;

namespace FountainOfObjects.Interfaces
{
    public interface IDisplay
    {
        public void DisplayPlayerExploration(Cave cave, Player player);
        public void DisplayFullCave(Cave cave);
        public void ShowHelp();
        public void DescribeSituation(Cave cave, Player player);
        public void InformPlayer(Cave cave, Player player);
        public void DisplayKill(RoomContent content);
        public void OnPlayerWinScreen(Player? player);
        public void OnPlayerLoseScreen(Player? player, RoomContent? roomContent);
        public void FinishingInfo(Cave cave, Score score, List<Score>? scores);
        public void ShowHighScores(List<Score> score);
    }
}
