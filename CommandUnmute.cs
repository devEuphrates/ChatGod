using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Euphrates
{
    class CommandUnmute : IRocketCommand
    {
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Both; }
        }

        public string Name
        {
            get { return "unmute"; }
        }

        public string Help
        {
            get { return "Unmute players."; }
        }

        public string Syntax
        {
            get { return "/unmute <\"player name\">"; }
        }

        public List<string> Aliases
        {
            get
            {
                return new List<string>
                {
                    "unmu"
                };
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>
                {
                    "ChatGod.Unmute"
                };
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            string playerName = ChatGod.Instance.ArgsToMessage(command);
            UnturnedPlayer mutePlayer = UnturnedPlayer.FromName(playerName);

            if (mutePlayer == null)
            {
                UnturnedChat.Say(caller, ChatGod.Instance.Translate("player_dont_exist"), Color.red);
                return;
            }

            if (RemoveFromMuteList(mutePlayer))
            {
                UnturnedChat.Say(caller, ChatGod.Instance.Translate("player_unmuted", mutePlayer.CharacterName), Color.green);
            }
            else
            {
                UnturnedChat.Say(caller, ChatGod.Instance.Translate("player_unmute_fail", mutePlayer.CharacterName), Color.green);
            }
        }

        public bool RemoveFromMuteList(UnturnedPlayer unturnedPlayer)
        {
            string[] players = ChatGod.Instance.mutedPlayers;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] == unturnedPlayer.CSteamID.ToString())
                {
                    players = players.Where(val => val != players[i]).ToArray();
                    File.WriteAllText(ChatGod.Instance.mutedPlayersListPath, string.Empty);
                    for (int i2 = 0; i < players.Length; i++)
                    {
                        File.AppendAllText(ChatGod.Instance.mutedPlayersListPath, players[i2] + System.Environment.NewLine);
                    }
                    ChatGod.Instance.mutedPlayers = players;
                    return true;
                }
            }
            return false;
        }
    }
}
