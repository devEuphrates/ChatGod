using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Euphrates
{
    class CommandMute : IRocketCommand
    {
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Both; }
        }

        public string Name
        {
            get { return "mute"; }
        }

        public string Help
        {
            get { return "Mute players."; }
        }

        public string Syntax
        {
            get { return "/mute <\"player name\">"; }
        }

        public List<string> Aliases
        {
            get
            {
                return new List<string>
                {
                    "mu"
                };
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>
                {
                    "ChatGod.Mute"
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

            if (AddToMuteList(mutePlayer))
            {
                UnturnedChat.Say(caller, ChatGod.Instance.Translate("player_muted", mutePlayer.CharacterName), Color.green);
            }
            else
                UnturnedChat.Say(caller, ChatGod.Instance.Translate("player_already_muted", mutePlayer.CharacterName), Color.red);
        }

        public bool AddToMuteList(UnturnedPlayer unturnedPlayer)
        {
            string[] players = ChatGod.Instance.mutedPlayers;

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] == unturnedPlayer.CSteamID.ToString())
                {
                    return false;
                }
            }

            File.AppendAllText(ChatGod.Instance.mutedPlayersListPath, unturnedPlayer.CSteamID.ToString() + System.Environment.NewLine);
            int ns = ChatGod.Instance.mutedPlayers.Length + 1;
            Array.Resize(ref ChatGod.Instance.mutedPlayers, ns);
            ChatGod.Instance.mutedPlayers[ns - 1] = unturnedPlayer.CSteamID.ToString();
            return true;
        }
    }
}
