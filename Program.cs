using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Numerics;

namespace FountainOfObjects
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Fountain of objects";
   
            var consoleClient = new ConsoleClient();
            var randomCaveGenerator = new RandomCaveGenerator();

            var menu = new Menu(consoleClient);
            var jsonSaver = new JsonInfo("Scores.json");

            var game = new Game(consoleClient, randomCaveGenerator, menu, jsonSaver, jsonSaver);

            game.Start();
        }
    }

    
}
