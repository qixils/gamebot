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
