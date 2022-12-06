using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Furnitures.Behaviors;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.ApplicationState.FurnitureBehaviors
{
    using FurnitureBehaviorsListStateSlice = ListStateSlice<FurnitureBehaviorList, FurnitureBehaviorBase, State.Events>;

    public class State : FurnitureBehaviorsListStateSlice
    {
        public class Input { }

        public new class Events : FurnitureBehaviorsListStateSlice.Events
        {
            public FurnitureBehaviorsListStateSlice.Events.ItemEvent onInteractStart;
            public FurnitureBehaviorsListStateSlice.Events.ItemEvent onInteractEnd;
        }

        public class Queries
        {
            State state;

            public Queries(State state)
            {
                this.state = state;
            }
        }

        public Queries queries { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            queries = new Queries(this);

            Setup();
        }

        public override void Setup()
        {
            appState.Furnitures.events.onItemsAdded += OnFurnituresAdded;
            appState.Furnitures.events.onItemsRemoved += OnFurnituresRemoved;
            appState.Furnitures.events.onItemBuilt += OnFurnituresBuilt;
        }

        public override void Teardown()
        {
            appState.Furnitures.events.onItemsAdded -= OnFurnituresAdded;
            appState.Furnitures.events.onItemsRemoved -= OnFurnituresRemoved;
            appState.Furnitures.events.onItemBuilt -= OnFurnituresBuilt;
        }

        /* 
            Public Interface
        */
        public FurnitureBehaviorBase AddFurnitureBehaviorForFurniture(Furniture furniture)
        {
            if (furniture.isInBlueprintMode) return null;

            FurnitureBehaviorBase furnitureBehavior = furniture.template.furnitureBehaviorFactory(appState, furniture);
            Add(furnitureBehavior);

            return furnitureBehavior;
        }

        public void RemoveFurnitureBehaviorForFurniture(Furniture furniture)
        {
            FurnitureBehaviorBase furnitureBehavior = list.FindByFurniture(furniture);

            if (furnitureBehavior != null)
            {
                Remove(furnitureBehavior);
            }
        }

        public FurnitureBehaviorBase StartInteraction(Resident resident, Furniture furniture)
        {
            FurnitureBehaviorBase furnitureBehavior = list.FindByFurniture(furniture);

            if (furnitureBehavior != null)
            {
                furnitureBehavior.InteractStart(resident);
                events.onInteractStart?.Invoke(furnitureBehavior);
            }

            return furnitureBehavior;
        }

        public void EndInteraction(Resident resident, Furniture furniture)
        {
            FurnitureBehaviorBase furnitureBehavior = list.FindByFurniture(furniture);

            if (furnitureBehavior != null)
            {
                furnitureBehavior.InteractEnd(resident);
                events.onInteractEnd?.Invoke(furnitureBehavior);
            }
        }

        public void InteractwithFurniture(Resident resident, Furniture furniture)
        {
            FurnitureBehaviorBase furnitureBehavior = list.FindByFurniture(furniture);

            if (furnitureBehavior != null)
            {
                if (furnitureBehavior.interactingResidentsList.Contains(resident))
                {
                    furnitureBehavior.InteractTick(resident);
                }
                else
                {
                    Debug.Log("this resident is not currently interacting with this furniture");
                }
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