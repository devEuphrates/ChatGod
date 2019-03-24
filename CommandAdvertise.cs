//using fr34kyn01535.Uconomy;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Euphrates
{
    class CommandAdvertise : IRocketCommand
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
            get { return "/ad <color | colors> <text>"; }
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
                    "ChatGod.Advert"
                };
            }
        }
        public List<string> Aliases
        {
            get { return new List<string> { "ad", "newad", "advertise" }; }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (!ChatGod.Instance.Configuration.Instance.AllowAdvertisements)
            {
                UnturnedChat.Say(player, ChatGod.Instance.Translate("advertisements_not_allowed"), Color.red);
                return;
            }

            if (command[0].ToString().ToLower() == "colors")
            {
                UnturnedChat.Say(player, ChatGod.Instance.Translate("allowed_colors_header"), Color.blue);
                foreach (string c in ChatGod.Instance.Configuration.Instance.AllowedAdColors)
                {
                    UnturnedChat.Say(player, c, Color.blue);
                }
                return;
            }
            int balance = ChatGod.Instance.Configuration.Instance.PlayerAdCost;
            string message = ChatGod.Instance.ArgsToMessage(command.Where(var => var != command[0].ToString().ToLower()).ToArray());
            //if (ChatGod.Instance.Configuration.Instance.UsingUconomy == false)
            //{

            if (player.Experience < balance)
            {
                UnturnedChat.Say(player, ChatGod.Instance.Translate("not_enough_xp"), Color.red);
                return;
            }

            if (!ChatGod.Instance.Configuration.Instance.AllowedAdColors.Contains(command[0].ToString().ToLower()))
            {
                UnturnedChat.Say(player, ChatGod.Instance.Translate("color_not_allowed"), Color.red);
                return;
            }

            UnturnedChat.Say(player, ChatGod.Instance.Translate("ad_success"), Color.blue);
            player.Experience -= (uint)balance;
            UnturnedChat.Say(message, UnturnedChat.GetColorFromName(command[0].ToString().ToLower(), Color.gray));
            return;

            //else
            //{
            //    int MySqlAdCost = ChatGod.Instance.Configuration.Instance.PlayerAdCost;
            //    decimal MySqlBalance = Uconomy.Instance.Database.GetBalance(player.CSteamID.ToString());

            //    if (MySqlBalance < balance)
            //    {
            //        UnturnedChat.Say(player, ChatGod.Instance.Translate("not_enough_xp"), Color.red);
            //        return;
            //    }

            //    if (!ChatGod.Instance.Configuration.Instance.AllowedAdColors.Contains(command[0].ToString().ToLower()))
            //    {
            //        UnturnedChat.Say(player, ChatGod.Instance.Translate("color_not_allowed"), Color.red);
            //        return;
            //    }

            //    UnturnedChat.Say(player, ChatGod.Instance.Translate("ad_success"), Color.blue);
            //    Uconomy.Instance.Database.IncreaseBalance(player.CSteamID.ToString(), (MySqlAdCost * -1));
            //    UnturnedChat.Say(message, UnturnedChat.GetColorFromName(command[0].ToString().ToLower(), Color.gray));
            //    return;
            //}
        }

    }
}