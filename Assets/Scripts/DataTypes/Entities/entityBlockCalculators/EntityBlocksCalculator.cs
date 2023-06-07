using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities
{
    public static class EntityBlocksCalculator
    {
        public static EntityBlocksCalculatorBase FromDefinition(EntityDefinition definition) =>
            definition.resizability switch
            {
                Resizability.Horizontal =>
                    new HorizontalResizabilityEntityBlocksCalculator(definition),
                Resizability.Vertical =>
                    new VerticalResizabilityEntityBlocksCalculator(definition),
                Resizability.Diagonal =>
                    new DiagonalResizabilityEntityBlocksCalculator(definition),
                Resizability.Flexible =>
                    new FlexibleResizabilityEntityBlocksCalculator(definition),
                Resizability.Inflexible =>
                    new InflexibleResizabilityEntityBlocksCalculator(definition),
                _ => null
            };
    }
}
