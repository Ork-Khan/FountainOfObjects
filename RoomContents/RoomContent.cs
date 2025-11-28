namespace FountainOfObjects.RoomContents
{
    public abstract class RoomContent
    {
        public bool IsKillable { get; }
        public bool IsDeadly { get; }
        public string ConfrontingMessage { get; internal set; }
        public string SensingMessage { get; internal set; }
        public int Damage { get; }

        public RoomContent(string sensingMessage, int damage, string cofrontingMessage, bool isKillable)
        {
            SensingMessage = sensingMessage;
            Damage = damage;
            ConfrontingMessage = cofrontingMessage;
            IsKillable = isKillable;
        }
        public abstract void Action(Player player, Cave cave);

    }
}
