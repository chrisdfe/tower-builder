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
        // public SaveableInputBase(object rawInput) { }
        // public abstract object ToRawInput();
    }

    public interface ISaveable
    {
        public SaveableInputBase ToInput();

        public void ConsumeInput(SaveableInputBase input);

        public ItemType CreateItemFrom<ItemType>() => throw new NotImplementedException();

        // public static void FromInput(SaveableInputBase input) => throw new NotImplementedException();
    }
}