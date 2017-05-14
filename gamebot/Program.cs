using Discord;
using Discord.WebSocket;

using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;


namespace gamebot
{
    class gamebot
    {
        private static DiscordSocketClient _client = new DiscordSocketClient();

        public List<PastGame> past_games = new List<PastGame>();

        public static string prefix = "g!"; // Sets custom bot prefix
        List<TicTacToe> TTTGames = new List<TicTacToe>();

        public static void Main(string[] args)
            => new gamebot().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            _client.Log += Log;
            _client.MessageReceived += MessageReceived;
            _client.Ready += Ready;

            string token = File.ReadAllText("bot-token.txt");
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Task Log(LogMessage e)
        {
            Console.WriteLine($"[{e.Severity}] {e.Source}: {e.Message}");

            return Task.CompletedTask;
        }

        private void SaveState()
        {
            List<JSON.TicTacToeStruct> ttts = new List<JSON.TicTacToeStruct>();
            foreach (TicTacToe t in TTTGames)
            {
                ttts.Add(t.ToStruct());
            }
            Save.Saves(ttts.ToArray(), "ttt.json");

            List<JSON.PastGame> games = new List<JSON.PastGame>();
            foreach (PastGame pg in past_games)
            {
                games.Add(pg.ToStruct());
            }
            Save.Saves(games.ToArray(), "pastgames.json");

            // Console.WriteLine("[Debug] TicTacToe: Saved!");
        }

