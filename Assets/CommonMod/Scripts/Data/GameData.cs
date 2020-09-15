using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;


namespace Monster.Data
{
    public class GameData
    {
        public const int GameDataGenerateKey = 20200914;


        public static void GetAllData()
        {
            GetRedPackCashAmountData();
            GetDayTimeData();
            GetTotalRunTimeData();
            GetCashingData();

        }
        #region Red Pack Cash Amount

        public static void GetRedPackCashAmountData()
        {
            fastCashIndex = GetData(FastCashIndexKey, fastCashIndex);
            cashSigninStatus = GetData(CashSigninStatusKey, 0) == 0;

            #region GetEncryptData
            ObscuredInt saveLevel = GetData(RedPackCashKey, -1);
            if (saveLevel == -1)
            {
                redPackCash = 0;
            }
            else
            {
                redPackCash = ObscuredFloat.Decrypt(saveLevel, GameDataGenerateKey);
            }
            #endregion
        }

        private const string RedPackCashKey = "RedPackCash";
        private static ObscuredFloat redPackCash=-1;
        public static ObscuredFloat RedPackCash
        {
            set
            {
                redPackCash = value;
                SaveData(RedPackCashKey, ObscuredFloat.Encrypt(redPackCash, GameDataGenerateKey));
            }
            get
            {
                return redPackCash;
            }
        }
        #endregion

        #region DayTime
        public static void GetDayTimeData()
        {
            loginDays = GetData(LoginDaysKey, 1);
            claimDayLoginReward = GetData(ClaimDayLoginRewardKey, 0) == 0;
            claimOnlineReward = GetData(ClaimOnlineRewardKey, 0);
            lastServerTime = GetData(LastServerTimeKey, 0);
        }

        private const string LoginDaysKey = "LoginDaysKey";
        private static ObscuredInt loginDays;

        public static ObscuredInt LoginDays
        {
            set
            {
                loginDays = value;
                SaveData(LoginDaysKey, loginDays);
            }
            get
            {
               
                return loginDays;

            }
        }

        private const string ClaimDayLoginRewardKey = "ClaimDayLoginReward";
        private static ObscuredBool claimDayLoginReward;
        public static ObscuredBool ClaimDayLoginReward
        {
            set
            {
                claimDayLoginReward = value;
                SaveData(ClaimDayLoginRewardKey, claimDayLoginReward?1:0);
            }
            get
            {
                return claimDayLoginReward;

            }
        }

        private const string ClaimOnlineRewardKey = "ClaimOnlineReward";
        private static ObscuredInt claimOnlineReward;
        public static ObscuredInt ClaimOnlineReward
        {
            set
            {
                claimOnlineReward = value;
                SaveData(ClaimOnlineRewardKey, 0);
            }
            get
            {
                return claimOnlineReward;

            }
        }

        private const string LastServerTimeKey = "LastServerTime";
        private static ObscuredLong lastServerTime;
        public static ObscuredLong LastServerTime
        {
            set
            {
                lastServerTime = value;
                SaveData(LastServerTimeKey, lastServerTime);
            }
            get
            {
                return lastServerTime;

            }
        }
        #endregion

        #region TotalRunTime
        public static void GetTotalRunTimeData()
        {
            totalRunTime = GetData(TotalRunTimeKey, totalRunTime);
        }
        private const string TotalRunTimeKey = "TotalRunTime";
        private static ObscuredLong totalRunTime;
        public static ObscuredLong TotalRunTime
        {
            set
            {
                totalRunTime = value;
                SaveData(TotalRunTimeKey, totalRunTime);
            }
            get
            {
                return totalRunTime;
            }
        }


        #endregion

        #region CashingManager
        public static void GetCashingData()
        {
            fastCashIndex = GetData(FastCashIndexKey, fastCashIndex);
            cashSigninStatus = GetData(CashSigninStatusKey, 0) == 0;
        }

        private const string FastCashIndexKey = "FastCashIndex";
        private static ObscuredInt fastCashIndex = -1;
        public static ObscuredInt FastCashIndex
        {
            set
            {
                fastCashIndex = value;
                SaveData(FastCashIndexKey, fastCashIndex);
            }
            get
            {
                return fastCashIndex;

            }
        }

        private const string CashSigninStatusKey = "CashSigninStatus";
        private static ObscuredBool cashSigninStatus;
        public static ObscuredBool CashSigninStatus
        {
            set
            {
                cashSigninStatus = value;
                SaveData(CashSigninStatusKey, cashSigninStatus?1:0);
            }
            get
            {
                return cashSigninStatus;

            }
        }
        #endregion


        #region User



        #endregion

        #region Method

        /// <summary>
        /// 获取游戏数据,例如 int string,也可以保存 对象 组件，例如 GameObject  Image等
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_key">获取索引Key</param>
        /// <param name="_value">默认值</param>
        public static T GetData<T>(string _key, T _value)
        {
         return   ES3.Load(_key, _value);
        }

        /// <summary>
        /// 保存游戏数据,例如 int string,也可以保存 对象 组件，例如 GameObject  Image等
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_key"></param>
        /// <param name="_value"></param>
        public static void SaveData<T>(string _key, T _value)
        {
            ES3.Save<T>(_key, _value);
        }


        #endregion


    }
}

