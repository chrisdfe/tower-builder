using TowerBuilder.Definitions.Templates;
using TowerBuilder.State;

namespace TowerBuilder
{
    public static class Registry
    {
        public static RoomTemplates roomTemplates = new RoomTemplates();

        public static AppState appState = new AppState();
    }
}