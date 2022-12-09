using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Furnitures.Behaviors;
using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.Definitions
{
    public class FurnitureDefinitionsList : EntityDefinitionsList<Furniture.Key, Furniture, FurnitureTemplate>
    {
        public override List<FurnitureTemplate> Definitions { get; } = new List<FurnitureTemplate>()
        {
            new FurnitureTemplate() {
                key = Furniture.Key.PilotSeat,
                title = "Pilot Seat",
                category = "PilotSeats",
                furnitureBehaviorFactory = (AppState appState, Furniture furniture) => new CockpitBehavior(appState, furniture),
            },

            new FurnitureTemplate() {
                key = Furniture.Key.Engine,
                title = "Engine",
                category = "Engines",
                furnitureBehaviorFactory = (AppState appState, Furniture furniture) => new EngineBehavior(appState, furniture),
            },

            new FurnitureTemplate() {
                key = Furniture.Key.Bed,
                title = "Bed",
                category = "Beds",
                homeSlotCount = 1,
                furnitureBehaviorFactory = (AppState appState, Furniture furniture) => new BedBehavior(appState, furniture),
            },

            new FurnitureTemplate() {
                key = Furniture.Key.MoneyMachine,
                title = "Money Machine",
                category = "Industry",
                furnitureBehaviorFactory = (AppState appState, Furniture furniture) => new MoneyMachineBehavior(appState, furniture),
            },
        };

        public FurnitureDefinitionsList() : base() { }
    }
}