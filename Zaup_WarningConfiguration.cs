using System;
using System.Collections.Generic;
using Rocket.API;

namespace Zaup_Warning
{
    public class Zaup_WarningConfiguration : IRocketPluginConfiguration
    {
        public string DatabaseAddress; 
        public string DatabaseName;
        public string DatabaseUsername;
        public string DatabasePassword;
        public ushort DatabasePort;
        public string TableName;
        public ulong KeepWarningsLengthDays;
        public bool WarningtoKickOn;
        public byte WarningstoKick;

        public void LoadDefaults()
        {
            DatabaseAddress = "localhost";
            DatabaseName = "unturned";
            DatabaseUsername = "unturned";
            DatabasePassword = "password";
            DatabasePort = 3306;
            TableName = "warnings";
            KeepWarningsLengthDays = 30;
            WarningtoKickOn = true;
            WarningstoKick = 3;
        }
    }
}
