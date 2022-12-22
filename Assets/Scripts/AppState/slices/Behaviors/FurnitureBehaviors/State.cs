using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Behaviors.Furnitures;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Behaviors.Furnitures
{
    using FurnitureBehaviorsListStateSlice = ListStateSlice<FurnitureBehaviorBase, State.Events>;

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

            public FurnitureBehaviorBase FindByFurniture(Furniture furniture) =>
                state.list.Find(behavior => behavior.furniture == furniture);
        }

        public Queries queries { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            queries = new Queries(this);
        }

        public override void Setup()
        {
            appState.Entities.Furnitures.events.onItemsAdded += OnFurnituresAdded;
            appState.Entities.Furnitures.events.onItemsRemoved += OnFurnituresRemoved;
            appState.Entities.Furnitures.events.onItemsBuilt += OnFurnituresBuilt;
        }

        public override void Teardown()
        {
            appState.Entities.Furnitures.events.onItemsAdded -= OnFurnituresAdded;
            appState.Entities.Furnitures.events.onItemsRemoved -= OnFurnituresRemoved;
            appState.Entities.Furnitures.events.onItemsBuilt -= OnFurnituresBuilt;
        }

        /* 
            Public Interface
        */
        public FurnitureBehaviorBase AddFurnitureBehaviorForFurniture(Furniture furniture)
        {
            if (furniture.isInBlueprintMode) return null;

            FurnitureBehaviorBase furnitureBehavior = (furniture.definition as FurnitureDefinition).furnitureBehaviorFactory(appState, furniture);
            Add(furnitureBehavior);

            return furnitureBehavior;
        }

        public void RemoveFurnitureBehaviorForFurniture(Furniture furniture)
        {
            FurnitureBehaviorBase furnitureBehavior = queries.FindByFurniture(furniture);

            if (furnitureBehavior != null)
            {
                Remove(furnitureBehavior);
            }
        }

        public FurnitureBehaviorBase StartInteraction(Resident resident, Furniture furniture)
        {
            FurnitureBehaviorBase furnitureBehavior = queries.FindByFurniture(furniture);

            if (furnitureBehavior != null)
            {
                furnitureBehavior.InteractStart(resident);
                events.onInteractStart?.Invoke(furnitureBehavior);
            }

            return furnitureBehavior;
        }

        public void EndInteraction(Resident resident, Furniture furniture)
        {
            FurnitureBehaviorBase furnitureBehavior = queries.FindByFurniture(furniture);

            if (furnitureBehavior != null)
            {
                furnitureBehavior.InteractEnd(resident);
                events.onInteractEnd?.Invoke(furnitureBehavior);
            }
        }

        public void InteractwithFurniture(Resident resident, Furniture furniture)
        {
            FurnitureBehaviorBase furnitureBehavior = queries.FindByFurniture(furniture);

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
        void OnFurnituresAdded(ListWrapper<Furniture> furnitureList)
        {
            foreach (Furniture furniture in furnitureList.items)
            {
                if (!furniture.isInBlueprintMode)
                {
                    AddFurnitureBehaviorForFurniture(furniture);
                }
            }
        }

        void OnFurnituresRemoved(ListWrapper<Furniture> furnitureList)
        {
            foreach (Furniture furniture in furnitureList.items)
            {
                RemoveFurnitureBehaviorForFurniture(furniture);
            }
        }

        void OnFurnituresBuilt(ListWrapper<Furniture> furnitureList)
        {
            foreach (Furniture furniture in furnitureList.items)
            {
                AddFurnitureBehaviorForFurniture(furniture);
            }
        }
    }
}