using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.DataTypes.Entities.Furnitures;
using UnityEngine;

namespace TowerBuilder.DataTypes.EntityGroups.Rooms.Builders
{
    public class RoomBuilder : EntityGroupBuilderBase<Room>
    {
        public RoomBuilder() : base() { }

        public override Room Build(SelectionBox selectionBox, bool isInBlueprintMode)
        {
            // List<Furniture> items = new List<Furniture>();

            // Furniture furniture = new Furniture(
            //     Registry.Definitions.Entities.Furnitures.Queries
            //         .FindByKey<Furniture.Key>(Furniture.Key.Bed) as FurnitureDefinition
            // );

            // furniture.PositionAtCoordinates(room.cellCoordinatesList.bottomLeftCoordinates);
            // furniture.isInBlueprintMode = isInBlueprintMode;
            // items.Add(furniture);

            // return new ListWrapper<Furniture>(items);
            return new Room();
        }
    }
}


