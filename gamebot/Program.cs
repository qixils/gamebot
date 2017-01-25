using System;
using System.Collections;
using System.IO;
using System.Linq;
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
				string cmd = e.Message.RawText.Split(' ')[0].Skip(prefix.Length).ToString();
				string[] par = e.Message.RawText.Split(' ').Skip(1).ToArray();
				if (cmd == "tictactoe")
				{
					if (tttProgress == 0)
					{
						tttProgress++;
						await e.Channel.SendMessage("Initializing game...");
					}
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