using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.DataTypes.Entities.Furnitures;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Rooms.FurnitureBuilders
{
    public class BedroomRoomFurnitureBuilder : RoomFurnitureBuilderBase
    {
        public BedroomRoomFurnitureBuilder(Room room) : base(room) { }

        public override ListWrapper<Furniture> BuildFurniture(bool isInBlueprintMode)
        {
            List<Furniture> items = new List<Furniture>();

            Furniture furniture = new Furniture(Registry.Definitions.Entities.Furnitures.Queries.FindByKey(Furniture.Key.Bed));
            furniture.PositionAtCoordinates(room.cellCoordinatesList.bottomLeftCoordinates);
            furniture.isInBlueprintMode = isInBlueprintMode;
            items.Add(furniture);

            return new ListWrapper<Furniture>(items);
        }
    }
}


