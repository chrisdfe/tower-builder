using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Furnitures.Behaviors;
using TowerBuilder.DataTypes.Residents;
using UnityEngine;

namespace TowerBuilder.ApplicationState.FurnitureBehaviors
{
    public class State : StateSlice
    {
        public class Input { }

        public class Events
        {
            public delegate void FurnitureBehaviorEvent(FurnitureBehaviorBase furnitureBehavior);
            public FurnitureBehaviorEvent onFurnitureBehaviorAdded;
            public FurnitureBehaviorEvent onFurnitureBehaviorRemoved;
        }

        public class Queries
        {
            State state;
            public Queries(State state)
            {
                this.state = state;
            }
        }

        public FurnitureBehaviorList furnitureBehaviorList { get; private set; } = new FurnitureBehaviorList();

        public Events events { get; private set; }
        public Queries queries { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            events = new Events();
            queries = new Queries(this);

            Setup();
        }

        public void Setup()
        {
            appState.Furnitures.events.onFurnituresAdded += OnFurnituresAdded;
            appState.Furnitures.events.onFurnituresRemoved += OnFurnituresRemoved;
            appState.Furnitures.events.onFurnituresBuilt += OnFurnituresBuilt;
        }

        public void Teardown()
        {
            appState.Furnitures.events.onFurnituresAdded -= OnFurnituresAdded;
            appState.Furnitures.events.onFurnituresRemoved -= OnFurnituresRemoved;
            appState.Furnitures.events.onFurnituresBuilt -= OnFurnituresBuilt;
        }

        /* 
            Public Interface
        */
        public void AddFurnitureBehavior(FurnitureBehaviorBase furnitureBehavior)
        {
            furnitureBehaviorList.Add(furnitureBehavior);

            if (events.onFurnitureBehaviorAdded != null)
            {
                events.onFurnitureBehaviorAdded(furnitureBehavior);
            }
        }

        public void RemoveFurnitureBehavior(FurnitureBehaviorBase furnitureBehavior)
        {
            furnitureBehaviorList.Remove(furnitureBehavior);

            if (events.onFurnitureBehaviorRemoved != null)
            {
                events.onFurnitureBehaviorRemoved(furnitureBehavior);
            }
        }

        public FurnitureBehaviorBase AddFurnitureBehaviorForFurniture(Furniture furniture)
        {
            if (furniture.isInBlueprintMode) return null;
            // TODO - use "behavior builder" pattern like with validators
            FurnitureBehaviorBase furnitureBehavior = new DefaultBehavior(furniture);
            AddFurnitureBehavior(furnitureBehavior);
            return furnitureBehavior;
        }

        public void RemoveFurnitureBehaviorForFurniture(Furniture furniture)
        {
            FurnitureBehaviorBase furnitureBehavior = furnitureBehaviorList.FindByFurniture(furniture);

            if (furnitureBehavior != null)
            {
                RemoveFurnitureBehavior(furnitureBehavior);
            }
        }

        public FurnitureBehaviorBase StartFurnitureBehaviorInteraction(Resident resident, Furniture furniture)
        {
            FurnitureBehaviorBase furnitureBehavior = furnitureBehaviorList.FindByFurniture(furniture);

            if (furnitureBehavior != null)
            {
                furnitureBehavior.InteractStart(resident);
            }

            return furnitureBehavior;
        }

        public void EndFurnitureBehaviorInteraction(Resident resident, Furniture furniture)
        {
            FurnitureBehaviorBase furnitureBehavior = furnitureBehaviorList.FindByFurniture(furniture);

            if (furnitureBehavior != null)
            {
                furnitureBehavior.InteractEnd(resident);
            }
        }

        /* 
            Event handlers
        */
        void OnFurnituresAdded(FurnitureList furnitureList)
        {
            foreach (Furniture furniture in furnitureList.items)
            {
                if (!furniture.isInBlueprintMode)
                {
                    AddFurnitureBehaviorForFurniture(furniture);
                }
            }
        }

        void OnFurnituresRemoved(FurnitureList furnitureList)
        {
            foreach (Furniture furniture in furnitureList.items)
            {
                RemoveFurnitureBehaviorForFurniture(furniture);
            }
        }

        void OnFurnituresBuilt(FurnitureList furnitureList)
        {
            foreach (Furniture furniture in furnitureList.items)
            {
                AddFurnitureBehaviorForFurniture(furniture);
            }
        }
    }
}