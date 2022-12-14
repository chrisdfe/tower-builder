using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Rooms
{
    public class RoomEntityMeshWrapper : EntityMeshWrapper
    {
        protected Dictionary<string, Transform> segments = new Dictionary<string, Transform>();

        public Room.Skin.Key skinKey { get; set; }

        // public RoomEntityMeshWrapper(Transform parent, GameObject prefabMesh, CellCoordinatesList cellCoordinatesList, Room.Skin.Key skinKey)
        //     : base(parent, prefabMesh, cellCoordinatesList)
        // {
        //     this.skinKey = skinKey;
        // }

        protected override EntityMeshCellWrapper CreateEntityCellMeshWrapper(Transform parent, GameObject prefabMesh, CellCoordinates cellCoordinates, CellNeighbors cellNeighbors, Tileable.CellPosition cellPosition) =>
            skinKey switch
            {
                Room.Skin.Key.Wheels => new RoomCellWheelsMeshWrapper(parent, prefabMesh, cellCoordinates, cellNeighbors, cellPosition),
                _ => new RoomCellDefaultMeshWrapper(parent, prefabMesh, cellCoordinates, cellNeighbors, cellPosition)
            };


        public override void Setup()
        {
            base.Setup();
        }
    }
}