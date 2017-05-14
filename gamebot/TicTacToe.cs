using Discord;
using Discord.WebSocket;

using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace gamebot
{
    public class TicTacToe
    {
        public SocketUser cross;
        public SocketUser circle;
        public SocketChannel channel;
        public bool?[,] game;
        public DateTime start_time;
        public DateTime end_time;

        public bool isCircleTurn;

        public TicTacToe(SocketUser cross, SocketUser circle, SocketChannel channel, int width = 3, int height = 3)
        {
            this.start_time = DateTime.Now;
            // variables are updated to match game
            this.cross = cross;
            this.circle = circle;
            this.channel = channel;

            game = new bool?[width, height];
        }
        public int? TakeTurn(SocketUser player, int x, int y)
        {
            x--;
            y--;

            if (x < 0 | x >= game.GetLength(0) | y < 0 | y >= game.GetLength(1))
                return 2;

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
                    return 0;
                }
            }
            else
                return -1;
            return 1;
        }
        public string DrawGame()
        {
            int width = game.GetLength(0); //get the width of the game
            int height = game.GetLength(1); //get the height of the game
            string result = "▪️"; //create empty result with one space

            for (int i = 0; i < width; i++)
            {
                result += (i + 1) + "⃣";    //write 1 to whatever the width is
            }                   //example: width is 4, it writes 1234

            for (int i = 0; i < height; i++)
            {
                result += '\n'; //everytime it go to the next row in the game, creates a new line
                result += "🇦 🇧 🇨 🇩 🇪 🇫 🇬 🇭 🇮".Split(' ')[i];
                for (int j = 0; j < width; j++) //loop in every column of the game
                {
                    if (game[j, i] == null)
                        result += "▪️";              //if it finds 'null' at [j,i] (x,y), it puts a space
                    else if (game[j, i] == false)
                        result += "❌";              //if it finds 'false' at [j,i] (x,y), it puts a cross
                    else if (game[j, i] == true)
                        result += "⭕️";              //if it finds 'true' at [j,i] (x,y), it puts a circle
                }
            }

            return result; //return the drawn string
                           /* should look like this:
                            *  123
                            * 1X O
                            * 2 XX
                            * 3OOX */
        }
        public GameStat CheckGame(int player_x, int player_y)
        {
            // define some variables
            int width = game.GetLength(0); //get the width of the game
            int height = game.GetLength(1); //get the height of the game

            bool game_tie = true;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (game[x, y] == null)
                    {
                        game_tie = false;
                    }
                }
            }
            if (game_tie)
            {
                this.end_time = DateTime.Now;
                return GameStat.Tie;
            }

            player_x--;
            player_y--;

            // Check L to R
            if (player_x < width - 2) // If there's space..
            {
                if (game[player_x, player_y] == true & game[player_x + 1, player_y] == true & game[player_x + 2, player_y] == true)
                {
                    return GameStat.CircleWin;
                }
                if (game[player_x, player_y] == false & game[player_x + 1, player_y] == false & game[player_x + 2, player_y] == false)
                {
                    return GameStat.CrossWin;
                }
            }

            // Check R to L
            if (player_x > 1) // If there's space..
            {
                if (game[player_x, player_y] == true & game[player_x - 1, player_y] == true & game[player_x - 2, player_y] == true)
                {
                    return GameStat.CircleWin;
                }
                if (game[player_x, player_y] == false & game[player_x - 1, player_y] == false & game[player_x - 2, player_y] == false)
                {
                    return GameStat.CrossWin;
                }
            }

            // Check U to D
            if (player_y > 1) // If there's space..
            {
                if (game[player_x, player_y] == true & game[player_x, player_y - 1] == true & game[player_x, player_y - 2] == true)
                {
                    return GameStat.CircleWin;
                }
                if (game[player_x, player_y] == false & game[player_x, player_y - 1] == false & game[player_x, player_y - 2] == false)
                {
                    return GameStat.CrossWin;
                }
            }

            // Check D to U
            if (player_y < height - 2) // If there's space..
            {
                if (game[player_x, player_y] == true & game[player_x, player_y + 1] == true & game[player_x, player_y + 2] == true)
                {
                    return GameStat.CircleWin;
                }
                if (game[player_x, player_y] == false & game[player_x, player_y + 1] == false & game[player_x, player_y + 2] == false)
                {
                    return GameStat.CrossWin;
                }
            }

            // Check POS to UR
            if (player_y < height - 2 & player_x < width - 2) // If there's space..
            {
                if (game[player_x, player_y] == true & game[player_x + 1, player_y + 1] == true & game[player_x + 2, player_y + 2] == true)
                {
                    return GameStat.CircleWin;
                }
                if (game[player_x, player_y] == false & game[player_x + 1, player_y + 1] == false & game[player_x + 2, player_y + 2] == false)
                {
                    return GameStat.CrossWin;
                }
            }

            // Check POS to DL
            if (player_y > 1 & player_x > 2) // If there's space..
            {
                if (game[player_x, player_y] == true & game[player_x - 1, player_y - 1] == true & game[player_x - 2, player_y - 2] == true)
                {
                    return GameStat.CircleWin;
                }
                if (game[player_x, player_y] == false & game[player_x - 1, player_y - 1] == false & game[player_x - 2, player_y - 2] == false)
                {
                    return GameStat.CrossWin;
                }
            }

            // Check POS to DR
            if (player_y > 1 & player_x < width -2 ) // If there's space..
            {
                if (game[player_x, player_y] == true & game[player_x + 1, player_y - 1] == true & game[player_x + 2, player_y - 2] == true)
                {
                    return GameStat.CircleWin;
                }
                if (game[player_x, player_y] == false & game[player_x + 1, player_y - 1] == false & game[player_x + 2, player_y - 2] == false)
                {
                    return GameStat.CrossWin;
                }
            }

            // Check POS to UL
            if (player_x > 1 & player_y < height - 2) // If there's space..
            {
                if (game[player_x, player_y] == true & game[player_x - 1, player_y + 1] == true & game[player_x - 2, player_y + 2] == true)
                {
                    return GameStat.CircleWin;
                }
                if (game[player_x, player_y] == false & game[player_x - 1, player_y + 1] == false & game[player_x - 2, player_y + 2] == false)
                {
                    return GameStat.CrossWin;
                }
            }

            return GameStat.Unfinished; // tell code the game is going on
        }
        public static int SearchPlayer(TicTacToe[] games, SocketUser player, SocketChannel channel)
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
        public static TicTacToe ToClass(JSON.TicTacToeStruct t, DiscordSocketClient client)
        {
            /*Console.WriteLine(t.Cross);
			Console.WriteLine(t.Circle);
			Console.WriteLine(t.Channel);*/

            //Console.WriteLine("d");
            SocketChannel c = client.GetChannel(t.Channel);
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