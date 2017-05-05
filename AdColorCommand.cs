using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Euphrates
{
    class AdColorCommand : IRocketCommand
    {
        Color ss = Color.blue;
        public static AdColorCommand Instance;
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Player; }
        }
        public string Name
        {
            get { return "advertcolor"; }
        }
        public string Syntax
        {
            get { return "<your ad color>"; }
        }
        public string Help
        {
            get { return "Select your ad's color"; }
        }
        public List<string> Permissions
        {
            get
            {
                return new List<string>
                {
                    "advertcolor"
                };
            }
        }
        public List<string> Aliases
        {
            get { return new List<string> { "adcol", "adcolor", "advertcolor" }; }
        }
        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (command[0].ToLower() == "list")
            {
                UnturnedChat.Say(player, ChatGod.Instance.Translate("ad_colors"), Color.cyan);
                UnturnedChat.Say(player, "blue", Color.blue);
                UnturnedChat.Say(player, "red", Color.red);
                UnturnedChat.Say(player, "green", Color.green);
                UnturnedChat.Say(player, "yellow", Color.yellow);
                UnturnedChat.Say(player, "black", Color.black);
            }
            else if (command[0].ToLower() == "blue")
            {
                ss = Color.blue;
            }
            else if (command[0].ToLower() == "red")
            {
                ss = Color.red;
            }
            else if (command[0].ToLower() == "green")
            {
                ss = Color.green;
            }
            else if (command[0].ToLower() == "yellow")
            {
                ss = Color.yellow;
            }
            else if (command[0].ToLower() == "black")
            {
                ss = Color.black;
            }
            else
            {
                UnturnedChat.Say(player, ChatGod.Instance.Translate("command_wrong_usage"), Color.red);
            }
        }

        public Color selectedColor()
        {
            return ss;
        }
    }
}
