using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Furnitures.Behaviors;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.DataTypes.Residents.Behaviors
{
    public class InteractingWithFurnitureStateHandler : StateHandlerBase
    {
        public class TransitionPayload : TransitionPayloadBase
        {
            public Furniture furniture;
        }

        public Furniture furniture;

        FurnitureBehaviorBase furnitureBehavior;

        public InteractingWithFurnitureStateHandler(ResidentBehavior residentBehavior) : base(residentBehavior)
        {
        }

        public void Setup(AppState appState, TransitionPayload payload)
        {
            base.Setup(appState);

            this.furniture = payload.furniture;

            this.furnitureBehavior = appState.FurnitureBehaviors.StartFurnitureBehaviorInteraction(residentBehavior.resident, furniture);
        }

        public override void Teardown()
        {
            appState.FurnitureBehaviors.EndFurnitureBehaviorInteraction(residentBehavior.resident, furniture);

            base.Teardown();
        }

        public override TransitionPayloadBase GetNextState()
        {
            if (residentBehavior.goalQueue.Count > 1)
            {
                residentBehavior.CompleteCurrentGoal();
                return residentBehavior.GetNextGoalTransitionPayload();
            }

            return null;
        }

        public override void ProcessTick()
        {
            furnitureBehavior.InteractTick(appState);
        }
    }
}