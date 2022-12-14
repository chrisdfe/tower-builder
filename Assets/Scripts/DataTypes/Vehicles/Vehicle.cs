using System.Threading;
using TowerBuilder.DataTypes.Entities.Rooms;

namespace TowerBuilder.DataTypes.Vehicles
{
    public class Vehicle
    {
        public int id = UIDGenerator.Generate("vehicle");
        public ListWrapper<Room> roomList { get; private set; } = new ListWrapper<Room>();
    }
}