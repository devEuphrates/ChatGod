//using fr34kyn01535.Uconomy;
//using MySql.Data.MySqlClient;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.IO;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace Euphrates
{
    public class ChatGod : RocketPlugin<ChatGodConfig>
    {
        public string chatLogPath = System.IO.Directory.GetCurrentDirectory() + @"\Plugins\ChatGod\ChatLog.txt";
        public string bannedWordsPath = System.IO.Directory.GetCurrentDirectory() + @"\Plugins\ChatGod\BannedWords.txt";
        public string mutedPlayersListPath = System.IO.Directory.GetCurrentDirectory() + @"\Plugins\ChatGod\MutedPlayersList.txt";
        public string[] bannedWords;
        public string[] mutedPlayers;
        public static ChatGod Instance;
        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList()
                {
                    {"globalchat_not_allowed", "Global chat is not allowed in this server!"},
                    {"groupchat_not_allowed", "Group chat is not allowed in this server!"},
                    {"areachat_not_allowed", "Area chat is not allowed in this server!"},
                    {"advertisements_not_allowed", "You can't put advertisements on this server!"},
                    {"allowed_colors_header", "Allowed colors in this server:"},
                    {"color_not_allowed", "The color you requested is not allowed by this server!"},
                    {"not_enough_xp", "You don't have enough XP to put an Advertisement!"},
                    //{"not_enough_money", "You don't have enough money."},
                    {"ad_success", "Your advertisement is on!"},
                    {"player_dont_exist", "No such player was found!"},
                    {"player_muted", "Successfully muted {0}!"},
                    {"player_already_muted", "{0} is already muted!"},
                    {"player_unmuted", "Successfully unmuted {0}!"},
                    {"player_unmute_fail", "Cannot unmute {0}!"},
                    {"on_muted_chat", "You are muted, you are not allowed to chat!"},
                    {"on_use_banned_word", "You can't use these words!"}
                };
            }
        }

        protected override void Load()
        {
            base.Load();
            UnturnedPlayerEvents.OnPlayerChatted += UnturnedPlayerEvents_OnPlayerChatted;
            bool playerConfigIsEnabled = Configuration.Instance.PluginIsEnabled;
            Logger.LogWarning("[ChatGod] Plugin loaded checking configuration file...");
            Logger.LogWarning("[ChatGod] Checking chat log file...");

            if (!File.Exists(chatLogPath))
            {
                Logger.LogWarning("[ChatGod] Chat log file doesn't exist, creating one...");
                using (File.Create(chatLogPath)) { };
                Logger.LogWarning("[ChatGod] Chat log file created!");
            }
            else
                Logger.LogWarning("[ChatGod] Chat log file exists!");

            if (!File.Exists(bannedWordsPath))
            {
                Logger.LogWarning("[ChatGod] Banned words file doesn't exist, creating one...");
                using (File.Create(bannedWordsPath)) { };
                Logger.LogWarning("[ChatGod] Banned words file created!");
            }
            else
                Logger.LogWarning("[ChatGod] Banned words file exists!");

            if (!File.Exists(mutedPlayersListPath))
            {
                Logger.LogWarning("[ChatGod] Muted players file doesn't exist, creating one...");
                using (File.Create(mutedPlayersListPath)) { };
                Logger.LogWarning("[ChatGod] Muted players file created!");
            }
            else
                Logger.LogWarning("[ChatGod] Muted players file exists!");
            mutedPlayers = File.ReadAllLines(mutedPlayersListPath);

            if (playerConfigIsEnabled == false)
            {
                Logger.LogError("[ChatGod] Plugin is been DISABLED from configuration");
                Logger.LogError("[ChatGod] Unloading Plugin!");
                UnturnedChat.Say("[ChatGod] Plugin is been DISABLED from configuration", Color.red);
                UnturnedChat.Say("[ChatGod] Unloading Plugin", Color.red);
                base.Unload();
                return;
            }
            else if (playerConfigIsEnabled == true)
            {
                Logger.LogWarning("[ChatGod] Plugin is ENABLED from configuration");
                Logger.LogWarning("[ChatGod] Now you have full control over chat!");
                Logger.LogWarning("[ChatGod]  _____________________________________________________");
                Logger.LogWarning("[ChatGod] |                                                     |");
                Logger.LogWarning("[ChatGod] |                 Plugin By Euphrates                 |");
                Logger.LogWarning("[ChatGod] |         Don't forget to check for updates!          |");
                Logger.LogWarning("[ChatGod] |    You can contact me through this Steam account:   |");
                Logger.LogWarning("[ChatGod] |        http://steamcommunity.com/id/FrtYldrm        |");
                Logger.LogWarning("[ChatGod] |_____________________________________________________|");
                Logger.LogWarning("[ChatGod]");
                Logger.LogWarning("[ChatGod]  ____________________Plugin Status____________________");
                Logger.LogWarning("[ChatGod] - Global Chat: " + !Configuration.Instance.BanGlobalchat);
                Logger.LogWarning("[ChatGod] - Area Chat: " + !Configuration.Instance.BanAreachat);
                Logger.LogWarning("[ChatGod] - Group Chat: " + !Configuration.Instance.BanGroupchat);
                Logger.LogWarning("[ChatGod] ");
                Logger.LogWarning("[ChatGod] - Chat Filter: " + Configuration.Instance.ChatFilter);
                if (Configuration.Instance.ChatFilter)
                {
                    bannedWords = File.ReadAllLines(bannedWordsPath);
                    Logger.LogWarning("[ChatGod] - Banned " + bannedWords.Length + " words from chat!");
                }
                Logger.LogWarning("[ChatGod] ");
                Logger.LogWarning("[ChatGod] - Log all chat activity: " + Configuration.Instance.LogAllChat);
                if (Configuration.Instance.LogAllChat)
                    Logger.LogWarning("[ChatGod] - Log chat dates: " + Configuration.Instance.LogChatDate);
                Logger.LogWarning("[ChatGod] ");
                Logger.LogWarning("[ChatGod] - Advertisements: " + Configuration.Instance.AllowAdvertisements);
                if (Configuration.Instance.AllowAdvertisements)
                {
                    Logger.LogWarning("[ChatGod] - Advertisement xp cost: " + Configuration.Instance.PlayerAdCost);
                    Logger.LogWarning("[ChatGod] - Allowed AD colors: ");
                    foreach (string c in Configuration.Instance.AllowedAdColors)
                    {
                        Logger.LogWarning("[ChatGod] * " + c);
                    }
                }

                UnturnedChat.Say("[ChatGod] Plugin is been ENABLED from configuration", Color.cyan);
                UnturnedChat.Say("[ChatGod] Plugin By Euphrates", Color.cyan);

                //if (Configuration.Instance.UsingUconomy == true)
                //{
                //    try
                //    {
                //    MySqlConnection cn = new MySqlConnection(string.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};PORT={4};", new object[] {
                //    Uconomy.Instance.Configuration.Instance.DatabaseAddress,
                //    Uconomy.Instance.Configuration.Instance.DatabaseName,
                //    Uconomy.Instance.Configuration.Instance.DatabaseUsername,
                //    Uconomy.Instance.Configuration.Instance.DatabasePassword,
                //    Uconomy.Instance.Configuration.Instance.DatabasePort}));
                //    }
                //    catch (Exception exc)
                //    {
                //        Logger.LogException(exc, "[ChatGod] Cannot connect to MySql Server");
                //    }
                //}
            }
            Instance = this;
        }
        private void UnturnedPlayerEvents_OnPlayerChatted(UnturnedPlayer player, ref Color color, string message, EChatMode Chatmode, ref bool cancel)
        {
            bool censored = false;
            bool muted = false;

            if (!player.HasPermission("mute.protect"))
            {
                for (int i = 0; i < mutedPlayers.Length; i++)
                {
                    if (mutedPlayers[i] == player.CSteamID.ToString())
                    {
                        UnturnedChat.Say(player, Translate("on_muted_chat"), Color.red);
                        cancel = true;
                        muted = true;
                        break;
                    }
                }
            }

            if (Configuration.Instance.BanGlobalchat == true)
            {
                if (Chatmode == EChatMode.GLOBAL)
                {
                    if (!player.HasPermission("allow.global"))
                    {
                        string globalNotAllowed = Translate("globalchat_not_allowed");
                        cancel = true;
                        UnturnedChat.Say(player, globalNotAllowed, Color.red);
                    }
                }
            }

            if (Configuration.Instance.BanGroupchat == true)
            {
                if (Chatmode == EChatMode.GROUP)
                {
                    if (!player.HasPermission("allow.group"))
                    {
                        string groupNotAllowed = Translate("groupchat_not_allowed");
                        cancel = true;
                        UnturnedChat.Say(player, groupNotAllowed, Color.red);
                    }
                }
            }

            if (Configuration.Instance.BanAreachat == true)
            {
                if (Chatmode == EChatMode.LOCAL)
                {
                    if (!player.HasPermission("allow.area"))
                    {
                        string areaNotAllowed = Translate("areachat_not_allowed");
                        cancel = true;
                        UnturnedChat.Say(player, areaNotAllowed, Color.red);
                    }
                }
            }

            if (Configuration.Instance.ChatFilter && !cancel && !muted && !player.HasPermission("ignore.badwords"))
            {
                string badWords = "";

                for (int i = 0; i < bannedWords.Length; i++)
                {
                    if (message.ToLower().Contains(bannedWords[i].ToLower()))
                    {
                        badWords += bannedWords[i] + " ";
                        cancel = true;
                        censored = true;
                    }
                }

                if (censored)
                    UnturnedChat.Say(player, Translate("on_use_banned_word") + " [" + badWords + "]", Color.red);
            }

            if (Configuration.Instance.LogAllChat)
            {
                string appendText = "";

                if (cancel && !censored && !muted)
                    appendText += "CANCELED -- ";

                if (censored)
                    appendText += "CENSORED -- ";

                if (muted)
                    appendText += "MUTED -- ";

                if (Configuration.Instance.LogChatDate)
                    appendText += "(" + DateTime.Now.ToString() + ") ";

                appendText += "[" + Chatmode.ToString() + "] ";
                appendText += player.CharacterName + ": ";

                File.AppendAllText(chatLogPath, appendText + message + System.Environment.NewLine);
            }
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
