using TowerBuilder.ApplicationState;

namespace TowerBuilder.DataTypes.EntityGroups
{
    public class EntityGroupValidator : Validator<EntityGroup>
    {
        public EntityGroupValidator(EntityGroup entityGroup) : base(entityGroup) { }

        public void ValidateWithChildren(AppState appState)
        {
            Validate(appState);
            ValidateDescendantEntities(appState);
            ValidateDescendantEntityGroups(appState);
        }

        public ListWrapper<ValidationError> GetAllValidationErrors()
        {
            ListWrapper<ValidationError> result = new ListWrapper<ValidationError>();

            result.Add(errors);
            result.Add(GetDescendantEntityValidationErrors());
            result.Add(GetDescendantEntityGroupValidationErrors());

            return result;
        }

        protected virtual void ValidateDescendantEntities(AppState appState) { }
        protected virtual void ValidateDescendantEntityGroups(AppState appState) { }

        protected virtual ListWrapper<ValidationError> GetDescendantEntityValidationErrors() =>
            new ListWrapper<ValidationError>();

        protected virtual ListWrapper<ValidationError> GetDescendantEntityGroupValidationErrors() =>
            new ListWrapper<ValidationError>();
    }
}