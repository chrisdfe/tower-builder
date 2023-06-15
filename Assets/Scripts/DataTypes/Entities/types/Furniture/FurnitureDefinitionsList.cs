using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Behaviors.Furnitures;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Furnitures
{
    public class FurnitureDefinitionsList : EntityDefinitionsList
    {
        public override List<EntityDefinition> Definitions { get; } = new List<EntityDefinition>()
        {
            new FurnitureDefinition() {
                key = "PilotSeat",
                title = "Pilot Seat",
                category = "PilotSeats",

                blockCellsTemplate = CellCoordinatesList.CreateRectangle(1, 1),

                furnitureBehaviorFactory = (AppState appState, Furniture furniture) => new CockpitBehavior(appState, furniture),
            },

            new FurnitureDefinition() {
                key = "Engine",
                title = "Engine",
                category = "Engines",

                blockCellsTemplate = CellCoordinatesList.CreateRectangle(1, 1),

                furnitureBehaviorFactory = (AppState appState, Furniture furniture) => new EngineBehavior(appState, furniture),
            },

            new FurnitureDefinition() {
                key = "Bed",
                title = "Bed",
                category = "Beds",
                homeSlotCount = 1,

                blockCellsTemplate = CellCoordinatesList.CreateRectangle(2, 1),

                furnitureBehaviorFactory = (AppState appState, Furniture furniture) => new BedBehavior(appState, furniture),
            },

            new FurnitureDefinition() {
                key = "MoneyMachine",
                title = "Money Machine",
                category = "Industry",

                blockCellsTemplate = CellCoordinatesList.CreateRectangle(1, 1),

                furnitureBehaviorFactory = (AppState appState, Furniture furniture) => new MoneyMachineBehavior(appState, furniture),
            },
        };

        public FurnitureDefinitionsList() : base() { }
    }
}