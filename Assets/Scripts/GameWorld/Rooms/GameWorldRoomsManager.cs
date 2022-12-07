using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Rooms;
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

        public AssetList<AssetKey> prefabAssets = new AssetList<AssetKey>();

        public enum MeshAssetKey
        {
            Default,
            Wheels
        };

        public MeshAssetList<MeshAssetKey> meshAssets = new MeshAssetList<MeshAssetKey>();

        void Awake()
        {
            meshAssets.ReplaceMaterials();
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