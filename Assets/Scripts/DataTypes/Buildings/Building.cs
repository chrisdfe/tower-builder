using System.Threading;
using TowerBuilder.DataTypes.Rooms;

namespace TowerBuilder.DataTypes.Buildings
{
    public class Building
    {
        public int id = UIDGenerator.Generate("building");
        public BuildingType type = BuildingType.Static;
    }
}