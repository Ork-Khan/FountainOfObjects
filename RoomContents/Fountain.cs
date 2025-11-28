using FountainOfObjects.RoomContents;

namespace FountainOfObjects
{
    public class Fountain : RoomContent
    {
        public bool IsActive = false;
        public Fountain() : base("", 0, "You hear water dripping in this room. The Fountain of Objects is here!", false)
        {
        }

        public override void Action(Player player, Cave cave)
        {
            IsActive = true;
            cave.WorkingFountain = true;
            this.ConfrontingMessage = "You hear the rushing waters from the Fountain of Objects.";
        }
        public override string ToString() => "Fountain";
    }
}
