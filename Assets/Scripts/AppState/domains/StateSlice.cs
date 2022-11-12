using System;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.State
{
    [Serializable]
    public class StateSlice
    {
        protected AppState appState;

        public StateSlice(AppState appState)
        {
            this.appState = appState;
        }
    }
}
