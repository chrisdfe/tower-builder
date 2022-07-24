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
            public BuildingList buildings = new BuildingList();
        }

        public BuildingList buildings;

        public delegate void BuildingEvent(Building building);
        public BuildingEvent onBuildingAdded;
        public BuildingEvent onBuildingDestroyed;

        public State() : this(new Input()) { }

        public State(Input input)
        {
            if (input == null)
            {
                input = new Input();
            }

            this.buildings = input.buildings;
        }

        public void AddBuilding(Building building)
        {
            buildings.Add(building);

            if (onBuildingAdded != null)
            {
                onBuildingAdded(building);
            }
        }

        public void DestroyBuilding(Building building)
        {
            buildings.Remove(building);

            if (onBuildingDestroyed != null)
            {
                onBuildingDestroyed(building);
            }
        }
    }
}