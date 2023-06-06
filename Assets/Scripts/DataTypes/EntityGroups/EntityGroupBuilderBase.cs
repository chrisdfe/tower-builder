using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.DataTypes.Entities.Furnitures;
using UnityEngine;

namespace TowerBuilder.DataTypes.EntityGroups
{
    public abstract class EntityGroupBuilderBase<EntityGroupType>
        where EntityGroupType : EntityGroup, new()
    {
        public EntityGroupBuilderBase() { }

        public abstract EntityGroupType Build(SelectionBox selectionBox, bool isInBlueprintMode);
    }
}


