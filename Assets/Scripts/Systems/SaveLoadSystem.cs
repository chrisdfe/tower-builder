using System;
using System.IO;
using Newtonsoft.Json;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.Systems
{
    using SaveData = ListWrapper<DataTypes.Entities.Foundations.Foundation>;

    public class SaveLoadSystem
    {
        static string SAVE_FILE_PATH = "./Assets/Resources/DebugOutput/save.json";


        public static void SaveToFile(System.Object saveData, string filePath)
        {

            SaveStateJsonSerializer serializer = new SaveStateJsonSerializer();
            using (StreamWriter sw = new StreamWriter(filePath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, saveData);
            }

            Debug.Log($"Saved to file {SAVE_FILE_PATH}");
        }

        public static void LoadFromFile(string filePath)
        {
            SaveStateJsonSerializer serializer = new SaveStateJsonSerializer();
            using (StreamReader sr = new StreamReader(filePath))
            using (JsonTextReader reader = new JsonTextReader(sr))
            {
                SaveData data = serializer.Deserialize<SaveData>(reader);
                Debug.Log("deserialized save data:");
                Debug.Log(data);

                Registry.appState.Entities.Add(data);
            }
        }

        public static void LoadFromFileDebug()
        {
            LoadFromFile(SAVE_FILE_PATH);
        }

        public static void SaveToFileDebug()
        {
            SaveToFile(Registry.appState.Entities.Foundations.list, SAVE_FILE_PATH);
        }
    }
}