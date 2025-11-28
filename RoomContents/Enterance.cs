using FountainOfObjects.RoomContents;

namespace FountainOfObjects
{
    public class Enterance : RoomContent
    {
        public Enterance() : base("", 0, "You see light in this room coming from outside the cavern. This is the entrance." ,false)
        {
        }

        public override void Action(Player player, Cave cave)
        {
            if (cave.WorkingFountain)
                player.WonGame();
        }
        public override string ToString() => "Enterance";
    }
}
