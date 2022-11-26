using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.TransportationItems;
using UnityEngine;

namespace TowerBuilder.Definitions
{
    public class TransportationItemDefinitions
    {
        public Queries queries;

        public List<TransportationItemTemplate> definitions { get; private set; } = new List<TransportationItemTemplate>()
        {
            new TransportationItemTemplate() {
                key = "Escalator",
                title = "Escalator",
                category = "Escalators",
                cellCoordinatesList = new CellCoordinatesList(
                    new List<CellCoordinates>() {
                        new CellCoordinates(0, 0),
                        new CellCoordinates(1, 1)
                    }
                )
            },

            new TransportationItemTemplate() {
                key = "Ladder",
                title = "Ladder",
                category = "Ladders",
                cellCoordinatesList = new CellCoordinatesList(
                    new List<CellCoordinates>() {
                        new CellCoordinates(0, 0),
                        new CellCoordinates(0, 1)
                    }
                )
            },

            new TransportationItemTemplate() {
                key = "Doorway",
                title = "Doorway",
                category = "Doorways",
                cellCoordinatesList = new CellCoordinatesList(
                    new List<CellCoordinates>() {
                        new CellCoordinates(0, 0),
                        new CellCoordinates(1, 0)
                    }
                )
            }
        };

        public class Queries
        {
            List<TransportationItemTemplate> definitions;

            public Queries(List<TransportationItemTemplate> definitions)
            {
                this.definitions = definitions;
            }

            public TransportationItemTemplate FindByTitle(string title)
            {
                return definitions.Find(template => template.title == title);
            }

            public TransportationItemTemplate FindByKey(string key)
            {
                return definitions.Find(template => template.key == key);
            }

            public List<TransportationItemTemplate> FindByCategory(string category)
            {
                return definitions.FindAll(template => template.category == category).ToList();
            }

            public List<string> FindAllCategories()
            {
                List<string> result = new List<string>();

                foreach (TransportationItemTemplate template in definitions)
                {
                    if (!result.Contains(template.category))
                    {
                        result.Add(template.category);
                    }
                }

                return result;
            }
        }

        public TransportationItemDefinitions()
        {
            this.queries = new Queries(definitions);
        }
    }
}