using TowerBuilder.ApplicationState;
using TowerBuilder.Definitions;

namespace TowerBuilder
{
    public static class Registry
    {
        public static AllDefinitions definitions = new AllDefinitions();

        public static AppState appState = new AppState();
    }
}