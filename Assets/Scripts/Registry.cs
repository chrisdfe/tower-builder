using TowerBuilder.ApplicationState;
using TowerBuilder.Definitions;

namespace TowerBuilder
{
    public static class Registry
    {
        public static RoomDefinitions roomDefinitions = new RoomDefinitions();
        public static FurnitureDefinitions furnitureDefinitions = new FurnitureDefinitions();

        public static AppState appState = new AppState();
    }
}