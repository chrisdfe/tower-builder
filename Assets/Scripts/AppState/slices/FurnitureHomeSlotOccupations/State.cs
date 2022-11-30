using System;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Residents;
using UnityEngine;

namespace TowerBuilder.ApplicationState.FurnitureHomeSlotOccupations
{
    public class State : StateSlice
    {
        public class Input { }

        public class Events
        {
            public delegate void FurnitureHomeSlotOccupationEvent(FurnitureHomeSlotOccupation furnitureHomeSlotOccupation);
            public FurnitureHomeSlotOccupationEvent onFurnitureHomeSlotOccupationAdded;
            public FurnitureHomeSlotOccupationEvent onFurnitureHomeSlotOccupationRemoved;
        }

        public class Queries
        {
            State state;

            public Queries(State state)
            {
                this.state = state;
            }

            public Furniture GetHomeFurnitureFor(Resident resident)
            {
                return state.furnitureHomeSlotOccupationList.GetHomeFurnitureFor(resident);
            }

            public ResidentsList GetResidentsLivingAt(Furniture furniture)
            {
                return state.furnitureHomeSlotOccupationList.GetResidentsLivingAt(furniture);
            }
        }

        public FurnitureHomeSlotOccupationList furnitureHomeSlotOccupationList { get; private set; } = new FurnitureHomeSlotOccupationList();

        public Events events { get; private set; }
        public Queries queries { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            events = new Events();
            queries = new Queries(this);

            Setup();
        }

        public void Setup() { }

        public void Teardown() { }

        /* 
            Public Interface
         */
        public void AddFurnitureHomeSlotOccupation(FurnitureHomeSlotOccupation FurnitureHomeSlotOccupation)
        {
            furnitureHomeSlotOccupationList.Add(FurnitureHomeSlotOccupation);

            if (events.onFurnitureHomeSlotOccupationAdded != null)
            {
                events.onFurnitureHomeSlotOccupationAdded(FurnitureHomeSlotOccupation);
            }
        }

        public void RemoveFurnitureHomeSlotOccupation(FurnitureHomeSlotOccupation FurnitureHomeSlotOccupation)
        {
            furnitureHomeSlotOccupationList.Remove(FurnitureHomeSlotOccupation);

            if (events.onFurnitureHomeSlotOccupationRemoved != null)
            {
                events.onFurnitureHomeSlotOccupationRemoved(FurnitureHomeSlotOccupation);
            }
        }
    }
}