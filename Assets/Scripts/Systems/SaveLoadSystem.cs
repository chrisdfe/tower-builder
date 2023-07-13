using System;
using System.IO;
using Newtonsoft.Json;
using TowerBuilder;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.Systems
{
    public class SaveLoadSystem
    {
        static string SAVE_FILE_PATH = "./Assets/Resources/DebugOutput/save.json";
        public static void SaveToFile(System.Object saveData)
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;
                // TODO - Debug mode only, otherwise "none"
                serializer.Formatting = Formatting.Indented;

                serializer.Converters.Add(new EntityDefinitionJsonConverter());

                using (StreamWriter sw = new StreamWriter(SAVE_FILE_PATH))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, saveData);
                }

                // string jsonifiedObject = serializer.Serialize(writer, saveData);

                // string[] lines = { jsonifiedObject };

                // File.WriteAllLines(filePath, lines);

                Debug.Log($"Saved to file {SAVE_FILE_PATH}");
            }
            catch (Exception e)
            {
                Debug.Log("error saving file");
                Debug.Log(e.GetType());
            }
        }

        public static void SaveToFileDebug()
        {
            SaveToFile(Registry.appState.Entities.Foundations.list.items[0]);
        }
    }
}