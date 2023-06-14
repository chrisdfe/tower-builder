using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
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
                Entity entity,
                CellNeighbors cellNeighbors,
                Tileable.CellPosition cellPosition
            ) =>
                new FoundationMeshCellWrapper(parent, prefabMesh, entity, cellNeighbors, cellPosition);
    }
}
