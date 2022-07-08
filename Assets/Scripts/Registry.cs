using TowerBuilder.State;
using TowerBuilder.Templates;

namespace TowerBuilder
{
    public static class Registry
    {
        public static RoomTemplates roomTemplates = new RoomTemplates();

        public static AppState appState = new AppState();
    }
}