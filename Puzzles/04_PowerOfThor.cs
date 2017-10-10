using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 * ---
 * Hint: You can use the debug stream to print initialTX and initialTY, if Thor seems not follow your orders.
 **/
namespace Puzzles.PowerOfThor
{
    class Player
    {
        class PosContainer
        {
            public int X {get;set;}
            public int Y {get;set;}
            public override string ToString()
            {
                return $"x:y {X}:{Y}";
            }
        }
        
        static void Main(string[] args)
        {
            string[] inputs = Console.ReadLine().Split(' ');
            int lightX = int.Parse(inputs[0]); // the X position of the light of power
            int lightY = int.Parse(inputs[1]); // the Y position of the light of power
            int initialTX = int.Parse(inputs[2]); // Thor's starting X position
            int initialTY = int.Parse(inputs[3]); // Thor's starting Y position

            var targetPos = new PosContainer() { X = lightX, Y = lightY };
            var myPos = new PosContainer() { X = initialTX, Y = initialTY };
            // game loop
            while (true)
            {
                int remainingTurns = int.Parse(Console.ReadLine()); // The remaining amount of turns Thor can move. Do not remove this line.
                LogConsole($"I'm @ {myPos} and have {remainingTurns} to go to the target");
            
                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");
                var move = DeterminePath(myPos, targetPos);
                LogConsole($"I decided to head: {move}");
                // A single line providing the move to be made: N NE E SE S SW W or NW
                Console.WriteLine(move);
            }
        }
        
        static string DeterminePath(PosContainer pos, PosContainer target)
        {
            //Determine move and update coords
            int xDelta = target.X - pos.X;
            int yDelta = target.Y - pos.Y;
            
            int xMove = 0;
            int yMove = 0;
            
            LogConsole($"I am x {xDelta} and y {yDelta} steps away from the target");
            
            if(xDelta != 0) { xMove = xDelta > 0 ? 1 : -1; }
            if(yDelta != 0) { yMove = yDelta > 0 ? 1 : -1; }

            LogConsole("moving: x {xMove} x {yMove}");
            pos.X += xMove;
            pos.Y += yMove;
            return DetermineDirection(xMove, yMove);
        }
        
        //Based on the X/Y move we determine the direction to head
        static string DetermineDirection(int deltaX, int deltaY)
        {
            string res = "";
            if(deltaY > 0) { res = "S"; }
            else if (deltaY < 0) { res = "N"; }
            
            if(deltaX > 0) { res += "E"; }
            else if (deltaX < 0) { res += "W"; }
            
            if(res == "") { throw new Exception("No direction"); }
            
            return res;
        }
        static void LogConsole(object obj)
        {
            Console.Error.WriteLine(obj);
        }
    }
}