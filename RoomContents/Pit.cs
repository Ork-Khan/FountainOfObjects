using FountainOfObjects.RoomContents;

namespace FountainOfObjects
{
    public class Pit : RoomContent
    {
        public Pit() : base("You feel a draft. There is a pit in a nearby room.", int.MaxValue, "You fell to the pit", isKillable: false)
        {
        }

        public override void Action(Player player, Cave cave)
        {
            player.ChangeHealth(-Damage, this);
        }
        public override string ToString() => "Pit";
    }
}
