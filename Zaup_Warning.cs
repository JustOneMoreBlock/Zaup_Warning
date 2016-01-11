using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Commands;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Permissions;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;

namespace Zaup_Warning
{
    public class Zaup_Warning : RocketPlugin<Zaup_WarningConfiguration>
    {
        public static Zaup_Warning Instance;
        public DatabaseMgr Database;

        public static Dictionary<CSteamID, string> Players = new Dictionary<CSteamID, string>();

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList() {
                    {
                        "warn_command_usage",
                        "/warn <name> \"[reason]\" [amt] Amt can be negative to reduce warning level."
                    },
                    {
                        "invalid_name_provided",
                        "An invalid player name was provided."
                    },
                    {
                        "not_warn_yourself",
                        "You cannot warn yourself!"
                    },
                    {
                        "error_warning_player",
                        "There was an error warning {0}."
                    },
                    {
                        "warn_msg",
                        "{0} was warned by {1} for {2}. This is warning {3}."
                    },
                    {
                        "warn_kick_public_msg",
                        "{0} was warned by {1} for {2}.  This is warning {3} and {0} has been kicked."
                    },
                    {
                        "warn_reduced_warner_msg",
                        "You have reduced {0}'s warning level by {1}.  It is now {2}."
                    },
                    {
                        "warnings_command_usage",
                        "/warnings [name] Will display yours or someone else's warning level."
                    },
                    {
                        "warnings_no_permission_others",
                        "You do not have permission to view others warning level."
                    },
                    {
                        "warnings_current_level",
                        "Your current warnings level is {0}."
                    },
                    {
                        "warnings_current_level_others",
                        "{0}'s current warnings level is {1}."
                    }
                };
            }
        }

        protected override void Load() {
            Zaup_Warning.Instance = this;
            this.Database = new DatabaseMgr();
            this.Database.DeleteWarnings();
        }

    }
}
