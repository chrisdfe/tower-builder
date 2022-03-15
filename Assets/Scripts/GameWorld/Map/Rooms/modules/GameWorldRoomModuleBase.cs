namespace TowerBuilder.GameWorld.Map.Rooms.Modules
{
    public abstract class GameWorldRoomModuleBase
    {
        public GameWorldRoom gameWorldRoom { get; private set; }

        public GameWorldRoomModuleBase(GameWorldRoom gameWorldRoom)
        {
            this.gameWorldRoom = gameWorldRoom;
        }

        public abstract void Initialize();
    }
}