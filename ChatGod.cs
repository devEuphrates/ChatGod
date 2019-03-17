using fr34kyn01535.Uconomy;
using MySql.Data.MySqlClient;
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
                    {"allowed_colors_header", "Allowed colors in this server:"},
                    {"color_not_allowed", "The color you requested is not allowed by this server!"},
                    {"not_enough_xp", "You don't have enough XP to put an Advertisement!"},
                    {"not_enough_money", "You don't have enough money."},
                    {"ad_success", "Your advertisement is on!"},
                    {"ad_by", "Advertisement by {0}"},
                    {"command_wrong_usage", "This is not how you use this command!"}
                };
            }
        }

        protected override void Load()
        {
            base.Load();
            UnturnedPlayerEvents.OnPlayerChatted += UnturnedPlayerEvents_OnPlayerChatted;
            bool playerConfigIsEnabled = Configuration.Instance.PluginIsEnabled;
            string PluginLoaded = "ChatGod Plugin By Euphrates";
            Logger.LogError("[ChatGod] Plugins is loaded now you have the full control over chat!");
            UnturnedChat.Say(PluginLoaded, Color.cyan);
            if (playerConfigIsEnabled == false)
            {
                Logger.LogError("[ChatGod] Plugin is been DISABLED from configuration");
                Logger.LogError("[ChatGod] Unloading Plugin!");
                UnturnedChat.Say("[ChatGod] Plugin is been DISABLED from configuration", Color.red);
                UnturnedChat.Say("[ChatGod] Unloading Plugin", Color.red);
                base.Unload();
                return;
            }
            else if(playerConfigIsEnabled == true)
            {
                Logger.LogError("[ChatGod] Plugin is been ENABLED from configuration");
                UnturnedChat.Say("[ChatGod] Plugin is been ENABLED from configuration", Color.cyan);
                Logger.LogError("[ChatGod] Plugin By Euphrates");
                UnturnedChat.Say("[ChatGod] Plugin By Euphrates", Color.cyan);
                Logger.LogError("[ChatGod] Don't forget to check for updates!");
                Logger.LogError("[ChatGod] You can contact me through this Steam account: ");
                Logger.LogError("[ChatGod] http://steamcommunity.com/id/FrtYldrm");
                if (Configuration.Instance.UsingUconomy == true)
                {
                    try
                    {
                    MySqlConnection cn = new MySqlConnection(string.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};PORT={4};", new object[] {
                    Uconomy.Instance.Configuration.Instance.DatabaseAddress,
                    Uconomy.Instance.Configuration.Instance.DatabaseName,
                    Uconomy.Instance.Configuration.Instance.DatabaseUsername,
                    Uconomy.Instance.Configuration.Instance.DatabasePassword,
                    Uconomy.Instance.Configuration.Instance.DatabasePort}));
                    }
                    catch (Exception exc)
                    {
                        Logger.LogException(exc, "[ChatGod] Cannot connect to MySql Server");
                    }
                }
            }
            Instance = this;
        }
        private void UnturnedPlayerEvents_OnPlayerChatted(UnturnedPlayer player, ref Color color, string message, EChatMode Chatmode, ref bool cancel)
        {
            bool PlayerConfigGlobalDisabled = Configuration.Instance.DontAllowGlobalchat;
            bool PlayerConfigGroupDisabled = Configuration.Instance.DontAllowGroupchat;
            bool PlayerConfigAreaDisabled = Configuration.Instance.DontAllowAreachat;

            if (PlayerConfigGlobalDisabled == true)
            {
                if (PlayerConfigGlobalDisabled == true)
                {
                    if (Chatmode == EChatMode.GLOBAL)
                    {
                        if (!player.HasPermission("allowglobal"))
                        {
                            string globalNotAllowed = Translate("globalchat_not_allowed");
                            cancel = true;
                            UnturnedChat.Say(player, globalNotAllowed, Color.red);
                        }
                    }
                }
            }

            if (PlayerConfigGroupDisabled == true)
            {
                if (Chatmode == EChatMode.GROUP)
                {
                    if (!player.HasPermission("allowgroup"))
                    {
                        string groupNotAllowed = Translate("groupchat_not_allowed");
                        cancel = true;
                        UnturnedChat.Say(player, groupNotAllowed, Color.red);
                    }
                }
            }

            if(PlayerConfigAreaDisabled == true)
            {
                if(Chatmode == EChatMode.LOCAL)
                {
                    if (!player.HasPermission("allowarea"))
                    {
                        string areaNotAllowed = Translate("areachat_not_allowed");
                        cancel = true;
                        UnturnedChat.Say(player, areaNotAllowed, Color.red);
                    }
                }
            }

            if (Configuration.Instance.LogAllChat)
            {
                string chatLogPath = System.IO.Directory.GetCurrentDirectory() + @"\Plugins\UnturnedRoleplayEssentials\ChatLog.txt";
                string appendText = "";

                if (cancel)
                    appendText += "CANCELED -- ";

                if (Configuration.Instance.LogChatDate)
                    appendText += "(" + DateTime.Now.ToString() + ") ";

                appendText += Chatmode.ToString();

                File.AppendAllText(chatLogPath, appendText + message + System.Environment.NewLine);
            }
        }
        }
    }
