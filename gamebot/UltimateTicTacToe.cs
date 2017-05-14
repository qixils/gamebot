using System;
using Discord;

namespace gamebot
{
	public class UltimateTicTacToe //literally copypasted tic tac toe
	{
		public User cross;
		public User circle;
		public Channel channel;
		public bool?[,] game;

		public bool isCircleTurn;

		public int swidth, sheight, bwidth, bheight;

		public UltimateTicTacToe(User cross, User circle, Channel channel, int swidth = 3, int sheight = 3, int bwidth = 3, int bheight = 3)
		{
			// variables are updated to match game
			this.cross = cross;
			this.circle = circle;
			this.channel = channel;

			this.swidth = swidth; //width of the small grids
			this.bwidth = bwidth; //width of the big grid
			this.sheight = sheight; //height of the small grids
			this.bheight = bheight; //height of the big grid;

			game = new bool?[swidth * bwidth, sheight * bheight];
		}
		public bool? TakeTurn(User player, int x, int y)
		{
			x--;
			y--;

			bool? isCircle = null;
			if (circle == player)
				isCircle = true;
			else if (cross == player)
				isCircle = false;

			if (isCircle == isCircleTurn)
			{
				if (isCircle == true)
					game[x, y] = true;
				else if (isCircle == false)
					game[x, y] = false;
				isCircleTurn = !isCircleTurn;
				return true;
			}
			return true;
		}
		public string DrawGame()
		{
			int width = game.GetLength(0); //get the width of the game
			int height = game.GetLength(1); //get the height of the game
			string result = "▪️"; //create empty result with one space

			for (int i = 0; i < width; i++)
			{
				if (i < 10)
					result += (i + 1) + "⃣";
			}
			for (int i = 0; i < height; i++)
			{
				result += '\n'; //everytime it go to the next row in the game, creates a new line
				if (i < 10)
					result += (i + 1) + "⃣";
				for (int j = 0; j < width; j++) //loop in every column of the game
				{
					if (game[j, i] == null)
						result += "🔲"; //if it finds 'null' at [j,i] (x,y), it puts a space
					else if (game[j, i] == false)
						result += '❌'; //if it finds 'false' at [j,i] (x,y), it puts a cross
					else if (game[j, i] == true)
						result += "⭕️"; //if it finds 'true' at [j,i] (x,y), it puts a circle
				}
			}
			return result; //return the drawn string
						   //if a empty square is white, you can place your shape in it
		}
		public static int SearchPlayer(UltimateTicTacToe[] games, User player, Channel channel)
		{
			int r = -1; //if it couldn't find it
			for (int i = 0; i < games.Length; i++) // iterate through all variables in 'games' by initially setting int 'i' to 0, checking if it's less than the total number of games, then adding one
			{
				UltimateTicTacToe game = games[i];
				if (game.channel == channel) // checks if the game's channel is the same as the requested channel
				{
					if (game.cross == player || game.circle == player) // checks if command runner is player 1 or 2 in any active game
					{
						r = i; // sets 'r' as 'i'
						break; // ends 'for' loop
					}
				}
			}
			return r; // returns 'r' (the index in the array) back to initiating code
		}
	}
}
