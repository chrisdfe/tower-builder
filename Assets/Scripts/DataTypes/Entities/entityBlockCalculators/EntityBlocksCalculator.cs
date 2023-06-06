using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities
{
    public static class EntityBlocksCalculator
    {
        public static EntityBlocksCalculatorBase FromDefinition(EntityDefinition entityDefinition) =>
            entityDefinition.resizability switch
            {
                Resizability.Horizontal =>
                    new HorizontalResizabilityEntityBlocksCalculator(entityDefinition),
                Resizability.Vertical =>
                    new VerticalResizabilityEntityBlocksCalculator(entityDefinition),
                Resizability.Diagonal =>
                    new DiagonalResizabilityEntityBlocksCalculator(entityDefinition),
                Resizability.Flexible =>
                    new FlexibleResizabilityEntityBlocksCalculator(entityDefinition),
                Resizability.Inflexible =>
                    new InflexibleResizabilityEntityBlocksCalculator(entityDefinition),
                _ => null
            };
    }
}
