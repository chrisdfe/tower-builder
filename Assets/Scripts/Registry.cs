using TowerBuilder.State;
using TowerBuilder.Templates;

namespace TowerBuilder
{
    public static class Registry
    {
        public static AppState appState = new AppState();
        public static RoomTemplates roomTemplates = new RoomTemplates();
    }
}