using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Buildings;
using UnityEngine;

namespace TowerBuilder.State.Buildings
{
    public class State
    {
        public class Input
        {
            public List<Building> buildingList;
        }

        public class Events
        {
            public delegate void BuildingListEvent(List<Building> buildingList);
            public BuildingListEvent onBuildingListUpdated;

            public delegate void BuildingListChangeEvent(Building building);
            public BuildingListChangeEvent onBuildingAdded;
            public BuildingListChangeEvent onBuildingRemoved;
        }

        public List<Building> buildingList = new List<Building>();

        public Events events;

        public State(Input input)
        {
            buildingList = input.buildingList ?? new List<Building>();
            events = new Events();
        }

        public State() : this(new Input()) { }

        public void AddBuilding(Building building)
        {
            buildingList.Add(building);

            if (events.onBuildingAdded != null)
            {
                events.onBuildingAdded(building);
            }

            if (events.onBuildingListUpdated != null)
            {
                events.onBuildingListUpdated(buildingList);
            }
        }

        public void RemoveBuilding(Building building)
        {
            buildingList.Remove(building);

            if (events.onBuildingRemoved != null)
            {
                events.onBuildingRemoved(building);
            }

            if (events.onBuildingListUpdated != null)
            {
                events.onBuildingListUpdated(buildingList);
            }
        }
    }
}