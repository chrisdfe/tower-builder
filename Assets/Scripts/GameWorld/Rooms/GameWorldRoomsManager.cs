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

        public static GameWorldRoomsManager Find()
        {
            return GameWorldFindableCache.Find<GameWorldRoomsManager>("RoomsManager");
        }
    }
}