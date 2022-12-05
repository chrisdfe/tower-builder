using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.GameWorld.Rooms
{
    public class GameWorldRoomsManager : MonoBehaviour, IFindable
    {
        public enum AssetKey
        {
            Room,
            RoomCell
        };

        public AssetList<AssetKey> assetList = new AssetList<AssetKey>();

        public enum MeshAssetKey
        {
            Default,
            Wheels
        };

        public AssetList<MeshAssetKey> meshAssetList = new AssetList<MeshAssetKey>();

        void Awake()
        {
            Debug.Log("hello.");
            ReplaceMaterials();
        }

        void ReplaceMaterials()
        {
            foreach (AssetList<MeshAssetKey>.ValueTypeWrapper wrapper in meshAssetList.assetList)
            {
                GameObject gameObject = wrapper.value;
                new MaterialsReplacer().ReplaceMaterials(gameObject.transform);
            }
        }

        /* 
            Static API
        */
        public static GameWorldRoomsManager Find()
        {
            return GameWorldFindableCache.Find<GameWorldRoomsManager>("RoomsManager");
        }
    }
}