using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Buildings;
using UnityEngine;

namespace TowerBuilder.State.Buildings
{
    public class State : ResourceList<Building>
    {
        public State() : base() { }
        public State(Input input) : base(input) { }
    }
}