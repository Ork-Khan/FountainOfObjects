using FountainOfObjects.Interfaces;
using FountainOfObjects.RoomContents;

namespace FountainOfObjects
{
    public class Game
    {
        public Cave Cave { get; set; }
        public int Size { get; }
        public GameState State { get; private set; }
        private IClient Client { get; }
        private Player GamePlayer { get; }
        public IReadInfo ReadInfo { get; }
        public ISaveInfo SaveInfo { get; }        
        public Game(IClient client, ICaveGenerator caveGenerator, Menu menu, IReadInfo readInfo, ISaveInfo saveInfo) 
        {
            Client = client;
            
            Size = menu.Size;

            Cave = caveGenerator.GenerateCave(Size);
            
            int initialArrowCount = Size/2;
            int health = 1;
            GamePlayer = new Player(Cave.CaveEnterance, initialArrowCount, Size, menu.PlayerName, health);
            
            GamePlayer.PlayerWon += OnPlayerWon;
            GamePlayer.PlayerWon += client.OnPlayerWinScreen;

            GamePlayer.PlayerDied += OnPlayerLost;
            GamePlayer.PlayerDied += client.OnPlayerLoseScreen;

            State = GameState.OnGoing; 

            ReadInfo = readInfo;
            SaveInfo = saveInfo;
        }

        public void OnPlayerWon(Player? player)
        {
            State = GameState.Win;
        }
        public void OnPlayerLost(Player? player, RoomContent? roomContent)
        {
            State = GameState.Lost;
        }

        public void Start()
        {
            DateTime start = DateTime.Now;

            //FOR DISPLAYING LAST KILLED CREATURE
            RoomContent? killObject = null;

            do
            {
                //show stats and previous action results.
                Client.InformPlayer(Cave, GamePlayer);
                
                if (killObject != null)
                {
                    Client.DisplayKill(killObject);
                    killObject = null;
                }

                //take action should return player action either, H, S or M and in last two cases direction as well
                var action = Client.GetPlayerAction(GamePlayer.ArrowCount);

                if(action.actionType == ActionType.Help)
                {
                    Client.ShowHelp();
                    continue;
                }
                else if (action.actionType == ActionType.Shoot && GamePlayer.ArrowCount > 0 && action.direction != null)
                {
                    killObject = GamePlayer.Shoot(Cave, action.direction.Value);
                }
                else if (action.actionType == ActionType.Move && action.direction != null)
                {
                    GamePlayer.Move(Cave, action.direction.Value);
                }
                else
                    continue;
            } while (State == GameState.OnGoing);

            var timeSpent = DateTime.Now - start;
            GamePlayer.CalculateScore(timeSpent, Size);
            var score = GamePlayer.CalculateScore(timeSpent, Size);

            Client.FinishingInfo(Cave, score, ReadInfo.GetScores());
            SaveInfo.SaveScore(score);
        }
    }
}

public record PlayerAction (Direction? direction, ActionType? actionType);
public enum ActionType { Help, Shoot, Move }
public enum GameState { OnGoing, Win, Lost }
public enum Direction { East, West, North, South }
