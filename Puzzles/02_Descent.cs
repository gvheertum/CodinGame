using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * The while loop represents the game.
 * Each iteration represents a turn of the game
 * where you are given inputs (the heights of the mountains)
 * and where you have to print an output (the index of the mountain to fire on)
 * The inputs you are given are automatically updated according to your last actions.
 **/
namespace Puzzles.Descent
{
    class Player
    {
        static void Main(string[] args)
        {

            // game loop
            while (true)
            {
                
                List<int> currentMountainHeights = GetMountainsFromConsole().ToList();
                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");
                int mountainToShoot = GetMountainToShoot(currentMountainHeights);
                Console.WriteLine(mountainToShoot); // The index of the mountain to fire on.
            }
        }
        
        private const int ExpectedMountains = 8;
        private static IEnumerable<int> GetMountainsFromConsole()
        {
            for (int i = 0; i < ExpectedMountains; i++)
            {
                yield return int.Parse(Console.ReadLine()); // represents the height of one mountain.
            }
        }
        
        private static int GetMountainToShoot(IEnumerable<int> mountains)
        {
            return mountains.ToList().IndexOf(mountains.Max(m => m));
        }
    }
}