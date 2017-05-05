using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rocket.API.Collections;
using Rocket.Core.Assets;
using UnityEngine;

namespace Euphrates
{
    public class ChatGodConfig : IRocketPluginConfiguration
    {
            public bool PluginIsEnabled, DontAllowGlobalchat, DontAllowGroupchat, DontAllowAreachat, IsMysql;
            public int PlayerAdCost;
        public void LoadDefaults()
        {
            PluginIsEnabled = true;
            DontAllowGlobalchat = false;
            DontAllowGroupchat = false;
            DontAllowAreachat = false;
            IsMysql = false;
            PlayerAdCost = 1000;
        }
    }
}
