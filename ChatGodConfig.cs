using Rocket.API;
using System.Collections.Generic;

namespace Euphrates
{
    public class ChatGodConfig : IRocketPluginConfiguration
    {
        public bool PluginIsEnabled, DontAllowGlobalchat, DontAllowGroupchat, DontAllowAreachat, UsingUconomy, LogAllChat, LogChatDate;
        public int PlayerAdCost;
        public List<string> AllowedAdColors;
        public void LoadDefaults()
        {
            AllowedAdColors.Add("blue");
            AllowedAdColors.Add("green");
            PluginIsEnabled = true;
            DontAllowGlobalchat = true;
            DontAllowGroupchat = false;
            DontAllowAreachat = false;
            UsingUconomy = false;
            LogAllChat = false;
            PlayerAdCost = 1000;
        }
    }
}
