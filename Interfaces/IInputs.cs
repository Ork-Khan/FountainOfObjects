namespace FountainOfObjects.Interfaces
{
    public interface IInputs
    {
        public string AskUser(string message);
        public PlayerAction GetPlayerAction(int arrowCount);
    }
}
