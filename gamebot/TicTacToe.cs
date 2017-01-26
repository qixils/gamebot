using System;
using Discord;

namespace gamebot
{
	public class TicTacToe
	{
		public User cross;
		public User circle;
		Channel channel;
		bool?[,] game;

		bool isCircleTurn;

		public TicTacToe(User cross, User circle, Channel channel, int width = 3, int height = 3)
		{
			// variables are updated to match game
			this.cross = cross;
			this.circle = circle;
			this.channel = channel;

			game = new bool?[width,height];
		}
		public bool TakeTurn(User player, int x, int y)
		{
			x--;
			y--;

			bool? isCircle = null; //if the player isn't part of the game, it will always be null
			if (circle == player)
				isCircle = true;  //set to true if the player is circle
			else if (cross == player)
				isCircle = false; //set to false if the player is cross

			if (isCircle == isCircleTurn) //check if it's the player's turn
			{
				if (isCircle == true)
					game[x, y] = true; //set the coordinate x y to a circle
				else if (isCircle == false)
					game[x, y] = false; //set the coordinate x y to a cross

				isCircleTurn = !isCircleTurn;
				return true;
			}	//the returns here is for checking if the operation is successful
			return false;
		}
		public string DrawGame()
		{
			int width = game.GetLength(0); //get the width of the game
			int height = game.GetLength(1); //get the height of the game
			string result = " "; //create empty result with one space

			for (int i = 0; i < width; i++)
			{
				result += i + 1;	//write 1 to whatever the width is
			}					//example: width is 4, it writes 1234
			for (int i = 0; i < height; i++)
			{
				result += '\n';	//everytime it go to the next row in the game, creates a new line
				result += i + 1;	//it also enters the current height line
				for (int j = 0; j < width; j++) //loop in every column of the game
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
		public GameStat CheckGame()
		{
			// define some variables
			var firstRow = game.GetValue(0);
			var secondRow = game.GetValue(1);
			var thirdTow = game.GetValue(2);
			bool gameTie = true;
			bool circleWin = false;
			bool crossWin = false;

			// tie check
			foreach (var row in game)
				foreach (bool? tile in row)
					if (tile = null)
						gameTie = false;

			if (circleWin)
				return GameStat.CircleWin;
			else if (crossWin)
				return GameStat.CrossWin;
			else if (gameTie)
				return GameStat.Tie;
		}
		public static int SearchPlayer(TicTacToe[] games, User player, Channel channel)
		{
			int r = -1; //if it couldn't find it
			for (int i = 0; i < games.Length; i++) // iterate through all variables in 'games' by initially setting int 'i' to 0, checking if it's less than the total number of games, then adding one
			{
				TicTacToe game = games[i];
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
		public enum GameStat //used for CheckGame()
		{
			Unfinished, //for when someone can still wins
			CrossWin,	//when cross wins
			CircleWin,	//when circle wins
			Tie			//when none wins, and cannot wins by placing more crosses or circles
		}
	}
}
