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

		public static string prefix = "gb!";

		List<TicTacToe> TTTGames = new List<TicTacToe>();

		public void Start()
		{
			_client.Log.Message += (s, e) => Console.WriteLine($"[{e.Severity}] {e.Source}: {e.Message}");

			_client.MessageReceived += async (s, e) =>
			{
				string cmd = e.Message.RawText.Split(' ')[0].Skip(prefix.Length).ToString();
				string[] par = e.Message.RawText.Split(' ').Skip(1).ToArray();
				if (cmd == "tictactoe")
				{
					string helpNew = $"Type `{prefix}tictactoe new <mention>` to invite someone to play Tic Tac Toe.";
					string helpPlay = $"Type `{prefix}tictactoe play <X> <Y>` to place a cross or a circle in a game.";
					if (par.Length == 1)
					{
						if (par[0] == "new")
							await e.Channel.SendMessage(helpNew);
						else if (par[0] == "play")
							await e.Channel.SendMessage(helpPlay);
						else
							await e.Channel.SendMessage($"{helpNew}\n{helpPlay}");
					}
					else if (par.Length == 2)
					{
						if (par[0] == "play")
							await e.Channel.SendMessage(helpPlay);
						else if (par[1] == "new")
						{
							User[] mentioned = e.Message.MentionedUsers.ToArray();
							if (mentioned.Length != 1)
								await e.Channel.SendMessage(helpNew);
							else
							{
								TTTGames.Add(new TicTacToe(e.User, mentioned[0], e.Channel));
							}
						}
						else
							await e.Channel.SendMessage($"{helpNew}\n{helpPlay}");

					}
					else if (par.Length == 3)
					{
						if (par[0] == "new")
							await e.Channel.SendMessage(helpNew);
						else if (par[1] == "play")
						{
							int i = TicTacToe.SearchPlayer(TTTGames.ToArray(), e.User);
							TTTGames[i].TakeTurn(e.User, int.Parse(par[2]), int.Parse(par[3]));
						}
						else
							await e.Channel.SendMessage($"{helpNew}\n{helpPlay}");
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