using System;
using System.Collections.Generic;
using Rocket.Core;
using Rocket.Core.Permissions;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Permissions;
using Rocket.Unturned.Player;
using Rocket.Unturned.Commands;
using Rocket.API;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using System.Linq;

namespace Zaup_Warning
{
    public class CommandWarnings : IRocketCommand
    {
        public string Name
        {
            get
            {
                return "warnings";
            }
        }
        public string Help
        {
            get
            {
                return "Warns a player's warning level.";
            }
        }
        public string Syntax
        {
            get
            {
                return "[name]";
            }
        }
        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public AllowedCaller AllowedCaller
        {
            get { return false; }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "warnings.others" };
            }
        }

        public void Execute(IRocketPlayer playerid, string[] msg)
        {
            if (playerid == null) return;
            if (msg.Length > 1)
            {
                UnturnedChat.Say(playerid, Zaup_Warning.Instance.Translate("warnings_command_usage", new object[] { }));
                return;
            }
            if (msg.Length == 0)
            {
                byte currentlevel = Zaup_Warning.Instance.Database.GetWarnings(playerid.CSteamID);
                UnturnedChat.Say(playerid, Zaup_Warning.Instance.Translate("warnings_current_level", new object[] { currentlevel }));
                return;
            }
            if (!playerid.HasPermission("warnings.others"))
            {
                UnturnedChat.Say(playerid, Zaup_Warning.Instance.Translate("warnings_no_permission_others", new object[] { }));
                return;
            }
            IRocketPlayer warnee = IRocketPlayer.FromName(msg[0]);
            if (warnee == null)
            {
                UnturnedChat.Say(playerid, Zaup_Warning.Instance.Translate("invalid_name_provided", new object[] { }));
                return;
            }
            byte currentlevel1 = Zaup_Warning.Instance.Database.GetWarnings(warnee.CSteamID);
            UnturnedChat.Say(playerid, Zaup_Warning.Instance.Translate("warnings_current_level_others", new object[] { warnee.CharacterName, currentlevel1 }));
            return;
        }
    }
}
