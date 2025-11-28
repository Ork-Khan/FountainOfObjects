using FountainOfObjects.Interfaces;
using FountainOfObjects.RoomContents;

namespace FountainOfObjects
{
    public class RandomCaveGenerator : ICaveGenerator
    {
        public void SetRandomRoomType(Room[,] rooms,RoomContent roomContent, Random random)
        {
            int size = rooms.GetLength(0);

            Coordinate coordinate;
            do
            {
                coordinate = new Coordinate(random.Next(0, size), random.Next(0, size));

            } while (rooms[coordinate.Row, coordinate.Column] != null);
            rooms[coordinate.Row, coordinate.Column] = new Room(coordinate, roomContent);
        }

        public Cave GenerateCave(int size)
        {
            Random random = new Random();
            
            Room[,] rooms = new Room[size, size];

            for(int i = 0; i < size/2 - 1; i++)
            {
                SetRandomRoomType(rooms, new Pit(), random);
                SetRandomRoomType(rooms, new WeirdBat(), random);
                SetRandomRoomType(rooms, new WereWolf(), random);
            }

            SetRandomRoomType(rooms, new Fountain(), random);
            SetRandomRoomType(rooms, new Enterance(), random);

            for(int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if(rooms[i, j] == null)
                    {
                        rooms[i, j] = new Room(new Coordinate(i, j), null);
                    }
                }
            }

            return new Cave(rooms);   
        }
    }
}
