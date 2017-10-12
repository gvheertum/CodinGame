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
        public Position GetCopy()
        {
            return new Position() { X = X, Y = Y };
        }
        public string GetAsJumpPosition()
        {
            return $"{X} {Y}";
        }
    }

    class Player
    {
         //Note: this silly tower has its floors numbered from 0 on the top downwards
       
        static void Main(string[] args)
        {
            string[] inputs;
            inputs = Console.ReadLine().Split(' ');
            int width = int.Parse(inputs[0]); // width of the building.
            int height = int.Parse(inputs[1]); // height of the building.
            Position flatMinPos = new Position() { X = 0, Y = 0 };
            Position flatMaxPos = new Position() { X = width-1, Y = height-1 }; //Arrays start at 0 ;)

            int nrOfTurns = int.Parse(Console.ReadLine()); // maximum number of turns before game over.
            inputs = Console.ReadLine().Split(' ');
            var batmanPos = new Position() { X = int.Parse(inputs[0]), Y = int.Parse(inputs[1])};
            Log($"Building: h: {height} w:{width}");
            Log($"Batman at: {batmanPos}");

            //The outerbound is used to determine 
            Position outerbound = null;
            // game loop
            while (true)
            {
                Log($"{nrOfTurns} left");
                string bombDir = Console.ReadLine(); // the direction of the bombs from batman's current location (U, UR, R, DR, D, DL, L or UL)
                Log($"Bomb is in {bombDir} direction of us");
                
                bool firstTryOuter = outerbound == null;
                outerbound = outerbound == null 
                    ? GetInitialMaxBound(batmanPos, flatMinPos, flatMaxPos, bombDir) 
                    : CalculateJumpPosition(bombDir, batmanPos);
                
                var jumpPos = firstTryOuter ? outerbound : GetSplitPoint(outerbound, batmanPos);

                Log($"Decided to move the battyman to: {jumpPos}");
                
                // the location of the next window Batman should jump to.
                Console.WriteLine(jumpPos.GetAsJumpPosition());
                nrOfTurns--;
            }
        }


        //Determine the outerbound based on the direction, batmans pas and the max pos of the flat.
        static Position GetInitialMaxBound(Position batman, Position minPos, Position maxPos, string direction)
        {
            Log($"Determine the bound based on batman: {batman} min: {minPos} max: {maxPos} dir: {direction}");
            //In some cases the bomb is hiding is the corner, we can directly try to go there
            Position newPos = batman.GetCopy();
            if(BombIsUp(direction)) { newPos.Y = minPos.Y; }
            if(BombIsDown(direction)) { newPos.Y = maxPos.Y; }

            if(BombIsLeft(direction)) { newPos.X = minPos.X; }
            if(BombIsRight(direction)) { newPos.X = maxPos.X; }
            Log($"Took the outerbound as new move dir: {newPos}");
            return newPos;
        }

       
        //Determine the next step based on direction: UD - LR and position of batman
        //The current implementation just lets batman jump in the direction of the bomb.
        //This is a simple quick solve, a smart algorithm with search/smart guessing is next.
        
        static Position CalculateJumpPosition(string direction, Position batmanPos)
        {   
            int yDelta = 0;
            int xDelta = 0;
        
            //Is the bomb up or down from us
            if(BombIsUp(direction)) { yDelta = -1; }
            else if(BombIsDown(direction)) { yDelta = +1; }
    
            //Is the bomb left or right from us
            if(BombIsLeft(direction)) { xDelta = -1; }
            else if(BombIsRight(direction)) { xDelta = +1; }
        
            batmanPos.X = batmanPos.X + xDelta;
            batmanPos.Y = batmanPos.Y + yDelta;

            return batmanPos;
        }
        private static bool BombIsDown(string dir) { return dir.IndexOf("D") > -1; }
        private static bool BombIsUp(string dir) { return dir.IndexOf("U") > -1; }
        private static bool BombIsLeft(string dir) { return dir.IndexOf("L") > -1; }
        private static bool BombIsRight(string dir) { return dir.IndexOf("R") > -1; }
        
        static Position GetSplitPoint(Position p1, Position p2)
        {
            var midPos = new Position()
            {
                X = (p1.X + p2.X) / 2,
                Y = (p1.Y + p2.Y) / 2
            };
            Log($"Split point: {midPos}");
            return midPos;
        }
        
        // static int TrySolveXAxis() 
        // {

        // }

        // static int TrySolveYAxis(int min, int currMax, string direction)
        // {

        // }

        static void Log(object obj)
        {
            Console.Error.WriteLine(obj);
        }
    }
}