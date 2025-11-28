using FountainOfObjects.RoomContents;

namespace FountainOfObjects
{
    public class WereWolf : RoomContent
    {
        public WereWolf() : base("You can smell the rotten stench of an werewolf in a nearby room.", int.MaxValue, "Werewolf fucking kills you!", isKillable: true) { }
        public override void Action(Player player, Cave cave)
        {
            player.ChangeHealth( -Damage, this);
        }
        public override string ToString() => "WereWolf";
        
    }
}
