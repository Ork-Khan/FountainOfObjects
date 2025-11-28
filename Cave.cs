using System.Drawing;
using FountainOfObjects.RoomContents;

namespace FountainOfObjects
{
    public class Cave
    {
        public int Size { get; }
        public Room[,] Rooms { get; }

        public Coordinate CaveEnterance { get; }
        public Coordinate CaveFountain { get; }
        public bool WorkingFountain { get; set; }
        public Cave(Room[,] rooms) 
        {
            Size = rooms.GetLength(0);
            Rooms = rooms;

            WorkingFountain = false;

            for(int i = 0; i < Size; i++)
            {
                for(int j = 0; j < Size; j++)
                {
                    if (rooms[i, j]?.Content is Enterance)
                        CaveEnterance = new Coordinate(i, j);
                    else if(rooms[i, j]?.Content is Fountain)
                        CaveFountain = new Coordinate(i, j);
                }
            }
        }

        public void EnableFountain(Coordinate roomLocation)
        {
            if(roomLocation == CaveFountain)
                WorkingFountain = true;
        }
 
    }

    public class Room
    {
        public Coordinate RoomLocation { get; }
        public RoomContent? Content { get; set; }

        public Room(Coordinate roomLocation, RoomContent? content)
        {
            RoomLocation = roomLocation;
            Content = content;
        }
    }
}
