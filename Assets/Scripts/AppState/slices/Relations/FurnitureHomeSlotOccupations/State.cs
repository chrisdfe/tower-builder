using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Relations;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Relations.FurnitureHomeSlotOccupations
{
    using FurnitureHomeSlotOccupiationStateSlice = ListStateSlice<FurnitureHomeSlotOccupation, State.Events>;

    public class State : FurnitureHomeSlotOccupiationStateSlice
    {
        public class Input { }

        public new class Events : FurnitureHomeSlotOccupiationStateSlice.Events { }

        public class Queries
        {
            State state;

            public Queries(State state)
            {
                this.state = state;
            }

            public Furniture GetHomeFurnitureFor(Resident resident) =>
                state.list.Find(furnitureHomeSlotOccupation => furnitureHomeSlotOccupation.resident == resident)?.furniture;

            public ListWrapper<Resident> GetResidentsLivingAt(Furniture furniture) =>
                new ListWrapper<Resident>(
                    state.list.items
                        .FindAll(furnitureHomeSlotOccupation => furnitureHomeSlotOccupation.furniture == furniture)
                        .Select(furnitureHomeSlotOccupation => furnitureHomeSlotOccupation.resident)
                        .ToList()
                );
        }

        public Queries queries { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            queries = new Queries(this);
        }

        public override void Setup() { }

        public override void Teardown() { }
    }
}