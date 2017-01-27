using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Discord;

namespace gamebot
{
	class gamebot
	{
		static void Main(string[] args) => new gamebot().Start();

		private static DiscordClient _client = new DiscordClient();

		public static string prefix = "gb!"; // Sets custom bot prefix

		List<TicTacToe> TTTGames = new List<TicTacToe>();

		public void Start()
		{
			_client.Log.Message += (s, e) => Console.WriteLine($"[{e.Severity}] {e.Source}: {e.Message}");

			_client.MessageReceived += async (s, e) =>
			{
				string cmd = new string(e.Message.RawText.Split(' ')[0].Skip(prefix.Length).ToArray()); // Grabs the command used by removing the prefix
				string[] par = e.Message.RawText.Split(' ').Skip(1).ToArray(); // Grabs the arguments used in the command
				if (cmd == "tictactoe") // tictactoe command code
				{
					string helpNew = $"Type `{prefix}tictactoe new <mention>` to invite someone to play Tic Tac Toe.";
					string helpPlay = $"Type `{prefix}tictactoe play <X> <Y>` to place a cross or a circle in a game.";
					if (par.Length == 1) // checks if only one command argument was supplied
					{
						if (par[0] == "new")
							await e.Channel.SendMessage(helpNew); // outputs the 'helpNew' string if the argument was 'new'
						else if (par[0] == "play")
							await e.Channel.SendMessage(helpPlay); // same as above comment, but with 'helpPlay' string and 'play' arg
						else
							await e.Channel.SendMessage($"{helpNew}\n{helpPlay}");
					}
					else if (par.Length == 2 || par.Length == 4) // checks if two arguments were supplied
					{
						if (par[0] == "play")
							await e.Channel.SendMessage(helpPlay); // too few requirements were supplied so help is shown
						else if (par[0] == "new")
						{
							int i = TicTacToe.SearchPlayer(TTTGames.ToArray(), e.User, e.Channel);
							if (i == -1)
							{
								User[] mentioned = e.Message.MentionedUsers.ToArray();
								if (mentioned.Length != 1)
									await e.Channel.SendMessage(helpNew); // too few (or many) users were mentioned, help is shown
								else
								{
									if (par.Length == 2)
										TTTGames.Add(new TicTacToe(e.User, mentioned[0], e.Channel)); // a new TTT game is added to 'TTTGames' with the command runner, opponent, and channel
									else
										TTTGames.Add(new TicTacToe(e.User, mentioned[0], e.Channel, int.Parse(par[2]), int.Parse(par[3]))); // a new TTT game is added to 'TTTGames' with the command runner, opponent, and channel
									await e.Channel.SendMessage("A new game has started!");
								}
							}
							else
								await e.Channel.SendMessage("You are already in a game in this channel."); //the user cannot play two game in a channel
						}
						else
							await e.Channel.SendMessage($"{helpNew}\n{helpPlay}"); // send default help message if no valid commands were detected

					}
					else if (par.Length == 3)
					{
						if (par[0] == "new")
							await e.Channel.SendMessage(helpNew); // too many requirements were supplied so help is shown
						else if (par[0] == "play")
						{
							int i = TicTacToe.SearchPlayer(TTTGames.ToArray(), e.User, e.Channel);
							if (i != -1) //checks if it actually finds a player
							{
								bool? isc = TTTGames[i].TakeTurn(e.User, int.Parse(par[1]), int.Parse(par[2])); //check the turn
								if (isc == true) //if the turn was successful
								{
									await e.Channel.SendMessage("```\n" + TTTGames[i].DrawGame() + "```"); //write down the game
									var c = TTTGames[i].CheckGame(); //check if someone wins
									if (c == TicTacToe.GameStat.CircleWin || c == TicTacToe.GameStat.CrossWin) //if someone wins
									{
										await e.Channel.SendMessage($"Congratulation, <@{e.User.Id}>, you won!");
										TTTGames.RemoveAt(i); //delete the game
									}
									else if (c == TicTacToe.GameStat.Tie) //if there is a tie
									{
										await e.Channel.SendMessage("You are both stuck, there is a tie. The game has ended.");
										TTTGames.RemoveAt(i); //delete the game
									}
									//otherwise well the game continues
								}
								else if(isc == false)
									await e.Channel.SendMessage("It's not your turn."); //the user cannot play if it's not his turn
								else
									await e.Channel.SendMessage("You can't place a shape onto another shape."); //the user cannot cheat by replacing a shape
							}
							else
								await e.Channel.SendMessage("You are currently not in a game in this channel."); //the user cannot play if he's not playing
						}
						else
							await e.Channel.SendMessage($"{helpNew}\n{helpPlay}"); // invalid arguments given, help displayed
					}
					else
						await e.Channel.SendMessage($"{helpNew}\n{helpPlay}");
				}
			};

			string token = File.ReadAllText("bot-token.txt");
			_client.ExecuteAndWait(async () =>
			{
				await _client.Connect(token, TokenType.Bot);
			});

		}
	}
}