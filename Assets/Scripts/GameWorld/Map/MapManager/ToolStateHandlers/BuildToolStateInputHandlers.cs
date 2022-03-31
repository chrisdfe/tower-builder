using System;
using TowerBuilder;
using TowerBuilder.GameWorld.Rooms.Blueprints;
using TowerBuilder.Stores;

using TowerBuilder.Stores.Rooms;
using TowerBuilder.Stores.UI;
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

            Registry.Stores.Rooms.onRoomAdded += OnRoomAdded;
        }

        public override void OnTransitionFrom(ToolState nextToolState)
        {
            DestroyBlueprint();

            Registry.Stores.Rooms.onRoomAdded -= OnRoomAdded;
        }

        public override void OnMouseDown()
        {
            Registry.Stores.UI.buildToolSubState.StartBuild();
        }

        public override void OnMouseUp()
        {
            Registry.Stores.UI.buildToolSubState.EndBuild();
        }

        void OnRoomAdded(Room room)
        {
            if (room.id == Registry.Stores.UI.buildToolSubState.currentBlueprint.room.id)
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