using System;
namespace gamebot.JSON
{
	public struct TicTacToe
	{
		public ulong Cross;
		public ulong Circle;
		public ulong Channel;
		public bool?[,] Game;

		public bool IsCircleTurn;
	}
}
