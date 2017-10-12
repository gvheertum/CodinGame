
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * CodinGame planet is being attacked by slimy insectoid aliens.
 * <---
 * Hint:To protect the planet, you can implement the pseudo-code provided in the statement, below the player.
 **/
namespace Puzzles.Onboarding
{
	class Player
	{
		class Enemy
		{
			public string Name {get;set;}
			public int Distance {get;set;}
			public override string ToString()
			{
				return $"Ship: {Name} dist {Distance}";
			}
		}
		
		static void Main(string[] args)
		{

			// game loop
			while (true)
			{
				List<Enemy> targets = new List<Enemy>();
				int expectedShips = 2;
				for(int i = 0; i < expectedShips; i++)
				{
					Log("Starting loop!");
					string crVal1 = Console.ReadLine();
					string crVal2 = Console.ReadLine();
					Log("Read: " + crVal1);
					
					//:( Would have expected to get no new lines if there are no ships, 
					//however we need to work with the expectation of 2 ships
					if(String.IsNullOrEmpty(crVal1)) { Log("No new ships"); break; }
					
					var enemy = new Enemy() { Name = crVal1, Distance = int.Parse(crVal2) };
					Log(enemy);
					targets.Add(enemy);
				}
				
				//Blow the closest
				// Write an action using Console.WriteLine()
				// To debug: Console.Error.WriteLine("Debug messages...");
		
				var shootAt = PickClosest(targets);
			
				// You have to output a correct ship name to shoot ("Buzz", enemy1, enemy2, ...)
				Console.WriteLine(shootAt?.Name);
			
			}
			
		}
		
		private static Enemy PickClosest(IEnumerable<Enemy> enemies)
		{
			if(!enemies.Any()) { return null; }
			return enemies.FirstOrDefault(e => e.Distance == enemies.Min(ee => ee.Distance));
		}
		
		private static void Log(object output) 
		{
			Console.Error.WriteLine(output);
		}
	}
}