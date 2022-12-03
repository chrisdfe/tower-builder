using TowerBuilder.GameWorld.Lights;
using UnityEngine;

namespace TowerBuilder.GameWorld.Rooms
{
    public class GameWorldRoomCellLight
    {
        Light light;
        LightsManager lightsManager;

        public GameWorldRoomCellLight(Light light)
        {
            this.light = light;
            lightsManager = LightsManager.Find();
        }

        public void Setup()
        {
            lightsManager.interiorLights.Add(light);
        }

        public void Teardown()
        {
            lightsManager.interiorLights.Remove(light);
        }
    }
}