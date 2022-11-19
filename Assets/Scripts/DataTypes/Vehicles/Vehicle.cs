using System.Threading;
using TowerBuilder.DataTypes.Rooms;

namespace TowerBuilder.DataTypes.Vehicles
{
    public class Vehicle
    {
        public int id = UIDGenerator.Generate("vehicle");
        public RoomList roomList { get; private set; } = new RoomList();
    }
}