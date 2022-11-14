using TowerBuilder.Definitions;
using TowerBuilder.State;

namespace TowerBuilder
{
    public static class Registry
    {
        public static RoomDefinitions roomDefinitions = new RoomDefinitions();
        public static FurnitureDefinitions furnitureDefinitions = new FurnitureDefinitions();

        public static AppState appState = new AppState();
    }
}