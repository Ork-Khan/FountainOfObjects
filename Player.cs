using FountainOfObjects.RoomContents;
using System;

namespace FountainOfObjects
{
    public class Player
    {
        public string Name { get; }
        public Coordinate CurrentCoordinate { get; set; }
        public int ArrowCount { get; private set; }
        public int Health { get; private set; }
        public int KillCount { get; private set; }

        public event Action<Player?>? PlayerWon;
        public event Action<Player?, RoomContent?>? PlayerDied;
        public void WonGame()
        {
            PlayerWon?.Invoke(this);
        }
        public bool[,] DiscoveredAreas { get; private set; }
        public Player(Coordinate coordinate, int arrowCount, int caveSize, string name, int health)
        {
            ArrowCount = arrowCount;
            Name = name;
            CurrentCoordinate = coordinate;

            DiscoveredAreas = new bool[caveSize, caveSize];
            DiscoveredAreas[coordinate.Row, coordinate.Column] = true;
            Health = health;
        }

        public void ChangeHealth(int healthEffect, RoomContent damageDealer)
        {
            Health += healthEffect;
            if(Health <= 0)
            {
                Health = 0;
                PlayerDied?.Invoke(this, damageDealer);
            }
        }

        public void ChangeCoordinate(Cave cave, int rowchange, int columnchange)
        {
            int currentRow = CurrentCoordinate.Row;
            int currentColumn = CurrentCoordinate.Column;

            if (rowchange != 0 && currentRow + rowchange >= 0 && currentRow + rowchange < cave.Size)
            {
                CurrentCoordinate = new Coordinate(currentRow + rowchange, currentColumn);
                DiscoveredAreas[CurrentCoordinate.Row, CurrentCoordinate.Column] = true;
            }
            
            if (columnchange != 0 && currentColumn + columnchange >= 0 && currentColumn + columnchange < cave.Size)
            {
                CurrentCoordinate = new Coordinate(CurrentCoordinate.Row, currentColumn + columnchange);
                DiscoveredAreas[CurrentCoordinate.Row, CurrentCoordinate.Column] = true;
            }

            //activates the room content
            var room = cave.Rooms[CurrentCoordinate.Row, CurrentCoordinate.Column];
            room?.Content?.Action(this, cave);
        }

        public void Move(Cave cave, Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    ChangeCoordinate(cave, -1, 0);
                    break;
                case Direction.South:
                    ChangeCoordinate(cave, 1, 0);
                    break;
                case Direction.East:
                    ChangeCoordinate(cave, 0, 1);
                    break;
                case Direction.West:
                    ChangeCoordinate(cave, 0, -1);
                    break;
            }
        }

        public RoomContent? Shoot(Cave cave, Direction direction)
        {
            bool killedSomething = false;
            int caveSize = cave.Rooms.GetLength(0);
            int arrowRow = CurrentCoordinate.Row;
            int arrowColumn = CurrentCoordinate.Column;
            
            switch(direction)
            {
                case Direction.North:
                    if (arrowRow > 0)
                        arrowRow--;
                    break;
                case Direction.South:
                    if (arrowRow < caveSize - 1)
                        arrowRow++;
                    break;
                case Direction.East:
                    if(arrowColumn < caveSize - 1)
                        arrowColumn++;
                    break;
                case Direction.West:
                    if (arrowColumn > 0)
                        arrowColumn--;
                    break;
            }

            var targetRoom = cave.Rooms[arrowRow, arrowColumn];
            var killObject = targetRoom.Content;
            if (killObject != null && killObject.IsKillable)
            {
                targetRoom.Content = null;
                killedSomething = true;
                KillCount++;
            }

            DiscoveredAreas[arrowRow, arrowColumn] = true;
            ArrowCount--;
            if (killedSomething)
                return killObject;
            else 
                return null;
        }

        public Score CalculateScore(TimeSpan time, int gameSize)
        {
            //if player finished with remaining arrows they give half point of killing something
            double score;
            score = (1000 - Math.Log2(time.TotalSeconds) + 1) * 1000 * gameSize + (KillCount * 100) + (ArrowCount * 50);
            score *= Health;
            return new Score(Name, score, time, gameSize);
        }
    }
}
