using FountainOfObjects.RoomContents;

namespace FountainOfObjects
{
    public class WeirdBat : RoomContent
    {
        public WeirdBat() : base("You hear Bat's echolocation", 0, "You have entered the Weird bat's nest and it yeets you", true) { }
        public override void Action(Player player, Cave cave)
        {
            var random = new Random();
            int size = cave.Size;

            // move => new Coordinate = old Coordinate + input 
            // so to get true range we substitute current number that'll be added in move method.

            int rowMove = random.Next(size) - player.CurrentCoordinate.Row;
            int columnMove = random.Next(size) - player.CurrentCoordinate.Column;

            player.ChangeCoordinate(cave, rowMove, columnMove);
        }
        public override string ToString() => "Weird Bat";
    }
}
