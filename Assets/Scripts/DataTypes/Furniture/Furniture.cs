using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures
{
    public abstract class Furniture
    {
        public virtual FurnitureCategory category { get { return FurnitureCategory.None; } }
        // public int occupancy;

        public bool isInBlueprintMode { get; private set; } = false;

        Room room;
        List<FurnitureAttributesBase> configs;

        public Furniture(Room room, List<FurnitureAttributesBase> configs)
        {
            this.room = room;
            this.configs = configs;
        }

        public void OnBuild()
        {
            isInBlueprintMode = false;
        }

        public void Initialize()
        {

        }

        public void OnDestroy()
        {

        }
    }
}