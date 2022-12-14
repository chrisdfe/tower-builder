using System.Collections.Generic;
using Newtonsoft.Json;
using TowerBuilder.DataTypes.Entities.Rooms.FurnitureBuilders;
using TowerBuilder.DataTypes.Entities.Rooms.Validators;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Rooms
{
    public partial class Room
    {
        public class Skin
        {
            public enum Key
            {
                Default,
                Wheels
            }

            public class Config
            {
                public int pricePerCell;
                public bool hasInteriorLights;
            }

            public Key key { get; }
            public Config config { get; }

            public Skin(Key key)
            {
                this.key = key;
                this.config = ConfigMap.Get(key);
            }

            public static class ConfigMap
            {
                static Dictionary<Key, Config> map = new Dictionary<Key, Config>() {
                {
                    Key.Default,
                    new Config() {
                        pricePerCell = 500,
                        hasInteriorLights = true
                    }
                },
                {
                    Key.Wheels,
                    new Config() {
                        pricePerCell = 1500,
                        hasInteriorLights = false
                    }
                },
            };

                public static Config Get(Key key) => map.GetValueOrDefault(key);
            }
        }
    }
}

