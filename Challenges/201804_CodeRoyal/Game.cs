//require: Position.cs
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Framework;
using System;

//TODO: Build towers 
//TODO: Traing giants (barracks and units)
//TODO: Keep away from enemy towers
//TODO: Prioritize movement and gold
//TODO: Move away queen when in a pickle

//https://www.codingame.com/ide/challenge/code-royale
namespace Challenges.CodeRoyal
{
	public partial class CodeRoyalGame : PuzzleMain
	{
		protected CodeRoyalGame(IGameEngine gameEngine) : base(gameEngine) { }

		public static void Main(string[] args)
		{
			new CodeRoyalGame(new CodingGameProxyEngine()).Run();
		}

		public override void Run()
		{
			var gameState = new GameState();
			gameState.Sites = ReadSites().ToList();

			var player = new CodeRoyalPlayer((o) => Log(o));

			// game loop
			while (IsRunning())
			{
				//Update the field
				UpdateState(gameState);
				var moves = player.GetMoves(gameState);
				Log("Found the following moves:");
				Log($"Queen: {moves.QueenAction}");
				Log($"Training: {moves.TrainingAction}");

				// First line: A valid queen action
				// Second line: A set of training instructions
				WriteLine(moves.QueenAction.ToUpperInvariant().Trim());
				WriteLine(moves.TrainingAction.ToUpperInvariant().Trim());
			}
		}

		//Game state readers and updaters
		private IEnumerable<Site> ReadSites()
		{
			int numSites = int.Parse(ReadLine());
			Log($"Reading {numSites} initial sites");
			for (int i = 0; i < numSites; i++)
			{
				var inputs = ReadLine().Split(' ');
				int siteId = int.Parse(inputs[0]);
				int x = int.Parse(inputs[1]);
				int y = int.Parse(inputs[2]);
				int radius = int.Parse(inputs[3]);
				yield return new Site()
				{
					SiteID = siteId,
					X = x,
					Y = y,
					Radius = radius
				};
			}
		}

		//On each tick update the state of the game
		private void UpdateState(GameState state)
		{
			//Read touch and gold
			var inputs = ReadLine().Split(' ');
			state.Gold = int.Parse(inputs[0]);
			state.TouchedSite = int.Parse(inputs[1]); // -1 if none

			//Update sites
			UpdateSites(state);

			//Update the units
			state.Units = ReadUnits().ToList();
		}

		//We get updated stats on the sites we have on the field
		private void UpdateSites(GameState state) 
		{
			var enumHelper = new EnumHelper();
			Log($"Updating state of {state.NumberOfSites} sites");
			for (int i = 0; i < state.NumberOfSites; i++)
			{
				var inputs = ReadLine().Split(' ');
				int siteId = int.Parse(inputs[0]);
				var site = state.GetSite(siteId);
				if(site == null) { throw new Exception($"No site found matching id {siteId}"); }
								
				site.Ignore1 = int.Parse(inputs[1]); // used in future leagues
				site.Ignore2 = int.Parse(inputs[2]); // used in future leagues
				site.StructureType = enumHelper.GetStructureType(int.Parse(inputs[3])); // -1 = No structure, 2 = Barracks
				site.Ownership = enumHelper.GetOwnership(int.Parse(inputs[4]));
				site.Param1 = int.Parse(inputs[5]);
				site.Param2 = int.Parse(inputs[6]);
			}
		}

		//Read a fresh list of units
		private IEnumerable<Unit> ReadUnits()
		{
			var enumHelper = new EnumHelper();
			int numUnits = int.Parse(ReadLine());
			Log($"Reading {numUnits} units");
			for (int i = 0; i < numUnits; i++)
			{
				var inputs = ReadLine().Split(' ');
				yield return new Unit()
				{
					X = int.Parse(inputs[0]),
					Y = int.Parse(inputs[1]),
					Ownership = enumHelper.GetOwnership(int.Parse(inputs[2])),
					UnitType = enumHelper.GetUnitType(int.Parse(inputs[3])),
					Health = int.Parse(inputs[4])
				};
			}
		}
	}
}