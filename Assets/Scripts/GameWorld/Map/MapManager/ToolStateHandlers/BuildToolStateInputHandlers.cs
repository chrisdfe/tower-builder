using System;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.GameWorld.Rooms.Blueprints;
using TowerBuilder.State.UI;
using UnityEngine;

namespace TowerBuilder.GameWorld.Map.MapManager
{
    public class BuildToolStateInputHandlers : ToolStateInputHandlersBase
    {
        public GameWorldBlueprint gameWorldBlueprint;

        public BuildToolStateInputHandlers(GameWorldMapManager parentMapManager) : base(parentMapManager)
        {
        }

        public override void Update() { }

        public override void OnTransitionTo(ToolState previousToolState)
        {
            CreateBlueprint();

            Registry.appState.Rooms.onRoomAdded += OnRoomAdded;
        }

        public override void OnTransitionFrom(ToolState nextToolState)
        {
            DestroyBlueprint();

            Registry.appState.Rooms.onRoomAdded -= OnRoomAdded;
        }

        public override void OnMouseDown()
        {
            Registry.appState.UI.buildToolSubState.StartBuild();
        }

        public override void OnMouseUp()
        {
            Registry.appState.UI.buildToolSubState.EndBuild();
        }

        void OnRoomAdded(Room room)
        {
            if (room.id == Registry.appState.UI.buildToolSubState.currentBlueprint.room.id)
            {
                ResetBlueprint();
            }
        }

        void CreateBlueprint()
        {
            gameWorldBlueprint = GameWorldBlueprint.Create();
            gameWorldBlueprint.transform.parent = parentMapManager.transform;
            gameWorldBlueprint.transform.SetParent(parentMapManager.transform);

            // TODO - set this to current selected cell
            gameWorldBlueprint.transform.position = Vector3.zero;
        }

        void DestroyBlueprint()
        {
            GameObject.Destroy(gameWorldBlueprint.gameObject);
            gameWorldBlueprint = null;
        }

        void ResetBlueprint()
        {
            DestroyBlueprint();
            CreateBlueprint();
        }
    }
}