        private async Task MessageReceived(SocketMessage args)
        {
            var e = args as SocketUserMessage;
            if (e == null) return;

            try
            {
                if (!e.Author.IsBot)
                {
                    string msg = e.Content;
                    string rawcmd = "no-cmd"; // Filler command
                    if (msg.StartsWith(prefix)) // Check if message starts with prefix
                        rawcmd = msg.Replace(prefix, ""); // Set rawcmd to full command (cmd + arguments)
                    string cmd = rawcmd.Split(' ')[0]; // Grab just the command

                    string[] par = msg.Split(' ').Skip(1).ToArray(); // Grabs the arguments used in the command

                    // string parContents = null;
                    // foreach (string arg in par)
                    //     parContents += arg + " ";
                    // Console.WriteLine(parContents);

                    if (cmd == "help") // help command code
                    {
                        string greet = "Heya! I'm gamebot, a bot for automating various games. Here's a list of games/commands you can use:";
                        string help = "`g!help` - displays info about the bot & bot commands";
                        string info = "`g!info` - displays extra info about the bot";
                        string ttt = "`g!ttt` - displays help about tic-tac-toe";

                        await e.Channel.SendMessageAsync($"{greet}\n\n{help}\n{info}\n{ttt}");
                    }
                    else if (cmd == "info") // info command
                    {
                        string contributors = "`Noahkiq` and `Technochips`";
                        string message = $"Heya! I'm gamebot, a bot for automating various games. I've been created by {contributors}. You can report issues, make suggestions, or examine my source code at <https://github.com/Noahkiq/gamebot/>.";
                        await e.Channel.SendMessageAsync(message);
                    }
                    else if (cmd == "credits") // credits command
                    {
                        string message = $"Main code: `Noahkiq` and `Technochips`. Some extra changes: `Bottersnike`. Hangman english words: `SIL International`.";
                        await e.Channel.SendMessageAsync(message);
                    }
                    else if (cmd == "ttt") // tictactoe command code
                    {
                        string helpNew = $"Type `{prefix}{cmd} new <mention>` to invite someone to play Tic Tac Toe.";
                        string helpPlay = $"Type `{prefix}{cmd} play <X> <Y>` or `{prefix}{cmd} play <XY>` to place a cross or a circle in a game.";
                        string helpCancel = $"Type `{prefix}{cmd} cancel` to cancel your current game in this channel.";
                        if (par.Length == 1) // checks if only one command argument was supplied
                        {
                            if (par[0] == "new")
                                await e.Channel.SendMessageAsync(helpNew); // outputs the 'helpNew' string if the argument was 'new'
                            else if (par[0] == "play")
                                await e.Channel.SendMessageAsync(helpPlay); // same as above comment, but with 'helpPlay' string and 'play' arg
                            else if (par[0] == "scoreboard" | par[0] == "scores")
                            {
                                foreach (PastGame j in past_games)
                                {
                                    Console.WriteLine(j.ToString());
                                }
                            }
                            else if (par[0] == "show")
                            {
                                int i = TicTacToe.SearchPlayer(TTTGames.ToArray(), e.Author, e.Channel as SocketChannel);
                                if (i != -1) //checks if it actually finds a player
                                {
                                    await e.Channel.SendMessageAsync(TTTGames[i].DrawGame());
                                } else
                                {
                                    await e.Channel.SendMessageAsync("You are not currently in a game in this channel.");
                                }
                            }
                            else if (par[0] == "cancel")
                            {
                                int i = TicTacToe.SearchPlayer(TTTGames.ToArray(), e.Author, e.Channel as SocketChannel); // searches for a game with command runner and channel
                                if (i != -1) //checks if it actually finds a player
                                {
                                    TTTGames.RemoveAt(i); // deletes game at 'i', which will be the current game if found
                                    await e.Channel.SendMessageAsync($"The game has successfully been cancelled.");
                                    SaveState();
                                }
                                else
                                    await e.Channel.SendMessageAsync($"You are currently not in a game in this channel.");
                            }
                            else if (par[0] == "save")
                            {
                                SaveState();
                                await e.Channel.SendMessageAsync("Saved!");
                            }
                            else
                                await e.Channel.SendMessageAsync($"{helpNew}\n{helpPlay}\n{helpCancel}");
                        }
                        else if (par.Length == 2 || par.Length == 4) // checks if two or four arguments were supplied
                        {
                            if (par[0] == "play")
                                if (par[1].Length == 2)
                                {
                                    int i = TicTacToe.SearchPlayer(TTTGames.ToArray(), e.Author, e.Channel as SocketChannel);
                                    if (i != -1) //checks if it actually finds a player
                                    {
                                        int x;
                                        bool isNumeric = int.TryParse(par[1][0].ToString(), out x);

                                        if (!isNumeric)
                                        {
                                            await e.Channel.SendMessageAsync("Invalid X co-ordinate.");
                                            return;
                                        }

                                        int y = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(par[1][1].ToString()) + 1;
                                        int? isc = TTTGames[i].TakeTurn(e.Author, x, y); //check the turn
                                        if (isc == 0) //if the turn was successful
                                        {
                                            await e.Channel.SendMessageAsync(TTTGames[i].DrawGame()); //write down the game
                                            var c = TTTGames[i].CheckGame(); //check if someone wins
                                            if (c == TicTacToe.GameStat.CircleWin || c == TicTacToe.GameStat.CrossWin) //if someone wins
                                            {
                                                await e.Channel.SendMessageAsync($"Congratulation, <@{e.Author.Id}>, you won!");
                                                past_games.Add(new PastGame(TTTGames[i].cross, TTTGames[i].circle, TTTGames[i].channel, TTTGames[i].start_time, TTTGames[i].end_time, c));
                                                TTTGames.RemoveAt(i); //delete the game
                                            }
                                            else if (c == TicTacToe.GameStat.Tie) //if there is a tie
                                            {
                                                await e.Channel.SendMessageAsync("You are both stuck, there is a tie. The game has ended.");
                                                past_games.Add(new PastGame(TTTGames[i].cross, TTTGames[i].circle, TTTGames[i].channel, TTTGames[i].start_time, TTTGames[i].end_time, c));
                                                TTTGames.RemoveAt(i); //delete the game
                                            }
                                            //otherwise well the game continues
                                        }
                                        else if (isc == 1)
                                            await e.Channel.SendMessageAsync("It's not your turn."); //the user cannot play if it's not his turn
                                        else if (isc == 2)
                                            await e.Channel.SendMessageAsync("Co-ordinates off grid."); //Invalid co-ords
                                        else
                                            await e.Channel.SendMessageAsync("You can't place a shape onto another shape."); //the user cannot cheat by replacing a shape
                                    }
                                    else
                                        await e.Channel.SendMessageAsync("You are currently not in a game in this channel."); //the user cannot play if he's not playing
                                    SaveState();
                                }
                                else
                                    await e.Channel.SendMessageAsync(helpPlay); // too few requirements were supplied so help is shown
                            else if (par[0] == "cancel")
                                await e.Channel.SendMessageAsync(helpCancel);
                            else if (par[0] == "new")
                            {
                                SocketUser[] mentioned = e.MentionedUsers.ToArray();
                                if (mentioned.Length != 1 || mentioned[0] == null)
                                    await e.Channel.SendMessageAsync(helpNew); // too few (or many) users were mentioned, help is show
                                else
                                {
                                    var i = TicTacToe.SearchPlayer(TTTGames.ToArray(), e.Author, e.Channel as SocketChannel); //search the user
                                    var j = TicTacToe.SearchPlayer(TTTGames.ToArray(), mentioned[0], e.Channel as SocketChannel); //search the mentioned user
                                    if (i == -1 && j == -1) //if it doesn't find anything
                                    {
                                        if (mentioned[0].IsBot)
                                            await e.Channel.SendMessageAsync($"You cannot play against another bot!");
                                        else if (mentioned[0].Status == UserStatus.Offline)
                                            await e.Channel.SendMessageAsync($"You cannot play against an offline/invisible user!");
                                        else if (mentioned[0] == e.Author)
                                            await e.Channel.SendMessageAsync($"You cannot play a game with yourself!");
                                        else
                                        {
                                            if (par.Length == 2)
                                            {
                                                TTTGames.Add(new TicTacToe(e.Author, mentioned[0], e.Channel as SocketChannel)); // a new TTT game is added to 'TTTGames' with the command runner, opponent, and channel
                                                await e.Channel.SendMessageAsync("A new game has started!");
                                            }
                                            else if (par.Length == 4)
                                            {
                                                bool validInts = true;

                                                try
                                                {
                                                    int.Parse(par[2]);
                                                    int.Parse(par[3]);
                                                }
                                                catch
                                                {
                                                    validInts = false;
                                                }

                                                if (validInts)
                                                {
                                                    TTTGames.Add(new TicTacToe(e.Author, mentioned[0], e.Channel as SocketChannel, int.Parse(par[2]), int.Parse(par[3]))); // a new TTT game is added to 'TTTGames' with the command runner, opponent, channel, and board size
                                                    await e.Channel.SendMessageAsync($"A new game has started with a board size of {int.Parse(par[2])} x {int.Parse(par[3])}!");
                                                }
                                                else
                                                    await e.Channel.SendMessageAsync($"**Error:** Invalid integers were supplied for the board size.");
                                            }
                                        }
                                    }
                                    else if (i != -1) //if it has found the user
                                        await e.Channel.SendMessageAsync("You are already in a game in this channel."); //the user cannot play two game in a channel
                                    else if (j != -1) //if it has found the mentioned user
                                        await e.Channel.SendMessageAsync("They are already in a game in this channel."); //the user cannot play with another user playing another game

                                    SaveState();
                                }
                            }
                            else
                                await e.Channel.SendMessageAsync($"{helpNew}\n{helpPlay}\n{helpCancel}"); // send default help message if no valid commands were detected

                        }
                        else if (par.Length == 3)
                        {
                            if (par[0] == "new")
                                await e.Channel.SendMessageAsync(helpNew); // too many requirements were supplied so help is shown
                            else if (par[0] == "cancel")
                                await e.Channel.SendMessageAsync(helpCancel);
                            else if (par[0] == "play")
                            {
                                int i = TicTacToe.SearchPlayer(TTTGames.ToArray(), e.Author, e.Channel as SocketChannel);
                                if (i != -1) //checks if it actually finds a player
                                {
                                    int x;
                                    bool isNumeric = int.TryParse(par[1], out x);

                                    if (!isNumeric)
                                    {
                                        await e.Channel.SendMessageAsync("Invalid X co-ordinate.");
                                        return;
                                    }

                                    int y = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(par[2]) + 1;
                                    int? isc = TTTGames[i].TakeTurn(e.Author, x, y); //check the turn
                                    if (isc == 0) //if the turn was successful
                                    {
                                        await e.Channel.SendMessageAsync("```\n" + TTTGames[i].DrawGame() + "```"); //write down the game
                                        var c = TTTGames[i].CheckGame(); //check if someone wins
                                        if (c == TicTacToe.GameStat.CircleWin || c == TicTacToe.GameStat.CrossWin) //if someone wins
                                        {
                                            await e.Channel.SendMessageAsync($"Congratulation, <@{e.Author.Id}>, you won!");
                                            TTTGames.RemoveAt(i); //delete the game
                                        }
                                        else if (c == TicTacToe.GameStat.Tie) //if there is a tie
                                        {
                                            await e.Channel.SendMessageAsync("You are both stuck, there is a tie. The game has ended.");
                                            TTTGames.RemoveAt(i); //delete the game
                                        }
                                        //otherwise well the game continues
                                    }
                                    else if (isc == 1)
                                        await e.Channel.SendMessageAsync("It's not your turn."); //the user cannot play if it's not his turn
                                    else if (isc == 2)
                                        await e.Channel.SendMessageAsync("Co-ordinates off grid."); //Invalid co-ords
                                    else
                                        await e.Channel.SendMessageAsync("You can't place a shape onto another shape."); //the user cannot cheat by replacing a shape

                                    SaveState();
                                }
                                else
                                    await e.Channel.SendMessageAsync("You are not currently in a game in this channel."); //the user cannot play if he's not playing
                            }
                            else
                                await e.Channel.SendMessageAsync($"{helpNew}\n{helpPlay}\n{helpCancel}"); // invalid arguments given, help displayed
                        }
                        else
                            await e.Channel.SendMessageAsync($"{helpNew}\n{helpPlay}\n{helpCancel}");
                    }
                    //						else if (cmd == "hangman") // hangman command code
                    //						{
                    //							if (par.Length == 1) // checks if only one command argument was supplied
                    //							{
                    //								if (par[0] == "new") {
                    //									await e.Channel.SendMessageAsync("Setting up game..."); // outputs string
                    //								}
                    //							}
                    //						}

                    else if (cmd == "crash")
                    {
                        throw new Exception("Manual crash tester.");
                    }
                }
            }
            catch (Exception ex)
            {
                await e.Channel.SendMessageAsync("**Error:** A unexcepted error happened.\nIf you think this bug should be fixed, go here: <https://github.com/Bottersnike>");
                if (ex.ToString().Length < 2000)
                    await e.Channel.SendMessageAsync($"```\n{ex}```");
                Console.WriteLine(ex);
            }
        }

