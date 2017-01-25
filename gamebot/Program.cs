using System;
using System.IO;
using Discord;
using Discord.Commands;

namespace gamebot
{
	class gamebot
	{
		static void Main(string[] args) => new gamebot().Start();

		private static DiscordClient _client = new DiscordClient();

		public static string prefix = "gb!";
		public static int tttProgress = 0;

		public void Start()
		{
			_client.Log.Message += (s, e) => Console.WriteLine($"[{e.Severity}] {e.Source}: {e.Message}");

			_client.MessageReceived += async (s, e) =>
			{
				if (e.Message.Text == prefix + "tictactoe")
				{
					if (tttProgress = 0)
					{
						tttProgress++;
						e.Channel.SendMessage("Initializing game...");
					}
				}
			};

			string token = File.ReadAllText("token.txt");
			_client.ExecuteAndWait(async () => {
				await _client.Connect(token, TokenType.Bot);
			});

		}
	}
}
