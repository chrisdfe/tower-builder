using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Map;

using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.Furniture.Definitions
{
    public enum ElevatorCarPosition
    {
        Top,
        Bottom
    }

    public class ElevatorCarConfig : RoomConfigBase
    {
        public int occupancy;
        public float speed;
    }

    public class ElevatorCar : RoomFurnitureBase
    {
        public int occupancy { get; private set; }
        public float speed { get; private set; }
        public ElevatorCarPosition position = ElevatorCarPosition.Top;

        public override RoomFurnitureCategory category { get { return RoomFurnitureCategory.Transportation; } }

        public ElevatorCar(Room room, ElevatorCarConfig config) : base(room, config)
        {
            this.occupancy = config.occupancy;
            this.speed = config.speed;
        }

        public override void Initialize() { }
        public override void OnDestroy() { }
    }
}