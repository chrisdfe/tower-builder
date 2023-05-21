using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using TowerBuilder.DataTypes.EntityGroups.Rooms.Validators;
using UnityEngine;

namespace TowerBuilder.Definitions
{
    // public class RoomDefinitionsList : EntityDefinitionsList<Room.Key, RoomDefinition>
    // {
    // public override List<RoomDefinition> Definitions { get; } = new List<RoomDefinition>() {
    //     new RoomDefinition()
    //     {
    //         title = "Empty",
    //         key = Room.Key.Default,
    //         category = "Empty",

    //         blockCellsTemplate = new CellCoordinatesList(
    //             new List<CellCoordinates>() {
    //                 new CellCoordinates(0, 0),
    //             }
    //         ),

    //         resizability = Resizability.Flexible,
    //         pricePerCell = 1000,
    //     },

    //     new RoomDefinition()
    //     {
    //         title = "Empty - Tall",
    //         key = Room.Key.OtherDefault,
    //         category = "Empty - Tall",

    //         blockCellsTemplate = CellCoordinatesList.CreateRectangle(1, 5),

    //         resizability = Resizability.Flexible,
    //         pricePerCell = 1000,
    //     },

    //     new RoomDefinition()
    //     {
    //         title = "Empty - Large",
    //         key = Room.Key.OtherDefault,
    //         category = "Empty",

    //         blockCellsTemplate = CellCoordinatesList.CreateRectangle(2, 2),

    //         resizability = Resizability.Flexible,
    //         pricePerCell = 1000,
    //     },
    // };

    //     public RoomDefinitionsList() : base() { }
    // }
}
