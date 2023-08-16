// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Reflection;
// using System.Runtime;
// using Newtonsoft.Json;
// using Newtonsoft.Json.Linq;
// using TowerBuilder;
// using TowerBuilder.DataTypes.Entities;
// using UnityEngine;

// namespace TowerBuilder.Systems
// {
//     public class SaveableObjectJsonConverter : JsonConverter
//     {
//         public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
//         {
//             // dynamic valueAsSaveable = value as dynamic;
//             // var input = valueAsSaveable.ToInput();
//             // serializer.Serialize(writer, input);
//             serializer.Serialize(writer, value);
//         }

//         public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
//         {
//             Debug.Log("ReadJSON");
//             Debug.Log(reader.Value);
//             Debug.Log(objectType);
//             Debug.Log(existingValue);
//             return ToSimplifiedValue(reader.Value);
//             // var FromInputMethod = objectType.GetMethod("FromInput", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

//             // Type SaveableInterface = GetSaveableInterface(objectType);
//             // var rawObject = JValue.Load(reader);

//             // Type SaveableInterfaceInputType = SaveableInterface.GetGenericArguments()[0];

//             // dynamic input = rawObject.ToObject(SaveableInterfaceInputType);
//             // if (FromInputMethod != null)
//             // {
//             //     return FromInputMethod.Invoke(null, new object[] { input });
//             // }

//             // throw new Exception("no static FromInput method found on {objectType}");

//             // TODO - create a value and use ConsumeInput instead
//             // dynamic value = Activator.CreateInstance(objectType);
//             // value.ConsumeInput(input);
//             // return value;
//         }

//         public override bool CanConvert(Type objectType) => true;

//         // public override bool CanConvert(Type objectType) => ImplementsSaveable(objectType);

//         // bool ImplementsSaveable(Type objectType) => GetSaveableInterface(objectType) != null;

//         // Type GetSaveableInterface(Type objectType) =>
//         //     objectType
//         //         .GetInterfaces()
//         //         .ToList()
//         //         .Find((i) =>
//         //             i.IsGenericType &&
//         //             i.GetGenericTypeDefinition() == typeof(ISaveable<>)
//         //         );
//     }
// }