using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Euphrates
{
    class AdCommand : IRocketCommand
     { 
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Player; }
        }
        public string Name
        {
            get { return "advert"; }
        }
        public string Syntax
        {
            get { return "<your ad>"; }
        }
        public string Help
        {
            get { return "Put up an advertisement"; }
        }
        public List<string> Permissions
        {
            get
            {
                return new List<string>
                {
                    "advert"
                };
            }
        }
        public List<string> Aliases
        {
            get { return new List<string> { "ad", "newad", "advertise" }; }
        }
        public void Execute(IRocketPlayer caller, string[] command)
        {
            int balance = ChatGod.Instance.Configuration.Instance.PlayerAdCost;
            string error = ChatGod.Instance.Translate("not_enough_xp");
            string mesage = ArgsToMessage(command);
            if (caller != null)
            {
                UnturnedPlayer player = (UnturnedPlayer)caller;
                if (player.Experience < balance)
                {
                    UnturnedChat.Say(player, error, Color.red);
                    return;
                }
                UnturnedChat.Say(player, ChatGod.Instance.Translate("ad_success"), Color.blue);
                player.Experience -= (uint)balance;
                UnturnedChat.Say(ChatGod.Instance.Translate("ad_by") + player.DisplayName + " :");
                UnturnedChat.Say(mesage);
                return;
            }
            return;
        }

        public string ArgsToMessage(string[] args)
        {
            string tmp = "";
            foreach (string arg in args)
                if (tmp == "")
                    tmp = arg;
                else
                    tmp += " " + arg;
            return tmp;
        }

    }
}