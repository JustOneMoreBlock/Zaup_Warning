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
using SDG.Unturned;
using System.Linq;

namespace Zaup_Warning
{
    public class CommandWarn : IRocketCommand
    {
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Both; }
        }

        public string Name
        {
            get
            {
                return "warn";
            }
        }
        public string Help
        {
            get
            {
                return "Warns a player for breaking the rules.";
            }
        }
        public string Syntax
        {
            get
            {
                return "<name> \"[reason]\" [amt]";
            }
        }
        public List<string> Aliases
        {
            get { return new List<string>(); }
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
            if (msg.Length <= 0 || msg.Length > 3)
            {
                UnturnedChat.Say(playerid, Zaup_Warning.Instance.Translate("warn_command_usage", new object[] { }));
                return;
            }
            IRocketPlayer warnee = IRocketPlayer.FromName(msg[0]);
            if (warnee == null)
            {
                UnturnedChat.Say(playerid, Zaup_Warning.Instance.Translate("invalid_name_provided", new object[] { }));
                return;
            }
            if (warnee.CharacterName == playerid.CharacterName)
            {
                UnturnedChat.Say(playerid, Zaup_Warning.Instance.Translate("not_warn_yourself", new object[] { }));
                return;
            }
            string reason = "";
            if (msg.Length >= 2)
            {
                reason = msg[1];
            }
            short amt = 1;
            if (msg.Length == 3)
            {
                short.TryParse(msg[2], out amt);
            }
            // Check their current warning level.
            byte currentlevel = Zaup_Warning.Instance.Database.GetWarnings(warnee.CSteamID);
            if (amt < 0)
            {
                if (currentlevel + amt < 0) amt = (short)currentlevel;
                if (!Zaup_Warning.Instance.Database.EditWarning(warnee.CSteamID, amt))
                {
                    UnturnedChat.Say(playerid, Zaup_Warning.Instance.Translate("error_warning_player", new object[] { warnee.CharacterName }));
                    return;
                }
                else
                {
                    UnturnedChat.Say(playerid, Zaup_Warning.Instance.Translate("warn_reduced_warner_msg", new object[] {
                        warnee.CharacterName,
                        amt.ToString(),
                        ((short)currentlevel + amt).ToString()
                    }));
                    return;
                }
            }
            if (((short)currentlevel + amt) >= Zaup_Warning.Instance.Configuration.WarningstoKick && Zaup_Warning.Instance.Configuration.WarningtoKickOn)
            {
                if (!Zaup_Warning.Instance.Database.EditWarning(warnee.CSteamID, amt))
                {
                    UnturnedChat.Say(playerid, Zaup_Warning.Instance.Translate("error_warning_player", new object[] { warnee.CharacterName }));
                    return;
                }
                else
                {
                    warnee.Kick(reason);
                    UnturnedChat.Say(Zaup_Warning.Instance.Translate("warn_kick_public_msg", new object[] {
                        warnee.CharacterName,
                        playerid.CharacterName,
                        reason,
                        ((short)currentlevel + amt).ToString()
                    }));
                    return;
                }
            }
            else
            {
                if (!Zaup_Warning.Instance.Database.EditWarning(warnee.CSteamID, amt))
                {
                    UnturnedChat.Say(playerid, Zaup_Warning.Instance.Translate("error_warning_player", new object[] { warnee.CharacterName }));
                    return;
                }
                else
                {
                    UnturnedChat.Say(Zaup_Warning.Instance.Translate("warn_msg", new object[] {
                        warnee.CharacterName,
                        playerid.CharacterName,
                        reason,
                        ((short)currentlevel + amt).ToString()
                    }));
                    return;
                }
            }
        }
    }
}
