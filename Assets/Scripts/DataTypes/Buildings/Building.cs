using System.Threading;
using TowerBuilder.DataTypes.Rooms;

namespace TowerBuilder.DataTypes.Buildings
{
    public class Building
    {
        private static int autoincrementingId;

        public int id;
        public BuildingType type = BuildingType.Static;

        public Building()
        {
            GenerateId();
        }

        void GenerateId()
        {
            id = Interlocked.Increment(ref autoincrementingId);
        }
    }
}