using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities
{
    public class EntityValidationError
    {
        public string message { get; } = "";

        public EntityValidationError(string message)
        {
            this.message = message;
        }
    }

    public class EntityValidationErrorList : ListWrapper<EntityValidationError>
    {
        public EntityValidationErrorList() : base() { }
        public EntityValidationErrorList(string message) : base(new EntityValidationError(message)) { }
        public EntityValidationErrorList(EntityValidationError entityValidationError) : base(entityValidationError) { }
        public EntityValidationErrorList(List<EntityValidationError> entityValidationErrors) : base(entityValidationErrors) { }
        public EntityValidationErrorList(EntityValidationErrorList entityValidationErrorList) : base(entityValidationErrorList) { }
    }
}