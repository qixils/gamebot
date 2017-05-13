using Discord;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamebot
{
    class PastGame
    {
        public SocketUser user;
        public SocketUser against;
        public SocketChannel channel;
        public DateTime game_started;
        public DateTime game_ended;
        public TicTacToe.GameStat winner;

        public PastGame(SocketUser user_, SocketUser against_, SocketChannel channel_, DateTime game_started_, DateTime game_ended_, TicTacToe.GameStat winner_)
        {
            user = user_;
            against = against_;
            channel = channel_;
            game_started = game_started_;
            game_ended = game_ended_;
            winner = winner_;
        }
        public override string ToString()
        {
            return $"{user} vs {against} in {channel} from {game_started.ToString()} to {game_ended.ToString()}. winner: {winner.ToString()}";
        }

        public JSON.PastGame ToStruct()
        {
            JSON.PastGame r = new JSON.PastGame();
            r.user = user.Id;
            r.against = against.Id;
            r.channel = channel.Id;
            r.game_started = game_started;
            r.game_ended = game_ended;
            r.winner = winner;
            return r;
        }
        public static JSON.PastGame ToStruct(PastGame pg)
        {
            return pg.ToStruct();
        }
        public static PastGame ToClass(JSON.PastGame t, DiscordSocketClient client)
        {
            SocketChannel c = client.GetChannel(t.channel);
            SocketUser u = client.GetUser(t.user);
            SocketUser a = client.GetUser(t.against);

            PastGame pg = new PastGame(u, a, c, t.game_started, t.game_ended, t.winner);

            return pg;
        }
    }
}
