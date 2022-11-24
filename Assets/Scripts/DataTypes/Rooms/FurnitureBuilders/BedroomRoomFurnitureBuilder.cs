using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.DataTypes.Furnitures;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms.FurnitureBuilders
{
    public class BedroomRoomFurnitureBuilder : RoomFurnitureBuilderBase
    {
        public BedroomRoomFurnitureBuilder(Room room) : base(room) { }

        public override FurnitureList BuildFurniture(bool isInBlueprintMode)
        {
            List<Furniture> items = new List<Furniture>();

            Furniture furniture = new Furniture(Registry.definitions.furnitures.queries.FindByKey("bed"));
            furniture.room = room;
            furniture.cellCoordinates = room.blocks.cells.coordinatesList.bottomLeftCoordinates;
            furniture.isInBlueprintMode = isInBlueprintMode;
            items.Add(furniture);

            return new FurnitureList(items);
        }
    }
}


