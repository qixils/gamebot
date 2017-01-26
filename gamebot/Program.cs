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
				string cmd = e.Message.RawText.Split(' ')[0].Skip(prefix.Length).ToString(); // Grabs the command used by removing the prefix
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
					else if (par.Length == 2) // checks if two arguments were supplied
					{
						if (par[0] == "play")
							await e.Channel.SendMessage(helpPlay); // too few requirements were supplied so help is shown
						else if (par[0] == "new")
						{
							User[] mentioned = e.Message.MentionedUsers.ToArray();
							if (mentioned.Length != 1)
								await e.Channel.SendMessage(helpNew); // too few (or many) users were mentioned, help is shown
							else
							{
								TTTGames.Add(new TicTacToe(e.User, mentioned[0], e.Channel)); // a new TTT game is added to 'TTTGames' with the command runner, opponent, and channel
							}
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
							int i = TicTacToe.SearchPlayer(TTTGames.ToArray(), e.User);
							TTTGames[i].TakeTurn(e.User, int.Parse(par[2]), int.Parse(par[3]));
						}
						else
							await e.Channel.SendMessage($"{helpNew}\n{helpPlay}"); // invalid arguments given, help displayed
					}
					else
						await e.Channel.SendMessage($"{helpNew}\n{helpPlay}");
				}
			};

			string token = File.ReadAllText("token.txt");
			_client.ExecuteAndWait(async () =>
			{
				await _client.Connect(token, TokenType.Bot);
			});

		}
	}
}