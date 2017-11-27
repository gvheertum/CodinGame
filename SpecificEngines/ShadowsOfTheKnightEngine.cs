using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Shared;
using Framework;

namespace SpecificEngines
{
	public class ShadowsOfTheKnightEngine : GameEngineBase, IGameEngine
	{
		private Position _playerPos;
		private Position _bombPos;
		private int _width;
		private int _height;
		private int _turns;
		public ShadowsOfTheKnightEngine(int width, int height, Position playerPos, Position bombPos, int turns)
		{
			_width = width;
			_height = height;
			_playerPos = playerPos.GetCopy();
			_bombPos = bombPos.GetCopy();
			_turns = turns;
		}

		public bool IsRunning()
		{
			if(_bombPos.X == _playerPos.X && _bombPos.Y == _playerPos.Y) 
			{ 
				Console.Error.Write("Congratulations, you found it!");
				return false; 
			}
			return _turns >= 0 ? true : throw new Exception("No more moves");
		}

		private bool _gaveDimensions = false;
		private bool _gaveTurns = false;
		private bool _gaveInitialPos = false;
		public string ReadLine()
		{
			//First 3 outputs are field data
			if(!_gaveDimensions) { _gaveDimensions = true; return $"{_width} {_height}"; }
			if(!_gaveTurns) { _gaveTurns = true; return _turns.ToString(); }
			if(!_gaveInitialPos) { _gaveInitialPos = true; return _playerPos.GetAsJumpPosition(); }
			
			Log($"Batman: {_playerPos}");
			Log($"Bomb: {_bombPos}");
			return GetDirectionOfBomb();
		}

		private string GetDirectionOfBomb()
		{
			string posStr = "";
			if(_bombPos.Y > _playerPos.Y) { posStr += "D"; }
			if(_bombPos.Y < _playerPos.Y) { posStr += "U"; }

			if(_bombPos.X > _playerPos.X) { posStr += "R"; }
			if(_bombPos.X < _playerPos.X) { posStr += "L"; }
			
			Log($"Direction: {posStr}");
			return posStr;
		}

		public void WriteLine(string resp)
		{
			if(_turns <= 0) { throw new Exception("no turns left!"); }
			
			var newPos = new Position(int.Parse(resp.Split(new [] { ' '})[0]), int.Parse(resp.Split(new [] { ' '})[1]));
			_playerPos = newPos;
			_turns--;
		}
	}
}