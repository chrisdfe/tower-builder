using System;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.MapUI;
using UnityEngine;

namespace TowerBuilder.Game.Maps
{
    public class BuildToolStateInputHandlers : ToolStateInputHandlersBase
    {
        GameObject mapRoomBlueprintPrefab;
        public GameRoomBlueprint mapRoomBlueprint;

        public BuildToolStateInputHandlers(GameMapManager parentMapManager) : base(parentMapManager)
        {
            mapRoomBlueprintPrefab = Resources.Load<GameObject>("Prefabs/MapUI/GameRoomBlueprint");
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
            mapRoomBlueprint = GameObject.Instantiate<GameObject>(mapRoomBlueprintPrefab).GetComponent<GameRoomBlueprint>();
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