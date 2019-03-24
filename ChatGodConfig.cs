using Rocket.API;
using System.Collections.Generic;

namespace Euphrates
{
    public class ChatGodConfig : IRocketPluginConfiguration
    {
        public bool PluginIsEnabled, AllowAdvertisements, BanGlobalchat, BanGroupchat, BanAreachat, LogAllChat, LogChatDate, ChatFilter;
        public int PlayerAdCost;
        public List<string> AllowedAdColors = new List<string>();
        public void LoadDefaults()
        {
            PluginIsEnabled = true;
            AllowedAdColors.Add("blue");
            AllowedAdColors.Add("green");
            AllowAdvertisements = false;
            BanGlobalchat = false;
            BanGroupchat = false;
            BanAreachat = false;
            //UsingUconomy = false;
            LogAllChat = false;
            PlayerAdCost = 1000;
            ChatFilter = false;
        }
    }
}
