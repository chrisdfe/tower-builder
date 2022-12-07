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

        public override FurnitureList BuildFurniture(bool isInBlueprintMode)
        {
            List<Furniture> items = new List<Furniture>();

            Furniture furniture = new Furniture(Registry.definitions.furnitures.queries.FindByKey(Furniture.Key.Bed));
            // furniture.room = room;
            furniture.cellCoordinatesList = new CellCoordinatesList(
                room.blocks.cells.coordinatesList.bottomLeftCoordinates
            );
            furniture.isInBlueprintMode = isInBlueprintMode;
            items.Add(furniture);

            return new FurnitureList(items);
        }
    }
}


