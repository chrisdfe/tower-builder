using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.DataTypes.Entities.Furnitures;
using UnityEngine;

namespace TowerBuilder.DataTypes.EntityGroups.Rooms.Builders
{
    public class BedroomRoomBuilder : EntityGroupBuilderBase<Room>
    {
        public BedroomRoomBuilder() : base() { }

        public override Room Build(bool isInBlueprintMode)
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


