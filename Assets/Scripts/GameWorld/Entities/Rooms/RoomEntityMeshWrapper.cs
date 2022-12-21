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

        public Room.SkinKey skinKey { get; set; }

        protected override EntityMeshCellWrapper CreateEntityCellMeshWrapper(
            Transform parent,
            GameObject prefabMesh,
            CellCoordinates cellCoordinates,
            CellCoordinates relatoveCellCoordinates,
            CellNeighbors cellNeighbors,
            Tileable.CellPosition cellPosition
        ) =>
            new RoomCellEntityMeshWrapper(parent, prefabMesh, cellCoordinates, relatoveCellCoordinates, cellNeighbors, cellPosition);

        public override void Setup()
        {
            base.Setup();
        }
    }
}