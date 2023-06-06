using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities
{
    public class EntityValidator : Validator<Entity>
    {
        public EntityValidator(Entity entity) : base(entity) { }
    }
}