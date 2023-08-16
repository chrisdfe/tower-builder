using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Foundations;
using UnityEngine;

namespace TowerBuilder.Systems
{
    public class SaveLoadSystem
    {
        static string SAVE_FILE_PATH = "./Assets/Resources/DebugOutput/save.json";


        public static void SaveToFile(System.Object saveData, string filePath)
        {
            Debug.Log(saveData);

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
                object raw = serializer.Deserialize<object>(reader);
                Debug.Log("raw");
                Debug.Log(raw);
                object input = ToSimplifiedValue(raw);
                Debug.Log("deserialized save data:");
                Debug.Log(input);

                var entity = Entity.FromInput(input as Dictionary<string, object>);

                // Registry.appState.Entities.Add(Entity.FromInput(input as Dictionary<string, object>));
            }
        }

        public static void LoadFromFileDebug()
        {
            LoadFromFile(SAVE_FILE_PATH);
        }

        public static void SaveToFileDebug()
        {
            SaveToFile(Registry.appState.Entities.Foundations.list.items[0].ToInput(), SAVE_FILE_PATH);
        }

        static object ToSimplifiedValue(object originalObject)
        {
            switch (originalObject)
            {
                // objects become Dictionary<string,object>
                case JObject jObject:
                    return ((IEnumerable<KeyValuePair<string, JToken>>)jObject).ToDictionary(j => j.Key, j => ToSimplifiedValue(j.Value));
                // arrays become List<object>
                case JArray jArray:
                    return jArray.Select(ToSimplifiedValue).ToList();
                // values just become the value
                case JValue jValue:
                    return jValue.Value;
                // don't know what to do here
                default:
                    throw new Exception($"Unsupported type: {originalObject.GetType()}");
            }
        }
    }
}