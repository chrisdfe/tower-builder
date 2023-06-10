using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Behaviors.Furnitures;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Behaviors.Furnitures
{
    using FurnitureBehaviorsListStateSlice = ListStateSlice<FurnitureBehavior>;

    public class State : FurnitureBehaviorsListStateSlice
    {
        public class Input { }

        public FurnitureBehaviorsListStateSlice.ItemEvent onInteractStart;
        public FurnitureBehaviorsListStateSlice.ItemEvent onInteractEnd;

        public State(AppState appState, Input input) : base(appState)
        {
        }

        public override void Setup()
        {
            appState.Entities.Furnitures.onItemsAdded += OnFurnituresAdded;
            appState.Entities.Furnitures.onItemsRemoved += OnFurnituresRemoved;
            appState.Entities.Furnitures.onItemsBuilt += OnFurnituresBuilt;
        }

        public override void Teardown()
        {
            appState.Entities.Furnitures.onItemsAdded -= OnFurnituresAdded;
            appState.Entities.Furnitures.onItemsRemoved -= OnFurnituresRemoved;
            appState.Entities.Furnitures.onItemsBuilt -= OnFurnituresBuilt;
        }

        /* 
            Public Interface
        */
        public override void Add(FurnitureBehavior behavior)
        {
            base.Add(behavior);
            behavior.Setup();
        }

        public override void Remove(FurnitureBehavior behavior)
        {
            base.Remove(behavior);
            behavior.Teardown();
        }

        public FurnitureBehavior AddFurnitureBehaviorForFurniture(Furniture furniture)
        {
            if (furniture.isInBlueprintMode) return null;

            FurnitureBehavior furnitureBehavior = (furniture.definition as FurnitureDefinition).furnitureBehaviorFactory(appState, furniture);
            Add(furnitureBehavior);

            return furnitureBehavior;
        }

        public void RemoveFurnitureBehaviorForFurniture(Furniture furniture)
        {
            FurnitureBehavior furnitureBehavior = FindByFurniture(furniture);

            if (furnitureBehavior != null)
            {
                Remove(furnitureBehavior);
            }
        }

        public bool StartInteraction(Resident resident, Furniture furniture)
        {
            FurnitureBehavior furnitureBehavior = FindByFurniture(furniture);

            if (furnitureBehavior != null)
            {
                bool wasSuccesful = furnitureBehavior.StartInteraction(resident);

                if (wasSuccesful)
                {
                    onInteractStart?.Invoke(furnitureBehavior);
                }

                return wasSuccesful;
            }

            return false;
        }

        public void EndInteraction(Resident resident, Furniture furniture)
        {
            FurnitureBehavior furnitureBehavior = FindByFurniture(furniture);

            if (furnitureBehavior != null)
            {
                furnitureBehavior.EndInteraction(resident);
                onInteractEnd?.Invoke(furnitureBehavior);
            }
        }

        public void InteractWithFurniture(Resident resident, Furniture furniture)
        {
            FurnitureBehavior furnitureBehavior = FindByFurniture(furniture);

            if (furnitureBehavior != null)
            {
                if (furnitureBehavior.interactingResidentsList.Contains(resident))
                {
                    bool wasSuccessful = furnitureBehavior.InteractTick(resident);

                    // TODO - maybe the notifications should get added here instead of in FurnitureBehavior itself
                    if (!wasSuccessful)
                    {
                        EndInteraction(resident, furniture);
                    }
                }
                else
                {
                    Debug.Log("this resident is not currently interacting with this furniture");
                }
            }
        }

        /*
            Queries
        */
        public FurnitureBehavior FindByFurniture(Furniture furniture) =>
            list.Find(behavior => behavior.furniture == furniture);

        /* 
            Event handlers
        */
        void OnFurnituresAdded(ListWrapper<Entity> furnitureList)
        {
            foreach (Entity furniture in furnitureList.items)
            {
                if (!furniture.isInBlueprintMode)
                {
                    AddFurnitureBehaviorForFurniture(furniture as Furniture);
                }
            }
        }

        void OnFurnituresRemoved(ListWrapper<Entity> furnitureList)
        {
            foreach (Entity furniture in furnitureList.items)
            {
                RemoveFurnitureBehaviorForFurniture(furniture as Furniture);
            }
        }

        void OnFurnituresBuilt(ListWrapper<Entity> furnitureList)
        {
            foreach (Entity furniture in furnitureList.items)
            {
                AddFurnitureBehaviorForFurniture(furniture as Furniture);
            }
        }
    }
}