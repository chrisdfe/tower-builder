using System;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.ApplicationState
{
    [Serializable]
    public class StateSlice : ISetupable
    {
        protected AppState appState;

        public StateSlice(AppState appState)
        {
            this.appState = appState;
        }

        public virtual void Setup() { }
        public virtual void Teardown() { }
    }
}
