using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TowerBuilder;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.Systems
{
    public abstract class SaveableInputBase
    {
        public SaveableInputBase() { }
        public SaveableInputBase(object rawInput) { }
        public abstract object ToRawInput();
    }

    public interface ISaveable
    {
        public SaveableInputBase ToInput();
        public object ToRawInput() => ToInput().ToRawInput();
        public void ConsumeInput(SaveableInputBase input);

        public static void FromInput(SaveableInputBase input) => throw new NotImplementedException();

        // public static bool IsImplementedBy(Type objectType) =>
        //     objectType
        //         .GetInterfaces()
        //         .ToList()
        //         .Find(i => i.GetType() == typeof(ISaveable)) != null;
    }
}