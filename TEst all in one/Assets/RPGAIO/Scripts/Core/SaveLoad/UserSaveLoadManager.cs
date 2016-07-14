using System;
using System.IO;
using LogicSpawn.RPGMaker.Generic;
using Newtonsoft.Json;
using UnityEngine;
using nCrypto.Crypters;

namespace LogicSpawn.RPGMaker.Core
{
    public class UserSaveLoadManager
    {
        private static readonly UserSaveLoadManager MyInstance = new UserSaveLoadManager();
        public static UserSaveLoadManager Instance
        {
            get { return MyInstance; }
        }

        private string _savePath;
        private string userSaveFileName = "userSave.save";
        private string PlayerPrefsKey = "User_Save";
        private bool encryptFiles = true;

        
        public void GetSavePath()
        {
            _savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                     @"My Games\" + Rm_RPGHandler.Instance.GameInfo.GameCompany +
                                     @"\" + Rm_RPGHandler.Instance.GameInfo.GameTitle + @"\");
        }


        public void LoadUserData()
        {
            GetSavePath();
            var foundData = false;
#if UNITY_WEBPLAYER
            var stringFromPlayerPrefs = PlayerPrefs.GetString(PlayerPrefsKey, "");
            if(!string.IsNullOrEmpty(stringFromPlayerPrefs))
            {
                var jsonFromEncode = stringFromPlayerPrefs;

#else
            if (File.Exists(_savePath + userSaveFileName))
            {
                var jsonFromEncode = File.ReadAllText(_savePath + userSaveFileName);
#endif

                var jsonDecoded = jsonFromEncode;
                if (encryptFiles)
                {
                    jsonDecoded = new Xor().Decrypt(jsonDecoded, GameDataSaveLoadManager.Instance.EncryptKey);
                }
                if (!String.IsNullOrEmpty(jsonFromEncode))
                {
                    foundData = true;
                    try
                    {

                        var loadedUserSave = JsonConvert.DeserializeObject<UserSave>(jsonDecoded,
                                                                                          new JsonSerializerSettings
                                                                                          {
                                                                                              TypeNameHandling =
                                                                                                  TypeNameHandling.
                                                                                                  Objects,
                                                                                              ObjectCreationHandling
                                                                                                  =
                                                                                                  ObjectCreationHandling
                                                                                                  .Replace
                                                                                          });

                        GameMaster.Instance.UserSave = loadedUserSave;
                    }
                    catch (Exception e)
                    {
                        foundData = false;
                    }
                }
            }

            if (!foundData)
            {
                Debug.Log("No user save found. Creating user save.");
                NewData();
            }

            //Debug.Log("[EDITOR] Loaded user save...");
        }


        public void SaveUserData()
        {
            GetSavePath();
            var gameMaster = GameMaster.Instance;
            if(gameMaster == null)
            {
                return;
            }

            var gameData = GameMaster.Instance.UserSave;
            if(gameData == null)
            {
                Debug.LogError("User Save is null.");
                return;
            }
            //Debug.Log("[RPGMaker] Saving settings ...");
                
            var saveFile = JsonConvert.SerializeObject(gameData, Formatting.Indented);
            var saveFileEncoded = saveFile; 
            if (encryptFiles)
            {
                saveFileEncoded = new Xor().Encrypt(saveFileEncoded, GameDataSaveLoadManager.Instance.EncryptKey);
            }

            Directory.CreateDirectory(_savePath);
#if UNITY_WEBPLAYER
            PlayerPrefs.SetString(PlayerPrefsKey,saveFile);
#else
            File.WriteAllText(_savePath + userSaveFileName, saveFileEncoded);
#endif

            Debug.Log("[RPGMaker] Saved user save.");
        }

        public void NewData()
        {
            GameMaster.Instance.UserSave = new UserSave();
            SaveUserData();
        }
    }
}