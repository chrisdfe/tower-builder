using TowerBuilder.ApplicationState;
using TowerBuilder.Definitions;
using UnityEngine;

namespace TowerBuilder
{
    public static class Registry
    {
        public static AllDefinitions definitions = new AllDefinitions();
        public static AppState appState = new AppState();

        static Registry()
        {
            appState.Setup();
        }
    }
}