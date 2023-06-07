using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities
{
    public class EntityBlocksBuilder
    {
        public static EntityBlocksBuilderBase FromDefinition(EntityDefinition definition) =>
            definition.resizability switch
            {
                Resizability.Horizontal =>
                    new HorizontalResizabilityEntityBlocksBuilder(definition),
                Resizability.Vertical =>
                    new VerticalResizabilityEntityBlocksBuilder(definition),
                Resizability.Diagonal =>
                    new DiagonalResizabilityEntityBlocksBuilder(definition),
                Resizability.Flexible =>
                    new FlexibleResizabilityEntityBlocksBuilder(definition),
                Resizability.Inflexible =>
                    new InflexibleResizabilityEntityBlocksBuilder(definition),
                _ => null
            };
    }
}
