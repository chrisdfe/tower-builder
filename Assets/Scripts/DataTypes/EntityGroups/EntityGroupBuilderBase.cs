using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.DataTypes.Entities.Furnitures;
using UnityEngine;

namespace TowerBuilder.DataTypes.EntityGroups
{
    public class EntityGroupBuilderBase<EntityGroupType>
        where EntityGroupType : EntityGroup, new()
    {
        public EntityGroupBuilderBase()
        {
        }

        public virtual EntityGroupType Build(bool isInBlueprintMode)
        {
            return new EntityGroupType();
        }
    }
}


