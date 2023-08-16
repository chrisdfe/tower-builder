using System;
using System.IO;
using Newtonsoft.Json;
using TowerBuilder;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.Systems
{
    public class SaveStateJsonSerializer : JsonSerializer
    {
        public SaveStateJsonSerializer() : base()
        {

            NullValueHandling = NullValueHandling.Ignore;

            // TODO - Debug mode only, otherwise "none"
            Formatting = Formatting.Indented;
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            // Converters.Add(new SaveableObjectJsonConverter());
        }
    }
}