using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Attributes
{
    public class State : StateSlice
    {
        public State(AppState appState) : base(appState)
        {

        }
    }
}