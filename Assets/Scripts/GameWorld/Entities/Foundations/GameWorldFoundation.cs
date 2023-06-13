using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Foundations;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Foundations
{
    public class GameWorldFoundation : GameWorldEntity
    {
        protected override string meshAssetKey => entity.definition.key != null ? entity.definition.key : "Default";

        protected override EntityMeshWrapper.EntityMeshCellWrapperFactory CreateEntityMeshCellWrapper =>
            (
                Transform parent,
                GameObject prefabMesh,
                CellCoordinates cellCoordinates,
                CellCoordinates relativeCellCoordinates,
                CellNeighbors cellNeighbors,
                Tileable.CellPosition cellPosition
            ) =>
                new FoundationMeshCellWrapper(parent, prefabMesh, cellCoordinates, relativeCellCoordinates, cellNeighbors, cellPosition);
    }
}
