using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities.Residents;

namespace TowerBuilder.DataTypes.Entities.Furnitures
{
    public class MoneyMachineBehavior : FurnitureBehavior
    {
        public override Key key { get; } = FurnitureBehavior.Key.MoneyMachine;

        public MoneyMachineBehavior(AppState appState, Furniture furniture) : base(appState, furniture) { }

        protected override void OnInteractTick(Resident resident)
        {
            appState.Wallet.AddBalance(1000);
        }
    }
}