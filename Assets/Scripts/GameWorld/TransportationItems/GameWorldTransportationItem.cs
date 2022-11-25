using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.TransportationItems;
using UnityEngine;

namespace TowerBuilder.GameWorld.Rooms
{
    public class GameWorldTransportationItem : MonoBehaviour
    {
        public TransportationItem transportationItem;

        /*
            Lifecycle Methods
        */
        void Awake()
        {
            transform.localPosition = Vector3.zero;
        }

        void OnDestroy()
        {
        }

        public void Setup()
        {
            UpdatePosition();
        }

        public void Teardown()
        {
        }

        public void Reset()
        {
        }

        public void OnBuild()
        {
        }

        /* 
            Public Interface
        */
        public void UpdatePosition()
        {
            float TILE_SIZE = DataTypes.Rooms.Constants.TILE_SIZE;

            transform.localPosition = GameWorldMapCellHelpers.CellCoordinatesToPosition(transportationItem.cellCoordinatesList.items[0]);
        }


        /* 
            Static API
         */
        public static GameWorldTransportationItem Create(Transform parent)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Map/TransportationItems/TransportationItem");
            GameObject roomGameObject = Instantiate<GameObject>(prefab);

            roomGameObject.transform.parent = parent;

            GameWorldTransportationItem gameWorldTransportationItem = roomGameObject.GetComponent<GameWorldTransportationItem>();
            return gameWorldTransportationItem;
        }
    }
}