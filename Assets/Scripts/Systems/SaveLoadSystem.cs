using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace TowerBuilder.Systems
{
    public class SaveLoadSystem
    {
        public static void SaveToFile<T>(T t)
        {

            string jsonifiedObject = JsonConvert.SerializeObject(t);
            // string filePath = Application.persistentDataPath + "/save.json";
            string filePath = "./Assets/Resources/DebugOutput/save.json";
            Debug.Log(filePath);

            // FileStream file;
            // if (File.Exists(filePath))
            // {
            //     file = File.OpenWrite(filePath);
            // }
            // else
            // {
            //     file = File.Create(filePath);
            // }

            string[] lines = { jsonifiedObject };

            File.WriteAllLines(filePath, lines);
            Debug.Log("done.");
        }
    }
}