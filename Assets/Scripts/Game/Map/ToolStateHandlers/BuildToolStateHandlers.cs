using System;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.MapUI;
using UnityEngine;

namespace TowerBuilder.UI
{
    public class BuildToolStateHandlers : ToolStateHandlersBase
    {
        MapManager parentMapManager;

        GameObject mapRoomBlueprintPrefab;
        public MapRoomBlueprint mapRoomBlueprint;

        public BuildToolStateHandlers(MapManager parentMapManager) : base(parentMapManager)
        {
            this.parentMapManager = parentMapManager;

            mapRoomBlueprintPrefab = Resources.Load<GameObject>("Prefabs/MapUI/MapRoomBlueprint");
        }

        public override void Update() { }

        public override void OnTransitionTo(ToolState previousToolState)
        {
            CreateBlueprint();
        }

        public override void OnTransitionFrom(ToolState nextToolState)
        {
            DestroyBlueprint();
        }

        public override void OnMouseDown()
        {
            Registry.Stores.MapUI.StartBuild();
        }

        public override void OnMouseUp()
        {
            Registry.Stores.MapUI.EndBuild();
        }

        void CreateBlueprint()
        {
            mapRoomBlueprint = GameObject.Instantiate<GameObject>(mapRoomBlueprintPrefab).GetComponent<MapRoomBlueprint>();
            mapRoomBlueprint.transform.SetParent(parentMapManager.transform);

            // TODO - set this to current selected cell
            mapRoomBlueprint.transform.position = Vector3.zero;
        }

        void DestroyBlueprint()
        {
            GameObject.Destroy(mapRoomBlueprint.gameObject);
            mapRoomBlueprint = null;
        }
        // void UpdateMapRoomBlueprint()
        // {
        //     if (!mapRoomBlueprint.isEnabled)
        //     {
        //         return;
        //     }


        // }
    }
}