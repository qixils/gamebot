using System;
using Discord;

namespace gamebot
{
	public class TicTacToe
	{
		public User player1;
		public User player2;
		Channel channel;
		bool?[,] game;

		bool isPlayer2Turn;

		public TicTacToe(User player1, User player2, Channel channel)
		{
			this.player1 = player1;
			this.player2 = player2;
			this.channel = channel;
		}
		public void TakeTurn(User player, int x, int y)
		{
			throw new NotImplementedException();
		}
		public string DrawGame()
		{
			throw new NotImplementedException();
		}
		public static int SearchPlayer(TicTacToe[] games, User player)
		{
			int r = -1;
			for (int i = 0; i < games.Length; i++)
			{
				TicTacToe game = games[i];
				if (game.player1 == player || game.player2 == player)
				{
					r = i;
					break;
				}
			}
			return r;
		}
	}
}