        private Task Ready()
        {
            if (File.Exists(Save.path + "ttt.json"))
            {
                Console.WriteLine("[Info] TicTacToe: Games file found. Loading.");
                JSON.TicTacToeStruct[] gamej = Save.Load<JSON.TicTacToeStruct[]>("ttt.json");

                foreach (JSON.TicTacToeStruct j in gamej)
                {
                    TTTGames.Add(TicTacToe.ToClass(j, _client));
                }

                if (gamej.Length == 1)
                    Console.WriteLine("[Info] TicTacToe: Loaded " + gamej.Length.ToString() + " game from file!");
                else
                    Console.WriteLine("[Info] TicTacToe: Loaded " + gamej.Length.ToString() + " games from file!");
            }

            if (File.Exists(Save.path + "pastgames.json"))
            {
                Console.WriteLine("[Info] TicTacToe: Scoreboard file found. Loading.");
                JSON.PastGame[] gamej = Save.Load<JSON.PastGame[]>("pastgames.json");

                foreach (JSON.PastGame j in gamej)
                {
                    past_games.Add(PastGame.ToClass(j, _client));
                }

                if (gamej.Length == 1)
                    Console.WriteLine("[Info] TicTacToe: Loaded " + gamej.Length.ToString() + " game from file!");
                else
                    Console.WriteLine("[Info] TicTacToe: Loaded " + gamej.Length.ToString() + " games from file!");
            }

            return Task.CompletedTask;
        }
    }
}
