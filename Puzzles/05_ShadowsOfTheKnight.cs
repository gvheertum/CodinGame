using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace Puzzles.ShadowsOfTheKnight
{
    class Position
    {
        public int X {get;set;}
        public int Y {get;set;}
        public override string ToString()
        {
            return $"Position: x:{X} y:{Y}";
        }
    }

    class Player
    {
        static void Main(string[] args)
        {
            string[] inputs;
            inputs = Console.ReadLine().Split(' ');
            int width = int.Parse(inputs[0]); // width of the building.
            int height = int.Parse(inputs[1]); // height of the building.
            int nrOfTurns = int.Parse(Console.ReadLine()); // maximum number of turns before game over.
            inputs = Console.ReadLine().Split(' ');
            var batmanPos = new Position() { X = int.Parse(inputs[0]), Y = int.Parse(inputs[1])};
            Log($"Building: h: {height} w:{width}");
            Log($"Batman at: {batmanPos}");

            // game loop
            while (true)
            {
                Log($"{nrOfTurns} left");
                string bombDir = Console.ReadLine(); // the direction of the bombs from batman's current location (U, UR, R, DR, D, DL, L or UL)
                Log($"Bomb is in {bombDir} direction of us");
                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");
                var newLocation = GetNewLocationForBatman(bombDir, batmanPos);
                Log($"Decided to move the battyman to: {newLocation}");
                
                // the location of the next window Batman should jump to.
                Console.WriteLine(newLocation);
                nrOfTurns--;
            }
        }

        //Determine the next step based on direction: UD - LR and position of batman
        //The current implementation just lets batman jump in the direction of the bomb.
        //This is a simple quick solve, a smart algorithm with search/smart guessing is next.
        
        //Note: this silly tower has its floors numbered from 0 on the top downwards
        static string GetNewLocationForBatman(string direction, Position batmanPos)
        {   
            int yDelta = 0;
            int xDelta = 0;
        
            //Is the bomb up or down from us
            if(direction.IndexOf("U") > -1) { yDelta = -1; }
            else if(direction.IndexOf("D") > -1) { yDelta = +1; }
    
            //Is the bomb left or right from us
            if(direction.IndexOf("L") > -1) { xDelta = -1; }
            else if(direction.IndexOf("R") > -1) { xDelta = +1; }
        
            batmanPos.X = batmanPos.X + xDelta;
            batmanPos.Y = batmanPos.Y + yDelta;

            return $"{batmanPos.X} {batmanPos.Y}";
        }

        static void Log(object obj)
        {
            Console.Error.WriteLine(obj);
        }
    }
}