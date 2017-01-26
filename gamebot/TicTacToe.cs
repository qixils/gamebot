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
			// variables are updated to match game
			this.player1 = player1;
			this.player2 = player2;
			this.channel = channel;
		}
		public void TakeTurn(User player, int x, int y)
		{
			throw new NotImplementedException(); // throw error as function isn't implemented yet
		}
		public string DrawGame()
		{
			int width = game.GetLength(0);
			int height = game.GetLength(1);
			string result = " ";

			for (int i = 1; i <= width; i++)
			{
				result += i;
			}
			for (int i = 1; i <= height; i++)
			{
				result += '\n';
				result += i;
				for (int j = 1; j <= width; j++)
				{
					if (game[j, i] == null)
						result += ' ';
					else if (game[j, i] == false)
						result += 'X';
					else if (game[j, i] == true)
						result += 'O';
				}
			}

			return result;
		}
		public static int SearchPlayer(TicTacToe[] games, User player)
		{
			int r = -1;
			for (int i = 0; i < games.Length; i++) // iterate through all variables in 'games' by initially setting int 'i' to 0, checking if it's less than the total number of games, then adding one
			{
				TicTacToe game = games[i];
				if (game.player1 == player || game.player2 == player) // checks if command runner is player 1 or 2 in any active game
				{
					r = i; // sets 'r' as 'i'
					break; // ends 'for' loop
				}
			}
			return r; // returns 'r' (the game ID) back to initiating code
		}
	}
}
