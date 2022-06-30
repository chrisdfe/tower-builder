using System;
using TowerBuilder;
using TowerBuilder.GameWorld.Rooms.Blueprints;
using TowerBuilder.State;
using TowerBuilder.State.Rooms;
using TowerBuilder.State.UI;
using UnityEngine;

namespace TowerBuilder.GameWorld.Map.MapManager
{
    public class BuildToolStateInputHandlers : ToolStateInputHandlersBase
    {
        GameObject blueprintPrefab;
        public GameWorldBlueprint gameWorldBlueprint;

        public BuildToolStateInputHandlers(GameWorldMapManager parentMapManager) : base(parentMapManager)
        {
            blueprintPrefab = Resources.Load<GameObject>("Prefabs/Map/Blueprints/Blueprint");
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
            GameObject blueprintGameObject = GameObject.Instantiate<GameObject>(blueprintPrefab);
            blueprintGameObject.transform.parent = parentMapManager.transform;

            gameWorldBlueprint = blueprintGameObject.GetComponent<GameWorldBlueprint>();
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