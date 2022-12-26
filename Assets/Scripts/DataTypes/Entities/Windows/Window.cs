using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Windows
{
    public class Window : Entity<Window.Key>
    {
        public enum Key
        {
            None,
            Default,
        }

        public static EnumStringMap<Key> KeyLabelMap = new EnumStringMap<Key>(
            new Dictionary<Key, string>() {
                { Key.Default, "Default" }
            }
        );

        public override string idKey { get => "windows"; }

        public Room room;

        public Window(WindowDefinition windowDefinition) : base(windowDefinition) { }
    }
}