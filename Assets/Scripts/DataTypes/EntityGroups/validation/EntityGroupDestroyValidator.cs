using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups;

namespace TowerBuilder.DataTypes.EntityGroups
{
    public class EntityGroupDestroyValidator : EntityGroupValidator
    {
        public EntityGroupDestroyValidator(EntityGroup entityGroup) : base(entityGroup) { }

        protected override void ValidateDescendantEntities(AppState appState) =>
            validationItem
                .GetDescendantEntities()
                .ForEach((entity) =>
                {
                    entity.destroyValidator.Validate(appState);
                });

        protected override void ValidateDescendantEntityGroups(AppState appState) =>
            validationItem
                .GetDescendantEntityGroups()
                .ForEach((entityGroup) =>
                {
                    entityGroup.destroyValidator.Validate(appState);
                });

        protected override ListWrapper<ValidationError> GetDescendantEntityValidationErrors() =>
            validationItem
                .GetDescendantEntities().items
                .Aggregate(new ListWrapper<ValidationError>(), (acc, entity) =>
                {
                    acc.Add(entity.destroyValidator.errors);
                    return acc;
                });

        protected override ListWrapper<ValidationError> GetDescendantEntityGroupValidationErrors() =>
            validationItem
                .GetDescendantEntityGroups().items
                .Aggregate(new ListWrapper<ValidationError>(), (acc, entity) =>
                {
                    acc.Add(entity.destroyValidator.errors);
                    return acc;
                });
    }
}