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
            if (command[0].ToLower() == "blue")
            {
                ChatGod.Instance.Configuration.Instance.adColor = Color.blue;
                ChatGod.Instance.Configuration.Save();
            }
            else if (command[0].ToLower() == "red")
            {
                ChatGod.Instance.Configuration.Instance.adColor = Color.red;
                ChatGod.Instance.Configuration.Save();
            }
            else if (command[0].ToLower() == "green")
            {
                ChatGod.Instance.Configuration.Instance.adColor = Color.green;
                ChatGod.Instance.Configuration.Save();
            }
            else if (command[0].ToLower() == "yellow")
            {
                ChatGod.Instance.Configuration.Instance.adColor = Color.yellow;
                ChatGod.Instance.Configuration.Save();
            }
            else if (command[0].ToLower() == "black")
            {
                ChatGod.Instance.Configuration.Instance.adColor = Color.black;
                ChatGod.Instance.Configuration.Save();
            }
            else if (command[0].ToLower() == "cyan")
            {
                ChatGod.Instance.Configuration.Instance.adColor = Color.cyan;
                ChatGod.Instance.Configuration.Save();
            }
            else if (command[0].ToLower() == "magenta")
            {
                ChatGod.Instance.Configuration.Instance.adColor = Color.magenta;
                ChatGod.Instance.Configuration.Save();
            }
            else
            {
                UnturnedChat.Say(player, ChatGod.Instance.Translate("command_wrong_usage"), Color.red);
            }
        }
    }
}
