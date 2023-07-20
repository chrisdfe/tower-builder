using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TowerBuilder;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.Systems
{
    public class SaveableObjectJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            dynamic valueAsSaveable = value as dynamic;
            var input = valueAsSaveable.ToInput();
            serializer.Serialize(writer, input);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            dynamic value = Activator.CreateInstance(objectType);

            Type SaveableInterface = GetSaveableInterface(objectType);
            var rawObject = JValue.Load(reader);

            Type SaveableInterfaceInputType = SaveableInterface.GetGenericArguments()[0];

            dynamic input = rawObject.ToObject(SaveableInterfaceInputType);

            value.ConsumeInput(input);

            return value;
        }

        public override bool CanConvert(Type objectType) => ImplementsSaveable(objectType);

        bool ImplementsSaveable(Type objectType) => GetSaveableInterface(objectType) != null;

        Type GetSaveableInterface(Type objectType) =>
            objectType
                .GetInterfaces()
                .ToList()
                .Find((i) =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(ISaveable<>)
                );
    }
}