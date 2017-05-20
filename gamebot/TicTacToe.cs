using System;
using Discord;

namespace gamebot
{
	public class TicTacToe
	{
		public User cross;
		public User circle;
		public Channel channel;
		public bool?[,] game;

		public bool isCircleTurn;

		public TicTacToe(User cross, User circle, Channel channel, int width = 3, int height = 3)
		{
			// variables are updated to match game
			this.cross = cross;
			this.circle = circle;
			this.channel = channel;

			game = new bool?[width, height];
		}
		public bool? TakeTurn(User player, int x, int y)
		{
			x--;
			y--;

			if (game[x, y] == null) //check the coordinate to see if it's empty
			{
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
				}
			}
			else
				return null;
			return false;
		}
		public string DrawGame()
		{
			int width = game.GetLength(0); //get the width of the game
			int height = game.GetLength(1); //get the height of the game
			string result = "▪️"; //create empty result with one space

			for (int i = 0; i < width; i++)
			{
				result += (i + 1) + "⃣"; //write 1 to whatever the width is
										 //example: width is 4, it writes 1234
			}
			for (int i = 0; i < height; i++)
			{
				result += '\n'; //everytime it go to the next row in the game, creates a new line
				result += (i + 1) + "⃣";    //it also enters the current height line
				for (int j = 0; j < width; j++) //loop in every column of the game
				{
					if (game[j, i] == null)
						result += '▪';              //if it finds 'null' at [j,i] (x,y), it puts a space
					else if (game[j, i] == false)
						result += '❌';            //if it finds 'false' at [j,i] (x,y), it puts a cross
					else if (game[j, i] == true)
						result += "⭕️";            //if it finds 'true' at [j,i] (x,y), it puts a circle
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
			int width = game.GetLength(0); //get the width of the game
			int height = game.GetLength(1); //get the height of the game
			bool gameTie = true;
			bool circleWin = false;
			bool crossWin = false;

			for (int i = 0; i < game.GetLength(0); i++)
			{
				// gets every row in 'game'
				// defines temporary variables
				int crossPointsC = 0;   //collumn points
				int circlePointsC = 0;
				int crossPointsR = 0;   //row points
				int circlePointsR = 0;
				int crossPointsDR = 0;  //down-right points
				int circlePointsDR = 0;

				for (int j = 0; j < game.GetLength(1); j++)
				{
					var tileC = game[i, j]; //collumn points
					var tileR = game[j, i]; //row points

					if (width == height)
					{
						if ((i == j) && (tileC == false))
						{
							crossPointsDR++;
							circlePointsDR = 0;
						}
						else if ((i == j) && (tileC == true))
						{
							circlePointsDR++;
							crossPointsDR = 0;
						}
					}

					if (tileC == false) //collumn check
					{
						crossPointsC++; // increments cross points by one
						circlePointsC = 0; // sets circle points to zero
					}

					else if (tileC == true)
					{
						circlePointsC++; // increments circle points
						crossPointsC = 0; // sets cross points to zero
					}

					else if (tileC == null)
						gameTie = false; // sets game tie to false if a null tile is detected, which means game is unfinished

					if (tileR == false) //row check
					{
						crossPointsR++; // increments cross points by one
						circlePointsR = 0; // sets circle points to zero
					}

					else if (tileR == true)
					{
						circlePointsR++; // increments circle points
						crossPointsR = 0; // sets cross points to zero
					}

					else if (tileR == null)
						gameTie = false; // sets game tie to false if a null tile is detected, which means game is unfinished
				}

				if (crossPointsC == height || crossPointsR == width || crossPointsDR == height) // checks if crossPoints are the same as height
					crossWin = true; // sets crossWin to true
				else if (circlePointsC == height || circlePointsR == width || circlePointsDR == height) // similar to above
					circleWin = true; // still similar to above
			}
			if (width == height)
			{
				int crossPoints = 0;  //down-right points
				int circlePoints = 0;
				for (int i = 0; i < game.GetLength(0); i++)
				{
					for (int j = game.GetLength(1) - 1; j >= 0; j--)
					{
						//Console.WriteLine(i + "," + j);
						var tile = game[i, j];

						if ((i == ((j * -1) + (game.GetLength(0) - 1))) && (tile == false))
						{
							crossPoints++;
							circlePoints = 0;
						}
						else if ((i == ((j * -1) + (game.GetLength(0) - 1))) && (tile == true))
						{
							circlePoints++;
							crossPoints = 0;
						}
					}
				}


				if (crossPoints == height) // checks if crossPoints are the same as height
					crossWin = true; // sets crossWin to true
				else if (circlePoints == height) // similar to above
					circleWin = true; // still similar to above
			}

			if (circleWin)
				return GameStat.CircleWin; // tell code circle won
			else if (crossWin)
				return GameStat.CrossWin; // tell code cross won
			else if (gameTie)
				return GameStat.Tie; // tell code nobody won
			else
				return GameStat.Unfinished; // tell code the game is going on
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
			CrossWin,   //when cross wins
			CircleWin,  //when circle wins
			Tie         //when none wins, and cannot wins by placing more crosses or circles
		}
		public JSON.TicTacToeStruct ToStruct()
		{
			JSON.TicTacToeStruct r = new JSON.TicTacToeStruct();
			r.Cross = cross.Id;
			r.Circle = circle.Id;
			r.Channel = channel.Id;
			r.Game = game;
			r.IsCircleTurn = isCircleTurn;
			return r;
		}
		public static JSON.TicTacToeStruct ToStruct(TicTacToe t)
		{
			return t.ToStruct();
		}
		public static TicTacToe ToClass(JSON.TicTacToeStruct t, DiscordClient client)
		{
			/*Console.WriteLine(t.Cross);
            Console.WriteLine(t.Circle);
            Console.WriteLine(t.Channel);*/

			//Console.WriteLine("d");
			Channel c = client.GetChannel(t.Channel);
			/*if (c != null)
                Console.WriteLine(c.Id);
            else
                Console.WriteLine("is null");*/
			TicTacToe ttt = new TicTacToe(c.GetUser(t.Cross), c.GetUser(t.Circle), c);
			//Console.WriteLine("f");
			ttt.game = t.Game;
			ttt.isCircleTurn = t.IsCircleTurn;
			//Console.WriteLine("g");
			return ttt;
		}
	}
}
