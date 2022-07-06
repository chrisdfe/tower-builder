using System;

namespace TowerBuilder.State.Furnitures
{
    [Serializable]
    public class State
    {
        public struct Input
        {
            // public BuildingList buildings;
            // public RoomConnections roomConnections;
        }

        public State() : this(new Input()) { }

        public State(Input input)
        {
            // buildings = input.buildings ?? new BuildingList();
            // roomConnections = input.roomConnections ?? new RoomConnections();
        }

        // public void AddFurniture() { }
    }
}
