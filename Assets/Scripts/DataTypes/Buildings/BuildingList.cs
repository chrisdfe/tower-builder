using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes.Rooms;

namespace TowerBuilder.DataTypes.Buildings
{
    public class BuildingList
    {
        public List<Building> buildings { get; private set; } = new List<Building>();

        public int Count
        {
            get
            {
                return buildings.Count;
            }
        }

        // Creation
        public void Add(Building building)
        {
            buildings.Add(building);
        }

        // Deletion
        public void Remove(Building building)
        {
            buildings.Remove(building);
        }
    }
}