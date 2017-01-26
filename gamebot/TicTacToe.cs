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
			int width = game.GetLength(0); //get the width of the game
			int height = game.GetLength(1); //get the height of the game
			string result = " "; //create empty result with one space

			for (int i = 1; i <= width; i++)
			{
				result += i;	//write 1 to whatever the width is
			}					//example: width is 4, it writes 1234
			for (int i = 1; i <= height; i++)
			{
				result += '\n';	//everytime it go to the next row in the game, creates a new line
				result += i;	//it also enters the current height line
				for (int j = 1; j <= width; j++) //loop in every column of the game
				{
					if (game[j, i] == null)
						result += ' ';				//if it finds 'null' at [j,i] (x,y), it puts a space
					else if (game[j, i] == false)
						result += 'X';              //if it finds 'false' at [j,i] (x,y), it puts a cross
					else if (game[j, i] == true)
						result += 'O';              //if it finds 'true' at [j,i] (x,y), it puts a circle
				}
			}

			return result; //return the drawn string
			/* should look like this:
			 *  123
			 * 1X O
			 * 2 XX
			 * 3OOX */
		}
		public static int SearchPlayer(TicTacToe[] games, User player)
		{
			int r = -1; //if it couldn't find it
			for (int i = 0; i < games.Length; i++) // iterate through all variables in 'games' by initially setting int 'i' to 0, checking if it's less than the total number of games, then adding one
			{
				TicTacToe game = games[i];
				if (game.player1 == player || game.player2 == player) // checks if command runner is player 1 or 2 in any active game
				{
					r = i; // sets 'r' as 'i'
					break; // ends 'for' loop
				}
			}
			return r; // returns 'r' (the index in the array) back to initiating code
		}
	}
}
