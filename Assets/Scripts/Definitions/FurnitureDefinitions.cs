using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Furnitures.Behaviors;
using TowerBuilder.DataTypes.Entities.Furnitures.Validators;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.Definitions
{
    public class FurnitureDefinitions
    {
        public Queries queries;

        public List<FurnitureTemplate> definitions { get; private set; } = new List<FurnitureTemplate>()
        {
            new FurnitureTemplate() {
                key = "PilotSeat",
                title = "Pilot Seat",
                category = "PilotSeats",
                furnitureBehaviorFactory = (AppState appState, Furniture furniture) => new CockpitBehavior(appState, furniture),
                furnitureValidatorFactory = (Furniture furniture) => new CockpitFurnitureValidator(furniture)
            },

            new FurnitureTemplate() {
                key = "Engine",
                title = "Engine",
                category = "Engines",
                furnitureBehaviorFactory = (AppState appState, Furniture furniture) => new EngineBehavior(appState, furniture),
                furnitureValidatorFactory = (Furniture furniture) => new EngineFurnitureValidator(furniture)
            },

            new FurnitureTemplate() {
                key = "Bed",
                title = "Bed",
                category = "Beds",
                homeSlotCount = 1,
                furnitureBehaviorFactory = (AppState appState, Furniture furniture) => new BedBehavior(appState, furniture),
                furnitureValidatorFactory = (Furniture furniture) => new BedFurnitureValidator(furniture)
            },

            new FurnitureTemplate() {
                key = "MoneyMachine",
                title = "Money Machine",
                category = "Industry",
                furnitureBehaviorFactory = (AppState appState, Furniture furniture) => new MoneyMachineBehavior(appState, furniture),
                // furnitureValidatorFactory = (Furniture furniture) => new BedFurnitureValidator(furniture)
            },
        };

        public class Queries
        {
            List<FurnitureTemplate> definitions;

            public Queries(List<FurnitureTemplate> definitions)
            {
                this.definitions = definitions;
            }

            public FurnitureTemplate FindByKey(string key)
            {
                return definitions.Find(furnitureDefinition => furnitureDefinition.key == key);
            }

            public List<FurnitureTemplate> FindByCategory(string category)
            {
                return definitions.FindAll(furnitureDefinition => furnitureDefinition.category == category);
            }

            public List<string> FindAllCategories()
            {
                List<string> result = new List<string>();

                foreach (FurnitureTemplate furnitureTemplate in definitions)
                {
                    if (!result.Contains(furnitureTemplate.category))
                    {
                        result.Add(furnitureTemplate.category);
                    }
                }

                return result;
            }
        }

        public FurnitureDefinitions()
        {
            this.queries = new Queries(definitions);
        }
    }
}