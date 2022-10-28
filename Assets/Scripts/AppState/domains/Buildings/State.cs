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

        public List<Building> buildingList = new List<Building>();

        public delegate void BuildingListEvent(List<Building> buildingList);
        public BuildingListEvent onBuildingListUpdated;

        public delegate void BuildingListChangeEvent(List<Building> buildingList, Building building);
        public BuildingListChangeEvent onBuildingAdded;
        public BuildingListChangeEvent onBuildingRemoved;

        public State(Input input)
        {
            buildingList = input.buildingList ?? new List<Building>();
        }

        public State() : this(new Input()) { }
    }
}