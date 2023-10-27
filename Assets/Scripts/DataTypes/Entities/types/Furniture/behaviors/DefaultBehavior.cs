using TowerBuilder.ApplicationState;

namespace TowerBuilder.DataTypes.Entities.Furnitures
{
    public class DefaultBehavior : FurnitureBehavior
    {
        public override Key key { get; } = FurnitureBehavior.Key.Default;

        public DefaultBehavior(AppState appState, Furniture furniture) : base(appState, furniture) { }
    }
}