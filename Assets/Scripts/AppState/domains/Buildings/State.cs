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

        public ResourceList<Building> buildingList = new ResourceList<Building>();

        public State(Input input)
        {
            buildingList.Set(input.buildingList ?? new List<Building>());
        }

        public State() : this(new Input()) { }
    }
}