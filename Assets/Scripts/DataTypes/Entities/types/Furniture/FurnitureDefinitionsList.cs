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
    public class FurnitureDefinitionsList : EntityDefinitionsList<Furniture.Key, FurnitureDefinition>
    {
        public override List<FurnitureDefinition> Definitions { get; } = new List<FurnitureDefinition>()
        {
            new FurnitureDefinition() {
                key = Furniture.Key.PilotSeat,
                title = "Pilot Seat",
                category = "PilotSeats",

                blockCellsTemplate = new CellCoordinatesList(
                    new List<CellCoordinates>() {
                        new CellCoordinates(0, 0),
                    }
                ),

                furnitureBehaviorFactory = (AppState appState, Furniture furniture) => new CockpitBehavior(appState, furniture),
            },

            new FurnitureDefinition() {
                key = Furniture.Key.Engine,
                title = "Engine",
                category = "Engines",

                blockCellsTemplate = new CellCoordinatesList(
                    new List<CellCoordinates>() {
                        new CellCoordinates(0, 0),
                    }
                ),

                furnitureBehaviorFactory = (AppState appState, Furniture furniture) => new EngineBehavior(appState, furniture),
            },

            new FurnitureDefinition() {
                key = Furniture.Key.Bed,
                title = "Bed",
                category = "Beds",
                homeSlotCount = 1,

                blockCellsTemplate = new CellCoordinatesList(
                    new List<CellCoordinates>() {
                        new CellCoordinates(0, 0),
                    }
                ),

                furnitureBehaviorFactory = (AppState appState, Furniture furniture) => new BedBehavior(appState, furniture),
            },

            new FurnitureDefinition() {
                key = Furniture.Key.MoneyMachine,
                title = "Money Machine",
                category = "Industry",

                blockCellsTemplate = new CellCoordinatesList(
                    new List<CellCoordinates>() {
                        new CellCoordinates(0, 0),
                    }
                ),

                furnitureBehaviorFactory = (AppState appState, Furniture furniture) => new MoneyMachineBehavior(appState, furniture),
            },
        };

        public FurnitureDefinitionsList() : base() { }
    }
}