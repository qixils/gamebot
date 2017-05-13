using Discord;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamebot.JSON
{
    public struct PastGame
    {
        public ulong user;
        public ulong against;
        public ulong channel;
        public DateTime game_started;
        public DateTime game_ended;
        public TicTacToe.GameStat winner;
    }
}
