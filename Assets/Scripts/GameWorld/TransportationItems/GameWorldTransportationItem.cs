using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.TransportationItems;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.GameWorld.Rooms
{
    public class GameWorldTransportationItem : MonoBehaviour
    {
        public TransportationItem transportationItem;

        Transform meshWrapper;
        Transform cube;

        /*
            Lifecycle Methods
        */
        void Awake()
        {
            transform.localPosition = Vector3.zero;

            meshWrapper = transform.Find("MeshWrapper");
            cube = meshWrapper.Find("Cube");
        }

        void OnDestroy()
        {
        }

        public void Setup()
        {
            UpdatePosition();
            CreateMesh();
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
            transform.localPosition = GameWorldMapCellHelpers.CellCoordinatesToPosition(transportationItem.cellCoordinatesList.items[0], 1f);
        }

        /* 
            Internals
        */
        void CreateMesh()
        {
            TransformUtils.DestroyChildren(meshWrapper);

            // for now just copy the cube into every other cell
            foreach (CellCoordinates cellCoordinates in transportationItem.cellCoordinatesList.items)
            {
                Transform cubeClone = Instantiate(cube, GameWorldMapCellHelpers.CellCoordinatesToPosition(cellCoordinates), Quaternion.identity);
                cubeClone.SetParent(transform);
            }
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