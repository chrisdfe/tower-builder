using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Furnitures.Behaviors;
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
                key = "cockpit",
                title = "cockpit",
                category = "cockpits",
                furnitureBehaviorFactory = (Furniture furniture) => new CockpitBehavior(furniture)
            },

            new FurnitureTemplate() {
                key = "engine",
                title = "engine",
                category = "engine",
                furnitureBehaviorFactory = (Furniture furniture) => new EngineBehavior(furniture)
            },
        };

        public class Queries
        {
            List<FurnitureTemplate> definitions;

            public Queries(List<FurnitureTemplate> definitions)
            {
                this.definitions = definitions;
            }

            public List<FurnitureTemplate> FindByCategory(string category)
            {
                return definitions.FindAll(furnitureDefinition => furnitureDefinition.category == category);
            }
        }

        public FurnitureDefinitions()
        {
            this.queries = new Queries(definitions);
        }
    }
}