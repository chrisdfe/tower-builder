using System;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.EntityGroups;
using UnityEngine;

namespace TowerBuilder.Definitions
{
    public class EntityGroupDefinitions
    {
        public virtual string key { get; set; } = "None";
        public virtual string title { get; set; } = "None";
        public virtual string category { get; set; } = "None";

        public virtual Resizability resizability { get; set; } = Resizability.Flexible;

        public delegate EntityGroupValidator ValidatorFactory(EntityGroup entityGroup);
        public virtual ValidatorFactory validatorFactory => (EntityGroup entityGroup) => new EntityGroupValidator(entityGroup);

        // public delegate EntityGroupBuilder EntityGroupBuilderFactory
    }
}