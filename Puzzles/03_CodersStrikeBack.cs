using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

//https://www.codingame.com/ide/puzzle/coders-strike-back

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace Puzzles.CodersStrikeBack
{
    class Player
    {
        static bool boosted = false;
        static void Main(string[] args)
        {
            string[] inputs;
            List<int> distances = new List<int>();
            
            // game loop
            while (true)
            {
                inputs = Console.ReadLine().Split(' ');
                int x = int.Parse(inputs[0]);
                int y = int.Parse(inputs[1]);
                int nextCheckpointX = int.Parse(inputs[2]); // x position of the next check point
                int nextCheckpointY = int.Parse(inputs[3]); // y position of the next check point
                int nextCheckpointDist = int.Parse(inputs[4]); // distance to the next checkpoint
                var useBoost = GoodToBoostNow(distances, nextCheckpointDist);
                distances.Add(nextCheckpointDist);
                int nextCheckpointAngle = int.Parse(inputs[5]); // angle between your pod orientation and the direction of the next checkpoint
                inputs = Console.ReadLine().Split(' ');
                int opponentX = int.Parse(inputs[0]);
                int opponentY = int.Parse(inputs[1]);
                Console.Error.WriteLine("Boosting? " + useBoost);
                
                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");

                int trust = nextCheckpointAngle < 90 && nextCheckpointAngle > -90 ? 100 : 10;
                
                // You have to output the target position
                // followed by the power (0 <= thrust <= 100)
                // i.e.: "x y thrust"
                Console.WriteLine(nextCheckpointX + " " + nextCheckpointY + " " + trust + (useBoost ? " boost" : ""));
                if(useBoost) { boosted = true; }
            }
        }
        static bool GoodToBoostNow(IEnumerable<int> distances, int currDist)
        {
            if(boosted || distances.Count() < 3) { return false; }
            if(currDist >= distances.Average(d => d)) { return true; }
            return false;
        }
    }
}