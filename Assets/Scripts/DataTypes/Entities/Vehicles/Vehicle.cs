using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities.Rooms;

namespace TowerBuilder.DataTypes.Entities.Vehicles
{
    public class Vehicle : Entity<Vehicle.Key>
    {
        public enum Key
        {
            Default,
        }

        public static EnumStringMap<Key> KeyLabelMap = new EnumStringMap<Key>(
            new Dictionary<Key, string>() {
                { Key.Default, "Default" },
            }
        );

        public override string idKey => "vehicles";

        public ListWrapper<Room> roomList { get; private set; } = new ListWrapper<Room>();

        public bool isPiloted { get; set; }

        public Vehicle(VehicleDefinition vehicleDefinition) : base(vehicleDefinition) { }
    }
}