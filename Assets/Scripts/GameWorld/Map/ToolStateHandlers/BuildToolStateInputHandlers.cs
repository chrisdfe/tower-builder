using System;
using TowerBuilder;
using TowerBuilder.GameWorld.Map.Blueprints;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.MapUI;
using UnityEngine;

namespace TowerBuilder.GameWorld.Map
{
    public class BuildToolStateInputHandlers : ToolStateInputHandlersBase
    {
        GameObject blueprintPrefab;
        public GameWorldBlueprint mapRoomBlueprint;

        public BuildToolStateInputHandlers(GameWorldMapManager parentMapManager) : base(parentMapManager)
        {
            blueprintPrefab = Resources.Load<GameObject>("Prefabs/Map/Blueprints/Blueprint");
        }

        public override void Update() { }

        public override void OnTransitionTo(ToolState previousToolState)
        {
            CreateBlueprint();
        }

        public override void OnTransitionFrom(ToolState nextToolState)
        {
            DestroyBlueprint();
            Registry.Stores.MapUI.buildToolSubState.Reset();
        }

        public override void OnMouseDown()
        {
            Registry.Stores.MapUI.buildToolSubState.StartBuild();
        }

        public override void OnMouseUp()
        {
            Registry.Stores.MapUI.buildToolSubState.EndBuild();
        }

        void CreateBlueprint()
        {
            GameObject blueprintGameObject = GameObject.Instantiate<GameObject>(blueprintPrefab);
            mapRoomBlueprint = blueprintGameObject.GetComponent<GameWorldBlueprint>();
            mapRoomBlueprint.transform.SetParent(parentMapManager.transform);

            // TODO - set this to current selected cell
            mapRoomBlueprint.transform.position = Vector3.zero;
        }

        void DestroyBlueprint()
        {
            GameObject.Destroy(mapRoomBlueprint.gameObject);
            mapRoomBlueprint = null;
        }
    }
}