using System;
using I18N.West;
using MySql.Data.MySqlClient;
using Rocket.Core.Logging;
using Rocket.API;
using UnityEngine;
using Steamworks;

namespace Zaup_Warning
{
    public class DatabaseMgr
    {
        internal DatabaseMgr()
        {
            CP1250 cP1250 = new CP1250();
            this.CheckSchema();
        }

        internal void CheckSchema()
        {
            try
            {
                MySqlConnection mySqlConnection = this.createConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = string.Concat("show tables like '", Zaup_Warning.Instance.Configuration.Instance.TableName, "'");
                mySqlConnection.Open();
                if (mySqlCommand.ExecuteScalar() == null)
                {
                    mySqlCommand.CommandText = string.Concat("CREATE TABLE `", Zaup_Warning.Instance.Configuration.Instance.TableName, "` (`steamId` varchar(32) NOT NULL,`warninglevel` tinyint unsigned NOT NULL,`lastwarningdate` TIMESTAMP NOT NULL DEFAULT current_timestamp, PRIMARY KEY (`steamId`)) ");
                    mySqlCommand.ExecuteNonQuery();
                }
                mySqlConnection.Close();
            }
            catch (Exception exception)
            {
                Rocket.Core.Logging.Logger.LogException(exception);
            }
        }

        private MySqlConnection createConnection()
        {
            MySqlConnection mySqlConnection = null;
            try
            {
                if (Zaup_Warning.Instance.Configuration.Instance.DatabasePort == 0)
                {
                    Zaup_Warning.Instance.Configuration.Instance.DatabasePort = 3306;
                }
                mySqlConnection = new MySqlConnection(string.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};PORT={4};", new object[] {
                    Zaup_Warning.Instance.Configuration.Instance.DatabaseAddress,
                    Zaup_Warning.Instance.Configuration.Instance.DatabaseName,
                    Zaup_Warning.Instance.Configuration.Instance.DatabaseUsername,
                    Zaup_Warning.Instance.Configuration.Instance.DatabasePassword,
                    Zaup_Warning.Instance.Configuration.Instance.DatabasePort}));
            }
            catch (Exception exception)
            {
                Rocket.Core.Logging.Logger.LogException(exception);
            }
            return mySqlConnection;
        }

        public byte GetWarnings(CSteamID id)
        {
            byte num = 0;
            try
            {
                MySqlConnection mySqlConnection = this.createConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = string.Concat(new string[] {
                    "select `warninglevel` from `",
                    Zaup_Warning.Instance.Configuration.Instance.TableName,
                    "` where `steamId` = '",
                    id.ToString(),
                    "';"
                });
                mySqlConnection.Open();
                object obj = mySqlCommand.ExecuteScalar();
                if (obj != null)
                {
                    byte.TryParse(obj.ToString(), out num);
                }
                mySqlConnection.Close();
            }
            catch (Exception exception)
            {
                Rocket.Core.Logging.Logger.LogException(exception);
            }
            return num;
        }

        public ushort GetWarningsTime(CSteamID id)
        {
            ushort num = 0;
            try
            {
                MySqlConnection mySqlConnection = this.createConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = string.Concat(new string[] {
                    "select timestampdiff(day, now(), 'select `lastwarningdate` from `",
                    Zaup_Warning.Instance.Configuration.Instance.TableName,
                    "` where `steamId` = '",
                    id.ToString(),
                    "' ');"
                });
                mySqlConnection.Open();
                object obj = mySqlCommand.ExecuteScalar();
                if (obj != null)
                {
                    ushort.TryParse(obj.ToString(), out num);
                }
                mySqlConnection.Close();
            }
            catch (Exception exception)
            {
                Rocket.Core.Logging.Logger.LogException(exception);
            }
            return num;
        }

        public bool EditWarning(CSteamID id, short amt = 1)
        {
            bool success = false;
            try
            {
                MySqlConnection mySqlConnection = this.createConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = string.Concat(new string[] {
                    "insert into `" +
                Zaup_Warning.Instance.Configuration.Instance.TableName +
                "` (steamId, warninglevel) VALUES ('" + id.ToString() + "', 1) on duplicate key update `warninglevel`=`warninglevel`+ " +
                amt.ToString()
                });
                mySqlConnection.Open();
                int affected = mySqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
                if (affected > 0) success = true;
            }
            catch (Exception exception)
            {
                Rocket.Core.Logging.Logger.LogException(exception);
            }
            return success;
        }

        public bool DeleteWarnings()
        {
            bool success = false;
            try
            {
                MySqlConnection mySqlConnection = this.createConnection();
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = string.Concat(new string[] {
                    "delete from `",
                    Zaup_Warning.Instance.Configuration.Instance.TableName,
                    "` where `lastwarningdate` < date_sub(now(), interval ",
                    Zaup_Warning.Instance.Configuration.Instance.KeepWarningsLengthDays.ToString(),
                    " day)"
                });
                mySqlConnection.Open();
                int affected = mySqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
                if (affected > 0) success = true;
            }
            catch (Exception exception)
            {
                Rocket.Core.Logging.Logger.LogException(exception);
            }
            return success;
        }
    }
}
