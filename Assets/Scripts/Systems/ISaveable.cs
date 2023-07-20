using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TowerBuilder;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.Systems
{
    public class ISaveableInputBase { }

    // public interface ISaveable
    // {
    //     public ISaveableInputBase ToInput();
    //     public void ConsumeInput(ISaveableInputBase input);
    // }

    public interface ISaveable<InputType>
    {
        public InputType ToInput();
        public void ConsumeInput(InputType input);
        public static void FromInput(InputType input) => throw new NotImplementedException();
    }
}