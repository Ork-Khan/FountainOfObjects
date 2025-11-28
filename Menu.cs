using FountainOfObjects.Interfaces;

namespace FountainOfObjects
{
    // TODO
    // add initial menu for creating both game and player classes
    // create a class that will get all console input and outputs seperated from rest of the game.
    //take ask user and get input from game class.


    // add a room class and IRoomContent interface that will contain room content
    // room should have Iroomcontent and coordinate

    //complete client logic
    public  class Menu
    {
        private IClient Client { get; }
        public string PlayerName { get; private set; }
        public int Size { get; private set; }
        
        public Menu(IClient client)
        {
            Client = client;
            PlayerName = GetPlayerName();
            Size = GetGameSize();
            
            client.ShowHelp();
        }
        
        public int GetGameSize()
        {
            int size;
            bool assignedSize = true;
            do
            {
                assignedSize = int.TryParse(Client.AskUser("What should be the Cave size? "), out size);
            } while (!assignedSize || size <= 1);
            return size;
        }

        public string GetPlayerName() => Client.AskUser("What is your name? "); 
    }
}
