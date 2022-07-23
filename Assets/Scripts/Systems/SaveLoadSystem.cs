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
            string filePath = "./Assets/Resources/DebugOutput/save.json";

            string[] lines = { jsonifiedObject };

            File.WriteAllLines(filePath, lines);
            Debug.Log("done.");
        }
    }
}