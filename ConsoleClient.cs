using FountainOfObjects.Interfaces;
using FountainOfObjects.RoomContents;
using System.Drawing;
using System.Numerics;

namespace FountainOfObjects
{
    public class ConsoleClient : IClient
    {

        private readonly Dictionary<string, string> _mapSymbols = new Dictionary<string, string>()
        {
           { "Enterance" , ">"},
           { "Fountain" , "Y"},
           { "Pit" , "U"},
           { "Weird Bat" , "M"},
           { "WereWolf" , "W"}
        };

        private readonly Dictionary<string, ConsoleColor> _contentColors = new Dictionary<string, ConsoleColor>()
        {
           { "Enterance" , ConsoleColor.Yellow},
           { "Fountain" , ConsoleColor.Magenta},
           { "Pit" , ConsoleColor.DarkCyan },
           { "Weird Bat" , ConsoleColor.DarkBlue},
           { "WereWolf" , ConsoleColor.DarkBlue}
        };

        public string AskUser(string message)
        {
            string? input;
            do
            {
                Console.Write(message);
                input = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(input));

            return input;
        }

        public Dictionary<ConsoleKey, Direction> keyBoardMapping = new Dictionary<ConsoleKey, Direction>()
        {
            { ConsoleKey.UpArrow, Direction.North},
            { ConsoleKey.DownArrow, Direction.South},
            { ConsoleKey.LeftArrow, Direction.West},
            { ConsoleKey.RightArrow, Direction.East},
        };
        public PlayerAction GetPlayerAction(int arrowCount)
        {
            Console.WriteLine($"You have {arrowCount} arrows left. What do you want to do? shoot (Spacebar) or move (arrow keys) (Press H for help) ");
            ConsoleKeyInfo input;
            input = Console.ReadKey();

            Direction? direction = null;
            ActionType? actionType = null;
            switch (input.Key)
            {
                case ConsoleKey.H:
                    actionType = ActionType.Help;
                    break;
                case ConsoleKey.Spacebar:
                    actionType = ActionType.Shoot;
                    Console.WriteLine("Which way do you want to shoot? ");
                    input = Console.ReadKey();
                    goto case ConsoleKey.LeftArrow;
                case ConsoleKey.LeftArrow:
                case ConsoleKey.RightArrow:
                case ConsoleKey.UpArrow:
                case ConsoleKey.DownArrow:
                    direction = keyBoardMapping[input.Key];
                    if (actionType == null)
                        actionType = ActionType.Move;
                    break;
            }
            return new PlayerAction(direction, actionType);
        }

        public void ShowHelp()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            Console.WriteLine("You enter the Cavern of Objects, a maze of rooms filled with dangerous pits in search of the Fountain of Objects (Y).");
            Console.WriteLine("Light is visible only in the entrance (>), and no other light is seen anywhere in the caverns.");
            Console.WriteLine("You must navigate the Caverns with your other senses.");
            Console.WriteLine("Find the Fountain of Objects, activate it, and return to the entrance.");
            Console.WriteLine("Look out for pits (U). You will feel a breeze if a pit is in an adjacent room. If you enter a room with a pit, you will die");
            Console.WriteLine("Maelstroms (M) are violent forces of sentient wind. Entering a room with one could transport you to any other location in the caverns. ");
            Console.WriteLine("You will be able to hear their growling and groaning in nearby rooms.");
            Console.WriteLine("Amaroks (W) roam the caverns. Encountering one is certain death, but you can smell their rotten stench in nearby rooms.");

            Console.ResetColor();
            Wait();
        }

        private void Wait()
        {
            Console.Write("Enter anything to continue ");
            string? _ = Console.ReadLine();
            Console.Clear();
        }

