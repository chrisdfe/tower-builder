using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Furnitures.Behaviors;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.Definitions
{
    public class AllDefinitions
    {
        public FurnitureDefinitions furnitures = new FurnitureDefinitions();
        public RoomDefinitions rooms = new RoomDefinitions();
    }
}