using System;
using System.Collections.Generic;
using Rocket.Unturned;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Player;
using Rocket.API;
using SDG.Unturned;

namespace Zaup_Warning
{
    class CommandWarnings : IRocketCommand
    {
        public bool RunFromConsole
        {
            get
            {
                return false;
            }
        }
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

        public void Execute(RocketPlayer playerid, string[] msg)
        {
            if (playerid == null) return;
            if (msg.Length > 1)
            {
                RocketChat.Say(playerid, Zaup_Warning.Instance.Translate("warnings_command_usage", new object[] { }));
                return;
            }
            if (msg.Length == 0)
            {
                byte currentlevel = Zaup_Warning.Instance.Database.GetWarnings(playerid.CSteamID);
                RocketChat.Say(playerid, Zaup_Warning.Instance.Translate("warnings_current_level", new object[] { currentlevel }));
                return;
            }
            if (!playerid.HasPermission("warnings.others"))
            {
                RocketChat.Say(playerid, Zaup_Warning.Instance.Translate("warnings_no_permission_others", new object[] { }));
                return;
            }
            RocketPlayer warnee = RocketPlayer.FromName(msg[0]);
            if (warnee == null)
            {
                RocketChat.Say(playerid, Zaup_Warning.Instance.Translate("invalid_name_provided", new object[] { }));
                return;
            }
            byte currentlevel1 = Zaup_Warning.Instance.Database.GetWarnings(warnee.CSteamID);
            RocketChat.Say(playerid, Zaup_Warning.Instance.Translate("warnings_current_level_others", new object[] { warnee.CharacterName, currentlevel1 }));
            return;
        }
    }
}