        public void DisplayPlayerExploration(Cave cave, Player player)
        {
            var rooms = cave.Rooms;
            int size = rooms.GetLength(0);

            Console.WriteLine();
            for (int j = 0; j < size; j++)
            {
                Console.Write("+---+");
            }
            Console.WriteLine();

            for (int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {

                    if (player.DiscoveredAreas[i, j])
                    {
                        string? roomContent = rooms[i, j]?.Content?.ToString();

                        if ( player.CurrentCoordinate == new Coordinate(i,j))
                        {
                            Console.Write("| İ |");
                        }
                        else if(string.IsNullOrEmpty(roomContent))
                        {
                            Console.Write("|   |");
                        }
                        else
                        {
                            WriteColor($"| {_mapSymbols[roomContent]} |", _contentColors[roomContent]);
                        }
                    }
                    else
                    {
                        Console.Write("|o0O|");
                    }
                    
                }
                Console.WriteLine();
                for (int _ = 0; _ < size; _++)
                {
                    Console.Write("+---+");
                }
                Console.WriteLine();

            }
        }
        public void DisplayKill(RoomContent content)
        {
            if(content is WereWolf || content is WeirdBat)
            {
                
                WriteLineColor($"You killed {content.ToString()}", ConsoleColor.Green);
            }
        }
        public void DisplayFullCave(Cave cave)
        {
            var rooms = cave.Rooms;
            int size = rooms.GetLength(0);

            Console.WriteLine();
            for (int j = 0; j < size; j++)
            {
                Console.Write("+---+");
            }
            Console.WriteLine();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    string? roomContent = rooms[i, j]?.Content?.ToString();

                    if (string.IsNullOrEmpty(roomContent))
                    {
                        Console.Write("|   |");
                    }
                    else
                    {
                        WriteColor($"| {_mapSymbols[roomContent]} |", _contentColors[roomContent]);
                    }
                }
                Console.WriteLine();
                for (int _ = 0; _ < size; _++)
                {
                    Console.Write("+---+");
                }
                Console.WriteLine();

            }
        }
        public void DescribeSituation(Cave cave, Player player)
        {
            Console.WriteLine($"You are in the room at {player.CurrentCoordinate}");

            var currentRoomContent = cave.Rooms[player.CurrentCoordinate.Row, player.CurrentCoordinate.Column].Content;
            if( currentRoomContent != null )
                WriteLineColor(currentRoomContent.ConfrontingMessage, _contentColors[currentRoomContent.ToString()]); 

            for (int i = Math.Max(0, player.CurrentCoordinate.Row - 1); i <= Math.Min(cave.Size - 1, player.CurrentCoordinate.Row + 1); i++)
            {
                for (int j = Math.Max(0, player.CurrentCoordinate.Column - 1); j <= Math.Min(cave.Size - 1, player.CurrentCoordinate.Column + 1); j++)
                {
                    if (i == player.CurrentCoordinate.Row && j == player.CurrentCoordinate.Column)
                        continue;

                    var aroundroom = cave.Rooms[i, j];
                    var aroundRoomContent = aroundroom?.Content;
                    if (aroundRoomContent != null)
                    {
                        WriteLineColor(aroundRoomContent.SensingMessage, _contentColors[aroundRoomContent.ToString()]);   
                    }
                    
                }
            }
        }
        public void OnPlayerWinScreen(Player? player)
        {
            Console.Clear();
            WriteLineColor("The Fountain of Objects has been reactivated, and you have escaped with your life! You win!", ConsoleColor.Magenta);
            if (player != null ) 
                WriteLineColor($"Thanks {player.Name}, you have done great job!", ConsoleColor.Magenta);
        }

        public void OnPlayerLoseScreen(Player? player, RoomContent? roomContent)
        {
            Console.Clear();
            if(!string.IsNullOrEmpty(roomContent?.ToString()))
                WriteLineColor(roomContent.ConfrontingMessage, _contentColors[roomContent.ToString()]);
            WriteLineColor("You DIED!", ConsoleColor.DarkRed);
            if(player != null)
                WriteLineColor($"Rest in peace {player.Name}, you fought well but now it is time to rest...", ConsoleColor.Magenta);
        }

        public void FinishingInfo(Cave cave, Score score, List<Score>? scores)
        {
            Console.WriteLine($"Time spent in cavern {score.TimeSpent.ToString()}");
            WriteLineColor($"Finishing Score {score.GameScore}", ConsoleColor.Yellow);
            DisplayFullCave(cave);
            if(scores != null && scores.Count > 0 ) 
                ShowHighScores(scores);

            Wait();
        }

        public void ShowHighScores(List<Score> score)
        {
            int minTableLength = 50;
            int maxTableLength = int.MinValue;

            DrawLine(minTableLength);
            Console.WriteLine($"| Player Name | TimeSpent | CaveSize | GameScore |");
            DrawLine(minTableLength);

            foreach (var scoreItem in score)
            {
                string row = $"| {scoreItem.PlayerName} | {scoreItem.TimeSpent} | {scoreItem.CaveSize} | {scoreItem.GameScore} |";
                
                int rowSize = Math.Max(row.Length, minTableLength);
                maxTableLength = rowSize > maxTableLength ? rowSize : maxTableLength;

                DrawLine(rowSize);
                Console.WriteLine(row);
                DrawLine(rowSize);
            }


            void DrawLine(int size)
            {
                for (int _ = 0; _ < size; _++)
                    Console.Write("-");
                Console.WriteLine();
            }
        }

        public void InformPlayer(Cave cave, Player player)
        {
            Console.Clear();
            this.DisplayPlayerExploration(cave, player);
            Console.WriteLine();
            Console.WriteLine("------------------------------------------------------------------------------------------------");
            this.DescribeSituation(cave, player);
        }

        public void WriteLineColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public void WriteColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }
    
    }
}
