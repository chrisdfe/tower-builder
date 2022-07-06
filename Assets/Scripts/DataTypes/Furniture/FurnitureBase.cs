using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Furniture
{
    public abstract class FurnitureBase
    {
        public virtual FurnitureCategory category { get { return FurnitureCategory.None; } }
        public int occupancy;

        Room room;
        FurnitureAttributesBase config;

        public FurnitureBase(Room room, FurnitureAttributesBase config)
        {
            this.room = room;
            this.config = config;
        }

        public abstract void Initialize();
        public abstract void OnDestroy();
    }
}